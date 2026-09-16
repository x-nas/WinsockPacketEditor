<script setup lang="ts">
/*
  多开设置 —— 对应 WinForms 的 Controls/DataBaseSetting。

  【与 WinForms 版的三处不同】
  ① 整屏而不是 500×250 的 Modal：外壳最小 1120×700，小方块浮在中间会很空，
     也放不下下面那块预览。标题栏的返回箭头就是退路。
  ② 多了「Preview」终端：保存<b>之前</b>就说清楚会发生什么 —— 目录要不要建、
     库是新建还是沿用。原界面按下保存前什么都看不出来。
  ③ 保存后会<b>重载配置</b>（C# 侧 saveInstance 里做）。WinForms 只改路径 + 建库，
     内存里的配置还是旧库那份，退出时写进新库，两个实例会互相污染。

  【主色用洋红】启动页那张卡就是 .cd.mg，一路跟过来，与代理模式的青色分开。
*/
import { computed, onBeforeUnmount, ref, watch } from 'vue'
import { call } from '../bridge'
import { lang, normalize } from '../i18n'
import { httpAddr, socks5Addr } from '../stores/runtime'
import SettingsModal from './proxy/SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', value: boolean): void }>()

interface Probe {
  valid: boolean
  dirExists: boolean
  fileExists: boolean
  size: number
  modified: string
  full: string
  current: string
  currentSize: number
}

const path = ref('')
const defaultPath = ref('')
const dbName = ref('')
const probe = ref<Probe | null>(null)
const saving = ref(false)
const picking = ref(false)

async function load(): Promise<void> {
  try {
    const s = await call<any>('getSystemCheck')
    path.value = s.dbDir || ''
    defaultPath.value = s.dbDir || ''
    dbName.value = s.dbFile || ''
  } catch (e) {
    console.error('[instance] 取当前数据库位置失败', e)
  }

  void refreshProbe()
}

watch(() => props.open, (open) => { if (open) void load() }, { immediate: true })

/*
  路径每改一个字符就问一次 C#（磁盘存在性检查很便宜，Directory.Exists 是一次
  文件系统调用），但仍要防抖 —— 连打十几个字符会发十几次往返，最后一次才作数。
*/
let probeTimer = 0
//改完路径 160ms 内离开这一屏的话，防抖那一下不该再去问 C#、往一个已卸载的组件里写
onBeforeUnmount(() => window.clearTimeout(probeTimer))

watch(path, () => {
  window.clearTimeout(probeTimer)
  probeTimer = window.setTimeout(() => { void refreshProbe() }, 160)
})

async function refreshProbe(): Promise<void> {
  try {
    probe.value = await call<Probe>('probeDbPath', { path: path.value })
  } catch (e) {
    console.error('[instance] 探测路径失败', e)
    probe.value = null
  }
}

async function pick(): Promise<void> {
  picking.value = true
  try {
    // 原生目录对话框由 C# 弹 —— 浏览器给不出完整路径
    const r = await call<{ path: string | null }>('pickFolder', { path: path.value })
    if (r?.path) path.value = r.path
  } catch (e) {
    console.error('[instance] 选择目录失败', e)
  } finally {
    picking.value = false
  }
}

function useDefault(): void { path.value = defaultPath.value }

const canSave = computed(() => !!probe.value?.valid && !saving.value)

async function save(): Promise<void> {
  if (!canSave.value) return

  saving.value = true
  try {
    const r = await call<any>('saveInstance', { path: path.value })
    if (!r?.ok) {
      console.error('[instance] 保存失败', r?.error)
      return
    }

    /*
      配置换了一份，两处缓存要跟着刷新：
        · socks5Addr / httpAddr 在状态栏与运行状态条上（新库里的代理配置可能不同）
        · 语言：C# 侧 ApplyAll 已经把 AntdUI 切过去了，前端字典也得跟上，
          否则会出现「弹窗英文、页面中文」。
      这里不调 setLang —— 那个会反过来再写一次 C#，绕一圈还可能把值写反。
    */
    socks5Addr.value = r.socks5Addr || ''
    httpAddr.value = r.httpAddr || ''
    lang.value = normalize(r.language)

    emit('update:open', false)
  } catch (e) {
    console.error('[instance] 保存失败', e)
  } finally {
    saving.value = false
  }
}

