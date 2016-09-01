using System.Collections.Generic;
using System.Data.Entity;

using TestApp;

namespace DataAccessEF
{

  
    public class ApplicationContext : DbContext
    {
        public DbSet<Earthquake> Earthquakes { get; set; }
        public DbSet<MagnitudeType> MagtitudesTypes { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<MyLogs> MyLogs { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<RegistrationDevice> RegistrationDevices { get; set; }
        public DbSet<DeviceLog> DevicesLogs { get; set; }

        public ApplicationContext()
            : base("DefaultConnection")
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //------------------------------Place---------------------------------------------------------------
            modelBuilder.Entity<Place>().ToTable("Place");
            modelBuilder.Entity<Place>().HasKey(p => p.Id);
            modelBuilder.Entity<Place>().Property(p => p.PlaceName);           
            modelBuilder.Entity<Place>().Property(p => p.Country);

            
            //------------------------------Source---------------------------------------------------------------
            modelBuilder.Entity<Source>().ToTable("Source");
            modelBuilder.Entity<Source>().HasKey(s => s.Id);
            modelBuilder.Entity<Source>().Property(s => s.SourceName);
            modelBuilder.Entity<Source>().Property(s => s.SourceDirection);
            modelBuilder.Entity<Source>().Property(s => s.IsOnline);
                  
            

            //------------------------------MagnitudeTye----------------------------------------------------
            modelBuilder.Entity<MagnitudeType>().ToTable("MagnitudeType");
            modelBuilder.Entity<MagnitudeType>().HasKey(m => m.Id);
            modelBuilder.Entity<MagnitudeType>().Property(m => m.Type);
           


            //------------------------------Earthquake----------------------------------------------------
            modelBuilder.Entity<Earthquake>().ToTable("Earthquake");
            modelBuilder.Entity<Earthquake>().HasKey(e => e.Id);
            modelBuilder.Entity<Earthquake>().Property(e => e.UTCDateTime);
            modelBuilder.Entity<Earthquake>().Property(e => e.Depth).HasPrecision(15, 10);
            modelBuilder.Entity<Earthquake>().Property(e => e.Latitude).HasPrecision(15, 10);
            modelBuilder.Entity<Earthquake>().Property(e => e.Longitude).HasPrecision(15, 10);
            modelBuilder.Entity<Earthquake>().Property(e => e.Magnitude).HasPrecision(15, 10);
            modelBuilder.Entity<Earthquake>().Property(e => e.PublicatedDatTime);
            modelBuilder.Entity<Earthquake>().HasRequired(e => e.MagnitudeType);
            modelBuilder.Entity<Earthquake>().HasRequired(e => e.Place);
            modelBuilder.Entity<Earthquake>().HasRequired(e => e.Source);
            modelBuilder.Entity<Earthquake>().Property(e => e.IsSensible);
            modelBuilder.Entity<Earthquake>().Property(e => e.WasNotified);


            //--------------------------------Tweet------------------------------------------------------------
            modelBuilder.Entity<Tweet>().ToTable("Tweet");
            modelBuilder.Entity<Tweet>().HasKey(t => t.Id);
            modelBuilder.Entity<Tweet>().Property(t => t.DateTime);
            modelBuilder.Entity<Tweet>().Property(t => t.Tweeter); 
            modelBuilder.Entity<Tweet>().Property(t => t.UserName);
            modelBuilder.Entity<Tweet>().Property(t => t.UserScreenName);

            //---------------------------------MyLogs----------------------------------------------------------
            modelBuilder.Entity<MyLogs>().ToTable("MyLogs");
            modelBuilder.Entity<MyLogs>().HasKey(ml=>ml.Id);
            modelBuilder.Entity<MyLogs>().Property(ml => ml.Date);
            modelBuilder.Entity<MyLogs>().Property(ml => ml.Level);
            modelBuilder.Entity<MyLogs>().Property(ml => ml.Message);



            //----------------------------------RegistrationDevice------------------------------------
            modelBuilder.Entity<RegistrationDevice>().ToTable("RegistrationDevice");
            modelBuilder.Entity<RegistrationDevice>().HasKey(rd => rd.Id);
            modelBuilder.Entity<RegistrationDevice>().Property(rd => rd.Date);
            modelBuilder.Entity<RegistrationDevice>().Property(rd => rd.DeviceId);
            modelBuilder.Entity<RegistrationDevice>().Property(rd => rd.RegistrationId);

            //-------------------------------------DeviceLog-------------------------------------------------------
            modelBuilder.Entity<DeviceLog>().ToTable("DeviceLog");
            modelBuilder.Entity<DeviceLog>().HasKey(dl => dl.Id);
            modelBuilder.Entity<DeviceLog>().Property(dl => dl.Date);
            modelBuilder.Entity<DeviceLog>().Property(dl => dl.DeviceId);
            modelBuilder.Entity<DeviceLog>().Property(dl => dl.Log);


            base.OnModelCreating(modelBuilder);
        }
        
    }
}