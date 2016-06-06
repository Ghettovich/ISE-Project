using AbInitio.Web.DAL;
using AbInitio.Web.DbContexts;
using AbInitio.Web.Models;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    public class AanvullendPersoonController : Controller
    {
        [HttpPost]
        public ActionResult GetNieuwAanvullendPersoon(int persoonid)
        {
            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.persoonid = persoonid;
            model.datumPrecisies = AanvullendPersoonDAL.datumPrecisiesOphalen();
            model.aanvullendPersoonInformatieTypes = AanvullendPersoonDAL.aanvullendPersoonInformatieTypesOphalen();

            return View("../AanvullendPersoon/NieuwAanvullendPersoon", model);
        }

        [HttpPost]
        public ActionResult NieuwAanvullendPersoon(AanvullendPersoonModel model)
        {
            try
            {
                NameValueCollection nvc = Request.Form;

                model.persoonid = Int32.Parse(nvc["persoonid"]);
                model.persooninformatie = nvc["persooninformatie"];
                if (!string.IsNullOrEmpty(nvc["van"]))
                {
                    DateTime d = DateTime.Parse(nvc["van"]);
                    model.van = d.ToString("yyyy-MM-dd");
                }
                else
                {
                    model.van = nvc["van"];
                }
                if (!string.IsNullOrEmpty(nvc["tot"]))
                {
                    DateTime d2 = DateTime.Parse(nvc["tot"]);
                    model.tot = d2.ToString("yyyy-MM-dd");
                }
                else
                {
                    model.tot = nvc["tot"];
                }
                model.datumPrecisie = nvc["datumPrecisie"];

                AanvullendPersoonDAL.nieuwAanvullendPersoonInDatabase(model);

                model.aanvullendPersoonInformatieLijst = AanvullendPersoonDAL.aanvullendePersoonInformatieVan(model.persoonid);

                return View("../AanvullendPersoon/AanvullendPersoonLijst", model);
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

        [HttpPost]
        public ActionResult GetWijzigAanvullendPersoon(int aanvullendPersoonInformatieId)
        {
            AanvullendPersoonModel m = new AanvullendPersoonModel();
            m = AanvullendPersoonDAL.getAanvullendPersoon(aanvullendPersoonInformatieId);

            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.aanvullendepersooninformatieid = m.aanvullendepersooninformatieid;
            model.persoonid = m.persoonid;
            model.persooninformatietypeid = m.persooninformatietypeid;
            model.persooninformatie = m.persooninformatie;
            model.van = m.van;
            model.tot = m.tot;
            model.datumPrecisie = m.datumPrecisie;
            model.gewijzigdOp = m.gewijzigdOp;
            model.datumPrecisies = AanvullendPersoonDAL.datumPrecisiesOphalen();
            model.aanvullendPersoonInformatieTypes = AanvullendPersoonDAL.aanvullendPersoonInformatieTypesOphalen();

            return View("WijzigAanvullendPersoon", model);
        }

        [HttpPost]
        public ActionResult WijzigAanvullendPersoon(AanvullendPersoonModel model)
        {
            try
            {
                NameValueCollection nvc = Request.Form;

                model.aanvullendepersooninformatieid = Int32.Parse(nvc["aanvullendPersoonInformatieId"]);
                model.persoonid = Int32.Parse(nvc["persoonId"]);
                model.persooninformatietypeid = Int32.Parse(nvc["persoonInformatieTypeId"]);
                model.persooninformatie = nvc["persoonInformatie"];
                if (!string.IsNullOrEmpty(nvc["van"]))
                {
                    DateTime d = DateTime.Parse(nvc["van"]);
                    model.van = d.ToString("yyyy-MM-dd");
                }
                else
                {
                    model.van = nvc["van"];
                }
                if (!string.IsNullOrEmpty(nvc["tot"]))
                {
                    DateTime d2 = DateTime.Parse(nvc["tot"]);
                    model.tot = d2.ToString("yyyy-MM-dd");
                }
                else
                {
                    model.tot = nvc["tot"];
                }
                model.datumPrecisie = nvc["datumPrecisie"];
                model.gewijzigdOp = DateTime.Parse(nvc["gewijzigdOp"]);

                AanvullendPersoonDAL.wijzigAanvullendPersoonInDatabase(model);

                model.aanvullendPersoonInformatieLijst = AanvullendPersoonDAL.aanvullendePersoonInformatieVan(model.persoonid);

                return View("../AanvullendPersoon/AanvullendPersoonLijst", model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerwijderAanvullendPersoon(int persoonid)
        {
            NameValueCollection nvc = Request.Form;
            int aanvullendPersoonInformatieId = Int32.Parse(nvc["aanvullendPersoonInformatieId"]);
            AanvullendPersoonDAL.verwijderAanvullendPersoonInDatabase(aanvullendPersoonInformatieId);

            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.aanvullendPersoonInformatieLijst = AanvullendPersoonDAL.aanvullendePersoonInformatieVan(persoonid);
            model.persoonid = persoonid;

            return View("../AanvullendPersoon/AanvullendPersoonLijst", model);
        }

        [HttpPost]
        public ActionResult AanvullendPersoonLijst(int persoonid)
        {
            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.aanvullendPersoonInformatieLijst = AanvullendPersoonDAL.aanvullendePersoonInformatieVan(persoonid);
            model.persoonid = persoonid;

            return View("../AanvullendPersoon/AanvullendPersoonLijst", model);
        }
    }
}