using AzDoDashboard.CLI.Services;
using AzDoDashboard.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logger = LoggerFactory
    .Create(config => config.AddConsole())
    .CreateLogger<AzureDevOpsClient>();

var client = new AzureDevOpsClient(
    "{Azure Devops Organization URL",
    "{Azure Devops PAT}",
    new LocalCache("AzDoCache"), logger);


// method to get all pullrequests 
var pullRequests = await client.GetAllOpenPullRequestsAsync();

Console.WriteLine(JsonSerializer.Serialize(pullRequests));
// method to get all projects in an organization
var projects = await client.GetAllProjectsAsync();

foreach (var project in projects)
{
    // method to get pullRequests by project Name
    var pullRequest = await client.GetOpenPullRequestsByProjectAsync(project.Name);
    var test = pullRequest;
}


// this is just junk for testing the cache functionality
var cache = new LocalCache("AzDoCache");
var result = await cache.GetOrExecuteAsync("whatever", async () =>
{
    return await Task.FromResult(new
    {
        Greeting = "hello",
        FavoriteMonster = "Godzilla"
    });
}, TimeSpan.FromMinutes(5));

Console.WriteLine(result);

Console.ReadLine();