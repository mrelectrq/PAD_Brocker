using API_Transaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionBroker.Storage.Tests
{
    [TestClass()]
    public class RequestStorageTests
    {
        [TestMethod()]
        public void AddTransactionTest()
        {


            var transaction = new Transaction()
            {
                owner_card_id = "gafdwaaw",
                aditional_comment = "gadwad",
                transactionType = API_Transaction.Enums.TransactionType.Authorization,
                ccy = "MDL",
                transaction_summ = 1234
            };
            var sender = new TransactionProtocol()
            {
                Type_message = API_Transaction.Enums.TypeMessage.add,
                Transaction = JsonConvert.SerializeObject(transaction),
                Request_id = "51231254",
                Sender_id = "5123124",

            };

           // var manager = new ApiManager();
            RequestStorage.GetInstance().AddTransaction(sender);

            var test_response = RequestStorage.GetInstance().GetTransaction();


            
        }
    }
}