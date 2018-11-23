using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.PlantFilter.Contract;

namespace WMS.PlantFilter.Web.Models
{
    public class ReponseResult
    {
        public ResponseCode Code { get; set; }
        public string Message { get; set; }

        public dynamic Data { get; set; }
    }
}