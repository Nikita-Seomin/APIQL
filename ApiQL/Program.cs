using System.Text.Json;
using ApiQL;

using BenchmarkDotNet.Running;

//var summary = BenchmarkRunner.Run<Bench>();

var jDoc = JsonDocument.Parse(Bench.json);

var a = jDoc.RootElement.EnumerateObject();
        foreach (var item in a)
        {
            if (item.Value.ValueKind is JsonValueKind.Array)
            {

            }
            else
            {

            }
        }

int f = 0;
