using System.Text;
using InfluxDB.Client.Writes;
using InfluxLoader.Config.Model;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace InfluxLoader.Influx;

public abstract class InfluxImporter<TModel> : IInfluxImporter
{
    protected readonly InfluxConnector InfluxConnector;
    protected readonly ICsvMapping<TModel> Mapper;

    protected InfluxImporter(InfluxDbServer server, ICsvMapping<TModel> mapper)
    {
        InfluxConnector = new InfluxConnector(server.ApiUrl, server.Token, server.Organization);
        Mapper = mapper;
    }

    public virtual async Task ImportDataAsync(string filePath, string bucket)
    {
        var points = new List<PointData>();
        var parsedRecords = await ParseCsvAsync(filePath);
        foreach (var parsedRecord in parsedRecords)
        {
            points.AddRange(BuildPoints(parsedRecord));
        }
        InfluxConnector.WriteMeasurement(bucket, points);
    }

    protected abstract IEnumerable<PointData> BuildPoints(TModel record);

    protected async Task<List<TModel>> ParseCsvAsync(string filePath)
    {
        // Initialize CsvReader
        var options = new Options('"', '\\', ',');
        var tokenizer = new RFC4180Tokenizer(options);
        var csvParserOptions = new CsvParserOptions(true, tokenizer);
        var csvParser = new CsvParser<TModel>(csvParserOptions, Mapper);

        // Parse csv file
        return await Task.Run(() =>
        {
            var parsedResults = csvParser
                .ReadFromFile(filePath, Encoding.GetEncoding("utf-8"))
                .ToList();
            
            var result = parsedResults
                .Where(i => i.IsValid)
                .Select(i => i.Result)
                .ToList();
            return result;
        });
    }
}