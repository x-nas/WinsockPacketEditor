using EasyHook;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Socket_RobotForm : Form
    {
        private int RobotIndex = -1;
        DataTable dtRobotInstruction = new DataTable();

        #region//窗体加载

        public Socket_RobotForm(int RIndex)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();

                this.RobotIndex = RIndex;

                if (this.RobotIndex > -1)
                {
                    this.InitFrom();
                    this.InitDGV();
                }
                else
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_28));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//初始化

        private void InitFrom()
        {
            try
            {
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();
                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_93), (RobotIndex + 1), sInjectProcesName, iInjectProcessID);

                this.txtRobotName.Text = Socket_Cache.RobotList.lstRobot[this.RobotIndex].RName;
                this.dtRobotInstruction = Socket_Cache.RobotList.lstRobot[RobotIndex].RInstruction.Copy();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV()
        {
            try
            {
                dgvSendList.AutoGenerateColumns = false;
                dgvSendList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSendList, true, null);
                dgvSendList.DataSource = Socket_Cache.SendList.dtSendList;

                dgvRobotInstruction.AutoGenerateColumns = false;
                dgvRobotInstruction.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvRobotInstruction, true, null);
                dgvRobotInstruction.DataSource = this.dtRobotInstruction;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvRobotInstruction_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvRobotInstruction.Columns["cRobotInstruction_Type"].Index)
                {
                    Socket_Cache.Robot.InstructionType instructionType = (Socket_Cache.Robot.InstructionType)dgvRobotInstruction.Rows[e.RowIndex].Cells["cRobotInstruction_Type"].Value;
                    e.Value = Socket_Cache.Robot.GetName_ByInstructionType(instructionType);
                    e.CellStyle.ForeColor = Socket_Cache.Robot.GetColor_ByInstructionType(instructionType);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvRobotInstruction.Columns["cRobotInstruction_ID"].Index)
                {
                    e.Value = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_98), e.RowIndex + 1);
                    e.CellStyle.ForeColor = Color.RoyalBlue;
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvRobotInstruction.Columns["cRobotInstruction_Content"].Index)
                {
                    string sContent = dgvRobotInstruction.Rows[e.RowIndex].Cells["cRobotInstruction_Content"].Value.ToString();
                    Socket_Cache.Robot.InstructionType instructionType = (Socket_Cache.Robot.InstructionType)dgvRobotInstruction.Rows[e.RowIndex].Cells["cRobotInstruction_Type"].Value;
                    e.Value = Socket_Cache.Robot.GetContentString_ByInstructionType(instructionType, sContent);                    
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSendList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSendList.Columns["cSendList_ID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存按钮

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                string RName_New = this.txtRobotName.Text.Trim();

                if (string.IsNullOrEmpty(RName_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    return;
                }

                Socket_Cache.Robot.UpdateRobot_ByRobotIndex(this.RobotIndex, RName_New, this.dtRobotInstruction);

                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//关闭按钮

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region//执行按钮

        private void bExecute_Click(object sender, EventArgs e)
        {
            try
            {
                bool bCheck = Socket_Cache.Robot.CheckRobotInstruction(this.dtRobotInstruction);

                if (bCheck) 
                { 
                    //
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//指令集的列表操作

        public int UpdateInstruction_ByListAction(Socket_Cache.ListAction listAction, int InstructionIndex)
        {
            int iReturn = -1;

            try
            {                
                int iInstructionCount = this.dtRobotInstruction.Rows.Count;
                DataRow dr = this.dtRobotInstruction.NewRow();
                dr.ItemArray = this.dtRobotInstruction.Rows[InstructionIndex].ItemArray;

                switch (listAction)
                {
                    case Socket_Cache.ListAction.Top:

                        if (InstructionIndex > 0)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, 0);
                            iReturn = 0;
                        }

                        break;

                    case Socket_Cache.ListAction.Up:

                        if (InstructionIndex > 0)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, InstructionIndex - 1);
                            iReturn = InstructionIndex - 1;
                        }

                        break;

                    case Socket_Cache.ListAction.Down:

                        if (InstructionIndex < iInstructionCount - 1)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, InstructionIndex + 1);
                            iReturn = InstructionIndex + 1;
                        }

                        break;

                    case Socket_Cache.ListAction.Bottom:

                        if (InstructionIndex < iInstructionCount - 1)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.Add(dr);
                            iReturn = this.dtRobotInstruction.Rows.Count - 1;
                        }

                        break;

                    case Socket_Cache.ListAction.Delete:

                        this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);                        

                        break;

                    case Socket_Cache.ListAction.CleanUp:

                        this.dtRobotInstruction.Clear();

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return iReturn;
        }

        #endregion

        #region//指令集右键菜单

        private void cmsRobotInstruction_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsRobotInstruction.Close();

            try
            {
                if (dgvRobotInstruction.Rows.Count > 0)
                {
                    int iIndex = -1;
                    int iInstructionIndex = this.dgvRobotInstruction.CurrentRow.Index;

                    if (iInstructionIndex > -1)
                    {
                        switch (sItemText)
                        {
                            case "cmsRobotInstruction_Top":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.Top, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Up":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.Up, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Down":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.Down, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Bottom":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.Bottom, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Delete":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.Delete, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_CleanUp":
                                iIndex = this.UpdateInstruction_ByListAction(Socket_Cache.ListAction.CleanUp, iInstructionIndex);
                                break;
                        }

                        if (iIndex > -1 && iIndex < dgvRobotInstruction.RowCount)
                        {                            
                            this.dgvRobotInstruction.ClearSelection();
                            this.dgvRobotInstruction.Rows[iIndex].Selected = true;
                            this.dgvRobotInstruction.CurrentCell = this.dgvRobotInstruction.Rows[iIndex].Cells[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//添加指令

        public void AddInstruction(Socket_Cache.Robot.InstructionType instructionType, string sContent)
        {
            try
            {
                DataRow dr = this.dtRobotInstruction.NewRow();
                dr[0] = instructionType;
                dr[1] = sContent;
                
                if (this.dgvRobotInstruction.CurrentCell != null)
                {
                    int iIndex = this.dgvRobotInstruction.CurrentCell.RowIndex + 1;
                    this.dtRobotInstruction.Rows.InsertAt(dr, iIndex);
                    this.dgvRobotInstruction.CurrentCell = dgvRobotInstruction.Rows[iIndex].Cells[0];
                }
                else
                {
                    this.dtRobotInstruction.Rows.Add(dr);                    
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送指令

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                int SendPacket_Index = ((int)this.nudSendIndex.Value);
                int SendPacket_Times = ((int)this.nudSendTimes.Value);
                int SendPacket_Interval = ((int)this.nudSendInterval.Value);

                string sContent = SendPacket_Index + "|" + SendPacket_Times + "|" + SendPacket_Interval;
                this.AddInstruction(Socket_Cache.Robot.InstructionType.Send, sContent);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           
        }

        #endregion

        #region//延迟指令

        private void bDelay_Click(object sender, EventArgs e)
        {
            try
            {
                int Delay = ((int)this.nudDelay.Value);

                string sContent = Delay.ToString();
                this.AddInstruction(Socket_Cache.Robot.InstructionType.Delay, sContent);                               
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//循环指令

        private void bLoopStart_Click(object sender, EventArgs e)
        {
            try
            {
                int Loop = ((int)this.nudLoop.Value);

                string sContent = Loop.ToString();
                this.AddInstruction(Socket_Cache.Robot.InstructionType.LoopStart, sContent);                     
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bLoopEnd_Click(object sender, EventArgs e)
        {
            try
            {
                this.AddInstruction(Socket_Cache.Robot.InstructionType.LoopEnd, string.Empty);                    
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        
    }
}
