using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class Director
	{
		public string tconst { get; set; }
		public string nconst { get; set; }

		public Director(string tconst, string nconst)
		{
			this.tconst = tconst;
			this.nconst = nconst;
		}
	}
}
