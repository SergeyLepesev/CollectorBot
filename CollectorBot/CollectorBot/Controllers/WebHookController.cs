using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CollectorBot.Controllers {
    [Route("api")]
    [ApiController]
    public class WebHookController : Controller {
        [HttpPost("webhook")]
        public async Task<IActionResult> Proccess(Update update) {
            return Ok();
        }
    }
}