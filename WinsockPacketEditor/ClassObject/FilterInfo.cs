using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class FilterInfo : NotifyProperty
    {
        #region//是否启用        

        bool _IsEnable;

        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                if (_IsEnable == value) return;
                _IsEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//滤镜序号

        Guid _FID;

        public Guid FID
        {
            get => _FID;
            set
            {
                if (_FID == value) return;
                _FID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//滤镜名称

        string _FName;

        public string FName
        {
            get => _FName;
            set
            {
                if (_FName == value) return;
                _FName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//滤镜已执行次数

        long _ExecutionCount;

        public long ExecutionCount
        {
            get => _ExecutionCount;
            set
            {
                if (_ExecutionCount == value) return;
                _ExecutionCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//指定包头

        bool _AppointHeader;

        public bool AppointHeader
        {
            get => _AppointHeader;
            set
            {
                if (_AppointHeader == value) return;
                _AppointHeader = value;
                OnPropertyChanged();
            }
        }

        string _HeaderContent;

        public string HeaderContent
        {
            get => _HeaderContent;
            set
            {
                if (_HeaderContent == value) return;
                _HeaderContent = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//指定套接字

        bool _AppointSocket;

        public bool AppointSocket
        {
            get => _AppointSocket;
            set
            {
                if (_AppointSocket == value) return;
                _AppointSocket = value;
                OnPropertyChanged();
            }
        }

        string _SocketContent;

        public string SocketContent
        {
            get => _SocketContent;
            set
            {
                if (_SocketContent == value) return;
                _SocketContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//指定长度

        bool _AppointLength;

        public bool AppointLength
        {
            get => _AppointLength;
            set
            {
                if (_AppointLength == value) return;
                _AppointLength = value;
                OnPropertyChanged();
            }
        }

        string _LengthContent;

        public string LengthContent
        {
            get => _LengthContent;
            set
            {
                if (_LengthContent == value) return;
                _LengthContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//指定端口

        bool _AppointPort;

        public bool AppointPort
        {
            get => _AppointPort;
            set
            {
                if (_AppointPort == value) return;
                _AppointPort = value;
                OnPropertyChanged();
            }
        }

        string _PortContent;

        public string PortContent
        {
            get => _PortContent;
            set
            {
                if (_PortContent == value) return;
                _PortContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//模式

        Operate.FilterConfig.Filter.FilterMode _FMode;

        public Operate.FilterConfig.Filter.FilterMode FMode
        {
            get => _FMode;
            set
            {
                if (_FMode == value) return;
                _FMode = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//动作

        Operate.FilterConfig.Filter.FilterAction _FAction;

        public Operate.FilterConfig.Filter.FilterAction FAction
        {
            get => _FAction;
            set
            {
                if (_FAction == value) return;
                _FAction = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//是否执行

        bool _IsExecute;

        public bool IsExecute
        {
            get => _IsExecute;
            set
            {
                if (_IsExecute == value) return;
                _IsExecute = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//执行类型

        Operate.FilterConfig.Filter.FilterExecuteType _FEType;

        public Operate.FilterConfig.Filter.FilterExecuteType FEType
        {
            get => _FEType;
            set
            {
                if (_FEType == value) return;
                _FEType = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//执行发送序号

        Guid _Execute_SID;

        public Guid Execute_SID
        {
            get => _Execute_SID;
            set
            {
                if (_Execute_SID == value) return;
                _Execute_SID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//执行机器人序号

        Guid _Execute_RID;

        public Guid Execute_RID
        {
            get => _Execute_RID;
            set
            {
                if (_Execute_RID == value) return;
                _Execute_RID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//执行滤镜序号

        Guid _Execute_FID;

        public Guid Execute_FID
        {
            get => _Execute_FID;
            set
            {
                if (_Execute_FID == value) return;
                _Execute_FID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//作用类别

        Operate.FilterConfig.Filter.FilterFunction _FFunction;

        public Operate.FilterConfig.Filter.FilterFunction FFunction
        {
            get => _FFunction;
            set
            {                
                _FFunction = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//修改起始于

        Operate.FilterConfig.Filter.FilterStartFrom _FStartFrom;

        public Operate.FilterConfig.Filter.FilterStartFrom FStartFrom
        {
            get => _FStartFrom;
            set
            {
                if (_FStartFrom == value) return;
                _FStartFrom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否递进完成

        bool _IsProgressionDone;

        public bool IsProgressionDone
        {
            get => _IsProgressionDone;
            set
            {
                if (_IsProgressionDone == value) return;
                _IsProgressionDone = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否连续递进

        bool _IsProgressionContinuous;

        public bool IsProgressionContinuous
        {
            get => _IsProgressionContinuous;
            set
            {
                if (_IsProgressionContinuous == value) return;
                _IsProgressionContinuous = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//递进步长

        int _ProgressionStep;

        public int ProgressionStep
        {
            get => _ProgressionStep;
            set
            {
                if (_ProgressionStep == value) return;
                _ProgressionStep = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否进位递进

        bool _IsProgressionCarry;

        public bool IsProgressionCarry
        {
            get => _IsProgressionCarry;
            set
            {
                if (_IsProgressionCarry == value) return;
                _IsProgressionCarry = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//进位位数

        int _ProgressionCarryNumber;

        public int ProgressionCarryNumber
        {
            get => _ProgressionCarryNumber;
            set
            {
                if (_ProgressionCarryNumber == value) return;
                _ProgressionCarryNumber = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//递进位置

        string _ProgressionPosition;

        public string ProgressionPosition
        {
            get => _ProgressionPosition;
            set
            {
                if (_ProgressionPosition == value) return;
                _ProgressionPosition = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//递进已执行次数

        int _ProgressionCount;

        public int ProgressionCount
        {
            get => _ProgressionCount;
            set
            {
                if (_ProgressionCount == value) return;
                _ProgressionCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//排除位置

        string _ExcludePosition;

        public string ExcludePosition
        {
            get => _ExcludePosition;
            set
            {
                if (_ExcludePosition == value) return;
                _ExcludePosition = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//搜索内容

        string _FSearch;

        public string FSearch
        {
            get => _FSearch;
            set
            {
                if (_FSearch == value) return;
                _FSearch = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//修改内容

        string _FModify;

        public string FModify
        {
            get => _FModify;
            set
            {
                if (_FModify == value) return;
                _FModify = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//FilterInfo

        public FilterInfo(
            bool IsEnable, 
            Guid FID,
            string FName, 
            bool AppointHeader, 
            string HeaderContent, 
            bool AppointSocket,
            string SocketContent, 
            bool AppointLength, 
            string LengthContent,
            bool AppointPort,
            string PortContent,
            Operate.FilterConfig.Filter.FilterMode FMode, 
            Operate.FilterConfig.Filter.FilterAction FAction,
            bool IsExecute,
            Operate.FilterConfig.Filter.FilterExecuteType FEType,
            Guid Execute_SID,
            Guid Execute_RID,
            Guid Execute_FID,
            Operate.FilterConfig.Filter.FilterFunction FFunction, 
            Operate.FilterConfig.Filter.FilterStartFrom FStartFrom,
            bool IsProgressionDone,
            bool IsProgressionContinuous,
            int ProgressionStep,
            bool IsProgressionCarry,
            int ProgressionCarryNumber,
            string ProgressionPosition,
            int ProgressionCount,
            string ExcludePosition,
            string FSearch, 
            string FModify) 
        {
            this._IsEnable = IsEnable;
            this._FID = FID;            
            this._FName = FName;        
            this._AppointHeader = AppointHeader;
            this._HeaderContent = HeaderContent;
            this._AppointSocket = AppointSocket;
            this._SocketContent = SocketContent;
            this._AppointLength = AppointLength;
            this._LengthContent = LengthContent;
            this._AppointPort = AppointPort;
            this._PortContent = PortContent;
            this._FMode = FMode;
            this._FAction = FAction;
            this._IsExecute = IsExecute;
            this._FEType = FEType;
            this._Execute_SID = Execute_SID;
            this._Execute_RID = Execute_RID;
            this._Execute_FID = Execute_FID;
            this._FFunction = FFunction;
            this._FStartFrom = FStartFrom;
            this._IsProgressionDone = IsProgressionDone;
            this._IsProgressionContinuous = IsProgressionContinuous;
            this._ProgressionStep = ProgressionStep;
            this._IsProgressionCarry = IsProgressionCarry;
            this._ProgressionCarryNumber = ProgressionCarryNumber;
            this._ProgressionPosition = ProgressionPosition;
            this._ProgressionCount = ProgressionCount;
            this._ExcludePosition = ExcludePosition;
            this._FSearch = FSearch;          
            this._FModify = FModify;            
            this._ExecutionCount = 0;            
        }

        #endregion
    }
}
