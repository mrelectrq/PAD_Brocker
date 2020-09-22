using API_Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionBroker.Storage;

namespace TransactionBroker.Implementation
{
    public class BrokerImplementation
    {
        internal void AddHandler(TransactionProtocol message, ConnectionParam connection)
        {
            if (string.IsNullOrWhiteSpace(message.Request_id) || string.IsNullOrWhiteSpace(message.Sender_id)
                || string.IsNullOrWhiteSpace(message.Transaction))
            {
                ErrorHandler(message, connection);
                return;
            }
            var request_storage = RequestStorage.GetInstance();
            request_storage.AddTransaction(message);
            var connection_storage = ConnectionStorage.GetInstance();
            connection_storage.AddConnection(connection, message.Request_id);
        }

        internal void ResponseHandler(TransactionProtocol message, ConnectionParam connection)
        {
            if (string.IsNullOrWhiteSpace(message.Request_id) || string.IsNullOrWhiteSpace(message.Sender_id)
                || string.IsNullOrWhiteSpace(message.Transaction))
            {
                ErrorHandler(message, connection);
                return;
            }
            var sender_connection = ConnectionStorage.GetInstance().GetConnection(message.Request_id);
            if (sender_connection is null)
            {
                ErrorHandler(message, connection);
                return;
            }
            var message_buffer = ConvertToByteArray(message);
            sender_connection.Socket.Send(message_buffer);

        }

        internal void GiveHandler(TransactionProtocol message, ConnectionParam connection)
        {
            if (string.IsNullOrWhiteSpace(message.Transaction) == false)
            {
                ErrorHandler(message, connection);
            }
            var task = RequestStorage.GetInstance().GetTransaction();
            var buffer = ConvertToByteArray(task);
            connection.Socket.Send(buffer);

        }

        internal void ErrorHandler(TransactionProtocol message, ConnectionParam connection)
        {
            message.Type_message = API_Transaction.Enums.TypeMessage.error;
            var byte_message = ConvertToByteArray(message);
            connection.Socket.Send(byte_message);
        }

        private byte[] ConvertToByteArray(TransactionProtocol message)
        {
            var transact_format = JsonConvert.SerializeObject(message);
            var byte_array = UTF8Encoding.UTF8.GetBytes(transact_format);
            return byte_array;
        }

    }
}
