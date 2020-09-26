using API_Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TransactionBroker.Storage
{
    public class RequestStorage
    {
        private static readonly Lazy<RequestStorage> storage =
            new Lazy<RequestStorage>(() => new RequestStorage());
        private static ConcurrentQueue<TransactionProtocol> transactions;
        private object _lock = new object();
        private static DateTime timestamp = DateTime.Now.AddMinutes(1);
        private RequestStorage()
        {
            if(transactions is null)
            {
                GetStorage();
               
            }
        }

        public static RequestStorage GetInstance()
        {
            return storage.Value;
        }

        public TransactionProtocol GetTransaction()
        {
            TransactionProtocol transaction = new TransactionProtocol();
            var status = false;
            while(!status)
            {
                status = transactions.TryDequeue(out transaction);
            }
            return transaction;
        }

        public void AddTransaction(TransactionProtocol protocol)
        {
            transactions.Enqueue(protocol);
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

                TransactionProtocol[] transact_array = new TransactionProtocol[transactions.Count];
                transactions.CopyTo(transact_array, 0);
                var list_transact = transact_array.OfType<TransactionProtocol>().ToList();
                formater.Serialize(stream, list_transact);
            }
        }
        private void GetStorage()
        {
            string storage_path = Config.STORAGE_PATH;
            string storage_name = Config.STORAGE_NAME;
            string path_comb = Path.Combine(storage_name, storage_path);
            
            using (Stream stream = File.Open(storage_path, FileMode.Open))
            {
                var formater = new BinaryFormatter();

                List<TransactionProtocol> storage; //= (List<TransactionProtocol>)formater.Deserialize(stream);
                transactions = new ConcurrentQueue<TransactionProtocol>();
                try
                {
                    storage = (List<TransactionProtocol>)formater.Deserialize(stream);
                    foreach (var transaction in storage)
                    {
                        
                        transactions.Enqueue(transaction);
                    }
                }
                catch
                {
                    transactions = new ConcurrentQueue<TransactionProtocol>();
                }
                //if(storage == null)
                //{
                //    transactions = new ConcurrentQueue<TransactionProtocol>();
                //}else
                //{

                //}
            }
        }
    }
}
