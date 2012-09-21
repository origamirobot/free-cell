using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeCell_Web.Controllers
{
    public class MainController : Controller
    {

		/// <summary>
		/// Displays the view that allows the user to select which type of free cell game they want to play.
		/// </summary>
		/// <returns></returns>
		public ActionResult Select()
		{
			return View();
		}

		/// <summary>
		/// Returns the view for playing the free cell game using HTML5's canvas tag.s
		/// </summary>
		/// <returns></returns>
        public ActionResult CanvasGame()
        {
            return View();
        }


		/// <summary>
		/// Displays the view for playing the free cell game using HTML tags.
		/// </summary>
		/// <returns></returns>
		public ActionResult HtmlGame()
		{
			return View();
		}



    }
}
