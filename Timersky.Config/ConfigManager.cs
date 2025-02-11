using Tomlet;

namespace Timersky.Config;

public sealed class ConfigManager
{
    private static T GenConfig<T>(string path) where T : IConfig
    {
        File.Create(path).Close();
            
        T config = Activator.CreateInstance<T>();

        using FileStream file = new(path, FileMode.Append);
        using StreamWriter writer = new(file);
        writer.WriteLine(TomletMain.TomlStringFrom(config));

        return config;
    }

    /// <summary>
    /// Loads the configuration of the specified type from a TOML file.
    /// If the file does not exist, a new configuration is generated and returned.
    /// In case of a parsing error, the invalid file is deleted, and a new configuration is generated.
    /// </summary>
    /// <param name="path">
    /// The file path where config will be stored. 
    /// If not specified, defaults to a "Config.toml" file in the application's base directory.
    /// </param>
    /// <typeparam name="T">The type of the configuration to load. Must implement the <see cref="IConfig"/> interface.</typeparam>
    /// <returns>An instance of the configuration object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="Exception">Logs and handles exceptions that occur during configuration loading.</exception>
    public static T LoadConfig<T>(string path = "") where T : IConfig
    {
        if (string.IsNullOrEmpty(path))
        {
            path = $"{AppDomain.CurrentDomain.BaseDirectory}Config.toml";
        }
        
        T config;
    
        if (!File.Exists(path))
        {
            config = GenConfig<T>(path);
        }
        else
        {
            try
            {
                config = TomletMain.To<T>(TomlParser.ParseFile(path));
            }
            catch (Exception e)
            {
                Log.Log.Error(e);
            
                File.Delete(path);
            
                config = GenConfig<T>(path);
            }
        }
        
        return config;
    }
}
