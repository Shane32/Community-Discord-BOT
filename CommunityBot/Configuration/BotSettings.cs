using CommunityBot.Entities;
using System;
using CommunityBot.Helpers;

namespace CommunityBot.Configuration
{
    public class BotSettings
    {
        internal BotConfig config;

        private readonly string configFile = "config.json";
        private readonly JsonDataStorage jsonDataStorage;

        public BotSettings(JsonDataStorage jsonDataStorage)
        {
            this.jsonDataStorage = jsonDataStorage;
            LoadConfig();
        }

        internal void LoadConfig()
        {
            if (jsonDataStorage.LocalFileExists(configFile))
            {
                config = jsonDataStorage.RestoreObject<BotConfig>(configFile);
            }
            else
            {
                // Setting up defaults
                config = new BotConfig()
                {
                    Token = "YOUR-TOKEN-HERE"
                };
                jsonDataStorage.StoreObject(config, configFile, useIndentations: true);
            }
        }

    }
}
