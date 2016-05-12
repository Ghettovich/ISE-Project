using AbInitio.Web.DAL;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AbInitio.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private AccountDAL acdlayer;

        [HttpGet]
        public ActionResult Index()
        {
            if (User.IsInRole("0"))
            {
                return RedirectToAction("Index", "Beheer");
            }
            else if (User.IsInRole("1"))
            {
                return RedirectToAction("Index", "Stamboom");
            } return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Registreer()
        {
            RegistreerModel model = new RegistreerModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registreer(RegistreerModel model)
        {
            if (ModelState.IsValid)
            {
                acdlayer = new AccountDAL();
                acdlayer.RegistreerGebruuker(model);
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "Vul opnieuw het formulier in.");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string error;
                AccountDAL dal = new AccountDAL();

                dal.Login(model, out error);
                if (string.IsNullOrEmpty(error))
                {
                    FormsAuthentication.SetAuthCookie(model.Gebruikersnaam, createPersistentCookie: false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", error);
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Vul opnieuw het formulier in.");
            return View(model);
        }

        [HttpGet]
        public ActionResult Uitloggen()
        {
            FormsAuthentication.SignOut();
            return View("Login");

        }
    }
}