﻿using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Socket_RobotForm : Form
    {
        private Socket_RobotInfo sriSelect;
        private bool bIsModifierKeys = true;
        private DataTable dtRobotInstruction = new DataTable();
        private readonly Socket_Robot sr = new Socket_Robot();        

        #region//窗体加载

        public Socket_RobotForm(Socket_RobotInfo sri)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();

                if (sri != null)
                { 
                    this.sriSelect = sri;

                    this.InitFrom();
                    this.InitDGV();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//初始化

        private void InitFrom()
        {
            try
            {
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_93), sriSelect.RName);

                this.txtRobotName.Text = sriSelect.RName;
                this.dtRobotInstruction = sriSelect.RInstruction.Copy();

                this.cbbKeyBoard_KeyType.SelectedIndex = 0;
                this.cbbMouse.SelectedIndex = 0;
                this.cbbMouseWheel_Direction.SelectedIndex = 0;
                
                this.InitComboBox();
                this.InitRobot();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitRobot()
        {
            try
            {
                this.sr.Worker.ProgressChanged -= this.Worker_ProgressChanged;
                this.sr.Worker.ProgressChanged += this.Worker_ProgressChanged;

                this.sr.Worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
                this.sr.Worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void InitDGV()
        {
            try
            {
                dgvRobotInstruction.AutoGenerateColumns = false;
                dgvRobotInstruction.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvRobotInstruction, true, null);
                dgvRobotInstruction.DataSource = this.dtRobotInstruction;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitComboBox()
        {
            try
            {
                Socket_Operation.InitSendListComboBox(this.cbbSend_SendLIst);

                if (this.cbbSend_SendLIst.Items.Count > 0)
                { 
                    this.cbbSend_SendLIst.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvRobotInstruction_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvRobotInstruction.Columns["cRobotInstruction_Type"].Index)
                {
                    Operate.RobotConfig.Robot.InstructionType instructionType = (Operate.RobotConfig.Robot.InstructionType)dgvRobotInstruction.Rows[e.RowIndex].Cells["cRobotInstruction_Type"].Value;
                    e.Value = Operate.RobotConfig.Robot.GetName_ByInstructionType(instructionType);
                    e.CellStyle.ForeColor = Operate.RobotConfig.Robot.GetColor_ByInstructionType(instructionType);
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
                    Operate.RobotConfig.Robot.InstructionType instructionType = (Operate.RobotConfig.Robot.InstructionType)dgvRobotInstruction.Rows[e.RowIndex].Cells["cRobotInstruction_Type"].Value;
                    e.Value = Operate.RobotConfig.Robot.GetContentString_ByInstructionType(instructionType, sContent);                    
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                if (this.dtRobotInstruction.Rows.Count > 0)
                {
                    int iReturn = Operate.RobotConfig.Robot.CheckRobotInstruction(this.dtRobotInstruction, false);

                    if (iReturn > -1 && iReturn < dgvRobotInstruction.Rows.Count)
                    {
                        this.dgvRobotInstruction.CurrentCell = dgvRobotInstruction.Rows[iReturn].Cells[0];
                        this.dgvRobotInstruction.FirstDisplayedScrollingRowIndex = iReturn;

                        return;
                    }
                }

                Operate.RobotConfig.Robot.UpdateRobot(sriSelect, RName_New, this.dtRobotInstruction);

                this.Close();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                if (this.dtRobotInstruction.Rows.Count > 0)
                {
                    int iReturn = Operate.RobotConfig.Robot.CheckRobotInstruction(this.dtRobotInstruction, false);

                    if (iReturn > -1 && iReturn < dgvRobotInstruction.Rows.Count)
                    {
                        this.dgvRobotInstruction.CurrentCell = dgvRobotInstruction.Rows[iReturn].Cells[0];
                        this.dgvRobotInstruction.FirstDisplayedScrollingRowIndex = iReturn;

                        return;
                    }

                    if (!this.sr.Worker.IsBusy)
                    {
                        this.txtExecute.Clear();
                        this.bExecute.Enabled = false;
                        this.bStop.Enabled = true;
                        this.tcRobotInstruction.Enabled = false;

                        if (this.dgvRobotInstruction.ContextMenuStrip != null)
                        {
                            this.dgvRobotInstruction.ContextMenuStrip.Enabled = false;
                        }

                        sr.StartRobot(sriSelect.RName, this.dtRobotInstruction, null);
                    }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_110), sriSelect.RName);
                    Socket_Operation.ShowMessageBox(sMsg);
                }
                else if (e.Error != null)
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_111), sriSelect.RName, e.Error.Message);
                    Socket_Operation.ShowMessageBox(sMsg);
                }
                else
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_112), sriSelect.RName);
                    Socket_Operation.ShowMessageBox(sMsg);
                }                

                this.bExecute.Enabled = true;
                this.bStop.Enabled = false;
                this.tcRobotInstruction.Enabled = true;

                if (this.dgvRobotInstruction.ContextMenuStrip != null)
                {
                    this.dgvRobotInstruction.ContextMenuStrip.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            try
            {
                int iIndex = e.ProgressPercentage;                

                if (iIndex > -1 && iIndex < dgvRobotInstruction.Rows.Count)
                {
                    this.dgvRobotInstruction.CurrentCell = this.dgvRobotInstruction.Rows[iIndex].Cells[0];
                    this.dgvRobotInstruction.FirstDisplayedScrollingRowIndex = iIndex;
                }

                this.txtExecute.AppendText((iIndex + 1).ToString() + ", ");
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//终止按钮

        private void bStop_Click(object sender, EventArgs e)
        {
            this.sr.StopRobot();
        }

        #endregion        

        #region//指令集的列表操作

        public int UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction listAction, int InstructionIndex)
        {
            int iReturn = -1;

            try
            {                
                int iInstructionCount = this.dtRobotInstruction.Rows.Count;
                DataRow dr = this.dtRobotInstruction.NewRow();
                dr.ItemArray = this.dtRobotInstruction.Rows[InstructionIndex].ItemArray;

                switch (listAction)
                {
                    case Operate.SystemConfig.ListAction.Top:

                        if (InstructionIndex > 0)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, 0);
                            iReturn = 0;
                        }

                        break;

                    case Operate.SystemConfig.ListAction.Up:

                        if (InstructionIndex > 0)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, InstructionIndex - 1);
                            iReturn = InstructionIndex - 1;
                        }

                        break;

                    case Operate.SystemConfig.ListAction.Down:

                        if (InstructionIndex < iInstructionCount - 1)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.InsertAt(dr, InstructionIndex + 1);
                            iReturn = InstructionIndex + 1;
                        }

                        break;

                    case Operate.SystemConfig.ListAction.Bottom:

                        if (InstructionIndex < iInstructionCount - 1)
                        {
                            this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);
                            this.dtRobotInstruction.Rows.Add(dr);
                            iReturn = this.dtRobotInstruction.Rows.Count - 1;
                        }

                        break;

                    case Operate.SystemConfig.ListAction.Delete:

                        this.dtRobotInstruction.Rows.RemoveAt(InstructionIndex);                        

                        break;

                    case Operate.SystemConfig.ListAction.CleanUp:

                        this.dtRobotInstruction.Clear();

                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.Top, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Up":
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.Up, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Down":
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.Down, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Bottom":
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.Bottom, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_Delete":
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.Delete, iInstructionIndex);
                                break;

                            case "cmsRobotInstruction_CleanUp":
                                iIndex = this.UpdateInstruction_ByListAction(Operate.SystemConfig.ListAction.CleanUp, iInstructionIndex);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//添加指令

        public void AddInstruction(Operate.RobotConfig.Robot.InstructionType instructionType, string sContent)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包指令 - 发送 - 发送列表

        private void bSend_SendList_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cbbSend_SendLIst.SelectedItem != null)
                {
                    Operate.SendConfig.List.SendListItem item = (Operate.SendConfig.List.SendListItem)this.cbbSend_SendLIst.SelectedItem;              
                    Guid SID = item.SID;
                    string sContent = SID.ToString().ToUpper();

                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SendSendList, sContent);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           
        }

        #endregion

        #region//封包指令 - 发送 - 封包列表

        private void bSend_SocketList_Click(object sender, EventArgs e)
        {
            try
            {
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SendSocketList, string.Empty);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包指令 - 设置 - 系统套接字

        private void bSet_SystemSocket_Click(object sender, EventArgs e)
        {
            try
            {
                string sContent = string.Empty;
                if (this.rbSet_SystemSocket_SocketList.Checked)
                {
                    sContent = "PacketConfig.List";
                }
                else if (this.rbSet_SystemSocket_Filter.Checked)
                {
                    sContent = "FilterSocket";
                }
                else if (this.rbSet_SystemSocket_Customize.Checked)
                {
                    string sSocket = this.nudSet_SystemSocket_Customize.Value.ToString();
                    sContent = "Customize" + "|" + sSocket;
                }

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SetSystemSocket, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包指令 - 延迟

        private void rbDelay_Fix_CheckedChanged(object sender, EventArgs e)
        {
            this.nudDelay_Fix.Enabled = this.rbDelay_Fix.Checked;
        }

        private void rbDelay_Random_CheckedChanged(object sender, EventArgs e)
        {
            this.nudDelay_RandomFrom.Enabled = this.nudDelay_RandomTo.Enabled = this.rbDelay_Random.Checked;
        }

        private void bDelay_Click(object sender, EventArgs e)
        {
            try
            {
                string sContent = string.Empty;

                if (this.rbDelay_Fix.Checked)
                {
                    int Delay = ((int)this.nudDelay_Fix.Value);
                    sContent = Delay.ToString();
                }
                else
                { 
                    int DelayFrom = ((int)this.nudDelay_RandomFrom.Value);
                    int DelayTo = ((int)this.nudDelay_RandomTo.Value);
                    sContent = DelayFrom.ToString() + "-" + DelayTo.ToString();
                }

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Delay, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//封包指令 - 循环

        private void bLoopStart_Click(object sender, EventArgs e)
        {
            try
            {
                int LoopCNT = ((int)this.nudLoop.Value);
                string sContent = LoopCNT.ToString();

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.LoopStart, sContent);                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bLoopEnd_Click(object sender, EventArgs e)
        {
            try
            {
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.LoopEnd, string.Empty);                    
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//键盘指令 - 按键

        private void bKeyBoard_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtKeyBoard_KeyCode.Text.Trim();

                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = new Operate.RobotConfig.Robot.KeyBoardType();

                    switch (this.cbbKeyBoard_KeyType.SelectedIndex)
                    {
                        case 0:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Press;
                            break;

                        case 1:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Down;
                            break;

                        case 2:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Up;
                            break;
                    }

                    string sContent = kbType + "|" + KeyCode;
                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtKeyBoard_Key_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;
                e.Handled = true;

                this.txtKeyBoard_KeyCode.Text = e.KeyCode.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//键盘指令 - 按键组合

        private void bKeyboard_combination_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtKeyboard_combination.Text.Trim();                

                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.KeyBoardType.Combine;
                    string sContent = kbType + "|" + KeyCode;

                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtKeyboard_combination_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;
                e.Handled = true;

                bIsModifierKeys = true;
                string sKeyCode = string.Empty;
                Keys modifiers = Control.ModifierKeys;

                if ((modifiers & Keys.Control) == Keys.Control && (modifiers & Keys.Shift) == Keys.Shift && (modifiers & Keys.Alt) == Keys.Alt)
                {
                    sKeyCode = Keys.ControlKey.ToString() + " + " + Keys.Menu.ToString() + " + " + Keys.ShiftKey.ToString() + " + ";
                }
                else if ((modifiers & Keys.Control) == Keys.Control && (modifiers & Keys.Shift) == Keys.Shift)
                {
                    sKeyCode = Keys.ControlKey.ToString() + " + " + Keys.ShiftKey.ToString() + " + ";
                }
                else if ((modifiers & Keys.Control) == Keys.Control && (modifiers & Keys.Alt) == Keys.Alt)
                {
                    sKeyCode = Keys.ControlKey.ToString() + " + " + Keys.Menu.ToString() + " + ";
                }
                else if ((modifiers & Keys.Shift) == Keys.Shift && (modifiers & Keys.Alt) == Keys.Alt)
                {
                    sKeyCode = Keys.ShiftKey.ToString() + " + " + Keys.Menu.ToString() + " + ";
                }
                else if ((modifiers & Keys.Control) == Keys.Control)
                {
                    sKeyCode = Keys.ControlKey.ToString() + " + ";
                }
                else if ((modifiers & Keys.Shift) == Keys.Shift)
                {
                    sKeyCode = Keys.ShiftKey.ToString() + " + ";
                }
                else if ((modifiers & Keys.Alt) == Keys.Alt)
                {
                    sKeyCode = Keys.Menu.ToString() + " + ";
                }

                if (e.KeyCode != Keys.Control &&
                e.KeyCode != Keys.ControlKey &&
                e.KeyCode != Keys.Shift &&
                e.KeyCode != Keys.ShiftKey &&
                e.KeyCode != Keys.Menu &&
                e.KeyCode != Keys.Alt)
                {
                    sKeyCode += e.KeyCode.ToString();
                    bIsModifierKeys = false;
                }
                
                this.txtKeyboard_combination.Text = sKeyCode;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtKeyboard_combination_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (bIsModifierKeys)
                {
                    this.txtKeyboard_combination.Clear();                    
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//键盘指令 - 文本

        private void bKeyboard_Text_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtKeyboard_Text.Text.Trim();

                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.KeyBoardType.Text;
                    string sContent = kbType + "|" + KeyCode;

                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 按键

        private void bMouse_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.RobotConfig.Robot.MouseType mType = new Operate.RobotConfig.Robot.MouseType();
                switch (this.cbbMouse.SelectedIndex)
                { 
                    case 0:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftClick;
                        break;

                    case 1:
                        mType = Operate.RobotConfig.Robot.MouseType.RightClick;
                        break;

                    case 2:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftDBClick;
                        break;

                    case 3:
                        mType = Operate.RobotConfig.Robot.MouseType.RightDBClick;
                        break;

                    case 4:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftDown;
                        break;

                    case 5:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftUp;
                        break;

                    case 6:
                        mType = Operate.RobotConfig.Robot.MouseType.RightDown;
                        break;

                    case 7:
                        mType = Operate.RobotConfig.Robot.MouseType.RightUp;
                        break;
                }
                
                string sContent = mType + "|" + string.Empty;
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 滚轮

        private void bMouseWheel_Click(object sender, EventArgs e)
        {
            try
            {
                int MouseWheel_Distance = ((int)this.nudMouseWheel_Distance.Value);             

                Operate.RobotConfig.Robot.MouseType mType = new Operate.RobotConfig.Robot.MouseType();
                switch (this.cbbMouseWheel_Direction.SelectedIndex)
                {
                    case 0:
                        mType = Operate.RobotConfig.Robot.MouseType.WheelUp;
                        break;

                    case 1:
                        mType = Operate.RobotConfig.Robot.MouseType.WheelDown;
                        break;              
                }

                string sContent = mType + "|" + MouseWheel_Distance;

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 移动

        private void bMouseMove_Click(object sender, EventArgs e)
        {
            try
            {
                int MouseMove_X = ((int)this.nudMouseMove_X.Value);
                int MouseMove_Y = ((int)this.nudMouseMove_Y.Value);

                Operate.RobotConfig.Robot.MouseType mType =new Operate.RobotConfig.Robot.MouseType();
                if (this.rbMoveTo.Checked)
                {
                    mType = Operate.RobotConfig.Robot.MouseType.MoveTo;
                }
                else
                {
                    mType = Operate.RobotConfig.Robot.MouseType.MoveBy;
                }                

                string sContent = mType + "|" + MouseMove_X + ", " + MouseMove_Y;

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
