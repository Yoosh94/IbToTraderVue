// See https://aka.ms/new-console-template for more information
// This program will convert the `TraderVue` report from IB into a useable format.
// The current TraderVue report has the following format
// Date/Time , UnderlyingSymbol, Quantity, Price, Symbol, Buy/Sell, Commission, Expiry, Put/Call, Description.

Console.WriteLine("Enter file name inside the downloads folder: ");
// Import CSV
Console.Write("~/Downloads/");
// var fileName = Console.ReadLine();
var homeFolder = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
// var pathToSearch = Path.GetRelativePath(Directory.GetCurrentDirectory(), $"{homeFolder}/Downloads/{fileName}");
var pathToSearch = Path.GetRelativePath(Directory.GetCurrentDirectory(), $"{homeFolder}/Downloads/TraderVue (10).csv");

var csvFile = File.ReadAllLines(pathToSearch);

var newCsv = new List<string> {GenerateCsvHeaders()};

// Split the incoming csv into equities, options and forex
foreach (var entry in csvFile.Skip(1))
{
    // Clean each string to remove all quotation marks.
    var cleanedEntry = entry.Replace("\"", "");
    var row = cleanedEntry.Split(",");
    
    if (string.IsNullOrEmpty(row[2]) && !row[5].Contains('.'))
    {
        // This is an equity
        ProcessEquityTrade(newCsv,row);
    }
    else if(!string.IsNullOrEmpty(row[2]))
    {
        // This is an options trade
        ProcessOptionsTrade(newCsv,row);
    }
}

string GenerateCsvHeaders()
{
    var headers = new List<string>{"Date", "Time", "Symbol", "Quantity", "Price", "Side", "Options"};
    return string.Join(',', headers);
}

void ProcessEquityTrade(ICollection<string> csv, IReadOnlyList<string> ibRow)
{
    string[] formattedRow = {ibRow[0], ibRow[1], ibRow[5], ibRow[3], ibRow[4], ibRow[6]};
    csv.Add(string.Join(',',formattedRow));
};

void ProcessOptionsTrade(ICollection<string> csv, IReadOnlyList<string> ibRow)
{
    string[] formattedRow = {ibRow[0], ibRow[1], ibRow[2], ibRow[3], ibRow[4], ibRow[6], ibRow[5]};
    csv.Add(string.Join(',',formattedRow));
}
