﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace Smart.FA.Catalog.UnitTests.Data;

public class JsonFileDataAttribute : DataAttribute
{
    private readonly string _filePath;
    private readonly string _propertyName;

    /// <summary>
    /// Load data from a JSON file as the data source for a theory
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
    public JsonFileDataAttribute(string filePath)
        : this(filePath, null)
    {
    }

    /// <summary>
    /// Load data from a JSON file as the data source for a theory
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
    /// <param name="propertyName">The name of the property on the JSON file that contains the data for the test</param>
    public JsonFileDataAttribute(string filePath, string propertyName)
    {
        _filePath = filePath;
        _propertyName = propertyName;
    }

    /// <inheritDoc />
    public override IEnumerable<object[]>? GetData(MethodInfo testMethod)
    {
        if (testMethod == null)
        {
            throw new ArgumentNullException(nameof(testMethod));
        }

        // Get the absolute path to the JSON file
        var path = Path.IsPathRooted(_filePath)
            ? _filePath
            : Path.Combine(Directory.GetCurrentDirectory(), "Data", _filePath);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"Could not find file at path: {path}");
        }

        // Load the file
        var fileData = File.ReadAllText(path);

        if (string.IsNullOrEmpty(_propertyName))
        {
            //whole file is the data
           yield return JsonSerializer.Deserialize<object[]>(fileData) ??  throw new Exception();
        }

        // Only use the specified property as the data
        var allData =  JsonNode.Parse(fileData);
        var data = allData?[_propertyName].AsArray() ?? throw new Exception();
        foreach (var objectTest in data)
        {
            yield return objectTest!.AsObject().Select(keyvalue => (object?)keyvalue.Value!.ToString()).ToArray()!;
        }
    }
}

