using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class NameProfession
	{
		public string nconst { get; set; }
		public int professionID { get; set; }

		public NameProfession(string nconst, int professionID)
		{
			this.nconst = nconst;
			this.professionID = professionID;
		}
	}
}
