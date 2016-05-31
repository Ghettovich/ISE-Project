using AbInitio.Web.DAL;
using AbInitio.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    public class StamboomRechtenController : Controller
    {
        StamboomRechtenDAL rechtenDAL = new StamboomRechtenDAL();
        // GET: StamboomRechten
        public ActionResult Index()
        {
            Session["stamboom"] = 1;
            if (Session["stamboom"] == null)
            {
                return Redirect("/Home");
            }
            List<AccountModel> accounts = rechtenDAL.getGerechtigden((int)Session["stamboom"]);

            return View(accounts);
        }

        [HttpGet]
        public ActionResult Toevoegen()
        {
            List<AccountModel> account = new List<AccountModel>();
            return View(account);
        }

        [HttpPost]
        public ActionResult Toevoegen(AccountModel model)
        {
            Regex reg = new Regex(@"^[a-zA-Z'.]{1,40}$");
            if (reg.IsMatch(model.gebruikersnaam) == false)
            {
                return Redirect("Toevoegen");
            }
            List<AccountModel> accounts = new List<AccountModel>();
            accounts = rechtenDAL.getGebruikers(model.gebruikersnaam, (int)Session["stamboom"]);
            return View(accounts);
        }

        [HttpGet]
        public ActionResult ToevoegenRecht()
        {

            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult ToevoegenRecht(AccountModel model)
        {
            rechtenDAL.toevoegenGerechtigde(model.accountId,(int)Session["stamboom"],Request["rechtenText"].ToString());
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult verwijderenRecht(int stamboomAccountId)
        {
            rechtenDAL.verwijderenGerechtigde(stamboomAccountId);
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult wijzigenRecht(int stamboomAccountId, int recht)
        {
            switch (recht)
            {
                case 2:
                    recht = 3;
                    break;
                case 3:
                    recht = 2;
                    break;   
            }
            AccountModel model = new AccountModel
            {
                stamboomAccountId = stamboomAccountId,
                stamboomRechten = recht
            };
            rechtenDAL.wijzigenGerechtigde(model.stamboomAccountId, model.stamboomRechten.ToString());
            return Redirect("Index");
        }
    }
}