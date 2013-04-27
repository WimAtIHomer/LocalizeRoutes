using System;
using System.Web.Mvc;

namespace WebsiteLocalizeRoutesService.Controllers
{
    public partial class HomeController : Controller
    {

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult About()
        {
            //throw new ApplicationException("View does not exist!");
            return View();
        }
    }
}
