using System.Threading.Tasks;
using CollectorBot.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CollectorBot.Controllers {
    [Route("api")]
    [ApiController]
    public class WebHookController : Controller {
        private readonly TelegramCommandService _commandService;

        public WebHookController(TelegramCommandService commandService) {
            _commandService = commandService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Proccess(Update update) {
            await _commandService.ExecuteCommand(update.Message);
            return Ok();
        }
    }
}