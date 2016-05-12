using AbInitio.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbInitio.Web.Controllers
{
    [Authorize(Roles = "1,2,3")]
    public class StamboomController : Controller
    {
        // GET: Stamboom
        [HttpGet]
        public ActionResult Index()
        {
            return View();
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