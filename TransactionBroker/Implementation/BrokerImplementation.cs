using API_Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionBroker.Storage;

namespace TransactionBroker.Implementation
{
    public class BrokerImplementation
    {
         internal void AddHandler(TransactionProtocol message,ConnectionParam connection)
        {
            if (string.IsNullOrWhiteSpace(message.Transaction))
            {
                SendError(message, connection);
                return;
            }else if(string.IsNullOrWhiteSpace(message.Request_id) || string.IsNullOrWhiteSpace(message.Sender_id))
            {
                SendError(message, connection);
                return;
            }
            RequestStorage.GetInstance().AddTransaction(message);

        }

        internal void ResponseHandler(TransactionProtocol message, ConnectionParam connection)
        {

        }

        internal void GiveHandler(TransactionProtocol message, ConnectionParam connection)
        {

        }

        internal void SendError(TransactionProtocol message,ConnectionParam connection)
        {

        }
    }
}
