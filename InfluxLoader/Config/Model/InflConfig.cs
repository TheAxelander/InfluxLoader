namespace InfluxLoader.Config.Model;

public class InflConfig
{
    public InfluxDbServer Server { get; set; }

    public InflConfig()
    {
        Server = new();
    }
}