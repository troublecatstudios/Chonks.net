using Newtonsoft.Json;
using System;

namespace Chonks {
    public static class SerializationUtility {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings() {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static void Configure(Func<JsonSerializerSettings, JsonSerializerSettings> configurator) => _settings = configurator(_settings);

        public static string SerializeObject<T>(T instance) => JsonConvert.SerializeObject(instance, _settings);

        public static T DeserializeObject<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
    }
}
