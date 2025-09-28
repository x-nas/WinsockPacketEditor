
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

                case "Target":
                    return "Target: {0}";

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

                case "Per":
                    return "Per";

                case "Page":
                    return "Page";

                case "Top":
                    return "Top";

                case "Up":
                    return "Up";

                case "Down":
                    return "Down";

                case "Bottom":
                    return "Bottom";

                case "Copy":
                    return "Copy";

                case "CopyText":
                    return "Copy Text";

                case "CopyHex":
                    return "Copy Hex";

                case "Import":
                    return "Import";

                case "Export":
                    return "Export";

                case "Delete":
                    return "Delete";

                case "Enable":
                    return "Enable";

                case "Disable":
                    return "Disable";

                case "Replace":
                    return "Replace";

                case "Change":
                    return "Change";

                case "Intercept":
                    return "Intercept";

                case "Other":
                    return "Other";

                case "Execute":
                    return "Execute";

                case "NoModifyNoDisplay":
                    return "NoModify NoDisplay";

                case "NoModifyDisplay":
                    return "NoModify Display";

                case "Refresh":
                    return "Refresh";

                case "Head":
                    return "Head";

                case "Socket":
                    return "Socket";

                case "Port":
                    return "Port";

                case "Length":
                    return "Length";

                case "Continuous":
                    return "Continuous";

                case "Carry":
                    return "Carry";

                case "Inserted":
                    return "Inserted";

                case "Deleted":
                    return "Deleted";

                case "Modified":
                    return "Modified";

                case "Same":
                    return "Same";

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

                case "Working":
                    return "Working";

                case "Customize":
                    return "Customize";

                case "Count":
                    return "Count";

                case "Interval":
                    return "Interval";

                case "Millisecond":
                    return "ms";

                case "Check":
                    return "Check";

                case "Position":
                    return "Position";

                case "XOR":
                    return "XOR";

                case "Input.LetterOrNum":
                    return "Input letters or numbers";

                case "Setting":
                    return "Setting";

                case "Cut":
                    return "Cut";

                case "Paste":
                    return "Paste";

                case "PasteText":
                    return "Paste Text";

                case "PasteHex":
                    return "Paste Hex";

                case "Edit":
                    return "Edit";

                case "SelectAll":
                    return "Select All";

                case "Extraction":
                    return "Extraction";

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
                    return "Send";

                case "Recv":
                    return "Recv";

                case "SendTo":
                    return "SendTo";

                case "RecvFrom":
                    return "RecvFrom";

                case "WSASend":
                    return "WSASend";

                case "WSARecv":
                    return "WSARecv";

                case "WSASendTo":
                    return "WSASendTo";

                case "WSARecvFrom":
                    return "WSARecvFrom";

                case "TCPReq":
                    return "TCP Req";

                case "TCPResp":
                    return "TCP Resp";

                case "UDPReq":
                    return "UDP Req";

                case "UDPResp":
                    return "UDP Resp";

                case "Feedback":
                    return "Questions and Feedback";

                case "OfficialWebsite":
                    return "Official Website";

                case "HexWithSpaces":
                    return "Please enter Hex with spaces";

                case "SureToDelete":
                    return "Are you sure to delete the data?";

                case "ExcelFile":
                    return "Excel File";

                case "Exporting":
                    return "Exporting...";

                case "PleaseSelect":
                    return "Please select";

                case "Presskey":
                    return "Please Press key";

                case "PressCombinationkey":
                    return "Please press the combination key";

                case "Save":
                    return "Save";

                case "Yes":
                    return "Yes";

                case "No":
                    return "No";

                case "IPAddress":
                    return "IP Address";

                case "PacketHead":
                    return "Packet Head";

                case "PacketContent":
                    return "Packet Content";

                case "Request":
                    return "Request";

                case "Response":
                    return "Response";

                case "Protocol":
                    return "Protocol";

                case "Host":
                    return "Host";

                case "Path":
                    return "Path";

                case "Detection":
                    return "Detection";

                case "FilterList":
                    return "Filter List";

                case "SendList":
                    return "Send List";

                case "SendCollection":
                    return "Send Collection";

                case "RobotList":
                    return "Robot List";

                case "ClientList":
                    return "Client List";

                case "Username":
                    return "Username";

                case "Password":
                    return "Password";

                case "AccountList":
                    return "Account List";

                case "Add":
                    return "Add:";

                case "Hour":
                    return "Hour";

                case "Day":
                    return "Day";

                case "Normal":
                    return "Normal";

                case "Advanced":
                    return "Advanced";

                case "InvalidHex":
                    return "Please enter a valid HEX or (*)";

                case "InvalidWildcard":
                    return "Please use blank as (**)";

                case "Paste.Success":
                    return "Pasting Completed";

                case "Times":
                    return "Times";

                case "Begin":
                    return "Begin";

                case "End":
                    return "End";

                case "Pixel":
                    return "Pixel";

                case "MoveTo":
                    return "Move To";

                case "MoveBy":
                    return "Move By";

                #endregion

                #region//Operate

                case "DeSelect":
                    return "DeSelect";

                case "PacketModification":
                    return "View Packet Modification";

                case "SetSSocket":
                    return "Set System Socket";

                case "ToSendList":
                    return "Add to Send List";

                case "ToFilterList":
                    return "Add to Filter List";

                case "ToSendList.Success":
                    return "Added to: {0}";

                case "ToSendList.Error":
                    return "Error Adding";

                case "ToTextA":
                    return "Added to Text A";

                case "ToTextB":
                    return "Added to Text B";

                case "SSocket.Success":
                    return "Successfully set System Socket";

                case "ToFilterList.Success":
                    return "Added to Filter List successfully";

                case "ToFilterList.Error":
                    return "Failed to add to Filter List";

                case "ExportToExcel.Success":
                    return "Export To Excel Success";

                case "ClearedData":
                    return "Cleared Data";

                case "ExportToExcel.Error":
                    return "Export To Excel Error";

                case "CheckSystemLog":
                    return "Please Check System Log";

                case "CopyToClipboard":
                    return "Copy to clipboard";

                case "SaveToExcel":
                    return "Save to Excel file";

                case "ExcelColumn.Log":
                    return "Log Time\tModule\tContent\t";

                case "ExcelColumn.FilterLog":
                    return "Log Time\tFilter Name\tAction\tMatch\tType\tLength\t";

                case "ExcelColumn.ProxyLog":
                    return "Log Time\tAccount\tIP Address\tContent\t";

                case "ExcelColumn.Proxy":
                    return "Time\tType\tSocket\tClient Addr\tServer Addr\tLength\tData\t";

                case "ExcelColumn.Packet":
                    return "Time\tType\tSocket\tFrom\tTo\tLength\tData\t";

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
                                
                case "AESKeyError":
                    return "Failed to load: Password incorrect";

                case "FilterListFile":
                    return "Filter List File";

                case "SemicolonDelimiter":
                    return "Support ; delimiter";                

                case "HEXSemicolonDelimiter":
                    return "Hex with spaces, Support ; delimiter";

                case "Password.Empty":
                    return "Password cannot be empty";

                case "Password.Incorrect":
                    return "Incorrect password";

                case "Input.Text":
                    return "Please input Text";

                case "Input.Password":
                    return "Please input Password";

                case "BatchAccounts":
                    return "Batch adjustment ( {0} ) Accounts";

                case "BatchSuccess":
                    return "Batch adjustment successful";

                case "MGT.Enabled":
                    return "Remote MGT Enabled: {0}";

                case "MGT.Error":
                    return "Remote MGT startup failed: Please try use administrator to start {0}";

                case "ProcessInfo":
                    return "{0} Handle: {1}";

                case "SOCKS.Unsupported":
                    return "Unsupported SOCKS protocol: {0} [ {1} ]";

                case "Command.Unsupported":
                    return "{0} - Unsupported command: {1}";

                case "ExportList.Error":
                    return "Export List Error";

                case "ImportList.Error":
                    return "Import List Error";

                case "ExportProxyAccountList":
                    return "Export Proxy Account List";

                case "ImportProxyAccountList":
                    return "Import Proxy Account List";

                case "ImportAccount.Error":
                    return "Import account failed! Username: {0}";

                case "ProxyAccountListFile":
                    return "Proxy Account List File";

                case "MapLocalFile":
                    return "Map Local File";

                case "MapRemoteFile":
                    return "Map Remote File";

                case "ExportMapLocal":
                    return "Export Map Local";

                case "ExportMapRemote":
                    return "Export Map Remote";

                case "ImportMapLocal":
                    return "Import Map Local";

                case "ImportMapRemote":
                    return "Import Map Remote";

                case "CopyName":
                    return "{0} - Copy";

                case "DoFilter":
                    return "[{0}] {1} | [{2}] Packet Length: {3}";

                case "DoFilterMatch":
                    return "[{0}] {1} | [{2}] Packet Length: {3} | Match: {4}";

                case "ImportFilterList":
                    return "Import Filter List";

                case "ImportFilterList.Success":
                    return "Successfully Imported Filter List";

                case "ExportFilterList":
                    return "Export Filter List";

                case "ExportFilterList.Success":
                    return "Successfully Exported Filter List";

                case "ExportFilterList.Error":
                    return "Export Filter List Failed";

                case "ImportSendCollection":
                    return "Import Send Collection";

                case "ExportSendCollection":
                    return "Export Send Collection";

                case "ExportSendCollection.Success":
                    return "Successfully Exported Send Collection";

                case "ExportSendCollection.Error":
                    return "Export Send Collection Failed";

                case "SendListFile":
                    return "Send List File";

                case "ImportSendList":
                    return "Import Send List";

                case "ExportSendList":
                    return "Export Send List";

                case "ExportSendList.Success":
                    return "Successfully Exported Send List";

                case "ExportSendList.Error":
                    return "Export Send List Failed";

                case "ImportSendList.Success":
                    return "Successfully Imported Send List";

                case "RobotListFile":
                    return "Robot List File";

                case "ImportRobotList":
                    return "Import Robot List";

                case "ExportRobotList":
                    return "Export Robot List";

                case "ExportRobotList.Success":
                    return "Successfully Exported Robot List";

                case "ExportRobotList.Error":
                    return "Export Robot List Failed";

                case "ImportRobotList.Success":
                    return "Successfully Imported Robot List";

                case "Input.Regex":
                    return "Please enter a Regex";

                case "EnableAll":
                    return "Enable ALL";

                case "DisableAll":
                    return "Disable ALL";

                case "StoreText":
                    return "Store Text";

                #endregion

                #region//StartForm

                case "StartForm":
                    return "Home";

                case "StartForm.ProxyMode":
                    return "Proxy Mode";

                case "StartForm.InjectMode":
                    return "Inject Mode";

                case "StartForm.RemoteMGT":
                    return "Remote MGT";

                case "StartForm.ProxyMode.Description":
                    return "Intercept packets by Proxy Server";

                case "StartForm.InjectMode.Description":
                    return "Intercept packets by Inject Process";

                case "StartForm.RemoteMGT.Description":
                    return "Configure Remote Management";

                case "StartForm.LearnMore":
                    return "Learn more";

                case "StartForm.Tutorials":
                    return "Tutorials";

                case "StartForm.OfficialWebsite":
                    return "Official Website";

                case "StartForm.QA":
                    return "Questions & Feedback";

                #endregion

                #region//RemoteMGTSetting

                case "RemoteMGTSetting.UserName":
                    return "Please enter username";

                case "RemoteMGTSetting.PassWord":
                    return "Please enter password";

                case "RemoteMGTSetting.EnableMGT":
                    return "Enable Remote MGT";

                case "RemoteMGTSetting.UserName.Empty":
                    return "Username Empty";

                case "RemoteMGTSetting.PassWord.Empty":
                    return "Password Empty";

                case "RemoteMGTSetting.RemoteEnable":
                    return "Remote MGT Enabled";

                case "RemoteMGTSetting.RemoteDisable":
                    return "Remote MGT Disabled";

                #endregion

                #region//ProcessList

                case "ProcessList":
                    return "Process List";

                case "ProcessList.Process":
                    return "Process : ";

                case "ProcessList.PID":
                    return "PID : ";

                case "ProcessList.Title":
                    return "Title : ";

                case "ProcessList.Path":
                    return "Path : ";

                case "ProcessList.Handle":
                    return "Handle : ";

                case "ProcessList.NoTitle":
                    return "No Title";

                case "ProcessList.NoInfo":
                    return "(No Information)";

                case "Table.ProcessList.Column.ICO":
                    return "";

                case "Table.ProcessList.Column.ProcessName":
                    return "Process Name";

                case "Table.ProcessList.Column.ProcessID":
                    return "Process ID";

                case "Table.ProcessList.Column.ProcessPath":
                    return "Path";

                case "ProcessList.SelectForm":
                    return "Please select a form";

                case "ProcessList.txtSelectProcess":
                    return "Please select a process or program";

                case "ProcessList.txtSearchProcess":
                    return "Filter process list";

                case "ProcessList.bSelectForm":
                    return "Select Form";

                case "ProcessList.bCreate":
                    return "Program";

                case "ProcessList.bRefresh":
                    return "Refresh";

                case "ProcessList.bInject":
                    return "Inject";

                case "ProcessList.InjectError":
                    return "Injection Failed";

                case "ProcessList.SearchOnWebSite":
                    return "Search On WPE64.com";

                case "ProcessList.SelectProgram":
                    return "Please select the program to inject";

                case "ProcessList.ProgramFilter":
                    return "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";

                #endregion

                #region//InjectModeForm

                case "InjectModeForm":
                    return "Inject Mode";                

                case "InjectModeForm.StartHook":
                    return "Start Hook";

                case "InjectModeForm.StopHook":
                    return "Stop Hook";

                case "InjectModeForm.miPacketList":
                    return "Packet List";

                case "InjectModeForm.miFilterList":
                    return "Filter List";

                case "InjectModeForm.miSendList":
                    return "Send List";

                case "InjectModeForm.miRobotList":
                    return "Robot List";

                case "InjectModeForm.miRobotInstruction":
                    return "Robot Instruction";

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
                    return "Total Packets :";

                case "InjectModeForm.ExecuteFilter":
                    return "Execute Filter :";

                case "InjectModeForm.Buffer":
                    return "Buffer :";

                case "InjectModeForm.Filter":
                    return "Filter :";

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

                case "InjectModeForm.SpeedInfo":
                    return "Sent : {0} Received : {1}";

                case "InjectModeForm.Send":
                    return "Send :";

                case "InjectModeForm.Recv":
                    return "Recv :";

                case "InjectModeForm.SendTo":
                    return "SendTo :";

                case "InjectModeForm.RecvFrom":
                    return "RecvFrom :";

                case "InjectModeForm.WSASend":
                    return "WSASend :";

                case "InjectModeForm.WSARecv":
                    return "WSARecv :";

                case "InjectModeForm.WSASendTo":
                    return "WSASendTo :";

                case "InjectModeForm.WSARecvFrom":
                    return "WSARecvFrom :";

                #endregion

                #region//ProxyModeForm

                case "ProxyModeForm":
                    return "Proxy Mode";

                case "ProxyModeForm.StartSocks5Proxy":
                    return "Start Socks5 Proxy";

                case "ProxyModeForm.StartSocks5Proxy.Fail":
                    return "Startup failed, please try lowering the MaxConnectionNumber";

                case "ProxyModeForm.SetupSocks5Proxy.Fail":
                    return "Failed to Setup Socks5 Proxy";

                case "ProxyModeForm.StopProxy":
                    return "Stop SOCKS5 Proxy";

                case "ProxyModeForm.ProxyServerIP":
                    return "Proxy Server IP : TCP [ {0} ] UDP [ {1} ]";

                case "ProxyModeForm.ProxyServer.Auth":
                    return "Proxy service authentication enabled";

                case "ProxyModeForm.ProxyServer.EXTProxy":
                    return "External proxy server enabled [ {0}:{1} ]";

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
                    return "Total Proxy :";

                case "ProxyModeForm.ExecuteFilter":
                    return "Execute Filter :";

                case "ProxyModeForm.Buffer":
                    return "Buffer :";

                case "ProxyModeForm.Filter":
                    return "Filter :";

                case "ProxyModeForm.Account":
                    return "Account :";

                case "ProxyModeForm.TCPLink":
                    return "TCP Link :";

                case "ProxyModeForm.UDPLink":
                    return "UDP Link :";

                case "ProxyModeForm.TCPReq":
                    return "TCP Req :";

                case "ProxyModeForm.TCPResp":
                    return "TCP Resp :";

                case "ProxyModeForm.UDPReq":
                    return "UDP Req :";

                case "ProxyModeForm.UDPResp":
                    return "UDP Resp :";

                case "ProxyModeForm.ProxySettings":
                    return "Proxy Settings";

                case "ProxyModeForm.LeachSettings":
                    return "Leach Settings";

                case "ProxyModeForm.HookSettings":
                    return "Hook Settings";

                case "ProxyModeForm.ListSettings":
                    return "List Settings";

                case "ProxyModeForm.MapSettings":
                    return "Map Settings";

                case "ProxyModeForm.ExternalProxySettings":
                    return "EXTProxy Settings";

                case "ProxyModeForm.HotKeySettings":
                    return "HotKey Settings";

                case "ProxyModeForm.BackUpSettings":
                    return "BackUp Settings";

                case "ProxyModeForm.SystemSettings":
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
                    return "Client Address";

                case "Table.ProxyList.Column.ClientLocation":
                    return "Location";

                case "Table.ProxyList.Column.ServerDomain":
                    return "Server Address";

                case "Table.ProxyList.Column.ServerLocation":
                    return "Location";

                case "Table.ProxyList.Column.PacketLen":
                    return "Length";

                case "Table.ProxyList.Column.PacketData":
                    return "Data";

                case "ProxyModeForm.ProxyBytesInfo":
                    return "Request : {0} Response : {1}";

                case "ProxyModeForm.ProxySpeedInfo":
                    return "UpLink : {0} KB/s DownLink : {1} KB/s";

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
                    return "IP Address";

                case "Table.AuthList.Column.IPLocation":
                    return "Location";

                case "Table.AuthList.Column.LinksNumber":
                    return "Links";

                case "Table.AuthList.Column.DevicesNumber":
                    return "Devices";

                case "Table.AuthList.Column.AuthResult":
                    return "Auth Result";                

                #endregion

                #region//AccountList

                case "DatePicker.PlaceholderS":
                    return "Expiration Time Start";

                case "DatePicker.PlaceholderE":
                    return "Expiration Time End";

                case "AccountList.SearchAccount":
                    return "Please enter Account";

                case "AccountList.Add":
                    return "Add Account";

                case "AccountList.Import":
                    return "Import Account";

                case "AccountList.Export":
                    return "Export Account";

                case "AccountList.Clear":
                    return "Clear Account";

                case "AccountList.BatchAdjustment":
                    return "Batch Adjustment";

                case "AccountList.ExpiryTime":
                    return "Expiry Time";

                case "AccountList.LimitLinks":
                    return "Links";

                case "AccountList.LimitDevices":
                    return "Devices";

                case "AccountList.BatchExport":
                    return "Batch Export";

                case "AccountList.Delete":
                    return "Batch Delete";

                case "AccountList.Empty":
                    return "Please select account";

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

                #region//FilterList

                case "FilterList.NewFilter":
                    return "Filter {0}";

                case "FilterList.ResetCount":
                    return "Reset Count";

                case "FilterList.Add":
                    return "Add Filter";

                case "FilterList.Import":
                    return "Import Filter";

                case "FilterList.Export":
                    return "Export Filter";

                case "FilterList.Clear":
                    return "Clear Filter";

                case "Table.FilterList.Column.IsEnable":
                    return "Enable";

                case "Table.FilterList.Column.FName":
                    return "Filter Name";

                case "Table.FilterList.Column.Status":
                    return "Status";

                case "Table.FilterList.Column.FAction":
                    return "Action";

                case "Table.FilterList.Column.ExecutionCount":
                    return "Execution";

                case "Table.FilterList.Column.Appoint":
                    return "Appoint";

                case "Table.FilterList.Column.Progression":
                    return "Progression";

                case "Table.FilterList.Column.CellLinks":
                    return "Operation";

                #endregion

                #region//SendList

                case "SendList.NewSend":
                    return "Send {0}";

                case "SendList.Add":
                    return "Add Send";

                case "SendList.Import":
                    return "Import Send";

                case "SendList.Export":
                    return "Export Send";

                case "SendList.Clear":
                    return "Clear Send";

                case "Table.SendList.Column.IsEnable":
                    return "Enable";

                case "Table.SendList.Column.SName":
                    return "Send Name";

                case "Table.SendList.Column.Status":
                    return "Status";

                case "Table.SendList.Column.ExecutionCount":
                    return "Execution";

                case "Table.SendList.Column.ExecutionSuccess":
                    return "Success";

                case "Table.SendList.Column.ExecutionFail":
                    return "Fail";

                case "Table.SendList.Column.SSystemSocket":
                    return "System Socket";

                case "Table.SendList.Column.SLoopCNT":
                    return "Loop Count";

                case "Table.SendList.Column.SNotes":
                    return "Notes";

                case "Table.SendList.Column.CellLinks":
                    return "Operation";

                case "SendList.SendCollectionFile":
                    return "Send Collection File";

                #endregion

                #region//RobotList

                case "RobotList.NewRobot":
                    return "Robot {0}";

                case "RobotList.Add":
                    return "Add Robot";

                case "RobotList.Import":
                    return "Import Robot";

                case "RobotList.Export":
                    return "Export Robot";

                case "RobotList.Clear":
                    return "Clear Robot";

                case "Table.RobotList.Column.IsEnable":
                    return "Enable";

                case "Table.RobotList.Column.RName":
                    return "Robot Name";

                case "Table.RobotList.Column.Status":
                    return "Status";

                case "Table.RobotList.Column.ExecutionCount":
                    return "Execution";

                case "Table.RobotList.Column.RInstruction":
                    return "Instruction";

                case "Table.RobotList.Column.CellLinks":
                    return "Operation";

                #endregion

                #region//StatisticalData

                case "StatisticalData.FilterExecution":
                    return "Filter Execution";

                case "StatisticalData.FilterAction":
                    return "Filter Action";

                case "StatisticalData.Details":
                    return "Details";

                case "StatisticalData.Replace":
                    return "Replace :";

                case "StatisticalData.Change":
                    return "Change :";

                case "StatisticalData.Intercept":
                    return "Intercept :";

                case "StatisticalData.NoDisplay":
                    return "NoDisplay :";

                case "StatisticalData.Display":
                    return "Display :";                

                case "Table.StatisticalData.Column.FName":
                    return "Filter Name";

                case "Table.StatisticalData.Column.Status":
                    return "Status";

                case "Table.StatisticalData.Column.FAction":
                    return "Action";

                case "Table.StatisticalData.Column.ExecutionCount":
                    return "Execution";

                #endregion

                #region//ComparisonText

                case "ComparisonText.tpComparison":
                    return "Text Comparison";

                case "ComparisonText.tpDuplicate":
                    return "Text Duplicate";

                case "ComparisonText.TextA":
                    return "Text A ( Length {0} )";

                case "ComparisonText.TextB":
                    return "Text B ( Length {0} )";

                case "ComparisonText.DuplicateCNT":
                    return "Duplicate CNT:";

                case "Table.Comparison.Column.ID":
                    return "ID";

                case "Table.Comparison.Column.Position":
                    return "Position";

                case "Table.Comparison.Column.ValueA":
                    return "Value A";

                case "Table.Comparison.Column.ValueB":
                    return "Value B";

                case "Table.Comparison.Column.ChangeType":
                    return "Type";

                case "Table.Duplicate.Column.ID":
                    return "ID";

                case "Table.Duplicate.Column.Sequence":
                    return "Duplicate Value";

                case "Table.Duplicate.Column.Length":
                    return "Length";

                case "Table.Duplicate.Column.CountInA":
                    return "Count in A";

                case "Table.Duplicate.Column.CountInB":
                    return "Count in B";

                case "Table.Duplicate.Column.PositionsInA":
                    return "Position in A";

                case "Table.Duplicate.Column.PositionsInB":
                    return "Position in B";

                case "Table.Comparison.Column.CellLinks":
                    return "Operation";

                case "Table.Duplicate.Column.CellLinks":
                    return "Operation";

                case "ComparisonText.Leach":
                    return "Leach";

                case "ComparisonText.Search":
                    return "Search :";

                #endregion

                #region//XORCalculation

                case "XORCalculation.XORValue":
                    return "XOR Value ( Supporting Cyclic )";

                case "XORCalculation.XOREmpty":
                    return "XOR Value Empty";

                case "XORCalculation.XORError":
                    return "XOR Value is not HEX";

                #endregion

                #region//Transcoding

                case "Transcoding.EnterText":
                    return "Please enter text";

                case "Transcoding.Encode":
                    return "Encode";

                case "Transcoding.Decode":
                    return "Decode";

                #endregion

                #region//ExtractionData

                case "ExtractionData.ExtractionType":
                    return "Please select the extraction type";

                case "ExtractionData.DragFiles":
                    return "Click or drag files to this area for data extraction";

                case "ExtractionData.ExtractionFile":
                    return "After extraction, Click the Extraction button to export the corresponding format of the data file";

                case "ExtractionData.Chlsx":
                    return "[ Charles XML session file（.chlsx）] Extraction To [ HEX Data ]";

                case "ExtractionData.Filt":
                    return "[ FILT Filter file（.filt）] Extraction To [ WPE64 Filter file（.sp）]";

                case "ExtractionData.Pa":
                    return "[ WPE Account File（.pa）] Extraction To [ CCProxy Account File（.ini）]";

                case "ExtractionData.Empty":
                    return "Extract data is empty";

                case "ExtractionData.Successful":
                    return "Data extraction successful";

                case "ExtractionData.FilterListFile":
                    return "Filter List File";

                #endregion

                #region//LogList

                case "LogList.LogList":
                    return "Log List";

                case "LogList.LogList.tpSystemLog":
                    return "System Log";

                case "LogList.LogList.tpFilterLog":
                    return "Filter Log";

                case "LogList.LogList.tpProxyLog":
                    return "Proxy Log";

                case "Table.LogList.Column.ID":
                    return "ID";

                case "Table.LogList.Column.LogTime":
                    return "Time";

                case "Table.LogList.Column.FuncName":
                    return "Module";

                case "Table.LogList.Column.LogContent":
                    return "Content";

                case "Table.ProxyLog.Column.ID":
                    return "ID";

                case "Table.ProxyLog.Column.LoginIP":
                    return "IP Address";

                case "Table.ProxyLog.Column.LogTime":
                    return "Time";

                case "Table.ProxyLog.Column.UserName":
                    return "Account";

                case "Table.ProxyLog.Column.LogContent":
                    return "Content";

                case "Table.FilterLogList.Column.MatchNum":
                    return "Match";

                case "LogList.CopyLog":
                    return "Copy Log";

                case "LogList.ToExcel":
                    return "Save To Excel";

                case "LogList.ClearUp":
                    return "Clear Log";

                case "LogList.DeSelect":
                    return "DeSelect";

                #endregion

                #region//ProxySettingsForm

                case "ProxySettingsForm":
                    return "Proxy Settings";

                case "ProxySettingsForm.Port":
                    return "Port:";

                case "ProxySettingsForm.ProxyServerIP":
                    return "Proxy Server IP";

                case "ProxySettingsForm.ProxyType":
                    return "Proxy Type";

                case "ProxySettingsForm.ProxyAuth":
                    return "Proxy Authentication";

                case "ProxySettingsForm.SystemProxy":
                    return "System Proxy";

                case "ProxySettingsForm.MaxConnection":
                    return "Max Connection";

                case "ProxySettingsForm.BufferSize":
                    return "Buffer Size";

                case "ProxySettingsForm.ProxyIPAuto":
                    return "Auto Detection";

                case "ProxySettingsForm.EnableAuth":
                    return "Enable";

                case "ProxySettingsForm.UNPW":
                    return "Username / Password";

                case "ProxySettingsForm.SystemProxy.Start":
                    return "System Proxy Enable";

                case "ProxySettingsForm.SystemProxy.Stop":
                    return "System Proxy Disable";

                case "ProxySettingsForm.ProxyType.Error":
                    return "Proxy Type Error";

                case "ProxySettingsForm.Success":
                    return "Proxy settings saved successfully";

                #endregion

                #region//LeachSetting

                case "LeachSetting":
                    return "Leach Setting";

                case "LeachSetting.IsShow":
                    return "Show or Not";

                case "LeachSetting.Length":
                    return "For example: 0-99;100";

                case "LeachSetting.Empty":
                    return "Leach Settings Empty";

                case "LeachSetting.Success":
                    return "Filter Settings Success";

                #endregion

                #region//HookSettingsForm

                case "HookSettingsForm":
                    return "Hook Settings";

                case "HookSettingsForm.Send1":
                    return "Send 1.1";

                case "HookSettingsForm.Recv1":
                    return "Recv 1.1";

                case "HookSettingsForm.SendTo1":
                    return "SendTo 1.1";

                case "HookSettingsForm.RecvFrom1":
                    return "RecvFrom 1.1";

                case "HookSettingsForm.Send":
                    return "Send";

                case "HookSettingsForm.SendTo":
                    return "SendTo";

                case "HookSettingsForm.Recv":
                    return "Recv";

                case "HookSettingsForm.RecvFrom":
                    return "RecvFrom";

                case "HookSettingsForm.WSASend":
                    return "WSASend";

                case "HookSettingsForm.WSASendTo":
                    return "WSASendTo";

                case "HookSettingsForm.WSARecv":
                    return "WSARecv";

                case "HookSettingsForm.WSARecvFrom":
                    return "WSARecvFrom";

                case "HookSettingsForm.TCP":
                    return "TCP Protocol";

                case "HookSettingsForm.UDP":
                    return "UDP Protocol";

                case "HookSettingsForm.Success":
                    return "Hook settings successfully";

                #endregion

                #region//ListSettingsForm

                case "ListSettingsForm":
                    return "List Settings";

                case "ListSettingsForm.PacketList":
                    return "Packet List";

                case "ListSettingsForm.LogList":
                    return "Log List";

                case "ListSettingsForm.TitleSetting":
                    return "Title Setting";

                case "ListSettingsForm.AutoRoll":
                    return "Auto Roll";

                case "ListSettingsForm.AutoClear":
                    return "Auto Clear";

                case "ListSettingsForm.Success":
                    return "List Settings Success";

                #endregion

                #region//MapSettingsForm

                case "MapSettingsForm":
                    return "Map Settings";

                case "MapSettingsForm.MapLocal":
                    return "Map Local";

                case "MapSettingsForm.MapRemote":
                    return "Map Remote";

                case "MapSettingsForm.MapLocal.Add":
                    return "Add Map Local";

                case "MapSettingsForm.MapLocal.Import":
                    return "Import Map Local";

                case "MapSettingsForm.MapLocal.Export":
                    return "Export Map Local";

                case "MapSettingsForm.MapLocal.Clear":
                    return "Clear Map Local";

                case "MapSettingsForm.MapRemote.Add":
                    return "Add Map Remote";

                case "MapSettingsForm.MapRemote.Import":
                    return "Import Map Remote";

                case "MapSettingsForm.MapRemote.Export":
                    return "Export Map Remote";

                case "MapSettingsForm.MapRemote.Clear":
                    return "Clear Map Remote";

                case "Table.MapLocal.Column.RemotePath":
                    return "Remote";

                case "Table.MapLocal.Column.LocalPath":
                    return "Local";

                case "Table.MapLocal.Column.CellLinks":
                    return "Operation";

                case "Table.MapRemote.Column.HostFrom":
                    return "From";

                case "Table.MapRemote.Column.HostTo":
                    return "To";

                case "Table.MapRemote.Column.CellLinks":
                    return "Operation";

                case "MapSettingsForm.Success":
                    return "Map settings successfully";

                #endregion

                #region//MapLocalForm

                case "MapLocalForm":
                    return "Map Local";

                case "MapLocalForm.Remote":
                    return "Remote";

                case "MapLocalForm.InputPath":
                    return "Please input remote path";

                case "MapLocalForm.Local":
                    return "Local";

                case "MapLocalForm.SelectLocal":
                    return "Please select a local file";

                case "MapLocalForm.DragFiles":
                    return "Click or drag files to this area";

                case "MapLocalForm.DragFilesText":
                    return "Please upload the local file and do not upload unsupported file";

                case "MapLocalForm.Empty":
                    return "Map Local Empty";

                case "MapLocalForm.Success":
                    return "Map Local Success";

                #endregion

                #region//MapRemoteForm

                case "MapRemoteForm":
                    return "Map Remote";

                case "MapRemoteForm.MapFrom":
                    return "Map From";

                case "MapRemoteForm.MapTo":
                    return "Map To";

                case "MapRemoteForm.InputPath":
                    return "Please input path";

                case "MapRemoteForm.Empty":
                    return "Map Remote Empty";

                case "MapRemoteForm.Success":
                    return "Map Remote Success";

                #endregion

                #region//EXTProxySettingsForm

                case "EXTProxySettingsForm":
                    return "EXTProxy Settings";

                case "EXTProxySettingsForm.EXTProxy":
                    return "External SOCKS proxy";

                case "EXTProxySettingsForm.InputIP":
                    return "Please enter IP or Domain";

                case "EXTProxySettingsForm.SpecifyPort":
                    return "Specify Port";

                case "EXTProxySettingsForm.Port":
                    return "Port:";

                case "EXTProxySettingsForm.RequireAuth":
                    return "Require Authentication";

                case "EXTProxySettingsForm.Username":
                    return "Username:";

                case "EXTProxySettingsForm.Password":
                    return "Password:";

                case "EXTProxySettingsForm.InputUsername":
                    return "Input Username";

                case "EXTProxySettingsForm.InputPassword":
                    return "Input Password";

                case "EXTProxySettingsForm.PortExample":
                    return "For example 80,443";

                case "EXTProxySettingsForm.ProxyIP.Empty":
                    return "EXTProxy address Empty";

                case "EXTProxySettingsForm.ProxyIP.Error":
                    return "EXTProxy address Error";

                case "EXTProxySettingsForm.SpecifyPort.Empty":
                    return "Specify Port Empty";

                case "EXTProxySettingsForm.UserName.Empty":
                    return "Username Empty";

                case "EXTProxySettingsForm.PassWord.Empty":
                    return "Password Empty";

                case "EXTProxySettingsForm.Connection":
                    return "EXTProxy connection successful";

                case "EXTProxySettingsForm.Success":
                    return "EXTProxy settings saved successfully";

                #endregion

                #region//HotKeyForm

                case "HotKeyForm":
                    return "HotKey Settings";

                case "HotKeyForm.Customize":
                    return "Customize HotKeys";

                case "HotKeyForm.Apply":
                    return "Apply To :";

                case "HotKeyForm.Key1":
                    return "HotKey 1 :";

                case "HotKeyForm.Key2":
                    return "HotKey 2 :";

                case "HotKeyForm.Key3":
                    return "HotKey 3 :";

                case "HotKeyForm.Key4":
                    return "HotKey 4 :";

                case "HotKeyForm.Key5":
                    return "HotKey 5 :";

                case "HotKeyForm.Key6":
                    return "HotKey 6 :";

                case "HotKeyForm.Key7":
                    return "HotKey 7 :";

                case "HotKeyForm.Key8":
                    return "HotKey 8 :";

                case "HotKeyForm.Key9":
                    return "HotKey 9 :";

                case "HotKeyForm.Key10":
                    return "HotKey 10 :";

                case "HotKeyForm.Key11":
                    return "HotKey 11 :";

                case "HotKeyForm.Key12":
                    return "HotKey 12 :";

                case "HotKeyForm.Success":
                    return "HotKey set successfully";

                case "HotKeyForm.Error":
                    return "HotKey set failed";

                case "HotKeyForm.InputHotKey":
                    return "Please enter the HotKey";

                case "HotKeyForm.Save.Success":
                    return "Save Hotkey successfully";

                #endregion

                #region//BackUpSettingsForm

                case "BackUpSettingsForm":
                    return "BackUp Settings";

                case "BackUpSettingsForm.SystemConfig":
                    return "System Config";

                case "BackUpSettingsForm.ProxyMode":
                    return "Proxy Mode";

                case "BackUpSettingsForm.InjectMode":
                    return "Inject Mode";

                case "BackUpSettingsForm.ListData":
                    return "List Data";

                case "BackUpSettingsForm.SystemOperation":
                    return "System operation configuration";

                case "BackUpSettingsForm.ProxyMode.Configuration":
                    return "Proxy mode configuration";

                case "BackUpSettingsForm.ProxyAccount":
                    return "Proxy Account";

                case "BackUpSettingsForm.ProxyMapping":
                    return "Proxy Mapping";

                case "BackUpSettingsForm.InjectMode.Configuration":
                    return "Inject Mode Configuration";

                case "BackUpSettingsForm.Import":
                    return "Import system backup";

                case "BackUpSettingsForm.Import.Success":
                    return "Successfully imported system backup";

                case "BackUpSettingsForm.Export":
                    return "Export system backup";

                case "BackUpSettingsForm.Export.Success":
                    return "Successfully exported system backup";

                case "BackUpSettingsForm.Export.Fail":
                    return "Export system backup failed";

                #endregion

                #region//SystemSettingsForm

                case "SystemSettingsForm":
                    return "System Settings";

                case "SystemSettingsForm.WorkMode":
                    return "Working Mode";

                case "SystemSettingsForm.FloatingButton":
                    return "Floating Button";

                case "SystemSettingsForm.ListMode":
                    return "List execution mode";

                case "SystemSettingsForm.FilterMode":
                    return "Filter execution mode";

                case "SystemSettingsForm.FilterAction":
                    return "Filter Action";

                case "SystemSettingsForm.SpeedMode":
                    return "Speed Mode";

                case "SystemSettingsForm.Simultaneously":
                    return "Execute Simultaneously";

                case "SystemSettingsForm.Order":
                    return "Execute in order";

                case "SystemSettingsForm.Priority":
                    return "Execute by priority";

                case "SystemSettingsForm.Success":
                    return "System settings saved successfully";

                case "SystemSettingsForm.BackColor":
                    return "Back Color";

                case "SystemSettingsForm.ForeColor":
                    return "Fore Color";

                #endregion

                #region//SearchPacketForm

                case "SearchPacketForm":
                    return "Search Packet";

                case "SearchPacketForm.FindText":
                    return "Find Text";

                case "SearchPacketForm.FindHex":
                    return "Find Hex";

                case "SearchPacketForm.FindNext":
                    return "Find Next";

                case "SearchPacketForm.FromScratch":
                    return "From Scratch";

                case "SearchPacketForm.SeekDown":
                    return "Seek Down";

                case "SearchPacketForm.Empty":
                    return "Search content Empty";

                case "SearchPacketForm.NoMatch":
                    return "No Match Found";

                #endregion

                #region//AccountEditForm

                case "AccountEditForm":
                    return "Account Edit";

                case "AccountEditForm.Username":
                    return "Username :";

                case "AccountEditForm.Password":
                    return "Password :";

                case "AccountEditForm.LimitLinks":
                    return "Limit Links :";

                case "AccountEditForm.LimitDevices":
                    return "Limit Devices :";

                case "AccountEditForm.ExpireTime":
                    return "Expire Time :";

                case "AccountEditForm.UserName.Empty":
                    return "Username Empty";

                case "AccountEditForm.Password.Empty":
                    return "Password Empty";

                case "AccountEditForm.UserName.Error":
                    return "Username already exists";

                case "AccountEditForm.Success":
                    return "Account saved successfully";

                #endregion

                #region//LocationForm

                case "LocationForm":
                    return "IP Location";

                case "Table.Location.Column.LoginTime":
                    return "Time";

                case "Table.Location.Column.LoginIP":
                    return "Login IP";

                case "Table.Location.Column.IPLocation":
                    return "Location";

                #endregion

                #region//ExpiryTimeForm

                case "ExpiryTimeForm":
                    return "Expiry Time";

                case "ExpiryTimeForm.FromExpiryTime":
                    return "Based on the Expiry Time";

                case "ExpiryTimeForm.FromNow":
                    return "Based on the Current Time";

                #endregion

                #region//LimitLinksForm

                case "LimitLinksForm":
                    return "Limit Links";

                case "LimitLinksForm.LimitLinks":
                    return "Limit Links :";

                #endregion

                #region//LimitDevicesForm

                case "LimitDevicesForm":
                    return "Limit Devices";

                case "LimitDevicesForm.LimitDevices":
                    return "Limit Devices :";

                #endregion

                #region//PacketEditForm

                case "PacketEditForm":
                    return "Packet Edit";

                case "PacketEditForm.UseSocket":
                    return "Use Socket :";

                case "PacketEditForm.ToAddr":
                    return "To Addr :";

                case "PacketEditForm.Length":
                    return "Length :";

                case "PacketEditForm.Send.ByTime":
                    return "By Time";

                case "PacketEditForm.Send.Continuously":
                    return "Continuously";

                case "PacketEditForm.Socket.Error":
                    return "Socket Error";

                case "PacketEditForm.Packet.Empty":
                    return "Packet data Empty";

                case "PacketEditForm.Position.Error":
                    return "Progressive position Error";

                case "PacketEditForm.Success":
                    return "Packet saved successfully";

                case "PacketEditForm.Error":
                    return "Packet save failed";

                #endregion

                #region//PacketModificationForm

                case "PacketModificationForm":
                    return "Packet Modification";

                case "PacketModificationForm.Raw":
                    return "Packet Raw Data ( Length {0} )";

                case "PacketModificationForm.Modified":
                    return "Packet Modified Data ( Length {0} )";

                case "Table.ComparisonText.Column.ChangeType":
                    return "Change Type";

                case "Table.ComparisonText.Column.ID":
                    return "ID";

                case "Table.ComparisonText.Column.Position":
                    return "Position";

                case "Table.ComparisonText.Column.ValueA":
                    return "Value A";

                case "Table.ComparisonText.Column.ValueB":
                    return "Value B";

                case "Table.ComparisonText.Column.CellLinks":
                    return "Operation";

                #endregion

                #region//FilterEditForm

                case "FilterEditForm":
                    return "Filter Edit";

                case "FilterEditForm.FilterAction":
                    return "Action";

                case "FilterEditForm.FilterName":
                    return "Filter Name";

                case "FilterEditForm.FilterName.Empty":
                    return "Filter Name Empty";

                case "FilterEditForm.AdvancedAppoint":
                    return "Advanced Appoint";

                case "FilterEditForm.Function":
                    return "Function";

                case "FilterEditForm.Appoint":
                    return "Appoint";

                case "FilterEditForm.StartFrom":
                    return "Start Modify From";

                case "FilterEditForm.Mode":
                    return "Mode";

                case "FilterEditForm.Progression":
                    return "Progression";

                case "FilterEditForm.Exclude.Error":
                    return "Cannot set Exclude for Empty";

                case "FilterEditForm.AppointHead":
                    return "Appoint Head";

                case "FilterEditForm.AppointHead.Error":
                    return "Appoint Head Error";

                case "FilterEditForm.AppointSocket.Error":
                    return "Appoint Socket Error";

                case "FilterEditForm.AppointPort.Error":
                    return "Appoint Port Error";

                case "FilterEditForm.AppointLength.Error":
                    return "Appoint Length Error";

                case "FilterEditForm.Change.Error":
                    return "Change Error";

                case "FilterEditForm.StartFrom.Packet":
                    return "Begin of the packet";

                case "FilterEditForm.StartFrom.Position":
                    return "Position of the chain";

                case "FilterEditForm.StartFrom.Progression.Continuous":
                    return "Continuous";

                case "FilterEditForm.StartFrom.Progression.Carry":
                    return "Carry";

                case "FilterEditForm.StartFrom.Progression.Step":
                    return "Step";

                case "FilterEditForm.StartFrom.Progression.Digits":
                    return "Digit";

                case "FilterEditForm.NoModifyNoDisplay":
                    return "NoModify - NoDisplay";

                case "FilterEditForm.NoModifyDisplay":
                    return "NoModify - Display";

                case "FilterEditForm.LoadComplete":
                    return "Loading Complete";

                case "FilterEditForm.Success":
                    return "Filter saved successfully";

                case "FilterEditForm.Error":
                    return "Filter save failed";

                case "FilterEditForm.Appoint.Example":
                    return "For example 80-89;100";

                case "FilterEditForm.Appoint.Progression.Enable":
                    return "Enable Progression";

                case "FilterEditForm.Appoint.Progression.Disable":
                    return "Disable Progression";

                case "FilterEditForm.Appoint.Exclude.Enable":
                    return "Enable Exclude";

                case "FilterEditForm.Appoint.Exclude.Disable":
                    return "Disable Exclude";

                #endregion

                #region//SendEditForm

                case "SendEditForm":
                    return "Send Edit";

                case "SendEditForm.TotalSend":
                    return "Total Send :";

                case "SendEditForm.SendSuccess":
                    return "Success :";

                case "SendEditForm.SendFailure":
                    return "Failure :";

                case "SendEditForm.LoopCount":
                    return "Loop Count";

                case "SendEditForm.SendInterval":
                    return "Send Interval";

                case "SendEditForm.UseSSocket":
                    return "Use System Socket";

                case "SendEditForm.Remarks":
                    return "Remarks";

                case "SendEditForm.Import":
                    return "Import Packet";

                case "SendEditForm.Export":
                    return "Export Packet";

                case "SendEditForm.Clear":
                    return "Clear Packet";

                case "SendEditForm.SystemSocket.Error":
                    return "System Socket Error";

                case "SendEditForm.Send.Stop":
                    return "Send has stopped";

                case "SendEditForm.Send.Error":
                    return "Send error occurred:";

                case "SendEditForm.Send.Success":
                    return "Send Completed";

                case "SendEditForm.SendName.Empty":
                    return "Send Name Empty";

                case "SendEditForm.Success":
                    return "Send saved successfully";

                case "SendEditForm.Error":
                    return "Send save failed";

                #endregion

                #region//RobotEditForm

                case "RobotEditForm":
                    return "Robot Edit";

                case "RobotEditForm.ciPacketINST":
                    return "Packet Instruction";

                case "RobotEditForm.ciControlINST":
                    return "Control Instruction";

                case "RobotEditForm.ciKeyBoardINST":
                    return "KeyBoard Instruction";

                case "RobotEditForm.ciMouseINST":
                    return "Mouse Instruction";

                case "RobotEditForm.SendList.Error":
                    return "Send List Error";

                case "RobotEditForm.LoopINST.Error":
                    return "Loop Instruction Error";

                case "RobotEditForm.RName":
                    return "Robot Name:";

                case "RobotEditForm.Record":
                    return "Record:";

                case "RobotEditForm.PacketINST.SendList":
                    return "Send - Send List";

                case "RobotEditForm.PacketINST.PacketList":
                    return "Send - Packet List";

                case "RobotEditForm.PacketINST.PacketList.Select":
                    return "Selected packet in the Packet List";

                case "RobotEditForm.PacketINST.SetSSocket":
                    return "Set - System Socket";

                case "RobotEditForm.PacketINST.SetSSocket.Select":
                    return "Socket of the selected packet";

                case "RobotEditForm.PacketINST.SetSSocket.Filter":
                    return "Socket for calling filter";

                case "RobotEditForm.ControlINST.Delay":
                    return "Delay";

                case "RobotEditForm.ControlINST.Delay.Fixed":
                    return "Fixed";

                case "RobotEditForm.ControlINST.Delay.Random":
                    return "Random";

                case "RobotEditForm.ControlINST.Loop":
                    return "Loop";

                case "RobotEditForm.KeyBoardINST.Key":
                    return "Key :";

                case "RobotEditForm.KeyBoardINST.KeyPress":
                    return "Key Press";

                case "RobotEditForm.KeyBoardINST.KeyDown":
                    return "Key Down";

                case "RobotEditForm.KeyBoardINST.KeyUp":
                    return "Key Up";

                case "RobotEditForm.KeyBoardINST.Key.Type":
                    return "Type :";

                case "RobotEditForm.KeyBoardINST.CombinationKey":
                    return "Combination Key";

                case "RobotEditForm.KeyBoardINST.Text":
                    return "Text";

                case "RobotEditForm.MouseINST.Key":
                    return "Key :";

                case "RobotEditForm.MouseINST.Wheel":
                    return "Wheel";

                case "RobotEditForm.MouseINST.Wheel.Scroll":
                    return "Scroll :";

                case "RobotEditForm.MouseINST.Wheel.Distance":
                    return "Distance :";

                case "RobotEditForm.INST":
                    return "Inst {0}";

                case "RobotEditForm.INST.Send":
                    return "Send";

                case "RobotEditForm.INST.Set":
                    return "Set";

                case "RobotEditForm.INST.Delay":
                    return "Delay";

                case "RobotEditForm.INST.LoopBegin":
                    return "Loop Begin";

                case "RobotEditForm.INST.LoopEnd":
                    return "Loop End";

                case "RobotEditForm.INST.KeyBoard":
                    return "KeyBoard";

                case "RobotEditForm.INST.Mouse":
                    return "Mouse";

                case "RobotEditForm.INST.Send.SendList":
                    return "Send List - [{0}]";

                case "RobotEditForm.INST.PacketList.Select":
                    return "[Packet List] Selected packet";

                case "RobotEditForm.INST.Socket.SelectPacket":
                    return "System Socket = Selected packet socket";

                case "RobotEditForm.INST.Socket.CallFilter":
                    return "System Socket = Socket for calling filter";

                case "RobotEditForm.INST.Socket.Customize":
                    return "System Socket = {0}";

                case "RobotEditForm.INST.Socket.Millisecond":
                    return "{0} ms";

                case "RobotEditForm.INST.Loop.Begin":
                    return "Loop {0} Times";

                case "RobotEditForm.INST.Loop.End":
                    return "Loop End";

                case "RobotEditForm.INST.KeyPress":
                    return "Key Press {0}";

                case "RobotEditForm.INST.KeyDown":
                    return "Key Down {0}";

                case "RobotEditForm.INST.KeyUp":
                    return "Key Up {0}";

                case "RobotEditForm.INST.KeyCombine":
                    return "key Combination {0}";

                case "RobotEditForm.INST.KeyText":
                    return "Input Text {0}";

                case "RobotEditForm.INST.LeftClick":
                    return "Left Click";

                case "RobotEditForm.INST.RightClick":
                    return "Right Click";

                case "RobotEditForm.INST.LeftDBClick":
                    return "Left DBClick";

                case "RobotEditForm.INST.RightDBClick":
                    return "Right DBClick";

                case "RobotEditForm.INST.LeftDown":
                    return "Left Down";

                case "RobotEditForm.INST.LeftUp":
                    return "Left Up";

                case "RobotEditForm.INST.RightDown":
                    return "Right Down";

                case "RobotEditForm.INST.RightUp":
                    return "Right Up";

                case "RobotEditForm.INST.WheelUp":
                    return "Wheel Up {0}";

                case "RobotEditForm.INST.WheelDown":
                    return "Wheel Down {0}";

                case "RobotEditForm.INST.MoveTo":
                    return "Move To ( {0} )";

                case "RobotEditForm.INST.MoveBy":
                    return "Move By ( {0} )";

                case "RobotEditForm.Robot.Stop":
                    return "Robot has stopped";

                case "RobotEditForm.Robot.Error":
                    return "Robot error occurred:";

                case "RobotEditForm.Robot.Success":
                    return "Robot Completed";

                case "RobotEditForm.RName.Empty":
                    return "Robot Name Empty";

                case "RobotEditForm.Success":
                    return "Robot saved successfully";

                case "RobotEditForm.Error":
                    return "Robot save failed";

                #endregion

                #region//EncryptionPassword

                case "EncryptionPassword.Info":
                    return "Please input password! If you don't need to input password, simply click the [ Cancel ] button!";             

                #endregion

                default:
                    return null;
            }
        }
    }
}
