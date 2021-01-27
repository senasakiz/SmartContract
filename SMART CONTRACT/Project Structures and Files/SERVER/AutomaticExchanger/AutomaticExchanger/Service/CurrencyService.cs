using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutomaticExchanger
{
    class CurrencyService
    {
        public List<Currency> currencies { get; set; }

        public CurrencyService()
        {
            currencies = new List<Currency>();
            takeCurrencyExchangeRates();
        }

        public List<Currency> takeCurrencyExchangeRates()
        {
            currencies.Clear();
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);

            string usdBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            string usdSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            currencies.Add(new Currency("USD", "$", double.Parse(usdBuying) / 10000.0, double.Parse(usdSelling) / 10000.0));

            string euroBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            string euroSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            currencies.Add(new Currency("EUR", "€", double.Parse(euroBuying) / 10000.0, double.Parse(euroSelling) / 10000.0));

            string sterlingBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteBuying").InnerXml;
            string sterlingSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteSelling").InnerXml;
            currencies.Add(new Currency("GBP", "£", double.Parse(sterlingBuying) / 10000.0, double.Parse(sterlingSelling) / 10000.0));

            return currencies;
        }

        public Currency getCurrencyByName(String name)
        {
            foreach (Currency currency in currencies)
            {
                if (currency.name.Equals(name))
                {
                    return currency;
                }
            }
            return null;
        }

        public Currency getCurrencyBySign(String sign)
        {
            foreach (Currency currency in currencies)
            {
                if (currency.sign.Equals(sign))
                {
                    return currency;
                }
            }
            return null;
        }
    }
}
