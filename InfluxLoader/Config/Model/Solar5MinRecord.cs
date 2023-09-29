namespace InfluxLoader.Config.Model;

public class Solar5MinRecord
{
    public DateTime Date { get; set; }
    public int TotalWatt { get; set; }
    public int WattPhaseA { get; set; }
    public int WattPhaseB { get; set; }
    public int WattPhaseC { get; set; }
}