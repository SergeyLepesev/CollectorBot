using CollectorBot.Extension.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace CollectorBot.Extension {
    public static class ServiceCollectionExtension {
        public static void AddTelegramBotClient(this IServiceCollection service, IConfiguration configuration) {
            var telegramBotParameters = configuration.GetSection("TelegramBotParameters").Get<TelegramBotParameters>();
            var telegramBotClient = new TelegramBotClient(telegramBotParameters.Token);
            telegramBotClient.SetWebhookAsync(telegramBotParameters.WebHookUrl);

            service.AddSingleton<ITelegramBotClient>(telegramBotClient);
        }
    }
}