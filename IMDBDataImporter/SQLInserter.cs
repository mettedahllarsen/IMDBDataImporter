using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using IMDBDataImporter.Models;

namespace IMDBDataImporter
{
	public class SQLInserter
	{
		public void InsertDataIntoGenres(SqlConnection sqlConn, List<Title> titles)
		{
			HashSet<string> genres = new HashSet<string>();
			Dictionary<string, int> genreDictionary = new Dictionary<string, int>();

			foreach (var title in titles)
			{
				foreach (var genre in title.genres) 
				{
					genres.Add(genre);
				}
			}

            foreach (string genreType in genres) 
			{
				SqlCommand sqlComm = new SqlCommand(
					"INSERT INTO Genres(genreType)" +
					" OUTPUT INSERTED.genreID " +
					" VALUES ('" + genreType + "')", sqlConn);

				try
				{
					SqlDataReader reader = sqlComm.ExecuteReader();
					if (reader.Read())
					{
						int newGenreId = (int)reader["genreID"];
						genreDictionary.Add(genreType, newGenreId);
					}
					reader.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine("An error occurred: " + ex.Message);
					continue;
				}
			}
		}

		public void InsertDataIntoTitlesGenres(SqlConnection sqlConn, List<Title> titles)
		{
			Dictionary<string, int> genreDictionary = LoadGenreDictionary(sqlConn);

			foreach (var title in titles)
			{
				foreach (var genre in title.genres)
				{
					if (genreDictionary.ContainsKey(genre))
					{
						SqlCommand sqlComm = new SqlCommand(
							"INSERT INTO Titles_Genres (tconst, genreID) " +
							"VALUES ('" + title.tconst + "', '" + genreDictionary[genre] + "')"
							, sqlConn);

						try
						{
							sqlComm.ExecuteNonQuery();
						}

						catch (Exception ex)
						{
							Console.WriteLine("An error occurred: " + ex.Message);
							continue;
						}
					}
					else
					{
						Console.WriteLine($"Genre '{genre}' not found in the database.");
					}
				}
			}
		}

		public void InsertDataIntoDirectors(SqlConnection sqlConn, List<Crew> crews)
		{
			Console.WriteLine($"Total number of crews: {crews.Count}");

			foreach (var crew in crews)
			{
				// Tjek om tconst findes i Titles-tabellen
				SqlCommand titleCheckComm = new SqlCommand(
					"SELECT COUNT(*) FROM Titles WHERE tconst = '" + crew.tconst + "'", sqlConn);
				int titleCount = (int)titleCheckComm.ExecuteScalar();

				// Tjek om nconst (directors) findes i Names-tabellen
				SqlCommand nameCheckComm = new SqlCommand(
					"SELECT COUNT(*) FROM Names WHERE nconst = '" + crew.directors + "'", sqlConn);
				int nameCount = (int)nameCheckComm.ExecuteScalar();

				// Hvis både tconst og nconst findes, udfør indsættelse
				if (titleCount > 0 && nameCount > 0)
				{
					SqlCommand sqlComm = new SqlCommand(
						"INSERT INTO Directors (tconst, nconst) " +
						"VALUES ('" + crew.tconst + "', '" + crew.directors + "')", sqlConn);

					Console.WriteLine(sqlComm.CommandText);


					sqlComm.ExecuteNonQuery();
				}

				else
				{
					Console.WriteLine($"tconst and/or nconst was not found in the database.");
				}
			}
		}

		public void InsertDataIntoWriters(SqlConnection sqlConn, List<Crew> crews) 
		{
			foreach (var crew in crews)
			{

				// Tjek om tconst findes i Titles-tabellen
				Console.WriteLine("Tjekker om tconst findes i Titles-tabellen");
				SqlCommand titleCheckComm = new SqlCommand(
					"SELECT COUNT(*) FROM Titles WHERE tconst = '" + crew.tconst + "'", sqlConn);
				int titleCount = (int)titleCheckComm.ExecuteScalar();

				// Tjek om nconst (writers) findes i Names-tabellen
				Console.WriteLine("Tjekker om nconst (writers) findes i Names-tabellen");
				SqlCommand nameCheckComm = new SqlCommand(
					"SELECT COUNT(*) FROM Names WHERE nconst = '" + crew.writers + "'", sqlConn);
				int nameCount = (int)nameCheckComm.ExecuteScalar();

				// Hvis både tconst og nconst findes, udfør indsættelse
				Console.WriteLine("Udfører indsættelse");
				if (titleCount > 0 && nameCount > 0)
				{
					SqlCommand sqlComm = new SqlCommand(
						"INSERT INTO Writers (tconst, nconst) " +
						"VALUES ('" + crew.tconst + "', '" + crew.writers + "')", sqlConn);

					Console.WriteLine(sqlComm.CommandText);

					sqlComm.ExecuteNonQuery();
				}
			}
		}

