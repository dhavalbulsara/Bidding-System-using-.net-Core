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
    public class HomePageController : Controller
    {
        string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            List<PropertyListModel> lstproperty = new List<PropertyListModel>();

            //ViewData["sessionString"] = HttpContext.Session.GetString("username");
            try
            {


                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select a.*, max(b.BIDAMOUNT) as CURRENT_PRICE from PROPERTYMASTER a inner join BIDDERMASTER b on a.ID = b.PROPERTYID where a.status = 'OPEN' group by a.ID;", con);

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
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("userID");

            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        public IActionResult AddProperty()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProperty([Bind] PropertyModel property)
        {
            string user = HttpContext.Session.GetString("username");
            int userID = HttpContext.Session.GetInt32("userID").Value;
            property.PostingDate = DateTime.Now;
            property.status = "OPEN";
            property.ID = userID;
            property.UserID = userID;
            if (ModelState.IsValid)
            {
                DataAccess exQuery = new DataAccess();
                DataSet ds = new DataSet();
                ds = exQuery.FireQuery("INSERT INTO PROPERTYMASTER VALUES('0', CURRENT_TIMESTAMP, '"+property.UserID+"', '"+property.Address.ToUpper()+"', '"+property.MinPrice+"','"+property.Type.ToUpper()+"','OPEN');");
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd2 = new MySqlCommand("call SP_DEFAULTBID();", con);
                    cmd2.CommandType = System.Data.CommandType.Text;
                    con.Open();
                    MySqlDataReader rdr2 = cmd2.ExecuteReader();
                    con.Close();
                }

            }
            return RedirectToAction("Index", "HomePage");
        }
        [HttpGet]
        public IActionResult PropertyDetail(int? Id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            PropertyListModel lstproperty = new PropertyListModel();

            //ViewData["sessionString"] = HttpContext.Session.GetString("username");
            try
            {


                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select a.*, max(b.BIDAMOUNT) as CURRENT_PRICE from PROPERTYMASTER a inner join BIDDERMASTER b on a.ID = b.PROPERTYID where a.status = 'OPEN' and a.ID = '" + Id + "' group by a.ID;", con);

                    cmd.CommandType = System.Data.CommandType.Text;


                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        lstproperty.ID = Convert.ToInt32(rdr["ID"]);
                        lstproperty.PostingDate = Convert.ToDateTime(rdr["POSTINGDATE"]);
                        lstproperty.Type = rdr["TYPE"].ToString();
                        lstproperty.Address = rdr["ADDRESS"].ToString();
                        lstproperty.MinPrice = Convert.ToInt32(rdr["MIN_PRICE"]);
                        lstproperty.status = rdr["STATUS"].ToString();
                        lstproperty.UserID = Convert.ToInt32(rdr["USER_ID"]);
                        lstproperty.CurrentPrice = Convert.ToInt32(rdr["CURRENT_PRICE"]);
                    }
                    con.Close();
                }




                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from BIDDERMASTER a inner join USERLOGIN b on a.USER_ID=b.USER_ID where a.PROPERTYID='" + Id + "' order by BIDAMOUNT desc;", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    List<BidModel> Bids = new List<BidModel>();
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        BidModel propertybid = new BidModel();
                        propertybid.ID = Convert.ToInt32(rdr["ID"]);
                        propertybid.BidDate = Convert.ToDateTime(rdr["BIDDATE"]);
                        propertybid.PropertyID = Convert.ToInt32(rdr["PROPERTYID"]);
                        propertybid.UserID = Convert.ToInt32(rdr["USER_ID"]);
                        propertybid.Username = rdr["USERNAME"].ToString();
                        propertybid.Bid_Amount = Convert.ToInt32(rdr["BIDAMOUNT"]);
                        Bids.Add(propertybid);

                    }
                    con.Close();
                    ViewBag.Bids = Bids.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            return View(lstproperty);
        }

        [HttpGet]
        public IActionResult AddBid(int? ID)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBid([Bind] AddBid Bid, int ID)
        {
            
            string user = HttpContext.Session.GetString("username");
            int userID = HttpContext.Session.GetInt32("userID").Value;
            Bid.PropertyID = ID;
            Bid.UserID = userID;
            if (ModelState.IsValid)
            {
                bool flag = false;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select max(BIDAMOUNT) as BID from BIDDERMASTER where PROPERTYID = '"+Bid.PropertyID+"';", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if(Convert.ToInt32(rdr["BID"]) >= Convert.ToInt32(Bid.Bid_Amount))
                        {
                            ModelState.AddModelError("", "Bid should be greater than Max bid");
                            return View(Bid);
                        }
                    }
                    con.Close();
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO BIDDERMASTER VALUES('0', CURRENT_TIMESTAMP, '" + Bid.PropertyID + "', '" + Bid.UserID + "', '" + Bid.Bid_Amount + "');", con);

                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    con.Close();
                }

            }
            return RedirectToAction("PropertyDetail", "HomePage", new { ID = Bid.PropertyID });
        }
    }
}