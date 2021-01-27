using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SmartContract
{
    class Condition
    {
        public int ID { get; set; }
        public string currency { get; set; }
        public string conditionOperator { get; set; }
        public double conditionValue { get; set; }
        public bool isActive { get; set; }
        public DateTime addedTime { get; set; }

        public Condition(DataRow row)
        {
            this.ID = Convert.ToInt32(row["ID"]);
            this.currency = row["CURRENCY"].ToString();
            this.conditionOperator = row["CONDITION_OPERATOR"].ToString();
            this.conditionValue = double.Parse(row["CONDITION_VALUE"].ToString());
            this.isActive = Convert.ToBoolean(row["IS_ACTIVE"].ToString());
            this.addedTime = DateTime.Parse(row["ADDED_TIME"].ToString());
        }
    }
}
