using Cocona;
using InfluxLoader.Config;
using InfluxLoader.Config.Csv;
using InfluxLoader.Influx;

var config = ConfigReader.Current.GetConfig();
var app = CoconaApp.Create();

app.AddCommand("import", async (
        [Argument(Description = "Folder path of files to be imported")]
        string filePath,
        [Option('p', Description = "Name of the profile to be used for import")]
        string profile,
        [Option('b', Description = "Name of the Bucket in InfluxDb")]
        string bucket) =>
    await ImportFileAsync(filePath, profile, bucket))
    .WithDescription("Import CSV file to InfluxDb using a Profile");

app.AddCommand("flush", async (
        [Option('b', Description = "Name of the Bucket in InfluxDb")]
        string bucket,
        [Option('m', Description = "Name of the Measurement that should be deleted")]
        string measurement) => 
    await FlushMeasurementAsync(bucket, measurement))
    .WithDescription("Flush Measurement in InfluxDb");

app.Run();
return;


async Task ImportFileAsync(string filePath, string profile, string bucket)
{
    IInfluxImporter importer;
    switch (profile)
    {
        case "SolarDaily":
            importer = new SolarDailyImporter(config.Server, new SolarDailyCsvMapping());
            break;
        case "Solar5Min":
            importer = new Solar5MinImporter(config.Server, new Solar5MinCsvMapping());
            break;
        default:
            Console.WriteLine("Profile not found.");
            return;
    }
    await importer.ImportDataAsync(filePath, bucket);
}

async Task FlushMeasurementAsync(string bucket, string measurement)
{
    var server = config.Server;
    var connector = new InfluxConnector(server.ApiUrl, server.Token, server.Organization);
    await connector.FlushMeasurementAsync(bucket, measurement);
}