// 字号守门：src 下所有 .vue / .css 的 font-size 只许用 var(--fs-*)（或四个语义别名 / inherit），
// 写死 px 的只允许下面这张白名单里的例外。改完样式跑一次：
//   node tools/check-font-sizes.mjs
// 有违规就 exit 1。规范与例外的理由见 CLAUDE.md「字号规范」。
import fs from 'node:fs'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../src')

const OK_VALUE = /^(var\(--fs-(caption|label|small|dense|body|lead|title|num|num-lg)\)|var\(--(th|btn|label|menu|ttl)-size\)|inherit)$/

// [文件（相对 src）, 选择器正则, 为什么]
const ALLOW = [
  ['App.vue', /^\.bname$/, '标题栏品牌字'],
  ['components/StartView.vue', /\.wm\b/, '机位号丝印'],
  ['components/InjectView.vue', /\.wm\b/, '机位号丝印'],
  ['components/InjectView.vue', /\.foot \.ar/, '→ 箭头字形'],
  ['components/proxy/StatData.vue', /^\.sep$/, '› 分隔字形'],
  ['components/proxy/AccountList.vue', /\.op \.n/, '图标角标，贴在 16px 图标上'],
  ['style.css', /\.gtool \.sx/, '× 清除字形'],
  ['reset.css', /./, '第三方全局 reset（ant-design-vue 4.2.6 原样搬来），不属于本项目的字号层级'],
]

function walk(d, out) {
  for (const f of fs.readdirSync(d)) {
    const p = path.join(d, f)
    if (fs.statSync(p).isDirectory()) walk(p, out)
    else if (/\.(vue|css)$/.test(f) && f !== 'tokens.css') out.push(p)
  }
  return out
}
const strip = (css) => css.replace(/\/\*[\s\S]*?\*\//g, (c) => c.replace(/[^\n]/g, ' '))

const bad = []
for (const file of walk(ROOT, [])) {
  const rel = path.relative(ROOT, file).replace(/\\/g, '/')
  const src = fs.readFileSync(file, 'utf8')
  const blocks = []
  if (file.endsWith('.css')) blocks.push([0, src])
  else { const re = /<style[^>]*>([\s\S]*?)<\/style>/g; let m; while ((m = re.exec(src))) blocks.push([m.index + m[0].indexOf('>') + 1, m[1]]) }
  for (const [base, raw] of blocks) {
    const css = strip(raw)
    ;(function parse(lo, hi) {
      let i = lo
      while (i < hi) {
        const open = css.indexOf('{', i); if (open < 0 || open >= hi) break
        const sel = css.slice(i, open).trim().replace(/\s+/g, ' ')
        let depth = 1, j = open + 1
        while (j < hi && depth) { if (css[j] === '{') depth++; else if (css[j] === '}') depth--; j++ }
        if (/^@(media|supports|layer)/.test(sel)) parse(open + 1, j - 1)
        else if (!sel.startsWith('@')) {
          const re = /font-size\s*:\s*([^;}]+)/gi; let m
          const body = css.slice(open + 1, j - 1)
          while ((m = re.exec(body))) {
            const v = m[1].trim()
            if (OK_VALUE.test(v)) continue
            if (ALLOW.some(([f, s]) => f === rel && s.test(sel))) continue
            const line = src.slice(0, base + open).split('\n').length
            bad.push(`${rel}:${line}  ${sel}  font-size: ${v}`)
          }
        }
        i = j
      }
    })(0, css.length)
  }
}

if (bad.length) {
  console.log('字号没走 var(--fs-*) 的 ' + bad.length + ' 处：\n  ' + bad.join('\n  '))
  process.exit(1)
}
console.log('字号守门：全部走令牌（或在白名单里）')
