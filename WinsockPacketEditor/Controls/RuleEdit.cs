using AntdUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RuleEdit : UserControl
    {
        private ServerInfo siSelect;
        private RuleInfo riSelect;
        private Form form;

        #region//窗体事件

        public RuleEdit(Form form, ServerInfo si, RuleInfo ri)
        {
            InitializeComponent();

            this.form = form;
            this.siSelect = si;
            this.riSelect = ri;
        }

        private void RuleEdit_Load(object sender, System.EventArgs e)
        {
            this.InitRuleType();
            this.InitRuleAction();

            if (this.riSelect == null)
            {
                this.cbIsEnable.Checked = true;
            }
            else
            { 
                this.cbIsEnable.Checked = this.riSelect.IsEnable;
                this.ddlRuleType.SelectedValue = this.riSelect.RType;
                this.txtRuleArgument.Text = this.riSelect.RArgument;
                this.ddlRuleAction.SelectedValue = this.riSelect.RAction;
            }

            this.IsEnable_Changed();
        }

        private void InitRuleType()
        {
            Array enumValues = Enum.GetValues(typeof(RuleType));

            this.ddlRuleType.Items.Clear();

            foreach (RuleType ruleType in enumValues)
            {
                string description = Operate.WPCConfig.ServerList.GetRuleTypeDescription(ruleType);
                this.ddlRuleType.Items.Add(new AntdUI.SelectItem(description, ruleType));
            }

            if (enumValues.Length > 0)
            {
                this.ddlRuleType.SelectedValue = enumValues.GetValue(0);
            }
        }

        private void InitRuleAction()
        {
            Array enumValues = Enum.GetValues(typeof(RuleAction));

            object[] enumArray = new object[enumValues.Length];
            enumValues.CopyTo(enumArray, 0);

            this.ddlRuleAction.Items.Clear();
            this.ddlRuleAction.Items.AddRange(enumArray);
            this.ddlRuleAction.SelectedValue = enumArray[0];
        }

        #endregion

        #region//启用

        private void cbIsEnable_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.IsEnable_Changed();
        }

        private void IsEnable_Changed()
        {
            this.tlpRuleInfo.Enabled = this.cbIsEnable.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                bool IsEnable = this.cbIsEnable.Checked;
                string ruleArgument = this.txtRuleArgument.Text.Trim();

                RuleType RType = RuleType.DOMAIN;
                if (this.ddlRuleType.SelectedValue is RuleType selectedRule)
                {
                    RType = selectedRule;
                }

                RuleAction RAction = RuleAction.DIRECT;
                if (this.ddlRuleAction.SelectedValue is RuleAction selectedAction)
                {
                    RAction = selectedAction;
                }

                if (this.riSelect == null)
                {
                    if (string.IsNullOrEmpty(ruleArgument))
                    {
                        Operate.WPCConfig.ServerList.AddRule(this.siSelect.SID, new RuleInfo(IsEnable, Guid.NewGuid(), RType, ruleArgument, RAction));
                    }
                    else
                    {
                        var arguments = ruleArgument.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(arg => arg.Trim())
                            .Where(arg => !string.IsNullOrEmpty(arg))
                            .ToList();

                        foreach (string argument in arguments)
                        {
                            Operate.WPCConfig.ServerList.AddRule(this.siSelect.SID, new RuleInfo(IsEnable, Guid.NewGuid(), RType, argument, RAction));
                        }
                    }
                }
                else
                {
                    string updateArgument = ruleArgument;

                    if (!string.IsNullOrEmpty(ruleArgument))
                    {
                        var firstArgument = ruleArgument.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(arg => arg.Trim())
                            .FirstOrDefault(arg => !string.IsNullOrEmpty(arg));

                        if (firstArgument != null)
                        {
                            updateArgument = firstArgument;
                        }
                    }

                    Operate.WPCConfig.ServerList.UpdateRule_ByRuleID(
                        this.siSelect.SID,
                        this.riSelect.RID,
                        IsEnable,
                        RType,
                        updateArgument,
                        RAction
                    );
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "规则信息保存成功", TType.Success)
                {
                    LocalizationText = "WPCConfig.RuleList.Save.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "规则信息保存失败", TType.Error)
                {
                    LocalizationText = "WPCConfig.RuleList.Error"
                });
            }
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}
