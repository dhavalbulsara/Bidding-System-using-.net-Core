using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;



namespace WebApplication4.Models
{
    public class DataAccess
    {
        public DataSet FireQuery(string query)
        {

            string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";
            //using (MySqlConnection con = new MySqlConnection(connectionString))
            //{
            //    MySqlCommand cmd = new MySqlCommand("select * from student;", con);
            //    MySqlDataReader dr = cmd.ExecuteReader();
            //    DataSet dataresderset = new DataSet();
            //    DataSet ds = new DataSet();
            //    DataTable dataTable = new DataTable();
            //    dataresderset.Tables.Add(dataTable);
            //    dataresderset.EnforceConstraints = false;
            //    dataTable.Load(dr);
            //    dr.Close();
            //    ds.Tables.Add(dataTable.DataSet.Tables[0]);
            //   // return ds;
            //}
            //Create a new DataSet.
            DataSet ds = new DataSet();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if(sdr.Read())
                            //ds.Tables.Add("Customers");

                            //Load DataReader into the DataTable.
                            ds.Tables[0].Load(sdr);
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ds;

        }
    }
}
