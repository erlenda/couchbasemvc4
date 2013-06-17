using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CouchbaseMVC4.Models;
using Enyim.Caching.Memcached;

namespace CouchbaseMVC4.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private static readonly Guid AppSecret = Guid.Parse("A9468ADE-8830-4955-B378-573BBCCC62EF");
        private const int SessionTimeoutMinutes = 60;

        public ActionResult Index()
        {
            ViewData["Message"] = "I dont know you.";
            
            return View();
        }

        //
        // GET: /Home/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var client = MvcApplication.CouchbaseClient;

            var userName = collection["UserName"];
            var password = collection["PassWord"];
            var fullName = collection["FullName"];

            var sessionToken = Guid.NewGuid();
            var cookie = new HttpCookie("COUCHBASE_SESSION", sessionToken.ToString());
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = fullName,
                Password = password,
                Username = userName
            };

            string userKey = string.Format("User-{0}-{1}{2}", 
                userName, AppSecret, user.Password);

            client.Store(StoreMode.Set,
                sessionToken.ToString() + AppSecret, userKey, TimeSpan.FromMinutes(SessionTimeoutMinutes));
            client.Store(StoreMode.Set, userKey, user.UserId);
            client.Store(StoreMode.Set, user.UserId.ToString(), user);

            return RedirectToAction("Index");
        }

        private static string EncodePassword(string password)
        {
            var hashBytes = new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(password));
            return BitConverter.ToString(hashBytes);
        }
    }
}
