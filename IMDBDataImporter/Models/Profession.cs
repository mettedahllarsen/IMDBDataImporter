using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
	public class Profession
	{
		public int professionID { get; set; }
		public string professionName { get; set; }

		public Profession(int professionID, string professionName)
		{
			this.professionID = professionID;
			this.professionName = professionName;
		}
	}
}
