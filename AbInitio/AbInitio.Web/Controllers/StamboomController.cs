using AbInitio.Web.DAL;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AbInitio.Web.Controllers
{
    public class StamboomController : Controller
    {
        StamboomDAL stamboomDAL = new StamboomDAL();
        // GET: Stamboom
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult maakStamboom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult maakStamboom(string familienaam)
        {
            if(Session["account"] != null)
            {
                return Redirect("/Home");
            }
            stamboomDAL.maakStamboom((int)Session["account"], familienaam);
            return Redirect("overzichtStambomen");
        }

        [HttpGet]
        public ActionResult overzichtStambomen()
        {
            List<StamboomModel> stambomen = new List<StamboomModel>();
            stambomen = stamboomDAL.getStambomen((int)Session["account"], "");
            return View(stambomen);
        }

        [HttpPost]
        public ActionResult eigenStambomen()
        {
            List<StamboomModel> stambomen = new List<StamboomModel>();
            stambomen = stamboomDAL.getEigenStambomen((int)Session["account"]);
            return View("OverzichtStambomen", stambomen);
        }

        [HttpPost]
        public ActionResult collaboratieStambomen()
        {
            List<StamboomModel> stambomen = new List<StamboomModel>();
            stambomen = stamboomDAL.getCollaboratieStambomen((int)Session["account"]);
            return View("OverzichtStambomen", stambomen);
        }

        [HttpPost]
        public ActionResult overzichtStambomen(string familieNaam)
        {
            if (Session["account"] == null)
            {
                return Redirect("/Home");
            }
            if (familieNaam.Length < 1 ) { return View(); }
            List<StamboomModel> stambomen = stamboomDAL.getStambomen((int)Session["account"], familieNaam);
            return View(stambomen);
        }

        [HttpPost]
        public ActionResult Stamboom(int stamboomId)
        {
            StamboomViewModel viewModel = new StamboomViewModel();
            try
            {
                System.Web.HttpContext.Current.Session["stamboomid"] = stamboomId;
                int accountId;
                viewModel.stamboom = StamboomDAL.GetStamboom(stamboomId);
                if (Session["account"] != null) {
                    accountId = (int)Session["account"];
                }
                else
                {
                    accountId = 0;
                }
                viewModel.personen = stamboomDAL.getPersonenInStamboom(stamboomId,accountId );

                return View(viewModel);
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
                }
                else
                {
                    throw ex;
                }

            }
        }


        [HttpGet]
        public ActionResult NieuwPersoon()
        {
            NieuwPersoonModel model = new NieuwPersoonModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult NieuwPersoon(NieuwPersoonModel model)
        {
            return View();
        }

    }
}