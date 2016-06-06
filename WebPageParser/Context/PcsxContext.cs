using System;
using System.Data.Entity;
using System.Linq;

namespace WebPageParser.Context {
	public class PcsxContext : DbContext {
		public const string DatabaseName = "GamesDatabase.sqlite";

		public PcsxContext()
			: base(new System.Data.SQLite.SQLiteConnection("Data Source=" + Environment.CurrentDirectory + "\\" + DatabaseName + "; New=True;"), true)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			var sqliteConnectionInitializer = new SQLite.CodeFirst.SqliteCreateDatabaseIfNotExists<PcsxContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}

		public DbSet<Models.GameDisk>			GameDisks			{ get; set; }
		public DbSet<Models.GameDiskInfo>		GameDiskInfoes		{ get; set; }
		//public DbSet<Models.GameBasicInfo>		GameBasicInfoes		{ get; set; }
		//public DbSet<Models.PcsxProperties>		PcsxProperties		{ get; set; }
		//public DbSet<Models.Plugin>				Plugins				{ get; set; }
		//public DbSet<Models.GraphicsPlugin>		GraphicsPlugins		{ get; set; }
		//public DbSet<Models.EmulationSettings>	EmulationSettings	{ get; set; }

		public void CreateTables(){
			GameDisks		.ToList();
			GameDiskInfoes	.ToList();
			//GameBasicInfoes		.ToList();
			//PcsxProperties		.ToList();
			//Plugins				.ToList();
			//GraphicsPlugins		.ToList();
			//EmulationSettings	.ToList();
		}
	}
}
