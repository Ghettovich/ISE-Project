using AbInitio.Web.DAL;
using AbInitio.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    
    public class BeheerController : Controller
    {
        // GET: Beheer
        public ActionResult Index()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = AccountDAL.AllePersonen();
            return View(viewmodel);
        }

        public ActionResult AllePersonenTest()
        {
            BeheerViewModel viewmodel = new BeheerViewModel();
            viewmodel.PersoonLijst = AccountDAL.AllePersonen();
            return View(viewmodel);
        }



    }
}