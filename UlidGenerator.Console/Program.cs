int amount = 0;
string? input;

do
{
    Console.Write("Enter the amount of ULIDs to generate (or 'q' to quit): ");
    input = Console.ReadLine();

    if (input?.ToLower() == "q")
        Environment.Exit(0);

    if (!int.TryParse(input, out amount))
    {
        Console.WriteLine("Please enter a valid number.");
        continue;
    }

    var ulids = new List<string>();
    for (int i = 0; i < amount; i++)
    {
        ulids.Add(Ulid.NewUlid().ToString());
    }

    // Display ULIDs
    foreach (var ulid in ulids)
    {
        Console.WriteLine(ulid);
    }

    Console.Write("\nWould you like to export these ULIDs to a text file? (y/n): ");
    var exportChoice = Console.ReadLine()?.ToLower();

    if (exportChoice == "y")
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"ulids_{timestamp}.txt";
        
        try
        {
            File.WriteAllLines(fileName, ulids);
            Console.WriteLine($"ULIDs have been exported to {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting to file: {ex.Message}");
        }
    }

    Console.Write("\nWould you like to generate more ULIDs? (y/n): ");
    var continueChoice = Console.ReadLine()?.ToLower();
    if (continueChoice != "y")
    {
        Environment.Exit(0);
    }

    Console.WriteLine(); // Add blank line for better readability
} while (true);
