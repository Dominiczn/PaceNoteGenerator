using Microsoft.Data.Sqlite;
using System.IO;

namespace Pace_Note_Generator.Backend.Database
{
    public static class DataAccess
    {
        private static readonly string dbpathFilename = "pacenotes.db";

        public static void InitialiseDatabase()
        {
            string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbpathFilename);

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string tableCommand = @"
CREATE TABLE IF NOT EXISTS ""Hazard"" (
	""HazardId""	INTEGER,
	""Name""	TEXT UNIQUE,
	PRIMARY KEY(""HazardId"")
);

CREATE TABLE IF NOT EXISTS ""Pacenote"" (
	""PacenoteId""	INTEGER,
	""StageId""	INTEGER NOT NULL,
	""NoteNumber""	INTEGER NOT NULL,
	""Type""	INTEGER NOT NULL,
	""Direction""	INTEGER,
	""CornerSeverity""	INTEGER,
	""StraightLength""	INTEGER,
	""IsLong""	INTEGER NOT NULL DEFAULT 0,
	""Tightens""	INTEGER NOT NULL DEFAULT 0,
	""Widens""	INTEGER NOT NULL DEFAULT 0,
	PRIMARY KEY(""PacenoteId""),
	UNIQUE(""StageId"",""NoteNumber""),
	FOREIGN KEY(""StageId"") REFERENCES ""Stage""(""StageId"")
);

CREATE TABLE IF NOT EXISTS ""PacenoteHazard"" (
	""PacenoteId""	INTEGER NOT NULL,
	""HazardId""	INTEGER NOT NULL,
	PRIMARY KEY(""PacenoteId"",""HazardId""),
	FOREIGN KEY(""HazardId"") REFERENCES ""Hazard""(""HazardId""),
	FOREIGN KEY(""PacenoteId"") REFERENCES ""Pacenote""(""PacenoteId"")
);

CREATE TABLE IF NOT EXISTS ""Stage"" (
	""StageId""	INTEGER,
	""Name""	TEXT,
	""Distance""	REAL,
	PRIMARY KEY(""StageId"")
);
";

				var createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteReader();
            }
        }
    }


}
