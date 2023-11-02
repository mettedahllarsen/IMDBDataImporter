using IMDBDataImporter.DataInserters;
using IMDBDataImporter.Models;
using System.Data.SqlClient;

// This C# program connects to a database using a SqlConnection object.
// It establishes a connection to the "MovieDB" database on the local server
// with the provided user ID and password. The TrustServerCertificate option
// indicates that the server's SSL certificate will be trusted. The program
// then proceeds to interact with the database based on user input.

string connectionString = "server=localhost;database=MovieDB;" + "user id=sa;password=Mela14ad;TrustServerCertificate=True";
SqlConnection sqlConnection = new SqlConnection(connectionString);

try
{
	sqlConnection.Open();

	Console.WriteLine("Velkommen til IMDB Data Importer!");

	while (true)
	{
		Console.WriteLine("\nVælg en handling:");
		Console.WriteLine("1. Indsæt data");
		Console.WriteLine("2. Slet data");
		Console.WriteLine("0. Afslut");

		Console.Write("\nIndtast dit valg (0-2):");
		string? choice = Console.ReadLine();

		switch (choice)
		{
			case "1":
				InsertData(sqlConnection);
				break;

			case "2":
				DeleteData(sqlConnection);
				break;

			case "0":
				sqlConnection.Close();
				return;

			default:
				Console.WriteLine("\nUgyldigt valg. Prøv igen.");
				break;
		}
	}
}
catch (Exception ex)
{
	Console.WriteLine("\nDer opstod en fejl ved oprettelse af forbindelsen: " + ex.Message);
}
finally
{
	sqlConnection.Close();
}


