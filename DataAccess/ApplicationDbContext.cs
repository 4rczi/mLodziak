using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Resources;
using System.Text.RegularExpressions;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<PhysicalLocation> PhysicalLocations { get; set; }
        public virtual DbSet<UserHistory> UserHistory { get; set; }
        public virtual DbSet<ApplicationLogging> ApplicationLogging { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique(true);

            modelBuilder.Entity<User>().Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<User>().Property(u => u.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);


            // Location
            modelBuilder.Entity<Location>()
               .HasKey(l => new { l.Id, l.CategoryId });

            modelBuilder.Entity<Location>()
                .Property(l => l.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Location>()
                .HasOne(l => l.Category)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.CategoryId)
                .HasPrincipalKey(c => c.Id)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.Name)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.Description);

            modelBuilder.Entity<Location>().Property(l => l.ImagePath)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.Latitude)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.Longitude)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.ZoomLevel)
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<Location>().Property(l => l.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);


            // Category
            modelBuilder.Entity<Category>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<Category>().Property(c => c.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Category>().Property(c => c.Name)
                .IsRequired(true);

            modelBuilder.Entity<Category>().Property(c => c.Description);

            modelBuilder.Entity<Category>().Property(c => c.ImagePath)
                .IsRequired(true);

            modelBuilder.Entity<Category>().Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<Category>().Property(c => c.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);


            // PhysicalLocation
            modelBuilder.Entity<PhysicalLocation>()
                .HasKey(pl => pl.Id);

            modelBuilder.Entity<PhysicalLocation>()
               .Property(pl => pl.Id)
               .ValueGeneratedNever();

            modelBuilder.Entity<PhysicalLocation>()
               .HasOne(pl => pl.Location)
               .WithMany()
               .HasForeignKey(pl => new { pl.LocationId, pl.CategoryId })
               .HasPrincipalKey(l => new { l.Id, l.CategoryId })
               .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.Name)
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.Description);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.ImagePath)
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.StartDate)
                .HasColumnType("date")
                .IsRequired(false);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.EndDate)
                .HasColumnType("date")
                .IsRequired(false);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.AlertStartEventMinutes);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.AlertEndEventMinutes);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.Latitude)
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.Longitude)
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.Radius)
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<PhysicalLocation>().Property(pl => pl.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);


            // UserHistory         
            modelBuilder.Entity<UserHistory>()
               .HasKey(uh => new { uh.PhysicalLocationId, uh.UserId });

            modelBuilder.Entity<UserHistory>()
                .HasOne(pl => pl.PhysicalLocation)
                .WithMany(u => u.UserHistory)
                .HasForeignKey(uh => uh.PhysicalLocationId)
                .HasPrincipalKey(pl => pl.Id)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            modelBuilder.Entity<UserHistory>()
                .HasOne(uh => uh.User)
                .WithMany(u => u.UserHistory)
                .HasForeignKey(uh => uh.UserId)
                .HasPrincipalKey(u => u.Id)
                .IsRequired();

            modelBuilder.Entity<UserHistory>().Property(uh => uh.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<UserHistory>().Property(uh => uh.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);


            // ApplicationLogging
            modelBuilder.Entity<ApplicationLogging>()
                .HasKey(al => al.Id);

            modelBuilder.Entity<ApplicationLogging>()
               .Property(al => al.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.Message)
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.Level)
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.Logger)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.Exception)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.ClassName)
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.MethodName)
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.UserId)
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.SessionId)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.CustomMessage)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);

            modelBuilder.Entity<ApplicationLogging>().Property(al => al.ModifiedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2")
                .IsRequired(true);



            //DataSeeding\\
            modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Parki",
                Description = Regex.Replace(@"Łódzkie parki to nie tylko miejsca odpoczynku, ale także świadkowie bogatej historii miasta.
                                              Wiele z nich powstało z myślą o mieszkańcach, oferując przestrzeń do rekreacji i bliski kontakt
                                              z naturą. Zielone przestrzenie, pełne drzew i ścieżek, zachęcają do spacerów,
                                              pikników i aktywnego wypoczynku, a ich różnorodność sprawia, że każdy znajdzie coś dla siebie.",
                                              @"\s+", " "),
                ImagePath = "Resources/Images/Categories/Park_Kategoria_01",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            });

            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Park Poniatowski",
                    Description = Regex.Replace(@"Wybudowany w latach 1904-1910 wg. projektu Teodora Chrząńskiego
                                                  pierwotnie nosił nazwę ""Ogród przy ul. Pańskiej"", a w roku 1917 otrzymał imię obecnego patrona
                                                  Park charakteryzuje się bogatym doborem gatunków roślin oraz zróżnicowanym układem przestrzennym.
                                                  Od lat 90. prowadzone są prace rewaloryzacyjne,
                                                  mające na celu przywrócenie parku do jego dawnej świetności
                                                  oraz dostosowanie go do potrzeb współczesnych użytkowników.",
                                                  @"\s+", " "),
                    ImagePath = "Resources/Images/Locations/Poniatowski_Park_01",
                    Latitude = 51.7544114,
                    Longitude = 19.4423864,
                    ZoomLevel = 0.5f,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<PhysicalLocation>().HasData(
                 new PhysicalLocation
                 {
                     Id = 1,
                     LocationId = 1,
                     CategoryId = 1,
                     Name = "Bike Park",
                     Description = Regex.Replace(@"Nowy tor, stworzony w 2008r. Przeznaczony dla rowerzystów, 
                                                   składa się z trzech sekcji z przeszkodami, w tym dużych skoków dla bardziej zaawansowanych.
                                                   W zimę teren ten idealnie nadaje się do zjeżdżania na sankach.",
                                                   @"\s+", " "),
                     ImagePath = "Resources/Images/PhysicalLocations/Bike_Park_01",
                     StartDate = null,
                     EndDate = null,
                     AlertStartEventMinutes = 0,
                     AlertEndEventMinutes = 0,
                     Latitude = 51.756849,
                     Longitude = 19.446678,
                     Radius = 75f,
                     CreatedDate = DateTime.UtcNow,
                     ModifiedDate = DateTime.UtcNow
                 },

                 new PhysicalLocation
                 {
                     Id = 2,
                     LocationId = 1,
                     CategoryId = 1,
                     Name = "Most Zakochanych",
                     Description = Regex.Replace(@"Most Zakochanych w parku Poniatowskiego stał się romantycznym miejscem,
                                                   gdzie zakochane pary przypinają kłódki z imionami, symbolizując wieczną miłość.
                                                   Zwyczaj ten, inspirowany tradycjami z innych miast Europy, polega na wrzuceniu kluczyka do wody po zawieszeniu kłódki, co ma przypieczętować związek.
                                                   Jest to jedno z najbardziej urokliwych miejsc w Łodzi, idealne na sesję zdęciową.",
                                                   @"\s+", " "),
                     ImagePath = "Resources/Images/PhysicalLocations/Most_Zakochanych_02",
                     StartDate = null,
                     EndDate = null,
                     AlertStartEventMinutes = 0,
                     AlertEndEventMinutes = 0,
                     Latitude = 51.753701,
                     Longitude = 19.439156,
                     Radius = 75f,
                     CreatedDate = DateTime.UtcNow,
                     ModifiedDate = DateTime.UtcNow
                 },

                 new PhysicalLocation
                 {
                     Id = 3,
                     LocationId = 1,
                     CategoryId = 1,
                     Name = "Fontanna",
                     Description = Regex.Replace(@"Nowa fontanna w parku Poniatowskiego, typu dry-plaza, jest ceniona przez dzieci jako
                                                   świetna okazja do schłodzenia się w gorące dni. Umożliwia tworzenie dynamicznych
                                                   kolorowych efektów wodnych i świetlnych, dzięki podświetleniu LED RGB.
                                                   Dostępna jest dla mieszkańców od wiosny 2018r. i czynna od maja do września.",
                                                   @"\s+", " "),
                     ImagePath = "Resources /Images/PhysicalLocations/Fontanna_03",
                     StartDate = null,
                     EndDate = null,
                     AlertStartEventMinutes = 0,
                     AlertEndEventMinutes = 0,
                     Latitude = 51.754574,
                     Longitude = 19.447375,
                     Radius = 50f,
                     CreatedDate = DateTime.UtcNow,
                     ModifiedDate = DateTime.UtcNow
                 }
            );
        }
    }
}
