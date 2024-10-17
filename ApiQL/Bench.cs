using System.Text.Json;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace ApiQL;

[InProcess]
[MemoryDiagnoser]
[RankColumn, MinColumn, MaxColumn, AllStatisticsColumn]
[GcServer(true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Bench
{
    public static string json = """
              {
                  "and": [
                      {
                          "gt": {
                              "attr_1693_": "41"
                          }
                      },
                      {
                          "eq": {
                              "attr_1692_": "1"
                          }
                      }
                  ],
                  "or": [
                      {
                          "gt": {
                              "attr_1693_": "41"
                          }
                      },
                      {
                          "eq": {
                              "attr_1692_": "1"
                          }
                      }
                  ]
              
              }
              """;

    [Benchmark(Baseline = true)]
    public static void DocBench()
    {
        for (var i = 0; i < 20; i++)
        {
            TraversalDoc(json);
        }
    }

    [Benchmark]
    public static void DocBenchV2()
    {
        for (var i = 0; i < 20; i++)
        {
            TraversalDocV2(json);
        }
    }

    static void TraversalDoc(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        TraverseJsonElement(root);
    }

    static void TraversalDocV2(string json)
    {
        using var document = JsonDocument.Parse(json);
        TraverseJsonElement(document.RootElement);
    }

    static void TraverseJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Undefined:
                throw new NotImplementedException();
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    // Console.WriteLine($"{indent}{property.Name}:");
                    TraverseJsonElement(property.Value);
                }
                break;

            case JsonValueKind.Array:
                var index = 0;
                foreach (var item in element.EnumerateArray())
                {
                    // Console.WriteLine($"{indent}[{index}]");
                    TraverseJsonElement(item);
                    index++;
                }
                break;

            case JsonValueKind.String:
                // Console.WriteLine($"{indent}\"{element.GetString()}\"");
                break;

            case JsonValueKind.Number:
                // Console.WriteLine($"{indent}{element.GetDecimal()}");
                break;

            case JsonValueKind.True:
            case JsonValueKind.False:
                // Console.WriteLine($"{indent}{element.GetBoolean()}");
                break;

            case JsonValueKind.Null:
                // Console.WriteLine($"{indent}null");
                break;

            default:
                throw new InvalidOperationException($"Unhandled JsonValueKind: {element.ValueKind}");
        }
    }
}
