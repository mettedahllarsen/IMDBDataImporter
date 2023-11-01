using IMDBDataImporter;
using IMDBDataImporter.Models;
using System.Collections.Immutable;
using System.Data.SqlClient;


string ConnString = "server=localhost;database=MovieDB;" + "user id=sa;password=Mela14ad;TrustServerCertificate=True";
SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

Console.WriteLine("Velkommen til IMDB Data Importer!");

while (true)
{
	Console.WriteLine("\nVil du slette data fra databasen eller tilføje data?");
	Console.WriteLine("1. Tilføje data");
	Console.WriteLine("2. Slette data");
	Console.WriteLine("0. Afslut");

	string choice = Console.ReadLine();

	switch (choice)
	{
		case "1":
			AddData(sqlConn);
			break;

		case "2":
			break;

		case "0":
			sqlConn.Close();
			return; // Afslut programmet

		default:
			Console.WriteLine("Ugyldigt valg. Prøv igen.");
			break;
	}
}

static void AddData(SqlConnection sqlConn)
{
	while (true)
	{
		Console.WriteLine("\nVælg tabel at tilføje data til:");
		Console.WriteLine("1. Names");
		Console.WriteLine("2. Titles");
		Console.WriteLine("3. TitleGenres");
		Console.WriteLine("4. Directors");
		Console.WriteLine("5. Genres");
		Console.WriteLine("6. KnownFor");
		Console.WriteLine("7. NamesProfessions");
		Console.WriteLine("8. Professions");
		Console.WriteLine("9. Writers");
		Console.WriteLine("0. Tilbage");

		string choice = Console.ReadLine();

		BulkInserter inserter = new BulkInserter();

		switch (choice)
		{
			case "0":
				break;
			case "1":
				break;

			case "2":
				// Implementer logik til tilføjelse af data til Titles
				List<Title> titles = new List<Title>();
				foreach (string line in File.ReadLines(@"C:\temp\title.basics.tsv").Skip(1).Take(100))
				{
					Console.WriteLine(line); // Add this line for debugging

					string[] values = line.Split("\t");
					if (values.Length == 9)
					{
						titles.Add(new Title(values[0], values[1], values[2], values[3],
							ConvertToBool(values[4]), ConvertToInt(values[5]),
							ConvertToInt(values[6]), ConvertToInt(values[7])
							));
					}
				}

				// Brug BulkInserter til at indsætte data
				inserter.InsertDataIntoTitles(sqlConn, titles);

				break;

			case "3":
				break;

			case "4":
				break;

			case "5":
				break;

			case "6":
				break;

			case "7":
				break;

			case "8":
				break;

			case "9":
				break;

			default:
				Console.WriteLine("Ugyldigt valg. Prøv igen.");
				break;
		}
	}

	bool ConvertToBool(string input)
	{
		if (input == "0")
		{
			return false;
		}
		else if (input == "1")
		{
			return true;
		}
		throw new ArgumentException(
			"Kolonne er ikke 0 eller 1, men " + input);
	}

	int? ConvertToInt(string input)
	{
		if (input.ToLower() == @"\n")
		{
			return null;
		}
		else
		{
			return int.Parse(input);
		}
	}
}

