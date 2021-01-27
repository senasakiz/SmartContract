using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AutomaticExchanger
{
    class Entity
    {
        public int ID { get; set; }
        public double entity { get; set; }
        public string sign { get; set; }
        public double status { get; set; }
        public bool isActive { get; set; }
        public DateTime updatedTime { get; set; }

        public Entity()
        {

        }

        public Entity(DataRow row)
        {
            this.ID = Convert.ToInt32(row["ID"]);
            this.entity = double.Parse(row["ENTITY"].ToString());
            this.sign = row["SIGN"].ToString();
            this.status = double.Parse(row["STATUS"].ToString());
            this.isActive = Convert.ToBoolean(row["IS_ACTIVE"].ToString());
            this.updatedTime = DateTime.Parse(row["UPDATED_TIME"].ToString());
        }

        public Entity(double entity, string sign, double status = 1, bool isActive = true)
        {
            this.entity = entity;
            this.sign = sign;
            this.status = status;
            this.isActive = isActive;
            this.updatedTime = DateTime.Now;
        }
    }
}
