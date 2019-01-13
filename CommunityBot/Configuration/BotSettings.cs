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
                    Prefix = "$",
                    Token = "YOUR-TOKEN-HERE"
                };
                jsonDataStorage.StoreObject(config, configFile, useIndentations: true);
            }
        }

        public ActionResult SetCommandPrefix(string prefix)
        {
            var result = new ActionResult();
            
            config.Prefix = prefix;

            var saveSettingsResult = SaveSettings();

            result.Merge(saveSettingsResult);

            return result;
        }

        private ActionResult SaveSettings()
        {
            var result = new ActionResult();
            
            try
            {
                jsonDataStorage.StoreObject(config, configFile);
            }
            catch (Exception)
            {
                result.AddAlert(new Alert("Settings error", "Could not save the Settings", LevelEnum.Exception));
            }

            return result;
        }
    }
}
