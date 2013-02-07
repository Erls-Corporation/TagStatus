using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TagStatus.Models;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Dynamic;
using System.Data;
using System.Json;

namespace TagStatus.Controllers
{
    
    public class TagController : Controller
    {
        //
        // GET: /Tag/

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Lookup()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Lookup(TagModel model)
        {
            if (ModelState.IsValid)
            {
            //    //string url = "https://myapicom/svc1/v2/tags?project=usax&format=JSON";

                model.Description = model.Name;


            //   /* CallApi apiPOItems = new CallApi()
            //    {
            //        ApiUrl = ConfigurationManager.AppSettings["Url_PO_LineItems"].Replace("{PO_NUMBER}", PONumber),
            //        Format = "xml",
            //        PageSize = intPageSize.ToString(),
            //        Page = mintCurPage.ToString(),
            //        Filter = getFilterString(),
            //        OrderBy = ((hdnPOLNSortOrder.Value != "") ? hdnPOLNSortOrder.Value : "poln_lineno asc")
            //    };

            //    ds = apiPOItems.RetrieveData(ref strErrorMsg, ref intRecordCount);*/

            //    string strErrorMsg = "";
            //    int intRecordCount = -1;
            //    DataSet ds;

            //    CallApi apiPOItems = new CallApi()
            //    {
                    
            //        //ApiUrl = "https://myapicom/svc1/v2/tags/00-RIE-FF-03?project=usax",
            //        ApiUrl = sApiUrl,
            //       // Format = "xml",
            //        Format = "",
            //        PageSize = "10",
            //        Page = "1",
            //        Filter = "",
            //        OrderBy = ""
            //    };

            //    CallApi.ConnectCallComplete = true;

            //   // ds = apiPOItems.RetrieveData(ref strErrorMsg, ref intRecordCount);
            //    string strjsonres = apiPOItems.RetrieveJSON();
            //    JsonObject jsonObject = (JsonObject)JsonValue.Parse(strjsonres);

            //    var strvar = JsonValue.Parse(strjsonres);

            //    var t1 = strvar["tags"];
            //    var t2 = t1["tags"];

            //    var t3 = t2["tags_delivery_status"];

            //    String strdeliverystatus = t3.ToString();
            //    strdeliverystatus = strdeliverystatus.Replace("\"","");
            //    strdeliverystatus = strdeliverystatus.Replace("\\u000a", "<bR>");
            //    strdeliverystatus = "<b>" + strdeliverystatus + "<b>";

            //   // var v = jsonObject["tags"];
            //   //// JsonArray items = (JsonArray)jsonObject["tags"];
            //   // var vv = v[1];
            //   // JsonValue jsval;
            //   // bool b = jsonObject.TryGetValue("tags_delivery_status", out jsval);

            //    if (strErrorMsg != "")
            //    {
            //        // There was an ERROR
            //    }

            //    //model.Description = ds.Tables[2].Rows[0]["tags_description"].ToString();
               

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
