using InfluxLoader.Config.Model;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace InfluxLoader.Config.Csv;

public class Solar5MinCsvMapping : CsvMapping<Solar5MinRecord>
{
    public Solar5MinCsvMapping() : base()
    {
        MapProperty(0, x => x.Date, new DateTimeConverter("dd.MM.yyyy HH:mm:ss"));
        MapProperty(7, x => x.TotalWatt);
        MapProperty(8, x => x.WattPhaseA);
        MapProperty(9, x => x.WattPhaseB);
        MapProperty(10, x => x.WattPhaseC);
    }
}