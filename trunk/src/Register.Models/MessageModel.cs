using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public enum EMessageType
    {
        Information,
        Warning,
        Error,
        Success
    }
    public class MessageModel
    {
        public EMessageType MessageType { get; set; }
        public string Message { get; set; }

        public string Message2 { get; set; }

        public string ReturnUrl { get; set; }
        public string ReturnText { get; set; }

        public string Title { get; set; }
    }
}
