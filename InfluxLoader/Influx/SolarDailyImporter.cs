using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxLoader.Config.Csv;
using InfluxLoader.Config.Model;

namespace InfluxLoader.Influx;

public class SolarDailyImporter : InfluxImporter<SolarDayRecord>
{
    private const string MeasurementName = "messung";
    
    public SolarDailyImporter(InfluxDbServer server, SolarDailyCsvMapping mapper) : base(server, mapper)
    {
    }

    protected override IEnumerable<PointData> BuildPoints(SolarDayRecord record)
    {
        var points = new List<PointData>();
        var measurement = PointData.Measurement(MeasurementName).Timestamp(record.Date, WritePrecision.S);
        points.AddRange(new List<PointData>()
        {
            measurement.Field(nameof(record.TotalWatt), record.TotalWatt),
            measurement.Field(nameof(record.Delta), record.Delta),
        });

        return points;
    }

    public override async Task ImportDataAsync(string filePath, string bucket)
    {
        var points = new List<PointData>();
        var records = 
            (await ParseCsvAsync(filePath))
            .OrderByDescending(i => i.Date) // Switch order for Stack
            .ToList();

        var stack = new System.Collections.Stack(records);
        if (stack.Pop() is not SolarDayRecord newPointRecord) return;
        
        while (stack.Count != 0)
        {
            var followUpPointRecord = stack.Pop() as SolarDayRecord;
            newPointRecord.Delta = followUpPointRecord!.TotalWatt - newPointRecord.TotalWatt;
            points.AddRange(BuildPoints(newPointRecord));
            newPointRecord = followUpPointRecord;
        }
        
        InfluxConnector.WriteMeasurement(bucket, points);
    }
}