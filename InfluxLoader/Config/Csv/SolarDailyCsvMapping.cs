using InfluxLoader.Config.Model;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace InfluxLoader.Config.Csv;

public class SolarDailyCsvMapping : CsvMapping<SolarDayRecord>
{
    public SolarDailyCsvMapping() : base()
    {
        MapProperty(0, x => x.Date, new DateTimeConverter("dd.MM.yyyy HH:mm:ss"));
        MapProperty(1, x => x.TotalWatt);
    }
}