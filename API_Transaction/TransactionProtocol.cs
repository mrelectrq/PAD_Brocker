using API_Transaction.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Transaction
{
    [Serializable()]
    public class TransactionProtocol
    {
        public string Request_id { get; set; }
        public string Sender_id { get; set; }
        public TypeMessage Type_message { get; set; }
        public string Transaction { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public bool isTestament { get; set; }
    }
}
