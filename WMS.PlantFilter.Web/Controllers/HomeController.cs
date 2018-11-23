using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.PlantFilter.Contract;
using WMS.PlantFilter.Web.Models;

namespace WMS.PlantFilter.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult SaveRelPlants(FormCollection form)
        {
            var result = new ReponseResult();
            try
            {
                var newlist = new List<RelPlantWeb>();
                if (!string.IsNullOrWhiteSpace(form["data"]))
                {
                    foreach (var item in form["data"].Split(','))
	                {
                        newlist.Add(new RelPlantWeb() { Plant_code = item, Web = 1 });
	                } 
                }

                var invokeResult = ClientProxy.ClientRelExecute<InvokeResult>(c => c.SaveRelPlantWeb(newlist));
                result.Code = invokeResult.Code;
                result.Data = null;
                result.Message = invokeResult.Message;
               
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.FAILED;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public JsonResult GetRelPlants(string name)
        {
            var result = new ReponseResult();
            try
            {
                result.Code = ResponseCode.SUCCESS;

                var left = ClientProxy.ClientPlantExecute<List<PlantSource>>(c => c.QueryPlants(name))
                    .Select(item => new { value = item.Plant_code, text = item.Plant_name });


                var right = ClientProxy.ClientRelExecute<List<RelPlantWeb>>(c => c.QueryRelPlantWebs())
                    .Select(item => new { value = item.Plant_code, text = item.Plant_name });

                result.Data = new { Left = left, Right = right };
            }
            catch (Exception ex)
            {
                result.Code = ResponseCode.FAILED;
                result.Message = ex.Message;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}