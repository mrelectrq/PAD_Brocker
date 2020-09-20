using API_Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TransactionBroker.Storage
{
    public class RequestStorage
    {
        private static readonly Lazy<RequestStorage> Storage =
            new Lazy<RequestStorage>(() => new RequestStorage());
        private static ConcurrentQueue<TransactionProtocol> Transactions;
        private object _lock = new object();
        private static DateTime timestamp = DateTime.Now.AddMinutes(1);
        private RequestStorage()
        {
            if(Transactions is null)
            {
                GetStorage();
            }
        }

        public RequestStorage GetInstance()
        {
            return Storage.Value;
        }

        public TransactionProtocol GetTransaction()
        {
            TransactionProtocol transaction = new TransactionProtocol();
            var status = false;
            while(status)
            {
                status = Transactions.TryDequeue(out transaction);
            }
            return transaction;
        }

        public void AddTransaction(TransactionProtocol protocol)
        {
            Transactions.Enqueue(protocol);
            lock (_lock)
            {
                if (timestamp < DateTime.Now)
                {
                    LoadStorage();
                    timestamp = DateTime.Now.AddMinutes(1);
                }
            }
        }

        private void LoadStorage()
        {
            string storage_path = Config.STORAGE_PATH;
            string storage_name = Config.STORAGE_NAME;
            string path_comb = Path.Combine(storage_name, storage_path);

            using (Stream stream = File.Open(path_comb, FileMode.Create))
            {
                var formater = new BinaryFormatter();
                formater.Serialize(stream, Transactions);
            }
        }
        private void GetStorage()
        {
            string storage_path = Config.STORAGE_PATH;
            string storage_name = Config.STORAGE_NAME;
            string path_comb = Path.Combine(storage_name, storage_path);
            
            using (Stream stream = File.Open(path_comb, FileMode.Open))
            {
                var formater = new BinaryFormatter();
                List<TransactionProtocol> storage = (List<TransactionProtocol>)formater.Deserialize(stream);
                if(storage == null)
                {
                    Transactions = new ConcurrentQueue<TransactionProtocol>();
                }else
                {
                    foreach(var transaction in storage)
                    {
                        Transactions.Enqueue(transaction);
                    }
                }
            }
        }
    }
}
