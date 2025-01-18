using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

public static class ConfigLoader
{
    public static string LicenseKey { get; private set; }


    public static string ResourceName = API.GetCurrentResourceName();

    static ConfigLoader()
    {
        LoadConfig();
    }

    public static void LoadConfig()
    {
        try
        {
            string configContent = API.LoadResourceFile(API.GetCurrentResourceName(), "config.ini");

            if (!string.IsNullOrEmpty(configContent))
            {
                Config config = new Config(configContent);
                LicenseKey = config.Get("LicenseKey", "none");
            }
            else
            {
                Console.WriteLine("Config file content is empty or not found.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading config: {e.Message}");
        }
    }
}