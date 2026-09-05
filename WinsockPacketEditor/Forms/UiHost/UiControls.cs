using AntdUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DiffPlex.DiffBuilder.Model;

namespace WinsockPacketEditor
{
    #region//控件操作

    /// <summary>
    /// 直接操作 AntdUI 控件的辅助方法（B7 批次从 Operate.cs 搬出）。
    ///
    /// 【为什么搬】
    /// 这些方法的参数直接是 AntdUI.Select / Tree / Input / Checkbox，或者直接 new 出悬浮按钮，
    /// 内容全是「把数据塞进控件」「读控件状态」「给文本上色」，没有任何业务逻辑，
    /// 却让 Operate 依赖 AntdUI 的控件类型。搬出来之后 Operate 只剩纯数据。
    ///
    /// 【搬迁原则】逐字搬运，只改缩进与必要的限定名，不改任何逻辑。
    /// 本类属于 UI 层，允许自由使用 AntdUI 与 System.Drawing。
    /// </summary>
    public static class UiControls
    {
        /// <summary>悬浮按钮实例（原 FloatButton）。</summary>
        public static AntdUI.FormFloatButton FloatButton = null;

        public static void InitFloatButton(Form form)
        {
            if (Operate.SystemConfig.IsShow_FloatButton)
            {
                if (FloatButton == null)
                {
                    FloatButton = AntdUI.FloatButton.open(
                        new AntdUI.FloatButton.Config(form,
                        new AntdUI.FloatButton.ConfigBtn[]
                        {
                            new AntdUI.FloatButton.ConfigBtn("GitHub", "QuestionOutlined", true)
                            {
                                Tooltip = "问题反馈",
                                LocalizationTooltip = "Feedback",
                                Type= AntdUI.TTypeMini.Success
                            },
                            new AntdUI.FloatButton.ConfigBtn("WebSite", "HomeOutlined", true)
                            {
                                Tooltip = "访问官网",
                                LocalizationTooltip = "OfficialWebsite",
                                Type= AntdUI.TTypeMini.Default
                            }
                        }, btn =>
                        {
                            btn.Loading = true;

                            AntdUI.ITask.Run(() =>
                            {
                                switch (btn.Name)
                                {
                                    case "GitHub":
                                        Process.Start(Operate.SystemConfig.WPE64_Issuse);
                                        break;

                                    case "WebSite":
                                        Process.Start(Operate.SystemConfig.WPE64_URL);
                                        break;
                                }

                                btn.Loading = false;
                            });
                        }));
                }
                else
                {
                    FloatButton.Show();
                }
            }
            else
            {
                if (FloatButton != null)
                {
                    FloatButton.Close();
                    FloatButton = null;
                }
            }
        }

        public static void InitSendInfo(AntdUI.Select sSendInfo, Guid SelectSID)
        {
            try
            {
                if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                {
                    var selectItems = Operate.SendConfig.List.lstSendInfo.Select(info => new SelectItem(info.SName, info)).ToArray();

                    sSendInfo.Items.Clear();
                    sSendInfo.Items.AddRange(selectItems);
                    sSendInfo.SelectedValue = Operate.SendConfig.Send.GetSend_ByGuid(SelectSID);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitSendInfo), ex);
            }
        }

