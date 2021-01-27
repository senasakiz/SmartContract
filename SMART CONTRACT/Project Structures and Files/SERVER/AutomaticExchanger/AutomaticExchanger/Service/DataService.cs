using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace AutomaticExchanger
{
    class DataService
    {
        private static String getConnectionString()
        {
            return "Data Source=SENASAKIZ;Initial Catalog=SMART_CONTRACT;Integrated Security=SSPI;";
        }

        private DataTable selectFromDB(string storedProcedureorCommandText, bool isStoredProcedure = true)
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

        public void makeInActiveAllElements(string tableName)
        {
            string cmdString = "UPDATE " + tableName + " SET IS_ACTIVE=0";

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

        public void makeInActiveSpecificElement(string tableName, string columnName, string name)
        {
            string cmdString = "UPDATE " + tableName + " SET IS_ACTIVE=0 WHERE IS_ACTIVE=1 AND " + columnName + "=@val1";

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        comm.Parameters.AddWithValue("@val1", name);
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void insertEntity(Entity entity)
        {
            string cmdString = "INSERT INTO ENTITY_STATE (ENTITY, SIGN, STATUS, IS_ACTIVE, UPDATED_TIME) " +
                               "VALUES (@val1, @val2, @val3, @val4, getdate())";

            makeInActiveAllElements("ENTITY_STATE");

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
                        Console.WriteLine("Varlığınız güncellenemedi!");
                    }
                }
            }
        }

        public void insertOperation(Operation operation)
        {
            string cmdString = "INSERT INTO EXCHANGE_OPERATION (MONEY, BUY_RATE, SELL_RATE, CURRENT_MONEY, EXCHANGED_TIME) " +
                               "VALUES (@val1, @val2, @val3, @val4, @val5)";

            makeInActiveAllElements("EXCHANGE_OPERATION");

            using (SqlConnection conn = new SqlConnection(getConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    try
                    {
                        comm.Connection = conn;
                        comm.CommandText = cmdString;
                        comm.Parameters.AddWithValue("@val1", operation.money);
                        comm.Parameters.AddWithValue("@val2", operation.buyRate);
                        comm.Parameters.AddWithValue("@val3", operation.sellRate);
                        comm.Parameters.AddWithValue("@val4", operation.currentMoney);
                        comm.Parameters.AddWithValue("@val5", operation.exchangedTime);
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Çevirim işlemi yapılamadı!");
                    }
                }
            }
        }

        public List<Entity> getEntities()
        {
            return ExcuteObject<Entity>("SELECT * FROM ENTITY_STATE WHERE IS_ACTIVE=1", false).ToList();
        }

        public List<Condition> getConditions()
        {
            return ExcuteObject<Condition>("SELECT * FROM EXCHANGE_CONDITION WHERE IS_ACTIVE=1", false).ToList();
        }

        public List<Operation> getOperations()
        {
            return ExcuteObject<Operation>("SELECT * FROM EXCHANGE_OPERATION ORDER BY ID ASC", false).ToList();
        }

        public List<Money> getMoney()
        {
            return ExcuteObject<Money>("SELECT TOP 1 * FROM ADDED_MONEY_IN_TL", false).ToList();
        }
    }
}
