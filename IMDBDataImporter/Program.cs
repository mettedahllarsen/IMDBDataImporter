using IMDBDataImporter;
using IMDBDataImporter.Models;
using System.Collections.Immutable;
using System.Data.SqlClient;

// This C# program connects to a database using a SqlConnection object.
// It establishes a connection to the "MovieDB" database on the local server
// with the provided user ID and password. The TrustServerCertificate option
// indicates that the server's SSL certificate will be trusted. The program
// then proceeds to interact with the database based on user input.

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
			DeleteData(sqlConn);
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
		Console.WriteLine("3. TitlesGenres");
		Console.WriteLine("4. Directors");
		Console.WriteLine("5. Genres");
		Console.WriteLine("6. KnownFor");
		Console.WriteLine("7. NamesProfessions");
		Console.WriteLine("8. Professions");
		Console.WriteLine("9. Writers");
		Console.WriteLine("0. Tilbage");

		string choice = Console.ReadLine();

		BulkInserter bulkInserter = new BulkInserter();
		SQLInserter sqlInserter = new SQLInserter();

		switch (choice)
		{
			case "0": // Tilbage
				break; 

			case "1": // Names
				bulkInserter.InsertDataIntoNames(sqlConn, LoadNamesData());
				break;

			case "2": // Titles
				bulkInserter.InsertDataIntoTitles(sqlConn, LoadTitlesData());
				break;

			case "3": // TitlesGenres
				sqlInserter.InsertDataIntoTitlesGenres(sqlConn, LoadTitlesData());
				break;

			case "4": // Directors

				break;

			case "5": // Genres
				sqlInserter.InsertDataIntoGenres(sqlConn, LoadTitlesData());
				break;

			case "6": // KnownFor
				break;

			case "7": // NamesProfessions
				break;

			case "8": // Professions
				break;

			case "9": // Writers
				break;

			default:
				Console.WriteLine("Ugyldigt valg. Prøv igen.");
				break;
		}
	}
}

static void DeleteData(SqlConnection sqlConn) 
{
	while (true)
	{
		Console.WriteLine("\nVælg tabel at slette data fra:");
		Console.WriteLine("1. Names");
		Console.WriteLine("2. Titles");
		Console.WriteLine("3. TitlesGenres");
		Console.WriteLine("4. Directors");
		Console.WriteLine("5. Genres");
		Console.WriteLine("6. KnownFor");
		Console.WriteLine("7. NamesProfessions");
		Console.WriteLine("8. Professions");
		Console.WriteLine("9. Writers");
		Console.WriteLine("0. Tilbage");

		string choice = Console.ReadLine();

		switch (choice)
		{
			case "0": // Tilbage
				break;

			case "1": // Names
				DeleteDataFromTable(sqlConn, "Names");
				break;

			case "2": // Titles
				DeleteDataFromTable(sqlConn, "Titles");
				break;
				
			case "3": // TitlesGenres
				DeleteDataFromTable(sqlConn, "TitlesGenres");
				break;

			case "4": // Directors
				DeleteDataFromTable(sqlConn, "Directors");
				break;

			case "5": // Genres
				DeleteDataFromTable(sqlConn, "Genres");
				ResetIdentityColumn(sqlConn, "Genres", "genreId");
				break;

			case "6": // KnownFor
				DeleteDataFromTable(sqlConn, "KnownFor");
				break;

			case "7": // NamesProfessions
				DeleteDataFromTable(sqlConn, "NamesProfessions");
				break;

			case "8": // Professions
				DeleteDataFromTable(sqlConn, "Professions");
				ResetIdentityColumn(sqlConn, "Professions", "professionId");
				break;

			case "9": // Writers
				DeleteDataFromTable(sqlConn, "Writers");
				break;

			default:
				Console.WriteLine("Ugyldigt valg. Prøv igen.");
				break;
		}
	}
}

static void DeleteDataFromTable(SqlConnection sqlConn, string tableName)
{
	try
	{
		string deleteQuery = $"DELETE FROM {tableName}";
		SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConn);
		deleteCommand.ExecuteNonQuery();

		Console.WriteLine($"Data fra {tableName} er slettet med succes.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Der opstod en fejl under sletning af data fra {tableName}: " + ex.Message);
	}
}

static void ResetIdentityColumn(SqlConnection sqlConn, string tableName, string identityColumnName)
{
	try
	{
		string resetQuery = $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
		SqlCommand resetCommand = new SqlCommand(resetQuery, sqlConn);
		resetCommand.ExecuteNonQuery();

		Console.WriteLine($"{identityColumnName} kolonne nulstillet i {tableName} tabel.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Der opstod en fejl under nulstilling af {identityColumnName} kolonne i {tableName}: " + ex.Message);
	}
}

static List<Title> LoadTitlesData()
{
	List<Title> titles = new List<Title>();

	foreach (string line in File.ReadLines(@"C:\temp\title.basics.tsv").Skip(1).Take(100))
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

	foreach (string line in File.ReadLines(@"C:\temp\name.basics.tsv").Skip(1).Take(100))
	{
		string[] values = line.Split("\t");
		if (values.Length == 6)
		{
			names.Add(new Name(values[0], values[1], ConvertToInt(values[2]), ConvertToInt(values[3])));
		}
	}
	return names;
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
