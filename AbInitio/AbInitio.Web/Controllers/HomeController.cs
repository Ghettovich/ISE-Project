using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [HttpGet]
        public ActionResult logIn(int accountId)
        {
            if (accountId == 0)
            {
                Session.Remove("account");
                return Redirect("/Home");
            }
            System.Web.HttpContext.Current.Session["account"] = accountId;
            return Redirect("/Home");
        }

        [HttpGet]
        public ActionResult Error(string errorMessage)
        {
            return View(model:errorMessage);
        }
    }
}