		public void InsertDataIntoKnownFor(SqlConnection sqlConn, List<Name> names)
		{
			foreach (var name in names)
			{
				foreach (var tconst in name.knownForTitles)
				{
					// Tjek om nconst (name) findes i Names-tabellen
					SqlCommand nameCheckComm = new SqlCommand(
						"SELECT COUNT(*) FROM Names WHERE nconst = '" + name.nconst + "'", sqlConn);
					int nameCount = (int)nameCheckComm.ExecuteScalar();

					// Tjek om tconst findes i Titles-tabellen
					SqlCommand titleCheckComm = new SqlCommand(
						"SELECT COUNT(*) FROM Titles WHERE tconst = '" + tconst + "'", sqlConn);
					int titleCount = (int)titleCheckComm.ExecuteScalar();

					// Hvis både nconst og tconst findes, udfør indsættelse
					if (nameCount > 0 && titleCount > 0)
					{
						SqlCommand sqlComm = new SqlCommand(
							"INSERT INTO KnownFor (tconst, nconst) " +
							"VALUES ('" + tconst + "', '" + name.nconst + "')", sqlConn);

						Console.WriteLine(sqlComm.CommandText);

						sqlComm.ExecuteNonQuery();
					}
					else
					{
						Console.WriteLine($"nconst '{name.nconst}' and/or tconst '{tconst}' was not found in the database.");
					}
				}
			}
		}

		public void InsertDataIntoProfessions(SqlConnection sqlConn, List<Name> names)
		{
			HashSet<string> professions = new HashSet<string>();
			Dictionary<string, int> professionDictionary = new Dictionary<string, int>();

			foreach (var name in names)
			{
				foreach (var profession in name.primaryProfession)
				{
					professions.Add(profession);
				}
			}

			foreach (string professionName in professions)
			{
				SqlCommand sqlComm = new SqlCommand(
					"INSERT INTO Professions(professionName)" +
					" OUTPUT INSERTED.professionID " +
					" VALUES ('" + professionName + "')", sqlConn);

				try
				{
					SqlDataReader reader = sqlComm.ExecuteReader();
					if (reader.Read())
					{
						int newProfessionId = (int)reader["professionID"];
						professionDictionary.Add(professionName, newProfessionId);
					}
					reader.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine("An error occurred: " + ex.Message);
					continue;
				}
			}
		}


		public void InsertDataIntoNamesProfessions(SqlConnection sqlConn, List<Name> names)
		{
			foreach (var name in names)
			{
				// Tjek om nconst findes i Names-tabellen
				SqlCommand nameCheckComm = new SqlCommand(
					"SELECT COUNT(*) FROM Names WHERE nconst = '" + name.nconst + "'", sqlConn);
				int nameCount = (int)nameCheckComm.ExecuteScalar();

				// Hvis nconst findes, udfør indsættelse
				if (nameCount > 0)
				{
					foreach (var profession in name.primaryProfession)
					{
						// Tjek om professionen allerede findes i Professions-tabellen
						SqlCommand professionCheckComm = new SqlCommand(
							"SELECT COUNT(*) FROM Professions WHERE professionName = '" + profession + "'", sqlConn);
						int professionCount = (int)professionCheckComm.ExecuteScalar();

						// Hvis professionen ikke findes, tilføj den
						if (professionCount == 0)
						{
							SqlCommand professionInsertComm = new SqlCommand(
								"INSERT INTO Professions (professionName) " +
								"OUTPUT INSERTED.professionID " +
								"VALUES ('" + profession + "')", sqlConn);

							int newProfessionId = (int)professionInsertComm.ExecuteScalar();
							// Gem professionId i en dictionary eller lignende, hvis det skal bruges senere
						}

						// Find professionId for den givne profession
						SqlCommand getProfessionIdComm = new SqlCommand(
							"SELECT professionID FROM Professions WHERE professionName = '" + profession + "'", sqlConn);
						int professionId = (int)getProfessionIdComm.ExecuteScalar();

						// Tjek om der allerede er en relation mellem nconst og professionId
						SqlCommand relationCheckComm = new SqlCommand(
							"SELECT COUNT(*) FROM Names_Professions WHERE nconst = '" + name.nconst + "' AND professionID = " + professionId, sqlConn);
						int relationCount = (int)relationCheckComm.ExecuteScalar();

						// Hvis relationen ikke findes, tilføj den
						if (relationCount == 0)
						{
							SqlCommand relationInsertComm = new SqlCommand(
								"INSERT INTO Names_Professions (nconst, professionID) " +
								"VALUES ('" + name.nconst + "', " + professionId + ")", sqlConn);
							relationInsertComm.ExecuteNonQuery();
						}
					}
				}
				else
				{
					Console.WriteLine($"nconst '{name.nconst}' was not found in the database.");
				}
			}
		}


		public Dictionary<string, int> LoadGenreDictionary(SqlConnection sqlConn)
		{
			Dictionary<string, int> genreDictionary = new Dictionary<string, int>();

			SqlCommand sqlComm = new SqlCommand(
				"SELECT genreType, genreID FROM Genres", sqlConn);

			SqlDataReader reader = sqlComm.ExecuteReader();

			while (reader.Read())
			{
				string genreType = reader["genreType"].ToString();
				int genreID = (int)reader["genreID"];
				genreDictionary.Add(genreType, genreID);
			}

			reader.Close();
			return genreDictionary;
		}
	}
}
