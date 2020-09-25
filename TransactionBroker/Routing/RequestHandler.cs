using API_Transaction;
using API_Transaction.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionBroker.Implementation;
using TransactionBroker.Interfaces;

namespace TransactionBroker.Routing
{

    public class RequestHandler : BrokerImplementation, IRouting
    {
       
        public void RouteRequests(ConnectionParam connection)
        {

           
           var request = Encoding.UTF8.GetString(connection.Context);

            TransactionProtocol message = JsonConvert.DeserializeObject<TransactionProtocol>(request);
            
            if(message.Type_message==TypeMessage.add)
            {
                AddHandler(message, connection);
            } else
                if(message.Type_message==TypeMessage.give)
            {
                GiveHandler(message, connection);
            } else
                if(message.Type_message== TypeMessage.response)
            {
                ResponseHandler(message, connection);
            }
            else
            {
                ErrorHandler(message, connection);
            }



        }
    }
}
