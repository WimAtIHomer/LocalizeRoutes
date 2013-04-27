using System.Web.Mvc;

namespace WebsiteLocalizeRoutesService.Controllers
{
    public partial class ErrorController : Controller
    {
        //
        // GET: /Error/

        public virtual ActionResult Index(int? id)
        {
            return View();
        }

    }
}
