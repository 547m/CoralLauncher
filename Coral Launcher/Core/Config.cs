using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CoralLauncher.Core
{
    public static class Config
    {
        private static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Coral", "Config.json");
        private static Dictionary<string, Dictionary<string, string>> configData = new();

        static Config()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    configData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json) ?? new();
                }
                else
                {
                    configData = new Dictionary<string, Dictionary<string, string>>();
                }
            }
            catch
            {
                configData = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        private static void SaveConfig()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath) ?? string.Empty);
                string json = JsonConvert.SerializeObject(configData, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save config: {ex.Message}");
            }
        }

        public static void WriteToConfig(string section, string key, string value)
        {
            if (!configData.ContainsKey(section))
                configData[section] = new Dictionary<string, string>();

            configData[section][key] = value;
            SaveConfig();
        }

        public static string ReadFromConfig(string section, string key, string defaultValue = "")
        {
            return configData.TryGetValue(section, out var sectionData) && sectionData.TryGetValue(key, out var value)
                ? value
                : defaultValue;
        }
    }
}
