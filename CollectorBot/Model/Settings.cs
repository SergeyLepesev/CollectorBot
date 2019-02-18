using Microsoft.Extensions.Configuration;

namespace CollectorBot.Model {
    public class Settings {
        private IConfiguration _configuration;

        public Settings(IConfiguration configuration) {
            _configuration = configuration;
        }

        public string this[string key] => _configuration[key];
    }
}