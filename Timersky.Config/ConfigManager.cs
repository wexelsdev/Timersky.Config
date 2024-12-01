using Timersky.Log;
using Tomlet;
using Tomlet.Models;

namespace Timersky.Config;

public sealed class ConfigManager
{
    private static readonly string ConfigFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Config.toml";
    private static readonly Logger Log = new();
    
    private static TCfg GenConfig<TCfg>() where TCfg : IConfig
    {
        File.Create(ConfigFilePath).Close();
            
        TCfg config = Activator.CreateInstance<TCfg>();
            
        using (FileStream file = new(ConfigFilePath, FileMode.Append))
        {
            using (StreamWriter writer = new(file))
            {
                writer.WriteLine(TomletMain.TomlStringFrom(config));
            }
        }
            
        return config;
    }

    public static TCfg LoadConfig<TCfg>() where TCfg : IConfig
    {
        TCfg config;
        
        if (!File.Exists(ConfigFilePath))
        {
            config = GenConfig<TCfg>();
        }
        else
        {
            try
            {
                config = TomletMain.To<TCfg>(TomlParser.ParseFile(ConfigFilePath));
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                
                File.Delete(ConfigFilePath);
                
                config = GenConfig<TCfg>();
            }
        }
        
        return config;
    }
}