static void InsertData(SqlConnection sqlConnection)
{
	SqlBulkCopyInserter sqlBulkCopyInserter = new SqlBulkCopyInserter();
	SqlCommandInserter sqlCommandInserter = new SqlCommandInserter();

	while (true)
	{
		Console.WriteLine("\nVælg tabel:");
		Console.WriteLine("1. Names");
		Console.WriteLine("2. Titles");
		Console.WriteLine("3. Titles_Genres");
		Console.WriteLine("4. Directors");
		Console.WriteLine("5. Genres");
		Console.WriteLine("6. KnownFor");
		Console.WriteLine("7. Names_Professions");
		Console.WriteLine("8. Professions");
		Console.WriteLine("9. Writers");
		Console.WriteLine("0. Tilbage");

		Console.Write("\nIndtast dit valg (0-9):");
		string? choice = Console.ReadLine();

		switch (choice)
		{
			case "1": // Names
				sqlBulkCopyInserter.InsertDataIntoNames(sqlConnection, LoadNamesData());
				break;

			case "2": // Titles
				sqlBulkCopyInserter.InsertDataIntoTitles(sqlConnection, LoadTitlesData());
				break;

			case "3": // Titles_Genres
				sqlCommandInserter.InsertDataIntoTitlesGenres(sqlConnection, LoadTitlesData());
				break;

			case "4": // Directors
				sqlCommandInserter.InsertDataIntoDirectors(sqlConnection, LoadCrewsData());
				break;

			case "5": // Genres
				sqlCommandInserter.InsertDataIntoGenres(sqlConnection, LoadTitlesData());
				break;

			case "6": // KnownFor
				sqlCommandInserter.InsertDataIntoKnownFor(sqlConnection, LoadNamesData());
				break;

			case "7": // Names_Professions
				sqlCommandInserter.InsertDataIntoNamesProfessions(sqlConnection, LoadNamesData());
				break;

			case "8": // Professions
				sqlCommandInserter.InsertDataIntoProfessions(sqlConnection, LoadNamesData());
				break;

			case "9": // Writers
				sqlCommandInserter.InsertDataIntoWriters(sqlConnection, LoadCrewsData());
				break;

			case "0": // Tilbage
				break;

			default:
				Console.WriteLine("\nUgyldigt valg. Prøv igen.");
				break;
		}
	}

	static List<Title> LoadTitlesData()
	{
		List<Title> titles = new List<Title>();

		foreach (string line in File.ReadLines(@"C:\temp\title.basics.tsv").Skip(1).Take(1000))
		{
			string[] values = line.Split("\t");
			if (values.Length == 9)
			{
				titles.Add(new Title(values[0], values[1], values[2], values[3], ConvertToBool(values[4]), ConvertToInt(values[5]), ConvertToInt(values[6]), ConvertToInt(values[7]), values[8]));
			}
		}

		return titles;
	}

	static List<Name> LoadNamesData()
	{
		List<Name> names = new List<Name>();

		foreach (string line in File.ReadLines(@"C:\temp\name.basics.tsv").Skip(1).Take(1000))
		{
			string[] values = line.Split("\t");
			if (values.Length == 6)
			{
				names.Add(new Name(values[0], values[1], ConvertToInt(values[2]), ConvertToInt(values[3]), values[4], values[5]));
			}
		}
		return names;
	}

	static List<Crew> LoadCrewsData()
	{
		List<Crew> crews = new List<Crew>();

		foreach (string line in File.ReadLines(@"C:\temp\title.crew.tsv").Skip(1).Take(1000))
		{
			string[] values = line.Split("\t");
			if (values.Length == 3)
			{
				crews.Add(new Crew(values[0], values[1], values[2]));
			}
		}

		return crews;
	}

	static bool ConvertToBool(string input)
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

	static int? ConvertToInt(string input)
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

static void DeleteData(SqlConnection sqlConnection) 
{
	while (true)
	{
		Console.WriteLine("\nVælg tabel at slette data fra:");
		Console.WriteLine("1. Names");
		Console.WriteLine("2. Titles");
		Console.WriteLine("3. Titles_Genres");
		Console.WriteLine("4. Directors");
		Console.WriteLine("5. Genres");
		Console.WriteLine("6. KnownFor");
		Console.WriteLine("7. Names_Professions");
		Console.WriteLine("8. Professions");
		Console.WriteLine("9. Writers");
		Console.WriteLine("0. Tilbage");

		Console.Write("\nIndtast dit valg (0-9):");
		string? choice = Console.ReadLine();

		switch (choice)
		{
			case "0": // Tilbage
				break;

			case "1": // Names
				DeleteDataFromTable(sqlConnection, "Names");
				break;

			case "2": // Titles
				DeleteDataFromTable(sqlConnection, "Titles");
				break;
				
			case "3": // Titles_Genres
				DeleteDataFromTable(sqlConnection, "Titles_Genres");
				break;

			case "4": // Directors
				DeleteDataFromTable(sqlConnection, "Directors");
				break;

			case "5": // Genres
				DeleteDataFromTable(sqlConnection, "Genres");
				ResetIdentityColumn(sqlConnection, "Genres", "genreID");
				break;

			case "6": // KnownFor
				DeleteDataFromTable(sqlConnection, "KnownFor");
				break;

			case "7": // Names_Professions
				DeleteDataFromTable(sqlConnection, "Names_Professions");
				break;

			case "8": // Professions
				DeleteDataFromTable(sqlConnection, "Professions");
				ResetIdentityColumn(sqlConnection, "Professions", "professionID");
				break;

			case "9": // Writers
				DeleteDataFromTable(sqlConnection, "Writers");
				break;

			default:
				Console.WriteLine("\nUgyldigt valg. Prøv igen.");
				break;
		}
	}

	static void DeleteDataFromTable(SqlConnection sqlConnection, string tableName)
	{
		try
		{
			string deleteQuery = $"DELETE FROM {tableName}";
			SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConnection);
			deleteCommand.ExecuteNonQuery();

			Console.WriteLine($"\nData fra {tableName} er slettet med succes.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"\nDer opstod en fejl under sletning af data fra {tableName}: " + ex.Message);
		}
	}

	static void ResetIdentityColumn(SqlConnection sqlConnection, string tableName, string identityColumnName)
	{
		try
		{
			string resetQuery = $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
			SqlCommand resetCommand = new SqlCommand(resetQuery, sqlConnection);
			resetCommand.ExecuteNonQuery();

			Console.WriteLine($"\n{identityColumnName} kolonne nulstillet i {tableName} tabel.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"\nDer opstod en fejl under nulstilling af {identityColumnName} kolonne i {tableName}: " + ex.Message);
		}
	}

}

