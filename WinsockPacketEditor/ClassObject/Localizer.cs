
namespace WinsockPacketEditor
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

                case "Start":
                    return "Start";

                case "Stop":
                    return "Stop";

                case "Clear":
                    return "Clear";

                case "Search":
                    return "Search";

                case "Reset":
                    return "Reset";

                case "Online":
                    return "Online";

                case "Offline":
                    return "Offline";

                case "Unlimited":
                    return "Unlimited";

                case "Piece":
                    return "Piece";

                case "Page":
                    return "Page";

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

                case "Send":
                    return "Send:";

                case "Recv":
                    return "Recv:";

                case "SendTo":
                    return "SendTo:";

                case "RecvFrom":
                    return "RecvFrom:";

                case "WSASend":
                    return "WSASend:";

                case "WSARecv":
                    return "WSARecv:";

                case "WSASendTo":
                    return "WSASendTo:";

                case "WSARecvFrom":
                    return "WSARecvFrom:";

                case "Feedback":
                    return "Questions and Feedback";

                case "OfficialWebsite":
                    return "Official Website";

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

                #region//WPEForm

                case "WPEForm.Login":
                    return "Login";

                case "WPEForm.ProxyMode":
                    return "Proxy Mode";

                case "WPEForm.InjectMode":
                    return "Inject Mode";

                case "WPEForm.SetRemote":
                    return "Set Remote MGT";

                case "WPEForm.UserName":
                    return "Please enter username";

                case "WPEForm.PassWord":
                    return "Please enter password";

                case "WPEForm.EnableMGT":
                    return "Enable Remote MGT";

                case "WPEForm.UserName.Empty":
                    return "Username Empty";

                case "WPEForm.PassWord.Empty":
                    return "Password Empty";

                case "WPEForm.RemoteError":
                    return "Remote URL Error";

                case "WPEForm.RemoteEnable":
                    return "Remote MGT Enabled";

                case "WPEForm.RemoteDisable":
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

                case "InjectModeForm.miSendList":
                    return "Send List";

                case "InjectModeForm.miRobotList":
                    return "Robot List";

                case "InjectModeForm.miStatistical":
                    return "Statistical Data";

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

                case "InjectModeForm.TotalPackets":
                    return "Total Packets:";

                case "InjectModeForm.ExecuteFilter":
                    return "Execute Filter:";

                case "InjectModeForm.Buffer":
                    return "Buffer:";

                case "InjectModeForm.Filter":
                    return "Filter:";

                case "Table.PacketList.Column.ID":
                    return "ID";

                case "Table.PacketList.Column.PacketTime":
                    return "Time";

                case "Table.PacketList.Column.PacketType":
                    return "Type";

                case "Table.PacketList.Column.PacketSocket":
                    return "Socket";

                case "Table.PacketList.Column.PacketFrom":
                    return "From";

                case "Table.PacketList.Column.FromLocation":
                    return "Location";

                case "Table.PacketList.Column.PacketTo":
                    return "To";

                case "Table.PacketList.Column.ToLocation":
                    return "Location";

                case "Table.PacketList.Column.PacketLen":
                    return "Length";

                case "Table.PacketList.Column.PacketData":
                    return "Data";

                case "InjectModeForm.miFilterSettings":
                    return "Filter Settings";

                case "InjectModeForm.miHookSettings":
                    return "Hook Settings";

                case "InjectModeForm.miListSettings":
                    return "List Settings";

                case "InjectModeForm.miHotKeySettings":
                    return "HotKey Settings";

                case "InjectModeForm.miBackUpSettings":
                    return "BackUp Settings";

                case "InjectModeForm.miSystemSettings":
                    return "System Settings";

                case "InjectModeForm.SpeedInfo":
                    return "Sent: {0} Received: {1}";

                #endregion

                #region//ProxyModeForm

                case "ProxyModeForm":
                    return "Proxy Mode";

                case "ProxyModeForm.miProxyList":
                    return "Proxy List";

                case "ProxyModeForm.miClientList":
                    return "Client List";

                case "ProxyModeForm.miAccountList":
                    return "Account List";

                case "ProxyModeForm.miFilterList":
                    return "Filter List";

                case "ProxyModeForm.miSendList":
                    return "Send List";

                case "ProxyModeForm.miRobotList":
                    return "Robot List";

                case "ProxyModeForm.miStatistical":
                    return "Statistical Data";

                case "ProxyModeForm.miComparison":
                    return "Text Comparison";

                case "ProxyModeForm.miXOR":
                    return "XOR Calculation";

                case "ProxyModeForm.miTranscoding":
                    return "Transcoding";

                case "ProxyModeForm.miExtraction":
                    return "Data Extraction";

                case "ProxyModeForm.miSystemLog":
                    return "System Log";

                case "ProxyModeForm.TotalProxy":
                    return "Total Proxy:";

                case "ProxyModeForm.ExecuteFilter":
                    return "Execute Filter:";

                case "ProxyModeForm.Buffer":
                    return "Buffer:";

                case "ProxyModeForm.Filter":
                    return "Filter:";

                case "ProxyModeForm.Account":
                    return "Account:";

                case "ProxyModeForm.TCPLink":
                    return "TCP Link:";

                case "ProxyModeForm.UDPLink":
                    return "UDP Link:";

                case "ProxyModeForm.TCPReq":
                    return "TCP Req:";

                case "ProxyModeForm.TCPResp":
                    return "TCP Resp:";

                case "ProxyModeForm.UDPReq":
                    return "UDP Req:";

                case "ProxyModeForm.UDPResp":
                    return "UDP Resp:";

                case "ProxyModeForm.miProxySettings":
                    return "Proxy Settings";

                case "ProxyModeForm.miFilterSettings":
                    return "Filter Settings";

                case "ProxyModeForm.miHookSettings":
                    return "Hook Settings";

                case "ProxyModeForm.miListSettings":
                    return "List Settings";

                case "ProxyModeForm.miMapSettings":
                    return "Map Settings";

                case "ProxyModeForm.miExternalProxySettings":
                    return "EXTProxy Settings";

                case "ProxyModeForm.miHotKeySettings":
                    return "HotKey Settings";

                case "ProxyModeForm.miBackUpSettings":
                    return "BackUp Settings";

                case "ProxyModeForm.miSystemSettings":
                    return "System Settings";

                case "Table.ProxyList.Column.ID":
                    return "ID";

                case "Table.ProxyList.Column.ProxyTime":
                    return "Time";

                case "Table.ProxyList.Column.PacketType":
                    return "Type";

                case "Table.ProxyList.Column.PacketSocket":
                    return "Socket";

                case "Table.ProxyList.Column.ClientAddr":
                    return "Client Addr";

                case "Table.ProxyList.Column.ClientLocation":
                    return "Location";

                case "Table.ProxyList.Column.ServerDomain":
                    return "Server Addr";

                case "Table.ProxyList.Column.ServerLocation":
                    return "Location";

                case "Table.ProxyList.Column.PacketLen":
                    return "Length";

                case "Table.ProxyList.Column.PacketData":
                    return "Data";

                case "ProxyModeForm.ProxyBytesInfo":
                    return "Request: {0}  Response: {1}";

                case "ProxyModeForm.ProxySpeedInfo":
                    return "UpLink: {0} KB/s  DownLink: {1} KB/s";

                #endregion

                #region//ClientList

                case "ClientList.TotalClients":
                    return "Total Clients:";

                case "ClientList.TotalLinks":
                    return "Total Links:";

                case "ClientList.TotalDevices":
                    return "Total Devices:";

                case "ClientList.tpAuthList":
                    return "Auth List";

                case "ClientList.tpProxyLog":
                    return "Proxy Log";

                case "Table.AuthList.Column.AuthTime":
                    return "Time";

                case "Table.AuthList.Column.AID":
                    return "Account";

                case "Table.AuthList.Column.AuthIP":
                    return "IP Addr";

                case "Table.AuthList.Column.IPLocation":
                    return "Location";

                case "Table.AuthList.Column.LinksNumber":
                    return "Links";

                case "Table.AuthList.Column.DevicesNumber":
                    return "Devices";

                case "Table.AuthList.Column.AuthResult":
                    return "Auth Result";

                case "Table.ProxyLog.Column.LoginIP":
                    return "IP Addr";

                case "Table.ProxyLog.Column.LogTime":
                    return "Time";

                case "Table.ProxyLog.Column.UserName":
                    return "Account";

                case "Table.ProxyLog.Column.LogContent":
                    return "Content";

                #endregion

                #region//AccountList

                case "DatePicker.PlaceholderS":
                    return "Expiration Time Start";

                case "DatePicker.PlaceholderE":
                    return "Expiration Time End";

                case "AccountList.SearchAccount":
                    return "Please enter Account";

                case "AccountList.miAdd":
                    return "Add Account";

                case "AccountList.miImport":
                    return "Import Account";

                case "AccountList.miExport":
                    return "Export Account";

                case "AccountList.miClear":
                    return "Clear Account";

                case "AccountList.BatchAdjustment":
                    return "Batch Adjustment";

                case "AccountList.ExpiryTime":
                    return "Expiry Time";

                case "AccountList.LimitLinks":
                    return "Links";

                case "AccountList.LimitDevices":
                    return "Devices";

                case "AccountList.Export":
                    return "Batch Export";

                case "AccountList.Delete":
                    return "Batch Delete";

                case "Table.AccountList.Column.ID":
                    return "ID";

                case "Table.AccountList.Column.UserName":
                    return "Account";

                case "Table.AccountList.Column.IsOnLine":
                    return "Status";

                case "Table.AccountList.Column.LimitLinks":
                    return "Links";

                case "Table.AccountList.Column.LimitDevices":
                    return "Devices";

                case "Table.AccountList.Column.ExpiryTime":
                    return "Expiry Time";

                case "Table.AccountList.Column.CellLinks":
                    return "Operation";

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
