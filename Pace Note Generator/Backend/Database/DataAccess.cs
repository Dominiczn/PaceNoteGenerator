using Microsoft.Data.Sqlite;
using System.IO;

namespace Pace_Note_Generator.Backend.Database
{
    public static class DataAccess
    {
        private static readonly string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pacenotes.db");

        public static void InitialiseDatabase()
        {
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
	""Direction""	INTEGER NOT NULL,
	""CornerSeverity""	INTEGER NOT NULL,
	""StraightLength""	INTEGER NOT NULL,
	""IsLong""	INTEGER NOT NULL,
	""Tightens""	INTEGER NOT NULL,
	""Widens""	INTEGER NOT NULL,
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
	""Distance""	INTEGER,
	PRIMARY KEY(""StageId"")
);
";

				var createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteReader();
            }
        }

		public static void AddStage(string name, int distance)
		{
			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				var insertCommand = new SqliteCommand();
				insertCommand.Connection = db;
				insertCommand.CommandText = "INSERT INTO Stage VALUES (NULL, @Name, @Distance);";
				insertCommand.Parameters.AddWithValue("@Name", name);
				insertCommand.Parameters.AddWithValue("@Distance", distance);

				insertCommand.ExecuteReader();
			}
		}

		public static void AddPacenote(string stageName, int noteNumber, Pacenote pacenote)
		{
			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();
				var insertCommand = new SqliteCommand();
				insertCommand.Connection= db;

				insertCommand.CommandText = "INSERT INTO Pacenote VALUES (NULL, " +
					"(SELECT StageId FROM Stage Where Name = @StageName), " +
					"@NoteNumber, @Direction, @CornerSeverity, @StraightLength, @IsLong, @Tightens, @Widens)";
				insertCommand.Parameters.AddWithValue("@StageName", stageName);
                insertCommand.Parameters.AddWithValue("@NoteNumber", noteNumber);
                insertCommand.Parameters.AddWithValue("@Direction", (int)pacenote.Direction);
                insertCommand.Parameters.AddWithValue("@CornerSeverity", (int)pacenote.CornerSeverity);
                insertCommand.Parameters.AddWithValue("@StraightLength", pacenote.StraightLength);
                insertCommand.Parameters.AddWithValue("@IsLong", pacenote.IsLong);
                insertCommand.Parameters.AddWithValue("@Tightens", pacenote.Tightens);
                insertCommand.Parameters.AddWithValue("@Widens", pacenote.Widens);

				insertCommand.ExecuteReader();

				foreach(string hazard in pacenote.Hazards)
				{
					var insertCommandHazard = new SqliteCommand();
					insertCommandHazard.Connection= db;

					insertCommandHazard.CommandText = "INSERT OR IGNORE INTO Hazard VALUES (NULL, @Name); " +
						"INSERT OR IGNORE INTO PacenoteHazard VALUES " +
						"((SELECT PacenoteId FROM Pacenote WHERE NoteNumber = @NoteNumber AND StageId = (SELECT StageId FROM Stage WHERE Name = @StageName)), " +
						"(SELECT HazardId FROM Hazard WHERE Name = @Name));";

					insertCommandHazard.Parameters.AddWithValue("@Name", hazard);
                    insertCommandHazard.Parameters.AddWithValue("@NoteNumber", noteNumber);
                    insertCommandHazard.Parameters.AddWithValue("@StageName", stageName);

					insertCommandHazard.ExecuteReader();
                }

            }
		}
    }
}
