using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class Crew
	{
		public string tconst { get; set; }
		public string? directors { get; set; }
		public string? writers { get; set; }

		public Crew(string tconst, string? directors, string? writers)
		{
			this.tconst = tconst;
			this.directors = directors;
			this.writers = writers;
		}
	}
}
