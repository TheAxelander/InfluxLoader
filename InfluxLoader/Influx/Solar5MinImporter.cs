using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxLoader.Config.Csv;
using InfluxLoader.Config.Model;

namespace InfluxLoader.Influx;

public class Solar5MinImporter : InfluxImporter<Solar5MinRecord>
{
    private const string MeasurementName = "messung5min";
    
    public Solar5MinImporter(InfluxDbServer server, Solar5MinCsvMapping mapper) : base(server, mapper)
    {
    }

    protected override IEnumerable<PointData> BuildPoints(Solar5MinRecord record)
    {
        var points = new List<PointData>();
        var measurement = PointData.Measurement(MeasurementName).Timestamp(record.Date, WritePrecision.S);
        points.AddRange(new List<PointData>()
        {
            measurement.Field(nameof(record.TotalWatt), record.TotalWatt),
            measurement.Field(nameof(record.WattPhaseA), record.WattPhaseA),
            measurement.Field(nameof(record.WattPhaseB), record.WattPhaseB),
            measurement.Field(nameof(record.WattPhaseC), record.WattPhaseC)
        });

        return points;
    }
}