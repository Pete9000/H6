using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace HomeSurveillanceApp.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        [Route("Secrets")]
        public string GetRandomToken()
        {
            DateTime thisTime = DateTime.Now;
            // get Denmark Standard Time zone - not sure about that
            bool isDaylight = TimeZoneInfo.Local.IsDaylightSavingTime(thisTime);
            var time = "";
            if (isDaylight)
                time = "It's DST " + DateTime.UtcNow.ToString();
            else
                time = DateTime.UtcNow.ToString();

            return time;
        }
    }
}
