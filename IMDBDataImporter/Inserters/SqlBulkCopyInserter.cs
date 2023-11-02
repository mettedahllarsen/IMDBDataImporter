using IMDBDataImporter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter.DataInserters
{
    public class SqlBulkCopyInserter
    {
        public void InsertDataIntoTitles(SqlConnection sqlConn, List<Title> titles)
        {
            DataTable titleTable = new DataTable("Titles");

            titleTable.Columns.Add("tconst", typeof(string));
            titleTable.Columns.Add("titleType", typeof(string));
            titleTable.Columns.Add("primaryTitle", typeof(string));
            titleTable.Columns.Add("originalTitle", typeof(string));
            titleTable.Columns.Add("isAdult", typeof(bool));
            titleTable.Columns.Add("startYear", typeof(int));
            titleTable.Columns.Add("endYear", typeof(int));
            titleTable.Columns.Add("runtimeMinutes", typeof(int));

            foreach (Title title in titles)
            {
                DataRow titleRow = titleTable.NewRow();
                FillParameter(titleRow, "tconst", title.tconst);
                FillParameter(titleRow, "titleType", title.titleType);
                FillParameter(titleRow, "primaryTitle", title.primaryTitle);
                FillParameter(titleRow, "originalTitle", title.originalTitle);
                FillParameter(titleRow, "isAdult", title.isAdult);
                FillParameter(titleRow, "startYear", title.startYear);
                FillParameter(titleRow, "endYear", title.endYear);
                FillParameter(titleRow, "runtimeMinutes", title.runtimeMinutes);
                titleTable.Rows.Add(titleRow);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.DestinationTableName = "Titles";
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.WriteToServer(titleTable);
        }

        public void InsertDataIntoNames(SqlConnection sqlConn, List<Name> names)
        {
            DataTable nameTable = new DataTable("Names");

            nameTable.Columns.Add("nconst", typeof(string));
            nameTable.Columns.Add("primaryName", typeof(string));
            nameTable.Columns.Add("birthYear", typeof(int));
            nameTable.Columns.Add("deathYear", typeof(int));

            foreach (Name name in names)
            {
                DataRow nameRow = nameTable.NewRow();
                FillParameter(nameRow, "nconst", name.nconst);
                FillParameter(nameRow, "primaryName", name.primaryName);
                FillParameter(nameRow, "birthYear", name.birthYear);
                FillParameter(nameRow, "deathYear", name.deathYear);
                nameTable.Rows.Add(nameRow);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.DestinationTableName = "Names";
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.WriteToServer(nameTable);
        }

        public void FillParameter(DataRow row, string columnName, object? value)
        {
            if (value != null)
            {
                row[columnName] = value;
            }
            else
            {
                row[columnName] = DBNull.Value;
            }
        }
    }
}
