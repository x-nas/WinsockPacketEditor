using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    #region//编辑类弹窗

    /// <summary>
    /// 编辑类弹窗的统一入口（B1 批次从 Operate.cs 搬出）。
    ///
    /// 【为什么搬】
    /// 这 17 个方法原先散落在 Operate 的 7 个子模块里，形态完全一致：
    /// 「开一个 Modal，里面装一个 UserControl」，不含任何业务逻辑，
    /// 却让 Operate 直接 new 出 UI 控件、甚至把另一个 UserControl 当回调宿主传进去
    /// （FireWallSetting / MapSetting / AutoStoresList），形成
    ///     逻辑层 → UI 控件 → 回调回逻辑层
    /// 的双向依赖。搬出来之后 Operate 少了 17 处 UI 触点，是「删除」而不是「改写」。
    ///
    /// 【将来接 WebView2 时】
    /// 这里的每一个方法对应前端一个 &lt;XxxEditModal&gt; 组件；
    /// 需要从 Operate 内部反向调起时走 IUiHost.PromptAsync(FormId, Arg)，
    /// WinForms 侧用 WinFormsUiHost.RegisterPrompt 把 FormId 映射到本类的方法。
    ///
    /// 【约定】
    /// 本类属于 UI 层，允许自由使用 AntdUI 与 System.Windows.Forms。
    /// 所有弹窗一律 Keyboard=false（禁 Esc）、MaskClosable=false（禁点遮罩关闭）、
    /// BtnHeight=0（不显示 Modal 自带的确定/取消，由内部控件自己出按钮）——
    /// 与搬迁前逐字一致。
    /// </summary>
    public static class UiDialogs
    {
        #region//公共打开逻辑

        /// <summary>
        /// 打开一个装载 UserControl 的编辑弹窗。
        ///
        /// 搬迁前 17 处里有 2 处（OpenRuleList / OpenRuleEdit）带 try/catch、其余 15 处没有；
        /// 这里统一加上：弹窗打不开不应该让整个程序崩掉，失败时记日志并给用户一个提示。
        /// 这是 B1 唯一一处有意的行为变化。
        /// </summary>
        /// <summary>
        /// 打开一个编辑弹窗（模态，阻塞到关闭）。
        ///
        /// <paramref name="Push"/> 是 B9d 加的：编辑弹窗<b>就地改对象的属性</b>，不动列表结构，
        /// 所以 BindingList 一个事件都不会触发（这些模型都没实现 INotifyPropertyChanged）。
        /// WinForms 侧靠 AntdUI 表格自己重绘，看不出问题；但桥那侧持有的是另一份副本，
        /// 不主动推就会一直显示改之前的内容。
        ///
        /// 传 null 表示这个弹窗不对应任何列表（或由调用方自己推，如封包编辑走单行 Update）。
        /// 纯 WinForms 运行时 <c>NeedsRows</c> 为 false，FeedPump 内部直接返回，不付代价。
        /// </summary>
        private static void OpenEditModal(Form form, string TitleKey, string TitleFallback, Control Content,
            FeedList? Push = null)
        {
            string Title = AntdUI.Localization.Get(TitleKey, TitleFallback);

            try
            {
                AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, Content)
                {
                    Keyboard = false,
                    MaskClosable = false,
                    BtnHeight = 0,
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OpenEditModal), ex);
                UI.Toast(UiIcon.Error, Title);
            }
            finally
            {
                //放在 finally 里：弹窗抛异常时列表也可能已经被改了一半，推一次总比不推强
                if (Push.HasValue) { FeedPump.PushNow(Push.Value); }
            }
        }

        #endregion

        #region//测试版提示

        /// <summary>
        /// 启动时的测试版提示（B6 批次从 Operate.SystemConfig.ShowBetaMessage 搬出）。
        ///
        /// 它不是确认框而是「知道了」单按钮提示，还带自定义按钮渐变色，
        /// 用 IUiHost.ConfirmAsync 表达不了，而且除了读一个 IsBeta 开关之外没有业务逻辑，
        /// 所以整体留在 UI 层，与 B1 搬出的那 17 个编辑弹窗同一处置。
        /// </summary>
        public static void ShowBetaMessage(Form form)
        {
            try
            {
                if (!Operate.SystemConfig.IsBeta)
                {
                    return;
                }

                string sTitle = UI.T("BetaVersion", "这是一个测试版程序");
                string sContent = UI.T("BetaVersionContent", "\r\n测试版程序可能存在未知的 Bug，请谨慎使用！\r\n\r\n如需使用正式版，请至官网下载最新发布的程序。");

                AntdUI.Modal.open(new AntdUI.Modal.Config(form, sTitle, sContent, AntdUI.TType.Warn)
                {
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    CancelText = null,
                    OkText = UI.T("GotIt", "知道了"),
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ShowBetaMessage), ex);
            }
        }

        #endregion

        #region//加密密码输入框

        /// <summary>
        /// 登记两个密码框的渲染方式（B6 批次，硬骨头 2）。
        ///
        /// 「密码留空则不关闭弹窗、提示后继续输入」这个循环留在这里，
        /// 靠 Modal.Config.OnOk 返回 false 实现，与搬迁前逐字一致；
        /// Operate 侧只 await 一个「密码 或 null」的结果。
        ///
        /// 由 Program.Main / WPEHook.Hook.Run 在建窗之前调用一次。
        /// </summary>
        public static void RegisterPrompts()
        {
            //入参从「一个标题字符串」换成了 PasswordAsk（多带一个 FilePath，桥那边验密码用）
            WinFormsUiHost.RegisterPrompt("encrypt-export", (owner, arg) =>
                AskPassword(owner, TitleOf(arg), Operate.SystemConfig.PWType.Export, "ExportList.Error"));

            WinFormsUiHost.RegisterPrompt("encrypt-import", (owner, arg) =>
                AskPassword(owner, TitleOf(arg), Operate.SystemConfig.PWType.Import, "ImportList.Error"));
        }

        /// <summary>
        /// 取标题。WinForms 侧不用 FilePath —— 它的「留空不关窗」循环够用了，
        /// 密码错误仍走原来的路（关窗后由 Operate 报错）。
        /// </summary>
        private static string TitleOf(object Arg)
        {
            PasswordAsk ask = Arg as PasswordAsk;
            return ask != null ? ask.Title : Arg as string;
        }

        private static object AskPassword(Form form, string Title, Operate.SystemConfig.PWType Type, string EmptyKey)
        {
            string Password = null;

            EncryptionPassword epControl = new EncryptionPassword(Type);

            AntdUI.Modal.open(new AntdUI.Modal.Config(form, Title, epControl, AntdUI.TType.Info)
            {
                Keyboard = false,
                MaskClosable = false,
                OnOk = config =>
                {
                    string sPW = epControl.GetPassword();

                    if (string.IsNullOrEmpty(sPW))
                    {
                        //留空：提示后保持弹窗打开，让用户继续输入
                        epControl.EncryptionText_Changed();
                        UI.Toast(UiIcon.Error, UI.T(EmptyKey, "密码不能为空"));

                        return false;
                    }

                    Password = sPW;
                    return true;
                }
            });

            return string.IsNullOrEmpty(Password) ? null : new PasswordResult(Password);
        }

        #endregion

        #region//防火墙 - 白名单 / 黑名单

        public static void OpenWhiteListEdit(Form form, FireWallSetting fwForm, WhiteListInfo wli)
        {
            OpenEditModal(form, "FireWallSetting.WhiteListEdit", "白名单编辑", new WhiteListEdit(form, fwForm, wli), FeedList.WhiteList);
        }

        public static void OpenBlackListEdit(Form form, FireWallSetting fwForm, BlackListInfo bli)
        {
            OpenEditModal(form, "FireWallSetting.BlackListEdit", "黑名单编辑", new BlackListEdit(form, fwForm, bli), FeedList.BlackList);
        }

        #endregion

        #region//代理账号

        public static void OpenAccountEdit(Form form, AccountInfo ai)
        {
            OpenEditModal(form, "AccountEditForm", "账号编辑", new AccountEdit(form, ai), FeedList.Account);
        }

        public static void BatchAddAccounts(Form form)
        {
            OpenEditModal(form, "AccountList.BatchAdd", "批量创建账号", new BatchAccounts(form), FeedList.Account);
        }

        #endregion

        #region//端口映射

        public static void OpenMapLocalEdit(Form form, MapSetting msForm, MapLocal ml)
        {
            OpenEditModal(form, "MapLocalForm", "本地映射编辑", new MapLocalEdit(form, msForm, ml), FeedList.MapLocal);
        }

        public static void OpenMapRemoteEdit(Form form, MapSetting msForm, MapRemote mr)
        {
            OpenEditModal(form, "MapRemoteForm", "远程映射编辑", new MapRemoteEdit(form, msForm, mr), FeedList.MapRemote);
        }

        #endregion

        #region//封包编辑

        /*
            封包 / 代理这两份是高频列表，走 Append 推送，整表推不起
            （列表可能有五千行，而且每秒还在涨）。所以这里用<b>单行 Update</b>：
            编辑改的就是这一行的字节，受影响的只有它的 Len 与 Preview。

            这是 IUiFeed.Update 目前唯一的真实用途 —— 另外 14 份中低频列表都整表 Replace。
        */

        public static void OpenPacketEdit(Form form, PacketInfo pi)
        {
            OpenEditModal(form, "PacketEditForm", "封包编辑", new PacketEdit(form, pi));

            if (pi != null) { UI.Feed.Update(FeedList.Packet, PacketRow.From_(pi)); }
        }

        public static void OpenPacketEdit(Form form, ProxyInfo pi)
        {
            OpenEditModal(form, "PacketEditForm", "封包编辑", new PacketEdit(form, pi));

            if (pi != null) { UI.Feed.Update(FeedList.Proxy, ProxyRow.From_(pi)); }
        }

        #endregion

        #region//滤镜 / 发送 / 机器人

        public static void OpenFilterEdit(Form form, FilterInfo fi)
        {
            OpenEditModal(form, "FilterEditForm", "滤镜编辑", new FilterEdit(form, fi), FeedList.Filter);
        }

        public static void OpenSendEdit(Form form, SendInfo si)
        {
            OpenEditModal(form, "SendEditForm", "发送编辑", new SendEdit(form, si), FeedList.Send);
        }

        public static void OpenRobotEdit(Form form, RobotInfo ri)
        {
            OpenEditModal(form, "RobotEditForm", "机器人编辑", new RobotEdit(form, ri), FeedList.Robot);
        }

        #endregion

        #region//封包仓库

        public static void OpenWareHouseEdit(Form form, WareHouseInfo whi)
        {
            OpenEditModal(form, "WareHouse.Edit", "编辑", new WareHouseEdit(form, whi), FeedList.WareHouse);
        }

        public static void OpenAutoStoresEdit(Form form, AutoStoresList aslForm, AutoStoresInfo asiSelect)
        {
            OpenEditModal(form, "AutoStores.Edit", "自动入库编辑", new AutoStoresEdit(form, aslForm, asiSelect), FeedList.AutoStores);
        }

        #endregion

        #region//ProxyCap 配置 - 服务器 / 规则 / 公告

        public static void OpenServerEdit(Form form, ServerInfo si)
        {
            OpenEditModal(form, "WPCConfig.ServerList.Edit", "服务器编辑", new ServerEdit(form, si), FeedList.Server);
        }

        public static void OpenRuleList(Form form, ServerInfo si)
        {
            OpenEditModal(form, "WPCConfig.RuleList", "规则列表", new RuleList(form, si), FeedList.Server);
        }

        public static void OpenRuleEdit(Form form, ServerInfo si, RuleInfo ri)
        {
            OpenEditModal(form, "WPCConfig.RuleList.Edit", "规则编辑", new RuleEdit(form, si, ri), FeedList.Server);
        }

        public static void OpenNoticeEdit(Form form, NoticeInfo ni)
        {
            OpenEditModal(form, "WPCConfig.NoticeList.Edit", "公告编辑", new NoticeEdit(form, ni), FeedList.Notice);
        }

        #endregion
    }

    #endregion
}
