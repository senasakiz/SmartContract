using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AutomaticExchanger
{
    class Program
    {
        public static ExchangeOperationService exchangeOperationService;
        public static EntityService entityService;

        static void Main(string[] args)
        {
            exchangeOperationService = new ExchangeOperationService();
            entityService = new EntityService();
            while (true)
            {
                Operation newOperation = exchangeOperationService.evaluateNewExchangeOperation();
                entityService.updateEntity(newOperation);

                Thread.Sleep(60000);                //check for every minutes
            }
        }
    }
}
