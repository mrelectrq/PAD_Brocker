using System;

namespace TransactionBroker
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine("Hello World!");

            var broker = new BrokerServer();
            broker.Start(540);

            Console.ReadKey();
        }
    }
}
