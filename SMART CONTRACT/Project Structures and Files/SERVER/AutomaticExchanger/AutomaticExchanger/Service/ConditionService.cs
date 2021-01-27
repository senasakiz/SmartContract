using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class ConditionService
    {
        public List<Condition> conditions { get; set; }
        public DataService dataService { get; set; }

        public ConditionService(){
            conditions = new List<Condition>();
            dataService = new DataService();
        }

        public List<Condition> getConditions()
        {
            conditions = dataService.getConditions();
            return conditions;
        }

        public void makeInActiveSpecificCondition(string currencyname)
        {
            dataService.makeInActiveSpecificElement("EXCHANGE_CONDITION", "CURRENCY", currencyname);
        }
    }
}
