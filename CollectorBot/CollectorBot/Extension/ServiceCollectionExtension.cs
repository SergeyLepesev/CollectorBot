using CollectorBot.Data;
using CollectorBot.Data.MongoRealization;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism;
using CollectorBot.Model;
using CollectorBot.TelegramCommands;
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

        public static void AddTelegramCommand(this IServiceCollection service) {
            service.AddScoped<ITelegramCommand, HelpCommand>();
        }

        public static void AddRepositoryAsync(this IServiceCollection service, IConfiguration configuration) {
            var mongoParameters = configuration.GetSection("MongoDB").Get<MongoParameters>();
            service.AddSingleton<EntityConstraint>();
            service.AddSingleton(mongoParameters);
            service.AddSingleton<MongoContext>();
            service.AddSingleton(typeof(IRepositoryAsync<>), typeof(MongoDbRepositoryAsync<>));
        }

        public static void AddSettings(this IServiceCollection service, IConfiguration configuration) {
            service.AddSingleton(new Settings(configuration));
        }
    }
}