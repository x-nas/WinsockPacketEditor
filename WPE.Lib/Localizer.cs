
namespace WPE.Lib
{
    public class Localizer : AntdUI.ILocalization
    {
        public string GetLocalizedString(string key)
        {
            switch (key)
            {
                #region//System

                case "ID":
                    return "en-US";

                case "Cancel":
                    return "Cancel";

                case "OK":
                    return "OK";

                case "Now":
                    return "Now";

                case "ToDay":
                    return "Today";

                case "NoData":
                    return "No data";

                case "ItemsPerPage":
                    return "Per/Page";

                case "Loading":
                    return "LOADING...";

                case "Input.LetterOrNum":
                    return "Input letters or numbers";

                case "Setting":
                    return "Setting";

                case "AnimationEnabled":
                    return "Animation Enabled";

                case "ShadowEnabled":
                    return "Shadow Enabled";

                case "PopupWindow":
                    return "Popup in the window";

                case "ScrollBarHidden":
                    return "ScrollBar Hidden Style";

                case "TextRenderingHighQuality":
                    return "TextRendering HighQuality";

                #endregion

                #region//Operate

                case "SaveToExcel":
                    return "Save to Excel file";

                case "ExcelColumn":
                    return "Log Time\tModule Name\tLog content\t";

                case "SystemBackupFile":
                    return "System backup file";

                case "ImportListFile":
                    return "Import List File";

                case "ExportListFile":
                    return "Export List File";

                case "InputPassword":
                    return "Please input a password";

                case "PasswordEncryption":
                    return "Please enter the password. This password will require verification when importing the list file.\r\n If you do not need to set a password, please click the[Cancel] button!";

                case "WPEBackUpFile":
                    return "WPE x64 BackUp File";

                case "AESKeyError":
                    return "Failed to load: Password incorrect";

                case "FilterListFile":
                    return "Filter List File";

                #endregion

                #region//StartForm

                case "StartForm":
                    return "Start";

                case "StartForm.bProxy":
                    return "Proxy Mode";

                case "StartForm.bInject":
                    return "Inject Mode";

                case "StartForm.cbIsRemote":
                    return "Enable Remote MGT";

                case "StartForm.lRemote_UserName":
                    return "Username";

                case "StartForm.lRemote_PassWord":
                    return "Password";

                case "StartForm.lRemote_Port":
                    return "Port Num";

                case "StartForm.lRemoteMGT":
                    return "Remote MGT:";

                case "StartForm.RemoteEmpty":
                    return "Username or Password Empty";

                case "StartForm.RemoteEnable":
                    return "Remote MGT Enabled";

                case "StartForm.RemoteDisable":
                    return "Remote MGT Disabled";

                #endregion

                #region//SelectProcessForm

                case "SelectProcessForm":
                    return "Select Process";

                case "Table.ProcessList.Column.ICO":
                    return "";

                case "Table.ProcessList.Column.ProcessName":
                    return "Process Name";

                case "Table.ProcessList.Column.ProcessID":
                    return "Process ID";

                case "Table.ProcessList.Column.ProcessPath":
                    return "Path";

                case "SelectProcessForm.txtSelectProcess":
                    return "Please select a process or program";

                case "SelectProcessForm.txtSearchProcess":
                    return "Filter process list";

                case "SelectProcessForm.bCreate":
                    return "Program";

                case "SelectProcessForm.bRefresh":
                    return "Refresh";

                case "SelectProcessForm.bInject":
                    return "Inject";

                case "SelectProcessForm.InjectError":
                    return "Injection Failed";

                case "SelectProcessForm.SearchOnWebSite":
                    return "Search On WPE64.com";

                case "SelectProcessForm.SelectProgram":
                    return "Please select the program to inject";

                case "SelectProcessForm.ProgramFilter":
                    return "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";

                #endregion

                #region//InjectModeForm

                case "InjectModeForm":
                    return "Inject Mode";

                case "InjectModeForm.miPacketList":
                    return "Packet List";

                case "InjectModeForm.miFilterList":
                    return "Filter List";

                case "InjectModeForm.miStatistical":
                    return "Statistical";

                case "InjectModeForm.miComparison":
                    return "Text Comparison";

                case "InjectModeForm.miXOR":
                    return "XOR Calculation";

                case "InjectModeForm.miTranscoding":
                    return "Transcoding";

                case "InjectModeForm.miExtraction":
                    return "Data Extraction";

                case "InjectModeForm.miSystemLog":
                    return "System Log";

                case "InjectModeForm.FilterSettings.Success":
                    return "Filter settings saved successfully";

                case "InjectModeForm.SpeedInfo":
                    return "Sent: {0} Received: {1}";

                #endregion

                #region//FilterSettingsForm

                case "FilterSettingsForm":
                    return "Filter Settings";

                case "FilterSettingsForm.FilterEmpty":
                    return "Filter Settings Empty";

                case "FilterSettingsForm.Success":
                    return "Filter Settings Success";

                #endregion

                #region//HookSettingsForm

                case "HookSettingsForm":
                    return "Hook Settings";

                case "HookSettingsForm.Success":
                    return "Hook Settings Success";

                #endregion

                #region//ListSettingsForm

                case "ListSettingsForm":
                    return "List Settings";

                case "ListSettingsForm.Success":
                    return "List Settings Success";

                #endregion

                #region//BackUpSettingsForm

                case "BackUpSettingsForm":
                    return "BackUp Settings";

                #endregion

                #region//SystemSettingsForm

                case "SystemSettingsForm":
                    return "System Settings";

                #endregion

                #region//SearchPacketForm

                case "SearchPacketForm":
                    return "Search Packet";

                case "SearchPacketForm.NoMatch":
                    return "No Match Found";

                #endregion

                default:
                    return null;
            }
        }
    }
}
