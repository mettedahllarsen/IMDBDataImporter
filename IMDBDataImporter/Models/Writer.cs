using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class Writer
	{
		public string tconst { get; set; }
		public string nconst { get; set; }

		public Writer(string tconst, string nconst)
		{
			this.tconst = tconst;
			this.nconst = nconst;
		}
	}
}
