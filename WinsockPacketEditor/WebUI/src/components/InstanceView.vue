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
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../bridge'
import { lang, normalize, t } from '../i18n'
import { httpAddr, socks5Addr } from '../stores/runtime'

const emit = defineEmits<{ (e: 'back'): void }>()

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
const dbName = ref('')
const probe = ref<Probe | null>(null)
const saving = ref(false)
const picking = ref(false)

/** 打字机，与启动页同一套。切语言要重打一遍，见 StartView 里的说明。 */
const typed = ref('')
let runId = 0

async function typeSubtitle(): Promise<void> {
  const mine = ++runId
  const text = t('inst.subtitle')
  typed.value = ''

  for (let i = 1; i <= text.length; i++) {
    if (mine !== runId) return
    typed.value = text.slice(0, i)
    await new Promise((r) => setTimeout(r, 32))
  }
}

watch(lang, () => { void typeSubtitle() })

onMounted(async () => {
  try {
    const s = await call<any>('getSystemCheck')
    path.value = s.dbDir || ''
    dbName.value = s.dbFile || ''
  } catch (e) {
    console.error('[instance] 取当前数据库位置失败', e)
  }

  void refreshProbe()
  void typeSubtitle()
})

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

    emit('back')
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
  <main class="inst scrn">
    <div class="eyebrow">
      <span class="dash" />
      <span class="lbl">{{ t('inst.eyebrow') }}</span>
    </div>

    <h1 class="ttl">Multiple Open</h1>

    <p class="subtitle">{{ typed }}<span class="cur" /></p>

    <!-- 这一屏最重要的一句话：设置只在本次运行有效 -->
    <div class="note">
      <span class="bang">{{ t('inst.onceTag') }}</span>
      <p>{{ t('inst.onceText') }}</p>
    </div>

    <div class="form">
      <div class="row">
        <div class="k">DataBase Path</div>
        <div class="v">
          <input
            v-model="path"
            class="inp"
            spellcheck="false"
            :class="{ bad: probe && !probe.valid }"
            :placeholder="t('inst.pathPlaceholder')"
          >
          <button class="browse" :disabled="picking" @click="pick">
            <svg class="ico sm" viewBox="0 0 24 24">
              <path d="M3 7a2 2 0 0 1 2-2h4l2 2h8a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z" />
            </svg>
            {{ t('inst.browse') }}
          </button>
        </div>
      </div>

      <div class="row">
        <div class="k">DataBase Name</div>
        <!-- 只读：文件名由主工程的版本号推导，不是用户能定的 -->
        <div class="v"><input class="inp ro" :value="dbName" readonly></div>
      </div>
    </div>

    <div class="term">
      <div class="term-bar">
        <span class="d" style="background:#ff5f57" />
        <span class="d" style="background:#febc2e" />
        <span class="d" style="background:#28c840" />
        <span class="lbl">Preview</span>
      </div>
      <div class="term-body">
        <span class="l"><span class="c">$</span> <span class="g">wpe64</span> --db <span class="y">"{{ probe?.full || '…' }}"</span></span>

        <span v-if="probe && !probe.valid" class="l">
          <span class="c">  ├─</span> <span class="r">{{ t('inst.badPath') }}</span>
        </span>
        <span v-else-if="probe" class="l">
          <span class="c">  ├─</span> {{ t('inst.dir') }}
          <span :class="probe.dirExists ? 'g' : 'a'">{{ probe.dirExists ? t('inst.dirOk') : t('inst.dirNew') }}</span>
          <span class="c">   ├─</span> {{ t('inst.db') }}
          <span :class="probe.fileExists ? 'g' : 'a'">{{ probe.fileExists ? t('inst.dbOld') : t('inst.dbNew') }}</span>
          <template v-if="probe.fileExists">
            <span class="c"> · </span><span class="g">{{ sizeText(probe.size) }}</span>
            <span class="c"> · </span>{{ probe.modified }}
          </template>
        </span>

        <span class="l">
          <span class="c">  └─</span> {{ t('inst.current') }}
          <span class="y">{{ probe?.current || '…' }}</span>
          <template v-if="probe?.currentSize">
            <span class="c"> · </span><span class="g">{{ sizeText(probe.currentSize) }}</span>
          </template>
        </span>
      </div>
    </div>

    <div class="acts">
      <button class="btn primary" :disabled="!canSave" @click="save">
        {{ saving ? t('inst.saving') : t('inst.save') }}
      </button>
      <button class="btn" @click="emit('back')">{{ t('inst.cancel') }}</button>
      <span class="grow" />
      <span class="hint">{{ t('inst.hint') }}</span>
    </div>
  </main>
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
