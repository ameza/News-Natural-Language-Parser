using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NewsParserDomain.Controllers;
using NewsParserDomain.Models;
using NewsParserParadigmas.Models;


namespace NewsParserParadigmas.Controllers
{
    public class HomeController : Controller
    {
     

        public ActionResult Index()
        {
          
            return View(new ReportViewModel(new List<ParseResult>(),String.Empty,String.Empty));
        }

        public ActionResult Analizar(FormCollection resultFormCollection)
        {
           try
            {
                var resultados = new List<ParseResult>();
                var newsItems = NewsController.GetNews(resultFormCollection["txtUrlFeed"]);
                var identificador = 1;
                foreach (var item in newsItems.Select(newsItem => ParserController.ParseString(newsItem, identificador.ToString())))
                {
                    resultados.Add(item);
                    identificador++;
                }

                return View("Index", new ReportViewModel(resultados, resultFormCollection["txtWord"], resultFormCollection["ddlCategoria"]));
           }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home", new {});
            }
           
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}