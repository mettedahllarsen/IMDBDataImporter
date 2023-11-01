using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class Genre
	{
		public int genreID { get; set; }
		public string genreType { get; set; }

		public Genre(int genreID, string genreType)
		{
			this.genreID = genreID;
			this.genreType = genreType;
		}
	}
}
