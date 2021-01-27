using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class Operation
    {
        public int ID { get; set; }
        public string money { get; set; }
        public double buyRate { get; set; }
        public double sellRate { get; set; }
        public string currentMoney { get; set; }
        public DateTime exchangedTime { get; set; }

        public Operation(string money, double buyRate, double sellRate, string currentMoney)
        {
            this.money = money;
            this.buyRate = buyRate;
            this.sellRate = sellRate;
            this.currentMoney = currentMoney;
            this.exchangedTime = DateTime.Now;
        }

        public string toString()
        {
            return "money=" + money +
                   "\nbuying rate=" + buyRate +
                   "\nselling rate=" + sellRate +
                   "\ncurrent money=" + currentMoney +
                   "\nexchanged time=" + exchangedTime;
        } 
    }
}
