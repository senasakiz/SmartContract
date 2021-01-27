using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class ExchangeOperationService
    {
        public CurrencyService currencyService { get; set; }
        public ConditionService conditionService { get; set; }
        public EntityService entityService { get; set; }
        public DataService dataService { get; set; }
        public List<Operation> operations;

        public ExchangeOperationService()
        {
            this.currencyService = new CurrencyService();
            this.conditionService = new ConditionService();
            this.entityService = new EntityService();
            this.dataService = new DataService();
            operations = new List<Operation>();
        }

        public List<Operation> getOperations(){
            operations = dataService.getOperations();
            return operations;
        }

        public Operation evaluateNewExchangeOperation()
        {
            string previousMoney = "", currentMoney = "";
            Currency previousCurrency = null;
            double newMoneyValue = 0.0, newBuying = 0.0, newSelling = 0.0;

            if (entityService.getEntities().Count() == 0)
            {
                return null;
            }
            Entity entity = entityService.getEntities()[0];

            Currency exchangedCurrency = getCurrencyOfOptimalAndProperCondition();
            if (exchangedCurrency == null)
            {
                return null;
            }

            previousCurrency = currencyService.getCurrencyBySign(entity.sign);
            if (previousCurrency != null)                                                   //If previous currency is not TL
            {
                newBuying = Math.Round(exchangedCurrency.buying / previousCurrency.buying, 7);
                newSelling = Math.Round(exchangedCurrency.selling / previousCurrency.selling, 7);
            }
            else
            {
                newBuying = Math.Round(exchangedCurrency.buying, 7);
                newSelling = Math.Round(exchangedCurrency.selling, 7);
            }

            newMoneyValue = evaluateNewMoney(entity.entity, newSelling);

            previousMoney = entity.entity + entity.sign;
            currentMoney = newMoneyValue + exchangedCurrency.sign;

            Operation newOperation = new Operation(previousMoney, newBuying, newSelling, currentMoney);
            addOperation(newOperation);
            Console.WriteLine("\n----------------------\nNew Exchange Operation added..\n" + newOperation.toString());
            return newOperation;
        }

        public void addOperation(Operation operation)
        {
            dataService.insertOperation(operation);
        }

        public double evaluateNewMoney(double previousMoneyValue, double selling){
            return Math.Round(previousMoneyValue / selling, 2);
        }

        public Currency getCurrencyOfOptimalAndProperCondition()                  //minimum difference between selling and buying 
        {
            Currency currencyOfOptimalAndProperCondition = null, currency = null;
            List<Condition> properConditions = getProperConditions();
            double differenceBetweenSellingAndBuying = 0;
            String conditionCurrencyName = "";

            if (properConditions.Count == 0)
            {
                return null;
            }

            currencyOfOptimalAndProperCondition = currencyService.getCurrencyByName(properConditions[0].currency);
            conditionCurrencyName = properConditions[0].currency;
            differenceBetweenSellingAndBuying = currencyOfOptimalAndProperCondition.selling - currencyOfOptimalAndProperCondition.buying;

            foreach (Condition condition in properConditions)
            {
                currency = currencyService.getCurrencyByName(condition.currency);

                if (differenceBetweenSellingAndBuying > (currency.selling - currency.buying))
                {
                    currencyOfOptimalAndProperCondition = currency;
                    differenceBetweenSellingAndBuying = currency.selling - currency.buying;
                    conditionCurrencyName = condition.currency;
                }   
            }

            conditionService.makeInActiveSpecificCondition(conditionCurrencyName);          //after exchanging make in-active this condition

            return currencyOfOptimalAndProperCondition;
        }

        public List<Condition> getProperConditions()
        {
            List<Condition> properConditions = new List<Condition>();

            foreach (Condition condition in conditionService.getConditions())
            {
                if (isProperCondition(condition, currencyService.getCurrencyByName(condition.currency)))
                {
                    properConditions.Add(condition);
                }
            }

            return properConditions;
        }

        public bool isProperCondition(Condition condition, Currency currency){
            double currencySellingValue = currency.selling;
            string conditionOperator = condition.conditionOperator;

            if (conditionOperator == "<" && currencySellingValue < condition.conditionValue ||
                conditionOperator == "<=" && currencySellingValue <= condition.conditionValue ||
                conditionOperator == "=" && currencySellingValue == condition.conditionValue ||
                conditionOperator == ">" && currencySellingValue > condition.conditionValue ||
                conditionOperator == ">=" && currencySellingValue >= condition.conditionValue)
            {
                return true;
            }
            return false;
        }
    }
}
