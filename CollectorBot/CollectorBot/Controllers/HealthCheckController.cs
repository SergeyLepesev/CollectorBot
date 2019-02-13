using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CollectorBot.Controllers {
    [Route("api/health")]
    [ApiController]
    public class HealthCheckController : ControllerBase {
        [HttpGet("check")]
        public IActionResult HealthCheck() {
            return Ok();
        }
    }
}