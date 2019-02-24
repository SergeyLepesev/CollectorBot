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
        public async Task<IActionResult> Process(Update update) {

            if (update.Message != null)
            {
                await _commandService.ExecuteCommand(update.Message);
            }   
            else if (update.CallbackQuery != null)
            {
                await _commandService.Process(update);
            }
            else
            {
                return BadRequest();
            }
                
            return Ok();
        }
    }
}