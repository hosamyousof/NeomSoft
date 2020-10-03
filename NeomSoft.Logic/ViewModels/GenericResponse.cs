using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NeomSoft.Logic.ViewModels
{
    public class GenericResponse<T>
    {
        // temporary read from Dictionary
        //todo: read from resource file 
        private Dictionary<string, string> msgCodes = new Dictionary<string, string> {
            { "SuccessUpdate","تم الحفظ بنجاح"},
            { "ErrorSaving","حدث خطأ أثناء الحفظ"},
        };

        public GenericResponse()
        {
            IsSuccess = false;
        }

        public bool IsSuccess { get; set; }
        public MessageCode? MsgCode { get; set; }

        private string _Message;
        public string Message
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_Message))
                    {
                        //var mng = new ResourceManager(typeof(SystemResponseMessages));
                        if (MsgCode.HasValue)
                        {
                            //return mng.GetString(MsgCode.ToString());
                            return msgCodes[MsgCode.Value.ToString()];
                        }
                        else
                        {
                            //return IsSuccess ? mng.GetString(MessageCode.SuccessUpdate.ToString()) : mng.GetString(MessageCode.ErrorSaving.ToString());
                            return IsSuccess ? msgCodes[MessageCode.SuccessUpdate.ToString()] : msgCodes[MessageCode.ErrorSaving.ToString()];
                        }
                    }
                }
                catch
                {
                }
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }

        public T ReturnedValue { get; set; }

    }

    public enum MessageCode
    {

        SuccessUpdate,

        ErrorSaving,
    }
}


