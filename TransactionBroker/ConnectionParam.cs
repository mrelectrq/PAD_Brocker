using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TransactionBroker
{
   public class ConnectionParam
    {
        public const int BUFF_SIZE = 5050;
        public Socket Socket { get; set; }
        public string Address { get; set; }
        
        public byte[] Context { get; set; }
        public DateTime TimeStamp { get; set; }

        public ConnectionParam()
        {
            Context = new byte[BUFF_SIZE];
        }
    }
}
