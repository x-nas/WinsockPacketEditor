using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinsockPacketEditor
{
    #region//模型的通知基类

    /// <summary>
    /// 23 个数据模型的基类。<b>与 AntdUI.NotifyProperty 的 API 逐字相同</b>
    /// （同名类型、同样的 protected OnPropertyChanged([CallerMemberName])），
    /// 所以那 23 个文件只需要删掉 <c>using AntdUI;</c>，其余一个字都不用改。
    ///
    /// 【为什么要自己写一个】
    /// 原来它来自 AntdUI，于是<b>每一个数据模型都拖着一个 UI 框架</b>。两个后果：
    ///
    ///   ① <b>注入模式的目标进程里被迫加载 AntdUI</b>。实测（WPEHookTest --footprint）
    ///      无头核心注入之后，WPE 带进目标的托管程序集<b>只剩 AntdUI 这一个</b> ——
    ///      Newtonsoft / EF6 / SQLite / OWIN / SuperSocket / QQWry / SunnyNet 全都没加载。
    ///      也就是说风险清单 R2（DLL 与运行时冲突）到最后只剩这一项，
    ///      换掉这个基类就归零了。
    ///
    ///   ② <b>外壳侧撞了 16 次 CS0012</b>。外壳只 ProjectReference 主工程，
    ///      而这些模型一出现在外壳能看到的签名上就是「类型在未引用的程序集中定义」，
    ///      于是每做一屏都要在 Operate 侧另开一组「按 Id 收发、只出基础类型」的入口。
    ///      根子就在这个基类上。
    ///
    /// 【为什么这么换是安全的】AntdUI 里对 NotifyProperty 做类型判断的只有它<b>自己的</b>
    /// 集合类型（iCollection`1.PropertyChanged / BaseCollection.PropertyChanged，
    /// IL 扫过全部 11493 个方法体确认），而 WPE 全项目不使用那些集合 ——
    /// 表格走的是 <c>Table.Binding(BindingList&lt;T&gt;)</c> 这一路，
    /// 刷新靠的是 BindingList 的 ListChanged，不是每个元素的 PropertyChanged。
    ///
    /// 【这是方案书阶段 3 的替代做法】方案原本要新建 WPECore 类库、
    /// 把四个子模块从三万行的 Operate 里拆出去（约 370 处跨模块引用），
    /// 目的同样是缩小目标足迹。实测之后确认两者收益相同，而这一个的回归面小得多。
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变了。<b>不传参数</b> —— 靠 <see cref="CallerMemberNameAttribute"/> 自动填属性名，
        /// 23 个模型里全部是这么调的（全项目 <c>OnPropertyChanged()</c> 无一处带参）。
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }

    #endregion
}
