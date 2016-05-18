using AbInitio.Web.DAL;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult maakStamboom(string familienaam, int accountId)
        {
            stamboomDAL.maakStamboom(accountId, familienaam);
            return Redirect("overzichtStambomen");
        }

        [HttpGet]
        public ActionResult overzichtStambomen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult overzichtStambomen(int accountId, string familieNaam)
        {
            if (familieNaam.Length < 1 ) { return View(); }
            List<StamboomModel> stambomen = stamboomDAL.getStambomen(accountId, familieNaam);
            return View(stambomen);
        }

        [HttpGet]
        public ActionResult Stamboom(int stamboomId, int accountId)
        {
            StamboomViewModel viewModel = new StamboomViewModel();
            try
            {
                viewModel.stamboom = StamboomDAL.GetStamboom(stamboomId);
                viewModel.personen = stamboomDAL.getPersonenInStamboom(stamboomId, accountId);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    return View(viewModel);
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