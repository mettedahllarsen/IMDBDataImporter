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
