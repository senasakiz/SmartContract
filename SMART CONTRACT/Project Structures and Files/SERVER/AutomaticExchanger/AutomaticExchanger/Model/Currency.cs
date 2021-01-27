using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticExchanger
{
    class Currency
    {
        public string name { get; set; }
        public string sign { get; set; }
        public double buying { get; set; }
        public double selling { get; set; }

        public Currency(string name, string sign, double buying, double selling)
        {
            this.name = name;
            this.sign = sign;
            this.buying = buying;
            this.selling = selling;
        }

    }
}
