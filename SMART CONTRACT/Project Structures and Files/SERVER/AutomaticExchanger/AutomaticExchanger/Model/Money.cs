using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AutomaticExchanger
{
    class Money
    {
        public int ID { get; set; }
        public double money { get; set; }
        public DateTime addedTime { get; set; }

        public Money(DataRow row)
        {
            this.ID = Convert.ToInt32(row["ID"]);
            this.money = double.Parse(row["MONEY"].ToString());
            this.addedTime = DateTime.Parse(row["ADDED_TIME"].ToString());
        }
    }
}
