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
        [HttpGet]
        public ActionResult NieuwAanvullendPersoon(int persoonid)
        {
            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.persoonid = persoonid;
            model.datumPrecisies = AanvullendPersoonDAL.datumPrecisiesOphalen();
            model.aanvullendPersoonInformatieTypes = AanvullendPersoonDAL.aanvullendPersoonInformatieTypesOphalen();

            return View(model);
        }

        [HttpPost]
        public ActionResult NieuwAanvullendPersoon(AanvullendPersoonModel model)
        {
            NameValueCollection nvc = Request.Form;

            model.persoonid = Int32.Parse(nvc["persoonid"]);
            model.persooninformatietypeid =Int32.Parse(nvc["persooninformatietypeid"]);
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

            // Redirect goed zetten.
            return Redirect("AanvullendPersoon/OverzichtAanvullendPersoon");
        }

        [HttpGet]
        public ActionResult WijzigAanvullendPersoon(int aanvullendPersoonInformatieId)
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

            return View(model);
        }

        [HttpPost]
        public ActionResult WijzigAanvullendPersoon(AanvullendPersoonModel model)
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
            // Pad nog goed zetten
            return Redirect("AanvullendPersoon/OverzichtAanvullendPersoon");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerwijderAanvullendPersoon()
        {
            NameValueCollection nvc = Request.Form;
            int aanvullendPersoonInformatieId = Int32.Parse(nvc["aanvullendPersoonInformatieId"]);
            AanvullendPersoonDAL.verwijderAanvullendPersoonInDatabase(aanvullendPersoonInformatieId);

            return RedirectToAction("");
        }

        [HttpGet]
        public ActionResult AanvullendPersoonLijst(int persoonid)
        {
            AanvullendPersoonModel model = new AanvullendPersoonModel();

            model.aanvullendPersoonInformatieLijst = AanvullendPersoonDAL.aanvullendePersoonInformatieVan(persoonid);
            model.persoonid = persoonid;

            return View(model);
        }
    }
}