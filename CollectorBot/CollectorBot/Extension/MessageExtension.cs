using Telegram.Bot.Types;

namespace CollectorBot.Extension {
    public static class MessageExtension {
        public static long GetChatId(this Message message) {
            return message.Chat.Id;
        }

        public static int GetTelegramUserId(this Message message) {
            return message.From.Id;
        }
    }
}