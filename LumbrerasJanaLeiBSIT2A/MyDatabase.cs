using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LumbrerasJanaLeiBSIT2A
{
    internal class MyDatabase
    {
        string connectionString = "Server=localhost;Port=3306;Database='lumbreras_db';Uid='root';Pwd=''";

        public bool TestConnection()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public DataTable ExecuteReturnQuery(string query, params MySqlParameter[] parameters)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, con);
                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }
                DataTable dataTable = new DataTable();
                try
                {
                    con.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Execution failed: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }

                return dataTable;
            }
        }


    }
}
