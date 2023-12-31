using InfluxLoader.Config.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace InfluxLoader.Config;

public class ConfigReader
{
    private static ConfigReader _current = new ConfigReader();
    public static ConfigReader Current => _current;
    
    public InflConfig GetConfig()
    {
        // Ensure config folder exists
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var configFolderPath = Path.Combine(appDataPath, "infl");
        Directory.CreateDirectory(configFolderPath);

        // Ensure config file exists
        var configFilePath = Path.Combine(configFolderPath, "config.yml");
        if (!File.Exists(configFilePath))
        {
            throw new Exception("config.yml file not available");
        }
        
        // Read config
        var content = File.ReadAllText(configFilePath);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();
        return deserializer.Deserialize<InflConfig>(content);
    }
}