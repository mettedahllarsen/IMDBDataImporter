using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class TitleGenre
	{
		public string tconst { get; set; }
		public int genreID { get; set; }

		public TitleGenre(string tconst, int genreID)
		{
			this.tconst = tconst;
			this.genreID = genreID;
		}
	}

}
