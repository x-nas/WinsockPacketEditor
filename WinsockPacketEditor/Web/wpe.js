/*
  远程管理台 —— 共用脚本。三个页面各自只写自己的取数与渲染，公共部分在这里。

  【零依赖】原生 DOM + fetch，不引 jQuery，也不引任何框架。
  旧版这三页各自带着 jQuery 1.11.3（2015 年）与一套 GitHub Pages 的旧主题，
  合计 640KB，其中大半是贴图；现在整套不到 100KB，绝大部分还是三个字体。

  【只用 textContent，不用 innerHTML】日志内容、账号名、IP 归属地这些都来自
  被代理的流量与用户输入 —— 拼进 innerHTML 就是一个存储型 XSS。
  下面 el() 一律走 textContent，调用点不需要自己记得转义。
*/
'use strict';

/* ── DOM ────────────────────────────────────────── */

function $(sel, root) { return (root || document).querySelector(sel); }
function $$(sel, root) { return Array.prototype.slice.call((root || document).querySelectorAll(sel)); }

/**
 * 建元素。text 走 textContent —— 不给 innerHTML 留入口。
 * el('td', { class: 'num', 'data-k': '序号' }, '12')
 */
function el(tag, attrs, text) {
    var n = document.createElement(tag);
    if (attrs) {
        for (var k in attrs) {
            if (attrs[k] !== null && attrs[k] !== undefined) n.setAttribute(k, attrs[k]);
        }
    }
    if (text !== null && text !== undefined) n.textContent = String(text);
    return n;
}

/* ── 取数 ───────────────────────────────────────── */

var _led = null;

/** 顶栏那颗连接灯。接口通了绿、断了红 —— 手机上最想先知道的就是这个。 */
function setLive(ok) {
    if (!_led) _led = $('#led');
    if (_led) _led.className = 'led ' + (ok ? 'ok' : 'bad');
}

/*
  ⚠️ 界面上不要出现 "Failed to fetch" / "HTTP 500" 这种字 —— 那是写代码的人才看得懂的，
  而这一页最常见的失败恰恰是<b>「WPE 被关掉了」</b>，用户完全有能力自己处理。

  两类失败要分开说，它们的下一步动作不一样：
    · fetch 抛 TypeError  = 请求根本没发出去或连接断了（程序关了、断网、地址不对）
    · 我们自己抛的 Error  = 服务端应答了，只是状态码不是 2xx（认证、路由、内部错）
*/
function httpText(status) {
    if (status === 401) return '认证失败 —— 用户名或密码不对';
    if (status === 403) return '没有权限访问这个功能';
    if (status === 404) return '找不到这个接口，程序版本可能对不上';
    if (status >= 500) return 'WPE 内部出错了，去程序的「系统日志」里看看';
    return '服务器拒绝了这次请求（' + status + '）';
}

/** 服务端回的文本能不能直接给用户看：业务提示才行，一整页 HTML 错误页不行。 */
function usableMsg(t) {
    return t && t.length < 200 && t.indexOf('<') < 0 ? t : '';
}

/**
 * 把异常翻成一句人话。所有 catch 都该经它，别再写 e.message。
 * ⚠️ 判据是 name === 'TypeError' —— fetch 只在网络层失败时抛它；
 *    HTTP 错误是上面两个函数已经翻好的，原样带出来即可。
 */
function errText(e, fallback) {
    if (e && e.name === 'TypeError') {
        return '连不上 WPE —— 程序可能已经关了，或者网络断了';
    }

    return (e && e.message) || fallback || '操作失败';
}

/**
 * GET 一个接口。
 * 相对路径：管理台可能挂在任意 IP:端口上，写死绝对路径换台机器就废了。
 */
function api(path) {
    return fetch('./' + path, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error(httpText(r.status));
            return r.json();
        })
        .then(function (d) { setLive(true); return d; })
        .catch(function (e) { setLive(false); throw e; });
}

/** POST JSON。Web API 那几个写接口收的是 [FromBody]。 */
function post(path, body) {
    return fetch('./' + path, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
    }).then(function (r) {
        return r.text().then(function (t) {
            var msg = t;
            try {
                var j = JSON.parse(t);
                // Ok("添加账号成功") 回的是一个 <b>JSON 字符串</b>，直接当文本用会连引号一起显示
                if (typeof j === 'string') msg = j;
                // BadRequest("...") 回的是 { "Message": "..." }
                else if (j && j.Message) msg = j.Message;
            } catch (_) { }
            if (!r.ok) throw new Error(usableMsg(msg) || httpText(r.status));
            setLive(true);
            return msg;
        });
    }).catch(function (e) {
        /*
          ⚠️ <b>只有连不上才熄灯</b>，HTTP 4xx/5xx 不熄。
          那两种是服务器<b>应答了</b>、只是拒绝了这次请求，灯说的是「还连着吗」，不是「这次成功了吗」。
          （api() 那边任何失败都熄，因为它是被轮询的，那条路上一次失败就该报警。）

          不补这一句的后果实测见过：WPE 关掉之后点保存，弹窗里已经写着「连不上」，
          右上角的灯却还是绿的 —— 两处自相矛盾，比没有那盏灯还糟。
        */
        if (e && e.name === 'TypeError') { setLive(false); }
        throw e;
    });
}

