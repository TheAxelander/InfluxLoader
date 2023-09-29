using InfluxDB.Client;
using InfluxDB.Client.Writes;

namespace InfluxLoader.Influx;

public class InfluxConnector
{
    private readonly string _organization;
    private readonly InfluxDBClient _influxDbClient;

    public InfluxConnector(string url, string token, string organization)
    {
        _organization = organization;
        var options = new InfluxDBClientOptions(url)
        {
            Org = organization,
            Token = token
        };
        _influxDbClient = new InfluxDBClient(options);
    }
    
    public void WriteMeasurement(string bucket, List<PointData> points)
    {
        using var writeApi = _influxDbClient.GetWriteApi();
        writeApi.WritePoints(points, bucket);
    }
    
    public async Task FlushMeasurementAsync(string bucket, string measurement)
    {
        var deleteApi = _influxDbClient.GetDeleteApi();

        await deleteApi.Delete(
            new DateTime(1900,1,1),
            DateTime.Now,
            $"_measurement=\"{measurement}\"",
            bucket,
            _organization
        );
    }
}