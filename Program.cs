using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir); 
var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);

// File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

var sales_201 = File.ReadAllText($"stores{Path.DirectorySeparatorChar}201{Path.DirectorySeparatorChar}sales.json");
var sales_202 = File.ReadAllText($"stores{Path.DirectorySeparatorChar}202{Path.DirectorySeparatorChar}sales.json");
var sales_203 = File.ReadAllText($"stores{Path.DirectorySeparatorChar}203{Path.DirectorySeparatorChar}sales.json");
var sales_204 = File.ReadAllText($"stores{Path.DirectorySeparatorChar}204{Path.DirectorySeparatorChar}sales.json");
var total_sales = File.ReadAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt");

var sales = new[] { sales_201, sales_202, sales_203, sales_204, total_sales };
if (sales.All(s => s != null))
{
    Console.Write("Total Sales: ");
    Console.WriteLine($"${total_sales} ");
    Console.WriteLine("Details: ");
    Console.Write("Sales 201: ");
    Console.WriteLine($"${JsonConvert.DeserializeObject<SalesTotal>(sales_201)?.Total ?? 0 } ");
    Console.Write("Sales 202: ");
    Console.WriteLine($"${JsonConvert.DeserializeObject<SalesTotal>(sales_202)?.Total ?? 0 }");
    Console.Write("Sales 203: ");
    Console.WriteLine($"${JsonConvert.DeserializeObject<SalesTotal>(sales_203)?.Total ?? 0 }");
    Console.Write("Sales 204: ");
    Console.WriteLine($"${JsonConvert.DeserializeObject<SalesTotal>(sales_204)?.Total ?? 0 }");
}



foreach (var file in salesFiles)
{
    // Console.WriteLine(file);
}

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

class SalesTotal
{
    public double Total { get; set; }
}

record SalesData (double Total);