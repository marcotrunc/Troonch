using Microsoft.AspNetCore.Mvc;

namespace Troonch.Retail.App.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statuscode}/{exceptionMessage?}")]
        public IActionResult HttpStatusCodeHandler(int statuscode, string exceptionMessage = "")
        {
            ViewBag.StatusCode = statuscode;

            ViewBag.ExceptionMessage = exceptionMessage;

            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    break;

                case 500:
                    ViewBag.ErrorMessage = "Internal server error!";
                    break;
                
                default:
                    ViewBag.ErrorMessage = "Generic Error";
                    break; ;
            }
            return View("ErrorPage");
        }
    }
}