        public static void InitRobotInfo(AntdUI.Select sRobotInfo, Guid SelectRID)
        {
            try
            {
                if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                {
                    var selectItems = Operate.RobotConfig.List.lstRobotInfo.Select(info => new SelectItem(info.RName, info)).ToArray();

                    sRobotInfo.Items.Clear();
                    sRobotInfo.Items.AddRange(selectItems);
                    sRobotInfo.SelectedValue = Operate.RobotConfig.Robot.GetRobot_ByGuid(SelectRID);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitRobotInfo), ex);
            }
        }

        public static void InitFilterInfo(AntdUI.Select sFilterInfo, Guid SelectFID, Guid ExcludeFID)
        {
            try
            {
                if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                {
                    var query = Operate.FilterConfig.List.lstFilterInfo.AsEnumerable();
                    query = query.Where(info => info.FID != ExcludeFID);

                    var selectItems = query
                        .Select(info => new SelectItem(info.FName, info))
                        .ToArray();

                    sFilterInfo.Items.Clear();
                    sFilterInfo.Items.AddRange(selectItems);
                    sFilterInfo.SelectedValue = Operate.FilterConfig.Filter.GetFilter_ByGuid(SelectFID);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitFilterInfo), ex);
            }
        }

        public static void InitWareHouseInfo(AntdUI.Select sSendInfo, Guid SelectWID)
        {
            try
            {
                if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count > 0)
                {
                    var selectItems = Operate.WareHouseConfig.List.lstWareHouseInfo.Select(info => new SelectItem(info.WName, info)).ToArray();

                    sSendInfo.Items.Clear();
                    sSendInfo.Items.AddRange(selectItems);
                    sSendInfo.SelectedValue = Operate.WareHouseConfig.WareHouse.GetWareHouse_ByGuid(SelectWID);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitWareHouseInfo), ex);
            }
        }

        public static TreeItem FindNodeByName(AntdUI.Tree tree, string NodeName, string SubTitle)
        {
            try
            {
                return FindNodeByName(tree.Items, NodeName, SubTitle);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(FindNodeByName), ex);
            }

            return null;
        }

        public static TreeItem FindNodeByName(TreeItemCollection items, string NodeName, string SubTitle)
        {
            try
            {
                if (items == null || items.Count == 0)
                {
                    return null;
                } 

                foreach (var item in items)
                {
                    if (item.Name == NodeName || item.Text == NodeName)
                    {
                        if (string.IsNullOrEmpty(SubTitle))
                        {
                            return item;
                        }
                        else
                        {
                            if (item.SubTitle.Equals(SubTitle))
                            {
                                return item;
                            }
                        }                                                        
                    }

                    var found = FindNodeByName(item.Sub, NodeName, SubTitle);
                    if (found != null)
                    {
                        return found;
                    } 
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(FindNodeByName), ex);
            }                

            return null;
        }

        public static List<Operate.SystemConfig.DifferenceItem> CompareText(AntdUI.Input box1, AntdUI.Input box2)
        {
            var differences = new List<Operate.SystemConfig.DifferenceItem>();

            try
            {
                string text1 = box1.Text;
                string text2 = box2.Text;
                int maxLength = Math.Max(text1.Length, text2.Length);

                for (int i = 0; i < maxLength; i++)
                {
                    ChangeType changeType = GetCharDiffType(text1, text2, i);

                    // 记录差异项
                    if (changeType != ChangeType.Unchanged)
                    {
                        differences.Add(new Operate.SystemConfig.DifferenceItem
                        {
                            Position = i + 1,
                            ValueA = i < text1.Length ? text1[i].ToString() : "N/A",
                            ValueB = i < text2.Length ? text2[i].ToString() : "N/A",
                            ChangeType = changeType
                        });
                    }

                    // 处理第一个文本框(input2) - 原始文本
                    if (i < text1.Length)
                    {
                        if (changeType == ChangeType.Deleted || changeType == ChangeType.Modified)
                        {
                            box1.SetStyle(i, 1,
                                        font: null,
                                        fore: Color.White,
                                        back: Color.FromArgb(220, 80, 80)); // 红色背景表示删除/修改
                        }
                    }

                    // 处理第二个文本框(input3) - 新文本
                    if (i < text2.Length)
                    {
                        if (changeType == ChangeType.Inserted || changeType == ChangeType.Modified)
                        {
                            box2.SetStyle(i, 1,
                                        font: null,
                                        fore: Color.White,
                                        back: Color.FromArgb(80, 180, 80)); // 绿色背景表示新增/修改
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CompareText), ex);
            }

            return differences;
        }

        private static ChangeType GetCharDiffType(string str1, string str2, int position)
        {
            if (position >= str1.Length) return ChangeType.Inserted;
            if (position >= str2.Length) return ChangeType.Deleted;
            return str1[position] == str2[position] ? ChangeType.Unchanged : ChangeType.Modified;
        }

        public static void FindRegexMatches(string pattern, AntdUI.Input textBoxA, AntdUI.Input textBoxB)
        {
            try
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    return;
                }

                textBoxA.ClearStyle();
                textBoxB.ClearStyle();

                foreach (Match match in Regex.Matches(textBoxA.Text, pattern))
                {
                    textBoxA.SetStyle(match.Index, match.Length, font: null, fore: Color.White, back: Color.DarkSeaGreen);
                }

                foreach (Match match in Regex.Matches(textBoxB.Text, pattern))
                {
                    textBoxB.SetStyle(match.Index, match.Length, font: null, fore: Color.White, back: Color.DarkSeaGreen);
                }
            }
            catch
            {
                //
            }
        }

        public static void LeachRegexMatches(string pattern, AntdUI.Input textBoxA, AntdUI.Input textBoxB)
        {
            try
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    return;
                }

                textBoxA.ClearStyle();
                textBoxB.ClearStyle();

                StringBuilder sbA = new StringBuilder();
                StringBuilder sbB = new StringBuilder();

                foreach (Match match in Regex.Matches(textBoxA.Text, pattern))
                {
                    sbA.Append(match.Value);
                }
                textBoxA.Text = sbA.ToString();

                foreach (Match match in Regex.Matches(textBoxB.Text, pattern))
                {
                    sbB.Append(match.Value);
                }
                textBoxB.Text = sbB.ToString();
            }
            catch
            {
                //
            }
        }

        public static (Operate.FilterConfig.Filter.FilterExecuteType feType, Guid gGuid) GetFilterExecuteType(AntdUI.Checkbox cbFilterExecute, AntdUI.Select sFilterExecuteType, AntdUI.Select sFilterExecuteInfo)
        {
            Operate.FilterConfig.Filter.FilterExecuteType feType = Operate.FilterConfig.Filter.FilterExecuteType.None;
            Guid gGuid = Guid.Empty;

            if (cbFilterExecute.Checked)
            {
                if (sFilterExecuteType.SelectedIndex == 0)
                {
                    feType = Operate.FilterConfig.Filter.FilterExecuteType.Send;

                    if (sFilterExecuteInfo.SelectedValue != null)
                    {
                        gGuid = ((SendInfo)sFilterExecuteInfo.SelectedValue).SID;
                    }
                }
                else if (sFilterExecuteType.SelectedIndex == 1)
                {
                    feType = Operate.FilterConfig.Filter.FilterExecuteType.Robot;

                    if (sFilterExecuteInfo.SelectedValue != null)
                    {
                        gGuid = ((RobotInfo)sFilterExecuteInfo.SelectedValue).RID;
                    }
                }
                else if (sFilterExecuteType.SelectedIndex == 2)
                {
                    feType = Operate.FilterConfig.Filter.FilterExecuteType.Filter;

                    if (sFilterExecuteInfo.SelectedValue != null)
                    {
                        gGuid = ((FilterInfo)sFilterExecuteInfo.SelectedValue).FID;
                    }
                }
                else if (sFilterExecuteType.SelectedIndex == 3)
                {
                    feType = Operate.FilterConfig.Filter.FilterExecuteType.WareHouse;

                    if (sFilterExecuteInfo.SelectedValue != null)
                    {
                        gGuid = ((WareHouseInfo)sFilterExecuteInfo.SelectedValue).WID;
                    }
                }
            }

            return (feType, gGuid);
        }

        /// <summary>十六进制输入校验（B8 搬出：参数是 AntdUI 的输入事件，属 UI 层）。</summary>
        public static void VerifyHexCharWithWildcard(InputVerifyCharEventArgs verifyArgs, bool allowWildcard)
        {
            try
            {
                char c = verifyArgs.Char;
                if (c == '\b') // 退格键
                {
                    verifyArgs.Result = true;
                    return;
                }

                // 根据参数决定是否允许通配符 *
                if (allowWildcard && c == '*')
                {
                    verifyArgs.Result = true;
                    return;
                }

                if (char.IsDigit(c))
                {
                    verifyArgs.Result = true;
                }
                else if (c >= 'A' && c <= 'F')
                {
                    verifyArgs.Result = true;
                }
                else if (c >= 'a' && c <= 'f')
                {
                    verifyArgs.ReplaceText = c.ToString().ToUpper();
                    verifyArgs.Result = true;
                }
                else
                {
                    verifyArgs.Result = false;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(VerifyHexCharWithWildcard), ex);
            }
        }

    }

    #endregion
}
