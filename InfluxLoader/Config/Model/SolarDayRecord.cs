namespace InfluxLoader.Config.Model;

public class SolarDayRecord
{
    public DateTime Date { get; set; }
    public int TotalWatt { get; set; }
    public int Delta { get; set; }
}