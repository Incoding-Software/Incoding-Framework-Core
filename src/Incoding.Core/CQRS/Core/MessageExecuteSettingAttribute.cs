using System;
using System.Data;

namespace Incoding.Core.CQRS.Core
{
    #region << Using >>

    #endregion

    public class MessageExecuteSettingAttribute : Attribute
    {
        #region Constructors

        public MessageExecuteSettingAttribute()
        {
            DataBaseInstance = string.Empty;            
        }

        #endregion

        #region Properties

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        #endregion
    }
}