/** 字节数按量级给单位。库通常在几百 KB 到几 MB。 */
function sizeText(n: number): string {
  if (!n) return '0 B'
  if (n < 1024) return n + ' B'
  if (n < 1024 * 1024) return (n / 1024).toFixed(1) + ' KB'
  return (n / 1024 / 1024).toFixed(2) + ' MB'
}
</script>

<template>
  <SettingsModal :open="props.open" title="多开设置" subtitle="本次运行使用独立数据库" :busy="saving" @update:open="emit('update:open', $event)" @save="save">
    <div class="instance-set">
      <p class="instance-lead">本次运行有效：数据库目录不会永久保存，重启后仍使用默认目录。</p>
      <section class="instance-sec">
        <div class="instance-sec-title"><b>01</b><strong>数据库目录</strong><span>选择本次运行使用的数据库目录</span></div>
        <div class="instance-row">
          <label>目录路径</label>
          <div class="path-line">
            <input v-model="path" class="inp" spellcheck="false" :class="{ bad: probe && !probe.valid }" placeholder="例如 D:\\WPE64DB\\instance-02">
            <button class="path-btn" :disabled="picking" @click="pick">浏览</button>
            <button class="path-btn reset" :disabled="picking" @click="useDefault">默认目录</button>
          </div>
        </div>
        <p v-if="probe && !probe.valid" class="error-text">目录路径无效，请选择一个有效的本地目录。</p>
        <p v-else class="instance-hint">目录不存在时会自动创建；选择已有目录会沿用其中的数据库文件。</p>
      </section>
      <section class="instance-sec">
        <div class="instance-sec-title"><b>02</b><strong>数据库状态</strong><span>当前路径和数据库文件信息</span></div>
        <div class="instance-info">
          <div><span>数据库文件</span><b>{{ dbName || '—' }}</b></div>
          <div><span>目录状态</span><b :class="probe?.dirExists ? 'good' : 'warn'">{{ probe ? (probe.dirExists ? '已存在' : '将自动创建') : '检查中…' }}</b></div>
          <div><span>数据库状态</span><b :class="probe?.fileExists ? 'good' : 'warn'">{{ probe ? (probe.fileExists ? '沿用已有数据库' : '将新建数据库') : '检查中…' }}</b></div>
          <div><span>当前数据库</span><b>{{ probe?.current || '—' }}</b></div>
        </div>
        <p v-if="probe" class="instance-hint">目标大小：{{ sizeText(probe.size) }}<span v-if="probe.modified">　最后修改：{{ probe.modified }}</span></p>
      </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.inst {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 0 56px;
  overflow: auto;
}

.instance-set {
  display: block;
  width: 100%;
  max-height: min(62vh, 520px);
  overflow-y: auto;
  padding: 2px 0 4px;
}

.instance-lead {
  margin: 0 0 12px;
  padding: 0 14px 12px;
  border-bottom: 1px solid var(--border);
  color: var(--amber);
  font-size: var(--fs-body);
  line-height: 1.5;
}

