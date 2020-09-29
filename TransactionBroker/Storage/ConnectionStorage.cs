using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionBroker.Storage
{
    public class ConnectionStorage
    {
        private static readonly ConnectionStorage storage = new ConnectionStorage();
        private static ConcurrentDictionary<string, ConnectionParam> connections
            = new ConcurrentDictionary<string, ConnectionParam>();
        private ConnectionStorage()
        {
        }


        public static ConnectionStorage GetInstance()
        {
            return storage;
        }

        public bool AddConnection(ConnectionParam connection,string request_id)
        {
            var status = connections.TryAdd(request_id, connection);
            return status;
        }

        public void ExcludeConnection(ConnectionParam connection)
        {
            var request_id = connections.Where(m => m.Value.Address == connection.Address)
                .Select(m => m.Key).FirstOrDefault();
            bool status = false;
            ConnectionParam discard;
            while (status)
            {
               status=connections.TryRemove(request_id,out discard);
            }
           
        }

        public void ExcludeConnection(string request_id)
        {
            ConnectionParam discard;
            bool status = false;
            while(!status)
            {
                status = connections.TryRemove(request_id, out discard);
            }
        }

        public ConnectionParam GetConnection(string request_id)
        {
            var result = connections.Where(m => m.Key == request_id)
                .Select(m => m.Value).FirstOrDefault();
            return result;
        }
    }
}
