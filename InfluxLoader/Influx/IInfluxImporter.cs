namespace InfluxLoader.Influx;

public interface IInfluxImporter
{
    public Task ImportDataAsync(string filePath, string bucket);
}