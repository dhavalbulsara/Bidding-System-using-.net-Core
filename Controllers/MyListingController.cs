using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace WebApplication4.Controllers
{
    public class MyListingController : Controller
    {
        string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            List<PropertyListModel> lstproperty = new List<PropertyListModel>();

            //ViewData["sessionString"] = HttpContext.Session.GetString("username");
            try
            {


                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select a.*, max(b.BIDAMOUNT) as CURRENT_PRICE from PROPERTYMASTER a inner join BIDDERMASTER b on a.ID = b.PROPERTYID where a.USER_ID = '"+userID+"' group by a.ID;", con);

                    cmd.CommandType = System.Data.CommandType.Text;


                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        PropertyListModel property = new PropertyListModel();
                        property.ID = Convert.ToInt32(rdr["ID"]);
                        property.PostingDate = Convert.ToDateTime(rdr["POSTINGDATE"]);
                        property.Type = rdr["TYPE"].ToString();
                        property.Address = rdr["ADDRESS"].ToString();
                        property.MinPrice = Convert.ToInt32(rdr["MIN_PRICE"]);
                        property.status = rdr["STATUS"].ToString();
                        property.UserID = Convert.ToInt32(rdr["USER_ID"]);
                        property.CurrentPrice = Convert.ToInt32(rdr["CURRENT_PRICE"]);
                        lstproperty.Add(property);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            return View(lstproperty);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            PropertyModel property = new PropertyModel();

            //ViewData["sessionString"] = HttpContext.Session.GetString("username");
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from PROPERTYMASTER where id = '"+id+"';", con);

                    cmd.CommandType = System.Data.CommandType.Text;


                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        property.ID = Convert.ToInt32(rdr["ID"]);
                        property.PostingDate = Convert.ToDateTime(rdr["POSTINGDATE"]);
                        property.Type = rdr["TYPE"].ToString();
                        property.Address = rdr["ADDRESS"].ToString();
                        property.MinPrice = Convert.ToInt32(rdr["MIN_PRICE"]);
                        property.status = rdr["STATUS"].ToString();
                        property.UserID = Convert.ToInt32(rdr["USER_ID"]);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return View(property);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind] PropertyModel property, int Id)
        {
            string user = HttpContext.Session.GetString("username");
            int userID = HttpContext.Session.GetInt32("userID").Value;
            if (ModelState.IsValid)
            {
                DataAccess exQuery = new DataAccess();
                DataSet ds = new DataSet();
                ds = exQuery.FireQuery("UPDATE PROPERTYMASTER set ADDRESS = '" + property.Address.ToUpper() + "', MIN_PRICE='" + property.MinPrice + "', TYPE = '" + property.Type.ToUpper() + "' where ID = '"+Id+"';");
                

            }
            return RedirectToAction("Index", "MyListing");
        }
    }
}