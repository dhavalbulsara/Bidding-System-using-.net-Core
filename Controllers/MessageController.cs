using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication4.Controllers
{
    public class MessageController : Controller
    {
        string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";

        public IActionResult Inbox()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            List<MessageModel> lstproperty = new List<MessageModel>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select ID, DATE, FROMUSER, (select FNAME from USERMASTER where ID=FROMUSER) as FROMUSERNAME, TOUSER, (select FNAME from USERMASTER where ID=TOUSER) TOUSERNAME, BODY from PORTALMESSAGE where TOUSER='"+userID+"';", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        MessageModel message = new MessageModel();
                        message.Date = Convert.ToDateTime(rdr["DATE"]);
                        message.FromUser = rdr["FROMUSERNAME"].ToString();
                        message.Body = rdr["BODY"].ToString();
                        lstproperty.Add(message);
                    }
                    con.Close();
                }
                List<MessageModel> sentMessages = new List<MessageModel>();
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select ID, DATE, FROMUSER, (select FNAME from USERMASTER where ID=FROMUSER) as FROMUSERNAME, TOUSER, (select FNAME from USERMASTER where ID=TOUSER) TOUSERNAME, BODY from PORTALMESSAGE where FROMUSER='" + userID + "';", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        MessageModel message = new MessageModel();
                        message.Date = Convert.ToDateTime(rdr["DATE"]);
                        message.ToUser = rdr["TOUSERNAME"].ToString();
                        message.Body = rdr["BODY"].ToString();
                        sentMessages.Add(message);
                    }
                    con.Close();
                    ViewBag.SentItems = sentMessages.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            return View(lstproperty);
        }
        [HttpGet]
        public IActionResult NewMessage()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            List<SelectListItem> items = new List<SelectListItem>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("select ID, concat( Fname, ' ', Lname) as Name from USERMASTER where ID != '"+userID+"';", con);

                cmd.CommandType = System.Data.CommandType.Text;


                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = rdr["Name"].ToString(),
                        Value = rdr["ID"].ToString()
                    });
                }
                con.Close();
                ViewBag.Users = items;
            }
                return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewMessage([Bind] MessageModel message)
        {
            string user = HttpContext.Session.GetString("username");
            int userID = HttpContext.Session.GetInt32("userID").Value;
            message.FromUserID = userID;
            if (ModelState.IsValid)
            {
                DataAccess exQuery = new DataAccess();
                DataSet ds = new DataSet();
                ds = exQuery.FireQuery("INSERT INTO PORTALMESSAGE VALUES('0', CURRENT_TIMESTAMP, '"+message.FromUserID+"','"+message.ToUserID+"','"+message.Body+"');");
                

            }
            return RedirectToAction("Inbox", "Message");
        }
    }
}