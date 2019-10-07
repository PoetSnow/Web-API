using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Services.EF;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.WebApi.Controllers
{
  
    public class PosCYController : BaseController
    {

        public string PostService([FromBody]List<PostData> data)
        {

            var result = ReciveDataHandlerHelper.HandleReciveData(data, ReciveDataHandler);
            return result;
        }
    }
}