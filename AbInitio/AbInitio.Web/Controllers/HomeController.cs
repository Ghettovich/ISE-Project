using AbInitio.Web.DAL;
using AbInitio.Web.Models;
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
        [HttpGet]
        public ActionResult Schakelbord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Schakelbord(int stamboomid)
        {
            System.Web.HttpContext.Current.Session["stamboomid"] = stamboomid;
            StamboomModel model = new StamboomModel();
            model.familieNaam = StamboomDAL.GetStamboom(stamboomid).familienaam;
            System.Web.HttpContext.Current.Session["familienaam"] = model.familieNaam;
            return View(model);
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