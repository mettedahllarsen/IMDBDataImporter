using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.Models
{
    public class Name
    {
        public string nconst { get; set; }
        public string primaryName { get; set; }
        public int? birthYear { get; set; }
        public int? deathYear { get; set; }
        public List<string> primaryProfession { get; set; }
        public List<string> knownForTitles { get; set; }


        public Name(string nconst, string primaryName, int? birthYear, int? deathYear, string primaryProfessionString, string knownForTitlesString)
        {
            this.nconst = nconst;
            this.primaryName = primaryName;
            this.birthYear = birthYear;
            this.deathYear = deathYear;

			primaryProfession = primaryProfessionString.Split(",").ToList();
            knownForTitles = knownForTitlesString.Split(",").ToList();
		}
	}
}
