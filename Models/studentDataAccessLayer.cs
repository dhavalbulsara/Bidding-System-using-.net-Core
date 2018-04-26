using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace WebApplication4.Models
{
    public class studentDataAccessLayer
    {
        string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=test;";

        //To View all employees details    
        public IEnumerable<student> GetAllStudent()
        {
            List<student> lstemployee = new List<student>();

            
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("select * from student;", con);
                cmd.CommandType = System.Data.CommandType.Text;
                

                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    student student = new student();

                    student.ID = Convert.ToInt32(rdr["id"]);
                    student.fname = rdr["fname"].ToString();
                    student.mname = rdr["mname"].ToString();
                    student.lname = rdr["lname"].ToString();

                    lstemployee.Add(student);
                }
                con.Close();
            }
            return lstemployee;
        }

       
        public void AddEmployee(student stud)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {

                MySqlCommand cmd = new MySqlCommand("insert into student values('0','"+ stud.fname+"','"+stud.mname+"','"+stud.lname+"');", con);
                cmd.CommandType = System.Data.CommandType.Text;



               
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

    }
}
