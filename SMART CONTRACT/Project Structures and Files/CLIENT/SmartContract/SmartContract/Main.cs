using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;

namespace SmartContract
{
    public partial class Main : Form
    {
        double usdConditionVal; 
        double euroConditionVal;
        double sterlingConditionVal;
        List<Condition> conditions;
        List<Entity> entity;

        public Main()
        {
            InitializeComponent();
            initializeFieldValues();
            initTimerForClock();
            initTimerForGettingCurrency();
        }

        private String getConnectionString()
        {
            return "Data Source=SENASAKIZ;Initial Catalog=SMART_CONTRACT;Integrated Security=SSPI;";
        }

        private void initializeFieldValues()
        {
            getAndSetConditionFieldValues();
            getAndSetEntityFieldValues();
            getCurrencyRates();
        }

        private void initTimerForClock()
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void initTimerForGettingCurrency()
        {
            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 60000; // to get currency rates for every minutes
            timer2.Start();
        }

        private Entity getAndSetEntityFieldValues()
        {
            entity = new List<Entity>();
            entity = ExcuteObject<Entity>("SELECT * FROM ENTITY_STATE WHERE IS_ACTIVE=1", false).ToList();
            int index = 0;

            if (entity.Count != 1)
            {
                return null;
            }

            labelEntityVal.Text = entity[index].entity.ToString() + entity[index].sign;

            if (entity[index].status < 0)
            {
                labelStatusVal.ForeColor = Color.Red;
                labelStatusVal.Text = Math.Round(double.Parse(entity[index].status.ToString()), 2) + "₺";
            }
            else if (entity[index].status > 0)
            {
                labelStatusVal.ForeColor = Color.Green;
                labelStatusVal.Text = "+" + Math.Round(double.Parse(entity[index].status.ToString()), 2) + "₺";
            }
            else
            {
                labelStatusVal.Text = "0.0₺";
            }

            return entity[index];
        }

        private List<Condition> getAndSetConditionFieldValues()
        {
            List<Condition> conditions = new List<Condition>();
            conditions = ExcuteObject<Condition>("SELECT * FROM EXCHANGE_CONDITION WHERE IS_ACTIVE=1", false).ToList();
            int index = -1;

            foreach (Condition condition in conditions){
                switch (condition.currency)
                {
                    case "USD":
                        index = listBoxUsdConditionOperators.FindString(condition.conditionOperator);
                        listBoxUsdConditionOperators.SetSelected(index, true);
                        tbUsdConditionVal.Text = condition.conditionValue.ToString();
                        break;
                    case "EUR":
                        index = listBoxEuroConditionOperators.FindString(condition.conditionOperator);
                        listBoxEuroConditionOperators.SetSelected(index, true);
                        tbEuroConditionVal.Text = condition.conditionValue.ToString();
                        break;
                    case "GBP":
                        index = listBoxSterlingConditionOperators.FindString(condition.conditionOperator);
                        listBoxSterlingConditionOperators.SetSelected(index, true);
                        tbSterlingConditionVal.Text = condition.conditionValue.ToString();
                        break;
                }
            }
            return conditions;        
        }
        private double isMoneyFormattedInputChecker(String value)
        {
            try
            {

                if (double.Parse(value) <= 0)
                {
                    MessageBox.Show("Lütfen pozitif sayı giriniz!", "Uyarı");
                    return -1;
                }
                if (value.Contains('.'))
                {
                    MessageBox.Show("Lütfen yüklemek istediğiniz para(TL) miktarını '.' kullanmadan ',' ile giriniz!", "Uyarı");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                if (value.Count() != 0)
                {
                    MessageBox.Show("Lütfen girdiğiniz değerin sayısal bir değer olması gerektiğine dikkat ediniz!", "Uyarı");
                    return -1;
                }
                return -2;  //for space
                
            }
            return double.Parse(value);
        }

        public void getCurrencyRates()
        {
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);

            string usdBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            string usdSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;

            string euroBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            string euroSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;

            string sterlingBuying = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteBuying").InnerXml;
            string sterlingSelling = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='GBP']/BanknoteSelling").InnerXml;

            labelUsdBuyVal.Text = usdBuying;
            labelUsdSellVal.Text = usdSelling;

            labelEuroBuyVal.Text = euroBuying;
            labelEuroSellVal.Text = euroSelling;

            labelSterlingBuyVal.Text = sterlingBuying;
            labelSterlingSellVal.Text = sterlingSelling;
 
        }

        private void tbAddMoney_TextChanged(object sender, EventArgs e)
        {
            isMoneyFormattedInputChecker(tbAddMoney.Text);
        }

        private void tbUsdConditionVal_TextChanged(object sender, EventArgs e)
        {
            isMoneyFormattedInputChecker(tbAddMoney.Text);
        }

