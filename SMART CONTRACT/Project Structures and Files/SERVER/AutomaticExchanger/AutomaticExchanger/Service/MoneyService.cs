using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class MoneyService
    {
        public List<Money> money { get; set; }
        public DataService dataService { get; set; }

        public MoneyService()
        {
            money = new List<Money>();
            dataService = new DataService();
        }

        public List<Money> getMoney()
        {
            money = dataService.getMoney();
            return money;
        }

    }
}
