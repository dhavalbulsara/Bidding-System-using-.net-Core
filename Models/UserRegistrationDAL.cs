using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication4.Models
{
    public class UserRegistrationDAL
    {
        public bool CreateUser(UserRegisteration user)
        {
            try
            {
                DataAccess exQuery = new DataAccess();
                DataSet ds = new DataSet();
                ds = exQuery.FireQuery("CALL UBBIDDER.SP_CREATEUSER('CLIENT', '" + user.fname.ToUpper() + "', '" + user.mname.ToUpper() + "', '" + user.lname.ToUpper() + "', '" + user.gender.ToString().ToUpper() + "', '" + user.email.ToUpper() + "', '" + user.phone.ToString() + "', '" + user.address.ToUpper() + "', '" + user.username.ToLower() + "', '" + user.password.ToString() + "');");
                if(ds.Tables[0] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<LoginModel> Login(string username, string password)
        {
            string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";
            List<LoginModel> list_lm = new List<LoginModel>();

            try
            {
                IList<ProcedureParameter> param_list = new List<ProcedureParameter>();
                param_list.Add(new ProcedureParameter { name = "PARAM_USERNAME", value = username });
                param_list.Add(new ProcedureParameter { name = "PARAM_PASSWORD", value = password });
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SP_LOGINCHECK"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var elem in param_list)
                        {
                            cmd.Parameters.Add(new MySqlParameter("@" + elem.name + "", elem.value));
                        }
                        //cmd.ExecuteNonQuery();
                        cmd.Connection = con;
                        con.Open();

                        using (MySqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                LoginModel lm = new LoginModel();
                                lm.username = sdr["USERNAME"].ToString();
                                lm.password = sdr["PASSWORD"].ToString();
                                lm.id =Int32.Parse(sdr["ID"].ToString());
                                list_lm.Add(lm);
                            }
                            return list_lm;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
