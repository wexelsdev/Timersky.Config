using Timersky.Log;
using Tomlet;

namespace Timersky.Config;

public sealed class ConfigManager
{
    private static readonly string ConfigFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Config.toml";
    private static readonly Logger Log = new();
    
    private static T GenConfig<T>() where T : IConfig
    {
        File.Create(ConfigFilePath).Close();
            
        T config = Activator.CreateInstance<T>();

        using FileStream file = new(ConfigFilePath, FileMode.Append);
        using StreamWriter writer = new(file);
        writer.WriteLine(TomletMain.TomlStringFrom(config));

        return config;
    }

    /// <summary>
    /// Loads the configuration of the specified type from a TOML file.
    /// If the file does not exist, a new configuration is generated and returned.
    /// In case of a parsing error, the invalid file is deleted, and a new configuration is generated.
    /// </summary>
    /// <typeparam name="T">The type of the configuration to load. Must implement the <see cref="IConfig"/> interface.</typeparam>
    /// <returns>An instance of the configuration object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="Exception">Logs and handles exceptions that occur during configuration loading.</exception>
    public static T LoadConfig<T>() where T : IConfig
    {
        T config;
    
        if (!File.Exists(ConfigFilePath))
        {
            config = GenConfig<T>();
        }
        else
        {
            try
            {
                config = TomletMain.To<T>(TomlParser.ParseFile(ConfigFilePath));
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            
                File.Delete(ConfigFilePath);
            
                config = GenConfig<T>();
            }
        }
        
        return config;
    }
}
