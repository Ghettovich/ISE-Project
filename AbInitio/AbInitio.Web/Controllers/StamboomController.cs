﻿using AbInitio.Web.DAL;
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
            StamboomViewModel viewModel = new StamboomViewModel();
            if (Session["account"] == null)
            {
                return Redirect("/Home");
            }
            viewModel.stamboom =  stamboomDAL.maakStamboom((int)Session["account"], familienaam);
            return RedirectToAction("NieuwPersoon", "Persoon", new { stamboomid = viewModel.stamboom.stamboomid });
        }

        [HttpGet]
        public ActionResult overzichtStambomen()
        {
            return View();
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

        [HttpGet]
        public ActionResult Stamboom(int stamboomId)
        {
            StamboomViewModel viewModel = new StamboomViewModel();
            try
            {
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


        public ActionResult AfschermenStamboom(int stamboomId)
        {
            stamboomDAL.afschermenStamboom(stamboomId);
            return Redirect("Stamboom?stamboomId="+stamboomId);
        }


        [HttpGet]
        public ActionResult WijzigStamboom(int stamboomid)
        {
            StamboomViewModel model = new StamboomViewModel();
            model.stamboomid = stamboomid;
            model.stamboom = StamboomDAL.GetStamboom(stamboomid);
            return View(model);
        }

        [HttpPost]
        public ActionResult WijzigStamboom(int stamboomid,string familienaam,DateTime gewijzigdOp)
        {
            StamboomModel update = new StamboomModel();
            update.stamboomId = stamboomid;
            update.familieNaam = familienaam;
            update.gewijzigdOp = gewijzigdOp;

            stamboomDAL.wijzigStamboom(update);
            return Redirect("Stamboom?stamboomId=" + stamboomid);
        }

    }
}