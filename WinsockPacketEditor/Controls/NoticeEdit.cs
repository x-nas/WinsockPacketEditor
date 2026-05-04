using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class NoticeEdit : UserControl
    {
        private NoticeInfo niSelect;
        private Form form;

        #region//窗体事件

        public NoticeEdit(Form form, NoticeInfo ni)
        {
            this.form = form;
            this.niSelect = ni;
            InitializeComponent();
        }

        private void NoticeEdit_Load(object sender, System.EventArgs e)
        {
            this.InitNoticeType();

            if (this.niSelect == null)
            {
                this.sNoticeType.SelectedIndex = 0;
            }
            else
            {
                this.sNoticeType.SelectedValue = this.niSelect.NoticeType.ToString();
                this.txtNoticeTitle.Text = this.niSelect.NoticeTitle;
                this.txtNoticeContent.Text = this.niSelect.NoticeContent;
                this.txtNoticeMore.Text = this.niSelect.NoticeMore;
            }
        }        

        private void InitNoticeType()
        {
            this.sNoticeType.Items.Clear();

            this.sNoticeType.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("活动情报") 
                {
                    LocalizationText = "WPCConfig.NoticeList.NoticeType_1",
                    Tag = "1"
                },
                new AntdUI.SelectItem("维护说明")
                {
                    LocalizationText = "WPCConfig.NoticeList.NoticeType_2",
                    Tag = "2"
                },
                new AntdUI.SelectItem("电竞赛事")
                {
                    LocalizationText = "WPCConfig.NoticeList.NoticeType_3",
                    Tag = "3"
                },
                new AntdUI.SelectItem("限时商城")
                {
                    LocalizationText = "WPCConfig.NoticeList.NoticeType_4",
                    Tag = "4"
                },
                new AntdUI.SelectItem("玩家社区")
                {
                    LocalizationText = "WPCConfig.NoticeList.NoticeType_5",
                    Tag = "5"
                }
            });
        }

        private void txtNoticeTitle_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoticeTitle.Text.Trim()))
            {
                this.txtNoticeTitle.Status = TType.Error;
            }
            else
            {
                this.txtNoticeTitle.Status = TType.Success;
            }
        }

        private void txtNoticeContent_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNoticeContent.Text.Trim()))
            {
                this.txtNoticeContent.Status = TType.Error;
            }
            else
            {
                this.txtNoticeContent.Status = TType.Success;
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtNoticeTitle.Text.Trim()))
                {
                    this.txtNoticeTitle.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "公告标题为空", TType.Error)
                    {
                        LocalizationText = "NoticeEdit.NoticeTitle.Empty"
                    });

                    return;
                }

                if (string.IsNullOrEmpty(this.txtNoticeContent.Text.Trim()))
                {
                    this.txtNoticeContent.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "公告内容为空", TType.Error)
                    {
                        LocalizationText = "NoticeEdit.NoticeContent.Empty"
                    });

                    return;
                }

                int NoticeType = Convert.ToInt32(this.sNoticeType.SelectedValue);
                string NoticeTitle = this.txtNoticeTitle.Text.Trim();
                string NoticeContent = this.txtNoticeContent.Text.Trim();
                string NoticeMore = this.txtNoticeMore.Text.Trim();

                if (this.niSelect == null)
                {
                    Operate.WPCConfig.NoticeList.AddNotice(Guid.NewGuid(), NoticeType, NoticeTitle, NoticeContent, NoticeMore, DateTime.Now);
                }
                else
                {
                    Operate.WPCConfig.NoticeList.UpdateNotice_ByNoticeID(this.niSelect.NID, NoticeType, NoticeTitle, NoticeContent, NoticeMore, DateTime.Now);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "公告信息保存成功", TType.Success)
                {
                    LocalizationText = "NoticeEdit.Success"
                });

                if (this.form is InterfaceInfo.IProxyMode pmForm)
                {
                    pmForm.RefreshSendList();
                }

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
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