.instance-sec {
  margin: 0 0 12px;
  border: 1px solid rgb(var(--border-rgb) / 80%);
  border-left: 2px solid var(--cyan);
  background: rgb(var(--inset-rgb) / 16%);
}
.instance-sec-title {
  display: flex;
  align-items: center;
  gap: 12px;
  min-height: 39px;
  padding: 0 14px;
  border-bottom: 1px solid rgb(var(--border-rgb) / 65%);
  background: var(--panel);
}
.instance-sec-title b {
  padding: 2px 5px;
  border: 1px solid var(--cyan);
  color: var(--cyan);
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .12em;
}
.instance-sec-title strong { color: var(--soft); font-size: var(--fs-body); }
.instance-sec-title span { color: var(--muted); font-size: var(--fs-caption); }
.instance-row { display: grid; grid-template-columns: 112px 1fr; align-items: center; min-height: 58px; padding: 8px 12px; }
.instance-row label { color: var(--muted); font-size: var(--fs-body); }
.instance-hint { margin: 0; padding: 0 14px 11px; color: var(--muted); font-size: var(--fs-caption); }
.instance-info { display: grid; grid-template-columns: 1fr 1fr; gap: 1px; margin: 0 12px 10px; background: var(--border); }
.instance-info > div { min-width: 0; padding: 10px 12px; background: var(--card); }
.instance-info span, .instance-info b { display: block; }
.instance-info span { margin-bottom: 4px; color: var(--muted); font-size: var(--fs-caption); }
.instance-info b { overflow: hidden; color: var(--soft); font-size: var(--fs-body); text-overflow: ellipsis; white-space: nowrap; }
.instance-info b.good { color: var(--green); }
.instance-info b.warn { color: var(--amber); }

.instance-note {
  padding: 11px 14px;
  border: 1px solid rgb(var(--amber-rgb) / 35%);
  background: rgb(var(--amber-rgb) / 7%);
  color: var(--amber);
  font-size: var(--fs-body);
  line-height: 1.5;
}

.instance-field { margin-top: 14px; }
.instance-field > label { display: block; margin-bottom: 6px; color: var(--soft); font-size: var(--fs-label); }
.path-line { display: flex; min-width: 0; border: 1px solid var(--border); background: var(--card); }
.path-line .inp { height: 42px; }
.path-line .path-btn { height: 42px; flex: 0 0 76px; justify-content: center; padding: 0 10px; background: transparent; border: 0; border-left: 1px solid var(--border); color: var(--cyan); font-family: var(--share); font-size: var(--fs-label); cursor: pointer; }
.path-line .path-btn:hover:not(:disabled) { background: rgb(var(--cyan-rgb) / 10%); }
.path-line .path-btn:disabled { opacity: .45; cursor: default; }
.path-line .path-btn.reset { flex-basis: 92px; color: var(--magenta); }
.path-line .path-btn.reset:hover:not(:disabled) { background: rgb(var(--magenta-rgb) / 10%); }
.instance-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1px; margin-top: 14px; border: 1px solid var(--border); background: var(--border); }
.instance-grid > div { min-width: 0; padding: 11px 14px; background: var(--card); }
.instance-grid span, .instance-grid b { display: block; }
.instance-grid span { color: var(--muted); font-size: var(--fs-caption); margin-bottom: 5px; }
.instance-grid b { overflow: hidden; color: var(--soft); font-size: var(--fs-body); text-overflow: ellipsis; white-space: nowrap; }
.instance-grid b.good { color: var(--green); }
.instance-grid b.warn { color: var(--amber); }
.error-text { margin: 10px 0 0; color: var(--danger); font-size: var(--fs-body); }
.instance-meta { display: flex; gap: 18px; margin-top: 10px; color: var(--muted); font-size: var(--fs-caption); }

/* eyebrow / 标题 / 副标题：与 StartView 同构，只是主色换成洋红 */
/*
  eyebrow 与标题走 style.css 的共用件 `.scrn`，这里只把强调色换成洋红。
  ⚠️ 这一屏<b>不做</b> StartView 那套三层错位 glitch —— 那是首屏的开场效果，
  二级页面每进一次都抖一遍会烦。
*/
.inst {
  --hd: var(--magenta);
  --hd-rgb: var(--magenta-rgb);
}

/* 标题是写死的英文，那两条 latin-only 的排版留在本屏（共用件里刻意没有）*/
.ttl { text-transform: uppercase; letter-spacing: -.02em; }

/* 副标题与光标的基样式在 style.css 的 `.scrn` 里（三屏共用）*/

