namespace InfluxLoader.Config.Model;

public class InfluxDbServer
{
    public string ApiUrl { get; set; }
    public string Token { get; set; }
    public string Organization { get; set; }

    public InfluxDbServer()
    {
        ApiUrl = string.Empty;
        Token = string.Empty;
        Organization = string.Empty;
    }
}