using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class EntityService
    {
        public List<Entity> entities { get; set; }
        public DataService dataService { get; set; }
        public CurrencyService currencyService { get; set; }
        public MoneyService moneyService { get; set; }

        public EntityService()
        {
            entities = new List<Entity>();
            dataService = new DataService();
            currencyService = new CurrencyService();
            moneyService = new MoneyService();
        }

        public List<Entity> getEntities()
        {
            entities = dataService.getEntities();
            return entities;
        }

        public void addEntity(Entity entity)
        {
            dataService.insertEntity(entity);
        }

        public void addEntity(Operation operation)
        {
            addEntity(converOperationToEntity(operation));
        }

        public Entity converOperationToEntity(Operation operation)
        {
            Entity entity = new Entity();

            string moneyWithSign = operation.currentMoney;
            string sign = moneyWithSign.Contains("$") == true ? "$" : moneyWithSign.Contains("€") == true ? "€" : moneyWithSign.Contains("£") == true ? "£" : "TL";
            double money = double.Parse(moneyWithSign.Replace(sign, ""));
            double status = 1;

            entity.entity = money;
            entity.sign = sign;           
            entity.isActive = true;
            entity.updatedTime = DateTime.Now;
            updateEntityStatus(entity);

            return entity;
        }

        public void updateEntityStatus(Entity entity)
        {
            double currencyBuyingRate = 0.0;
            double addedMoney = 0.0;

            if (entity.sign.Equals("TL"))
            {
                entity.status = 0.0;
            }
            else
            {
                try
                {
                    addedMoney = moneyService.getMoney()[0].money;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Yüklenmiş TL yok!");
                    return;
                }
                currencyBuyingRate = currencyService.getCurrencyBySign(entity.sign).buying;
                entity.status = (entity.entity * currencyBuyingRate) - (addedMoney);
            }          
        }

        public void updateEntity(Operation newOperation)
        {
            if (newOperation == null)               //load from db
            {
                Entity entity = null;
                try
                {
                    entity = getEntities()[0];
                }
                catch (Exception ex)
                {
                    return;
                }

                updateEntityStatus(entity);               
            }
            else
            {
                addEntity(newOperation);
            }
        }
    }
}
