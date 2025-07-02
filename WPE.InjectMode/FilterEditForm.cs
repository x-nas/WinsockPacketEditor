using AntdUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class FilterEditForm : Form
    {
        private BindingList<AntdUI.AntItem[]> data = new BindingList<AntdUI.AntItem[]>();

        #region//初始化

        public FilterEditForm()
        {
            InitializeComponent();
        }        

        private void FilterEditForm_Load(object sender, EventArgs e)
        {
            this.tabFilterEdit.TabMenuVisible = false;
            this.tabFilterFrom.TabMenuVisible = false;

            this.InitTable_FilterNormal();
        }

        private void InitTable_FilterNormal()
        {
            int rowCount = 2;
            for (int row = 0; row < rowCount; row++)
            {
                AntdUI.AntItem[] items = new AntItem[Operate.FilterConfig.Filter.FilterSize_MaxLen];
                var columns = new AntdUI.ColumnCollection();

                for (int i = 0; i < Operate.FilterConfig.Filter.FilterSize_MaxLen; i++)
                {
                    string Title = (i + 1).ToString("D3");
                    string Key = "col" + Title;

                    items[i] = new AntdUI.AntItem(Key, string.Empty);

                    AntdUI.Column column = new AntdUI.Column(Key, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                    columns.Add(column);
                }

                tFilterNormal.Columns = columns;
                data.Add(items);
            }

            tFilterNormal.Binding(data);
        }

        private void tFilterNormal_CellBeginEditInputStyle(object sender, TableBeginEditInputStyleEventArgs e)
        {
            e.Input.MaxLength = 2;

            e.Input.VerifyChar += (inputSender, verifyArgs) =>
            {
                char c = verifyArgs.Char;
                if (c == '\b') { verifyArgs.Result = true; return; }

                if (char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
                {
                    if (c >= 'a' && c <= 'f')
                        verifyArgs.ReplaceText = c.ToString().ToUpper();
                    verifyArgs.Result = true;
                }
                else
                {
                    verifyArgs.Result = false;
                }
            };
        }

        private bool tFilterNormal_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Value, "^[0-9A-F]{2}$"))
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "请输入有效的十六进制数值", TType.Error)
                {
                    LocalizationText = "InvalidHex"
                });

                return false;
            }

            return true;
        }

        private Table.CellStyleInfo tFilterNormal_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            return new AntdUI.Table.CellStyleInfo
            {
                ForeColor = Color.RoyalBlue,
                BackColor = Color.LightYellow,
            };
        }

        #endregion

        #region//模式切换

        private void rbFilterMode_Normal_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterModeChange();
        }

        private void FilterModeChange()
        {
            if (rbFilterMode_Normal.Checked)
            {
                this.tabFilterEdit.SelectTab("tpNormal");
            }
            else if (rbFilterMode_Advanced.Checked)
            {
                this.tabFilterEdit.SelectTab("tpAdvance");
            }
        }

        #endregion

        #region//修改起始于切换

        private void rbFilterModifyFrom_Head_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterModifyFromChange();
        }

        private void FilterModifyFromChange()
        {
            try
            {
                if (rbFilterModifyFrom_Head.Checked)
                {
                    
                }
                else if (rbFilterModifyFrom_Position.Checked)
                {
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        
    }
}
