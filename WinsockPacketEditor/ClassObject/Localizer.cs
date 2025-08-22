
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

                case "Execute":
                    return "Execute";

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

                case "HexWithSpaces":
                    return "Please enter Hex with spaces";

                case "SureToDelete":
                    return "Are you sure to delete all data?";

                case "ExcelFile":
                    return "Excel File";

                case "Exporting":
                    return "Exporting...";

                case "PleaseSelect":
                    return "Please select";

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

                #endregion

                #region//Operate

                case "ExportToExcel.Success":
                    return "Export To Excel Success";

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

                case "SemicolonDelimiter":
                    return "Support ; delimiter";                

                case "HEXSemicolonDelimiter":
                    return "Hex with spaces, Support ; delimiter";

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

                #region//FilterList

                case "FilterList.NewFilter":
                    return "Filter {0}";

                case "FilterList.ResetCount":
                    return "Reset Count";

                case "FilterList.miAdd":
                    return "Add Filter";

                case "FilterList.miImport":
                    return "Import Filter";

                case "FilterList.miExport":
                    return "Export Filter";

                case "FilterList.miClear":
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

                case "SendList.miAdd":
                    return "Add Send";

                case "SendList.miImport":
                    return "Import Send";

                case "SendList.miExport":
                    return "Export Send";

                case "SendList.miClear":
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

                #endregion

                #region//RobotList

                case "RobotList.NewRobot":
                    return "Robot {0}";

                case "RobotList.miAdd":
                    return "Add Robot";

                case "RobotList.miImport":
                    return "Import Robot";

                case "RobotList.miExport":
                    return "Export Robot";

                case "RobotList.miClear":
                    return "Clear Robot";

                case "Table.RobotList.Column.IsEnable":
                    return "Enable";

                case "Table.RobotList.Column.RName":
                    return "Robot Name";

                case "Table.RobotList.Column.Status":
                    return "Status";

                case "Table.RobotList.Column.ExecutionCount":
                    return "Execution";

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
                    return "Text A  ( Length {0} )";

                case "ComparisonText.TextB":
                    return "Text B  ( Length {0} )";

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
                    return "[ Charles XML session file（.chlsx）] Extraction [ HEX Data ]";

                case "ExtractionData.Filt":
                    return "[ FILT Filter file（.filt）] Extraction [ WPE64 Filter file（.sp）]";

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

                case "Table.LogList.Column.ID":
                    return "ID";

                case "Table.LogList.Column.LogTime":
                    return "Time";

                case "Table.LogList.Column.FuncName":
                    return "Module";

                case "Table.LogList.Column.LogContent":
                    return "Content";

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

                #region//FilterSettingsForm

                case "FilterSettingsForm":
                    return "Filter Settings";

                case "FilterSettingsForm.IsShow":
                    return "Show or Not";

                case "FilterSettingsForm.Length":
                    return "For example: 0-99;100";

                case "FilterSettingsForm.FilterEmpty":
                    return "Filter Settings Empty";

                case "FilterSettingsForm.Success":
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

                case "MapSettingsForm.MapLocal.miAdd":
                    return "Add Map Local";

                case "MapSettingsForm.MapLocal.miImport":
                    return "Import Map Local";

                case "MapSettingsForm.MapLocal.miExport":
                    return "Export Map Local";

                case "MapSettingsForm.MapLocal.miClear":
                    return "Clear Map Local";

                case "MapSettingsForm.MapRemote.miAdd":
                    return "Add Map Remote";

                case "MapSettingsForm.MapRemote.miImport":
                    return "Import Map Remote";

                case "MapSettingsForm.MapRemote.miExport":
                    return "Export Map Remote";

                case "MapSettingsForm.MapRemote.miClear":
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

                case "HotKeyForm.Apply":
                    return "HotKeys - Apply to Send and Robot List";

                case "HotKeyForm.Key1":
                    return "HotKey 1";

                case "HotKeyForm.Key2":
                    return "HotKey 2";

                case "HotKeyForm.Key3":
                    return "HotKey 3";

                case "HotKeyForm.Key4":
                    return "HotKey 4";

                case "HotKeyForm.Key5":
                    return "HotKey 5";

                case "HotKeyForm.Key6":
                    return "HotKey 6";

                case "HotKeyForm.Key7":
                    return "HotKey 7";

                case "HotKeyForm.Key8":
                    return "HotKey 8";

                case "HotKeyForm.Key9":
                    return "HotKey 9";

                case "HotKeyForm.Key10":
                    return "HotKey 10";

                case "HotKeyForm.Key11":
                    return "HotKey 11";

                case "HotKeyForm.Key12":
                    return "HotKey 12";

                case "HotKeyForm.Success":
                    return "HotKey set successfully";

                case "HotKeyForm.Error":
                    return "HotKey set failed";

                case "HotKeyForm.InputHotKey":
                    return "Please enter the HotKey";

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