/* ── 格式化 ─────────────────────────────────────── */

/** 千分位。计数动辄上千万，不分位读不出量级。 */
function num(v) {
    var n = Number(v);
    if (!isFinite(n)) return v === null || v === undefined ? '—' : String(v);
    return n.toLocaleString('en-US');
}

function pad(n) { return n < 10 ? '0' + n : String(n); }

/**
 * .NET 那边 DateTime 序列化成 ISO 串（含 7 位小数与时区）。
 * 直接显示是 "2026-09-09T12:34:56.7891234+08:00" —— 手机上一行都放不下。
 */
function time(v) {
    if (!v) return '—';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v);
    return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
}

function date(v) {
    if (!v) return '—';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v);
    return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
}

function dateTime(v) {
    if (!v) return '—';
    var d = new Date(v);
    if (isNaN(d.getTime())) return String(v);
    return date(v) + ' ' + time(v);
}

/** 给 <input type="datetime-local"> 用的值（它只认 "YYYY-MM-DDTHH:mm"）。 */
function toLocalInput(v) {
    var d = v ? new Date(v) : new Date();
    if (isNaN(d.getTime())) d = new Date();
    return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate())
        + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes());
}

function bytes(v) {
    var n = Number(v);
    if (!isFinite(n)) return v || '—';
    var u = ['B', 'KB', 'MB', 'GB', 'TB'], i = 0;
    while (n >= 1024 && i < u.length - 1) { n /= 1024; i++; }
    return (i === 0 ? n : n.toFixed(1)) + ' ' + u[i];
}

/** 运行时长：从启动时刻到现在。 */
function uptime(start) {
    var d = new Date(start);
    if (isNaN(d.getTime())) return '—';
    var s = Math.max(0, Math.floor((Date.now() - d.getTime()) / 1000));
    var day = Math.floor(s / 86400);
    var h = Math.floor(s / 3600) % 24;
    return (day > 0 ? day + 'd ' : '') + pad(h) + ':' + pad(Math.floor(s / 60) % 60) + ':' + pad(s % 60);
}

/* ── 轻提示 ─────────────────────────────────────── */

function toast(text, kind) {
    var box = $('#toasts');
    if (!box) return;
    var d = el('div', { class: kind || '' }, text);
    box.appendChild(d);
    setTimeout(function () { if (d.parentNode) d.parentNode.removeChild(d); }, 2600);
}

/* ── 轮询 ───────────────────────────────────────── */

/**
 * 起一个轮询，并接管「暂停」按钮。
 *
 * ⚠️ 页面切到后台就停（visibilitychange）。手机上这一条特别要紧：
 * 用户按 Home 键之后页面并不会被销毁，不停的话会在后台一直发请求耗电耗流量。
 */
function poll(fn, ms) {
    var timer = 0;
    var paused = false;

    function tick() { if (!paused && !document.hidden) fn(); }

    function start() { stop(); timer = setInterval(tick, ms); }
    function stop() { if (timer) { clearInterval(timer); timer = 0; } }

    document.addEventListener('visibilitychange', function () {
        if (document.hidden) stop();
        else { if (!paused) fn(); start(); }   //暂停着的页面切回来不该偷偷刷一次
    });

    fn();
    start();

    return {
        get paused() { return paused; },
        toggle: function () { paused = !paused; if (!paused) fn(); return paused; },
        now: fn,
    };
}

/* ── 顶栏 ───────────────────────────────────────── */

/** 按当前文件名点亮导航项。管理台可能被挂在 / 或 /index.html，两种都要认。 */
function markNav() {
    var here = (location.pathname.split('/').pop() || 'index.html').toLowerCase();
    if (here === '') here = 'index.html';
    $$('.nav a').forEach(function (a) {
        var target = (a.getAttribute('href') || '').toLowerCase();
        var isHome = (target === 'index.html' && (here === 'index.html' || here === '/'));
        if (target === here || isHome) a.classList.add('on');
    });
}

document.addEventListener('DOMContentLoaded', markNav);
