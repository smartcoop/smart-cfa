using System.Text.Json;
using System.Text.Json.Nodes;

namespace Smart.FA.Catalog.Tests.Common.Extensions;

/// <summary>
/// Set of extensions for injecting data from json files inside tests
/// </summary>
public static class JsonFileTestExtensions
{
    /// <summary>
    /// Create a list of data of type <see cref="T"/> parsed and deserialized from a list of name <see cref="objectName"/> json file located at relative path <see cref="filePath"/>
    /// </summary>
    /// <param name="filePath">Source Json file</param>
    /// <param name="objectName">Name of the object</param>
    /// <typeparam name="T">Type to deserialize</typeparam>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static IEnumerable<T> CreateDataSet<T>(string filePath, string? objectName = null)
    {
        // Get the absolute path to the JSON file
        var path = Path.IsPathRooted(filePath)
            ? filePath
            : Path.Combine(Directory.GetCurrentDirectory(), "Data", filePath);

        // Load the file
        var fileData = File.ReadAllText(path);

        if (string.IsNullOrEmpty(objectName))
        {
            //whole file is the data
            yield return JsonSerializer.Deserialize<T>(fileData) ?? throw new JsonException();
        }

        // Only use the specified property as the data
        var allData = JsonNode.Parse(fileData);
        var data = allData?[objectName!]?.AsArray() ?? throw new JsonException();
        foreach (var objectTest in data)
        {
            yield return objectTest.AsObject().Deserialize<T>()!;
        }
    }
}
