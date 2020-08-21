using LibraryManagementSystem.Data.Models; 
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public abstract class ExceptionController : Controller
    {
        /// <summary>
        /// Handles 500 errors thrown in Controllers 
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        internal  ActionResult HandleServerError(LogicError err)
        {
            var error = err;
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, error.Message); 
        }
    }
}