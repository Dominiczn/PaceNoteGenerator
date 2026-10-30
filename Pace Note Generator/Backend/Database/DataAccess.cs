using Microsoft.Data.Sqlite;
using Pace_Note_Generator.Backend.Models;
using System.Data;
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

INSERT OR IGNORE INTO ""Hazard"" VALUES (NULL, 'Blind'), (NULL, 'Crest'), (NULL, 'Jump'), (NULL, 'Narrows'), (NULL, 'Junction'), (NULL, 'Off Camber'), (NULL, 'Gravel'), (NULL, 'Tarmac'), (NULL, 'Gate'), (NULL, 'Cut'), (NULL, 'Dont Cut'), (NULL, 'Unconfirmed');

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
	""Name""	TEXT UNIQUE,
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

					insertCommandHazard.CommandText = "INSERT OR IGNORE INTO PacenoteHazard VALUES " +
                        "((SELECT PacenoteId FROM Pacenote WHERE NoteNumber = @NoteNumber AND StageId = (SELECT StageId FROM Stage WHERE Name = @StageName)), " +
						"(SELECT HazardId FROM Hazard WHERE Name = @Name));";

					insertCommandHazard.Parameters.AddWithValue("@Name", hazard);
                    insertCommandHazard.Parameters.AddWithValue("@NoteNumber", noteNumber);
                    insertCommandHazard.Parameters.AddWithValue("@StageName", stageName);

					insertCommandHazard.ExecuteReader();
                }

            }
		}

		public static List<Stage> GetStages()
		{
			var stages = new List<Stage>();

			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();
				var selectCommand = new SqliteCommand("SELECT StageId, Name, Distance FROM Stage ORDER BY StageId", db);

				SqliteDataReader query = selectCommand.ExecuteReader();

				while (query.Read())
				{
					stages.Add(new Stage
					{
						StageId = query.GetInt32(0),
						Name = query.GetString(1),
						Distance = query.GetInt32(2),
					});
				}

				return stages;
			}
		}

		public static List<string> GetHazards()
		{
			var hazards = new List<string>();

			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();
				var selectCommand = new SqliteCommand("SELECT Name FROM Hazard ORDER BY Name", db);

				SqliteDataReader query = selectCommand.ExecuteReader();

				while (query.Read())
				{
					hazards.Add(query.GetString(0));
				}
			}

			return hazards;
		}

		public static List<Pacenote> GetPacenotes(int stageId)
		{
			List<Pacenote> pacenotes = new List<Pacenote>();

			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();
				var selectCommand = new SqliteCommand("SELECT PacenoteId, Direction, CornerSeverity, StraightLength, IsLong, Tightens, Widens " +
					"FROM Pacenote " +
					"WHERE StageId = @StageId " +
					"ORDER BY NoteNumber", db);
				selectCommand.Parameters.AddWithValue("@StageId", stageId);

				SqliteDataReader query = selectCommand.ExecuteReader();

				while(query.Read())
				{
					Pacenote pacenote = new Pacenote
					{
						Direction = (Direction)query.GetInt32(1),
						CornerSeverity = (CornerSeverity)query.GetInt32(2),
						StraightLength = query.GetInt32(3),
						IsLong = query.GetBoolean(4),
                        Tightens = query.GetBoolean(5),
						Widens = query.GetBoolean(6),
                    };

                    var selectCommandHazard = new SqliteCommand("SELECT Hazard.Name " +
                    "FROM PacenoteHazard " +
                    "JOIN Hazard ON Hazard.HazardId = PacenoteHazard.HazardId " +
                    "WHERE PacenoteHazard.PacenoteId = @PacenoteId", db);
                    selectCommandHazard.Parameters.AddWithValue("@PacenoteId", query.GetInt32(0));

                    SqliteDataReader hazardQuery = selectCommandHazard.ExecuteReader();

                    while (hazardQuery.Read())
                    {
						pacenote.Hazards.Add(hazardQuery.GetString(0));
                    }
					
					pacenotes.Add(pacenote);
                }

                return pacenotes;
            }
		}

		public static void DeleteStage(int stageId)
		{
			using (var db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				var deleteCommand = new SqliteCommand();
				deleteCommand.Connection = db;

				deleteCommand.CommandText = "DELETE FROM PacenoteHazard WHERE PacenoteId IN (SELECT PacenoteId FROM Pacenote WHERE StageId = @StageId);" +
					"DELETE FROM Pacenote WHERE StageId = @StageId;" +
					"DELETE FROM Stage WHERE StageId = @StageId;";

				deleteCommand.Parameters.AddWithValue("@StageId", stageId);

				deleteCommand.ExecuteReader();
			}
		}
    }
}