        private void buttonAddMoney_Click(object sender, EventArgs e)
        {
            bool isError = false;
            string cmdString = "INSERT INTO ADDED_MONEY_IN_TL (MONEY, ADDED_TIME) VALUES (@val1, getdate())";
            double money = 0.0;
            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {

                        money = isMoneyFormattedInputChecker(tbAddMoney.Text);
                        if (money <= 0)
                        {
                            return;
                        }

                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        comm.Parameters.AddWithValue("@val1", money);
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        MessageBox.Show("Lütfen yüklemek istediğiniz para(TL) miktarını giriniz!", "Uyarı");
                    }

                    if (!isError)
                    {
                        tbAddMoney.Text = "";
                        Entity entity = new Entity(money, "TL");
                        insertEntity(entity);
                        MessageBox.Show(money + "₺ hesabınıza yüklenmiştir.", "İşlem Başarılı");

                        tbAddMoney.Enabled = false;
                        tbAddMoney.BackColor = System.Drawing.SystemColors.Window;

                        buttonAddMoney.Enabled = false;
                        buttonAddMoney.BackColor = System.Drawing.SystemColors.Window;
                    }
                }
            }         
        }

        private void insertEntity(Entity entity)
        {
            string cmdString = "INSERT INTO ENTITY_STATE (ENTITY, SIGN, STATUS, IS_ACTIVE, UPDATED_TIME) " +
                               "VALUES (@val1, @val2, @val3, @val4, getdate())";

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        comm.Parameters.AddWithValue("@val1", entity.entity);
                        comm.Parameters.AddWithValue("@val2", entity.sign);
                        comm.Parameters.AddWithValue("@val3", entity.status);
                        comm.Parameters.AddWithValue("@val4", entity.isActive);
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void buttonConverterConditions_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkIsLegalCondition())
                {
                    makeInActiveAllConditions();
                    addConditions();
                }
                MessageBox.Show("İşlem koşulu başarıyla eklenmiştir.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem koşulu eklenememiştir!", "UYARI");
            }               
        }

        private void makeInActiveAllConditions()
        {
            string cmdString = "UPDATE EXCHANGE_CONDITION SET IS_ACTIVE=0";

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private bool checkIsLegalCondition()
        {
            usdConditionVal = isMoneyFormattedInputChecker(tbUsdConditionVal.Text);
            euroConditionVal = isMoneyFormattedInputChecker(tbEuroConditionVal.Text);
            sterlingConditionVal = isMoneyFormattedInputChecker(tbSterlingConditionVal.Text);

            try
            {
                if (!((usdConditionVal == -1 || euroConditionVal == -1 || sterlingConditionVal == -1) ||
                    (usdConditionVal == -2 && euroConditionVal == -2 && sterlingConditionVal == -2)))      //illegal
                {
                    
                    if (usdConditionVal > 0)
                    {
                        listBoxUsdConditionOperators.SelectedItem.ToString();
                    }
                    if (euroConditionVal > 0)
                    {
                        listBoxEuroConditionOperators.SelectedItem.ToString();
                    }
                    if (sterlingConditionVal > 0)
                    {
                        listBoxSterlingConditionOperators.SelectedItem.ToString();
                    } 
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Lütfen kullanmak istediğiniz operatörü seçiniz", "Uyarı");
            }
            return false;
        }
        private void addConditions()
        {
            if (usdConditionVal > 0)
            {
                addCondition("USD", listBoxUsdConditionOperators.SelectedItem.ToString(), usdConditionVal);
            }
            if (euroConditionVal > 0)
            {
                addCondition("EUR", listBoxEuroConditionOperators.SelectedItem.ToString(), euroConditionVal);
            }
            if (sterlingConditionVal > 0)
            {
                addCondition("GBP", listBoxSterlingConditionOperators.SelectedItem.ToString(), sterlingConditionVal);
            }               
        }

        private void addCondition(string currency, string conditionOperator, double conditionValue){
            string cmdString = "INSERT INTO EXCHANGE_CONDITION(CURRENCY, CONDITION_OPERATOR, CONDITION_VALUE, IS_ACTIVE, ADDED_TIME) VALUES(@val1, @val2, @val3, 1, getdate())";

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    comm.Parameters.AddWithValue("@val1", currency);
                    comm.Parameters.AddWithValue("@val2", conditionOperator);
                    comm.Parameters.AddWithValue("@val3", conditionValue);
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
        }

        public DataTable selectFromDB(string storedProcedureorCommandText, bool isStoredProcedure = true)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    if (!isStoredProcedure)
                    {
                        command.CommandType = CommandType.Text;
                    }
                    command.CommandText = storedProcedureorCommandText;
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            
        }

        public IEnumerable<T> ExcuteObject<T>(string storedProcedureorCommandText, bool isStoredProcedure = true)
        {
            List<T> items = new List<T>();
            var dataTable = selectFromDB(storedProcedureorCommandText, isStoredProcedure); 
            foreach (var row in dataTable.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), row);
                items.Add(item);
            }
            return items;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void listBoxUsdOperators_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
        private void tbAddMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void labelAddMoneyInfo_Click(object sender, EventArgs e)
        {

        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pbUsdFlag_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sMART_CONTRACTDataSet1.EXCHANGE_OPERATION' table. You can move, or remove it, as needed.
            this.eXCHANGE_OPERATIONTableAdapter1.Fill(this.sMART_CONTRACTDataSet1.EXCHANGE_OPERATION);
            // TODO: This line of code loads data into the 'sMART_CONTRACTDataSet.EXCHANGE_OPERATION' table. You can move, or remove it, as needed.
            //this.eXCHANGE_OPERATIONTableAdapter.Fill(this.sMART_CONTRACTDataSet.EXCHANGE_OPERATION);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelEntityTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }

        private void panelEntity_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            getCurrencyRates();
            getAndSetEntityFieldValues();
            this.eXCHANGE_OPERATIONTableAdapter1.Fill(this.sMART_CONTRACTDataSet1.EXCHANGE_OPERATION);
        }
    }
}