/* 「仅本次有效」——用琥珀色警示，与测试版提示同一套语汇 */
.note {
  display: flex;
  gap: 12px;
  align-items: flex-start;
  margin: 26px 0 0;
  padding: 13px 16px;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: linear-gradient(90deg, rgb(var(--amber-rgb) / 7%), transparent 60%);
}

.note .bang {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .18em;
  text-transform: uppercase;
  color: var(--amber);
  white-space: nowrap;
  padding-top: 2px;
}

.note p { font-size: var(--fs-body); color: var(--dim3); margin: 0; }

/* 表单：整块一个边框，行与行之间发丝线，与卡片网格同一种做法 */
.form { margin: 22px 0 0; border: 1px solid var(--border); background: var(--card); }

.row {
  display: grid;
  grid-template-columns: 132px 1fr;
  align-items: center;
  border-bottom: 1px solid var(--border);
}

.row:last-child { border-bottom: 0; }

.row > .k {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
  padding-left: 18px;
}

.row > .v { display: flex; align-items: center; min-width: 0; }

.inp {
  flex: 1;
  min-width: 0;
  height: 46px;
  padding: 0 14px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 0;
  border-left: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: var(--fs-lead);
  outline: none;
  /* 路径要能选中复制 —— body 上是 user-select: none */
  user-select: text;
}

/*
  ⚠️ <b>border-left-color 必须自己写一遍。</b>

  style.css 里现在有一条全局「.inp:focus { border-color: var(--cyan) }」，
  它是 (0,2,0)，与这里的「.inp[data-v-…]」（也是 (0,2,0)）打平 ——
  平局时 style.css 赢（main.ts 里组件样式在前、style.css 在后）。
  于是这个路径条一聚焦左边那道线会变青，而它的聚焦提示<b>本来是底色</b>。

  写成「.inp:focus」+ scoped 之后是 (0,3,0)，稳赢，不再依赖顺序。
*/
.inp:focus { background: rgb(var(--cyan-rgb) / 6%); border-left-color: var(--border); }
.inp.bad { color: var(--danger); }
.inp.ro { color: var(--muted); background: transparent; }

.browse {
  height: 46px;
  padding: 0 18px;
  background: transparent;
  border: 0;
  border-left: 1px solid var(--border);
  color: var(--cyan);
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
}

.browse:hover:not(:disabled) { background: rgb(var(--cyan-rgb) / 10%); }
.browse:disabled { opacity: .45; cursor: default; }

/* 焦点环画在内侧：输入框与按钮之间只有 1px 发丝线，正偏移会压到邻居 */
.browse:focus-visible { outline-offset: -2px; outline-color: var(--cyan); }

.ico.sm { width: 15px; height: 15px; }

/* 预览终端：外壳与启动页的 System Check 完全一致 */
.term { margin: 22px 0 0; background: var(--sink); border: 1px solid var(--border); }

.term-bar {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 14px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
}

.term-bar .d { width: 10px; height: 10px; border-radius: 50%; }

.term-bar .lbl {
  margin-left: 8px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .16em;
  text-transform: uppercase;
  color: var(--muted);
}

.term-body { padding: 12px 16px; font-size: var(--fs-body); color: var(--soft); overflow-x: auto; }
.term-body .l { display: block; white-space: pre; }
.term-body .g { color: var(--green); }
.term-body .c { color: var(--muted); }
.term-body .y { color: var(--cyan); }
.term-body .a { color: var(--amber); }
.term-body .r { color: var(--danger); }

/* 动作条 */
.acts { display: flex; gap: 12px; margin: 24px 0 0; }

.btn {
  padding: 15px 26px 15px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .16em;
  text-transform: uppercase;
  cursor: pointer;
  transition: .15s;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .4; cursor: default; }
.btn.primary { border-color: rgb(var(--magenta-rgb) / 45%); color: var(--magenta); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--magenta-rgb) / 10%); box-shadow: 0 0 16px rgb(var(--magenta-rgb) / 20%); }
.btn.primary:focus-visible { outline-color: var(--magenta); }

.acts .grow { flex: 1; }

.hint {
  align-self: center;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}
</style>
