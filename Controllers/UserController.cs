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
    public class UserController : Controller
    {
        string connectionString = "server=35.231.181.254;userid=appuser;password=appuser;database=UBBIDDER;";

        UserRegistrationDAL userDALobj = new UserRegistrationDAL();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] UserRegisteration user)
        {
            if (ModelState.IsValid)
            {
                userDALobj.CreateUser(user);
                TempData["msg"] = "<script>alert('User Registered');</script>";
                return RedirectToAction("Login");
            }
            return View(Index());
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind] LoginModel user)
        {
            if (ModelState.IsValid)
            {
                DataTable ds = new DataTable();
                List<LoginModel> lm = new List<LoginModel>();
                lm = userDALobj.Login(user.username, user.password).ToList();
                if(lm[0].username.ToString() == "0")
                {
                    ModelState.AddModelError("", "No User Exists!");
                }
                else if(lm[0].username.ToUpper() == user.username.ToUpper())
                {
                    if(lm[0].password.ToString() == user.password.ToString())
                    {
                        HttpContext.Session.SetString("username", user.username.ToString());
                        HttpContext.Session.SetInt32("userID",lm[0].id);

                        return RedirectToAction("Index", "HomePage");
                        //return View("../HomePage/Index", user);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong!");
                }

            }
            return View(user);
        }
        [HttpGet]
        public IActionResult UserProfile()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");

            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            List<SelectListItem> items = new List<SelectListItem>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("select ID, concat( Fname, ' ', Lname) as Name from USERMASTER where ID != '" + userID + "';", con);

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
        public IActionResult UserProfile(int? Id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            int userID = HttpContext.Session.GetInt32("userID").Value;
            List<SelectListItem> items = new List<SelectListItem>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("select ID, concat( Fname, ' ', Lname) as Name from USERMASTER where ID != '" + userID + "';", con);

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
            //ViewData["sessionString"] = HttpContext.Session.GetString("username");
            try
            {
                UserRegisteration user = new UserRegisteration();
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from USERMASTER where ID = '" + Id + "';", con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        user.Id = Convert.ToInt32(rdr["ID"]);
                        user.fname = rdr["FNAME"].ToString();
                        user.mname = rdr["MNAME"].ToString();
                        user.lname = rdr["LNAME"].ToString();
                        user.address = rdr["ADDRESS"].ToString();
                        user.gender = Convert.ToChar(rdr["SEX"]);
                        user.email = rdr["EMAIL"].ToString();
                        user.phone = rdr["PHONE"].ToString();
                    }
                    con.Close();
                }
                return View(user);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}