using AntdUI;
using System;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    public enum RuleType
    {
        [Description("DOMAIN")]
        DOMAIN = 0,

        [Description("DOMAIN-SUFFIX")]
        DOMAIN_SUFFIX = 1,

        [Description("DOMAIN-KEYWORD")]
        DOMAIN_KEYWORD = 2,

        [Description("DOMAIN-REGEX")]
        DOMAIN_REGEX = 3,

        [Description("GEOIP")]
        GEOIP = 4,

        [Description("GEOSITE")]
        GEOSITE = 5,

        [Description("IP-CIDR")]
        IP_CIDR = 6,

        [Description("IP-CIDR6")]
        IP_CIDR6 = 7,

        [Description("SRC-IP-CIDR")]
        SRC_IP_CIDR = 8,

        [Description("SRC-PORT")]
        SRC_PORT = 9,

        [Description("DST-PORT")]
        DST_PORT = 10,

        [Description("PROCESS-NAME")]
        PROCESS_NAME = 11,

        [Description("PROCESS-PATH")]
        PROCESS_PATH = 12,

        [Description("NETWORK")]
        NETWORK = 13,

        [Description("RULE-SET")]
        RULE_SET = 14,

        [Description("MATCH")]
        MATCH = 15,

        [Description("AND")]
        AND = 16,

        [Description("OR")]
        OR = 17,

        [Description("NOT")]
        NOT = 18,

        [Description("SUB-RULE")]
        SUB_RULE = 19,

        [Description("IN-PORT")]
        IN_PORT = 20,

        [Description("UI-EX")]
        UI_EX = 21,

        [Description("COMMAND")]
        COMMAND = 22,

        [Description("DEVICE-NAME")]
        DEVICE_NAME = 23,
    }

    public enum RuleAction
    {
        PROXY = 0,
        REJECT = 1,
        DIRECT = 2,
    }

    public class RuleInfo : NotifyProperty
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

        #region//规则编号

        private Guid _RID;

        public Guid RID
        {
            get => _RID;
            set
            {
                if (_RID == value) return;
                _RID = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region //规则类型

        private RuleType _RType;

        public RuleType RType
        {
            get => _RType;
            set
            {
                if (_RType == value) return;
                _RType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region //规则参数

        private string _RArgument = string.Empty;

        public string RArgument
        {
            get => _RArgument;
            set
            {
                if (_RArgument == value) return;
                _RArgument = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region //规则动作

        private RuleAction _RAction = RuleAction.PROXY;

        public RuleAction RAction
        {
            get => _RAction;
            set
            {
                if (_RAction == value) return;
                _RAction = value;
                OnPropertyChanged();
            }
        }

        #endregion  

        #region//RuleInfo

        public RuleInfo(bool IsEnable, Guid RID, RuleType RType, string RArgument, RuleAction RAction)
        {
            this._IsEnable = IsEnable;
            this._RID = RID;
            this._RType = RType;
            this._RArgument = RArgument;
            this._RAction = RAction;
        }

        #endregion
    }
}