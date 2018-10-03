using HealthCareSoft.Repository.Interfaces;
using HealthCareSoft.Repository.Models;
using HealthCareSoft.Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthCareSoft.Entity;
using HealthCareSoft.Admin.Models;
using System.IO;
using System.Web.Configuration;

namespace HealthCareSoft.Admin.Controllers
{
    public class CommonViewController : Controller
    {
     
        private readonly IUserServices _userservice;
       private string _profileImagesPath = WebConfigurationManager.AppSettings["ProfileImages"];
        HttpCookie cookie = new HttpCookie("Login");
        public CommonViewController()
        {           
            _userservice = new UserService();
        }
        // GET: CommonView
        public ActionResult Index()
        {
            return View();
        }
        //GET:Login
       
        public ActionResult Login()
       {
            UserEntity model = new UserEntity();
            if (Request.Cookies["Login"] != null) 
            {
                model.Email = Request.Cookies["Login"].Values["Email"];
                model.Password = Request.Cookies["Login"].Values["Password"];
                model.Rememberme = Convert.ToBoolean(Request.Cookies["Login"].Values["Rememberme"]);
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserEntity u)
        {
            if (u.Email!=null && u.Password!=null)
            {
                var user = _userservice.GetUserByName(u.Email, u.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(u.Email, u.Rememberme);
                    if (u.Rememberme)
                    {
                        
                        cookie.Values.Add("Email", user.Email);
                        cookie.Values.Add("Password", u.Password);
                        cookie.Values.Add("Rememberme", (u.Rememberme).ToString());
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                        //var createPersistentCookie = u.Rememberme;
                        //int timeout = createPersistentCookie ? 525600 : 2; // Timeout in minutes,525600 = 365 days
                        //var ticket = new FormsAuthenticationTicket(u.Email, createPersistentCookie, timeout);
                        //string encrypted = FormsAuthentication.Encrypt(ticket);
                        //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        //cookie.Expires = System.DateTime.Now.AddMinutes(timeout);//My Line
                        //HttpContext.Response.Cookies.Add(cookie);

                        // FormsAuthentication.SetAuthCookie(u.Email, false);
                        //var authTicket = new FormsAuthenticationTicket(1, u.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, "Testing");
                        //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        //HttpContext.Response.Cookies.Add(authCookie);
                    }
                    else
                    {
                        Response.Cookies["Login"].Values["Rememberme"] = "false";
                        //cookie.Values.Remove(Convert.ToBoolean(Request.Cookies["Login"].Values["Rememberme"]).ToString());
                    }
                  
                    ApplicationSession.Login(user);
                        Session["Id"] = user.Id;
                        Session["username"] = user.FirstName + "  " + user.LastName;
                           Session["rememberme"] = u.Rememberme;
                    //  LoggedInCkeck.g_IsLoggedIn = Session["IsLoggedIn"] == null ? false : (bool)Session["IsLoggedIn"];
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name or Password ");
                }
            }
            return View(u);
        }       
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserEntity u, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg"
        };
                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + u.FirstName + ext; //appending the name with id  
                                                                    // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/Content/ProfileImages"), myfile);
                    u.ProfilePicture = _profileImagesPath + myfile;
                    u.CreatedBy = 2;
                    //obj.tbl_details.Add(tbl);
                    //obj.SaveChanges();
                    file.SaveAs(path);
                }
                else
                {
                    ModelState.AddModelError("", "Please selet the iamge in .Jpg .png .jpg .jpeg Extensions Only!!");
                }
            }
            if (ModelState.IsValid)
            {
                var user = _userservice.CreateUser(u);
                return RedirectToAction("Login", "CommonView");
            }
            else
            {
                ModelState.AddModelError("", "Please Fill the Required field Correctly!!");
            }
            return View(u);
        }

        //GET:Forgot password
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]     
        public ActionResult ForgotPassword(string UserName)
        {
            //check user existance         
            if (UserName == null)
            {
                TempData["Message"] = "User Not exist.";
            }
            else
            {                          
                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "CommonView", new { un = UserName }, "http") + "'>Reset Password</a>";
                //get user emailid             
                HealthCareEntities db = new HealthCareEntities();
                var emailid = (from i in db.UserProfiles
                               where i.Email == UserName
                               select i.Email).FirstOrDefault();
                //send mail
                string subject = "Password Reset Token";
                string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it
                try
                {                  
                    Mail.SendEMail(emailid, subject, body);
                    TempData["Message"] = "Mail Sent.";
                    return RedirectToAction("Login", "CommonView");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error occured while sending email." + ex.Message;
                }
                //only for testing
                TempData["Message"] = resetLink;
            }

            return View();
        }

        public ActionResult ResetPassword(string un)
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(string un,string password)
        {
            HealthCareEntities db = new HealthCareEntities();
            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            var userid = (from i in db.UserProfiles
                          where i.Email == un
                          select i.Id).FirstOrDefault();
            //check userid and token matches           
            var userEmail = (from i in db.UserProfiles
                             where i.Email == un
                             select i.Email).FirstOrDefault();
            if (userEmail != null)
            {                
                var passwordUpdated = _userservice.ResetPassword(userid,un, password);
                if (passwordUpdated == true)
                {
                    //get user emailid to send password
                    var emailid = (from i in db.UserProfiles
                                   where i.Email == un
                                   select i.Email).FirstOrDefault();
                    //send email
                    string subject = "New Password";
                    string body = "<b>Please find the New Password</b><br/>" + password; //edit it
                    try
                    {
                        Mail.SendEMail(emailid, subject, body);
                        TempData["Message"] = "Mail Sent.";
                        return RedirectToAction("Login", "CommonView");
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "Error occured while sending email." + ex.Message;
                    }

                    //display message
                    TempData["Message"] = "Success! Check email we sent. Your New Password Is " + password;
                }
                else
                {
                    TempData["Message"] = "Hey, avoid random request on this page.";
                }
            }
            else
            {
                TempData["Message"] = "Username and token not maching.";
            }

            return View();
        }

      
        [AllowAnonymous]
        public ActionResult Logout()
        {           
            //var rememberme = Request.Cookies["Login"].Values["Rememberme"];
            //if(rememberme=="True")
            //{
            //    cookie.Values.Add("rememberme",(true).ToString());
            //}
            //else
            //{
            //    cookie.Values.Add("rememberme", (false).ToString());
            //}
            Session.Clear();
            Session.Abandon();
            ApplicationSession.Logout();
            FormsAuthentication.SignOut();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return  RedirectToAction("Login","CommonView");
        }

    }
}