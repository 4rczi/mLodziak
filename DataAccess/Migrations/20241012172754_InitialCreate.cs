using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationLogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MethodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLogging", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ZoomLevel = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => new { x.Id, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_Locations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    AlertStartEventMinutes = table.Column<int>(type: "int", nullable: false),
                    AlertEndEventMinutes = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Radius = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalLocations_Locations_LocationId_CategoryId",
                        columns: x => new { x.LocationId, x.CategoryId },
                        principalTable: "Locations",
                        principalColumns: new[] { "Id", "CategoryId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    PhysicalLocationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => new { x.PhysicalLocationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserHistory_PhysicalLocations_PhysicalLocationId",
                        column: x => x.PhysicalLocationId,
                        principalTable: "PhysicalLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Description", "ImagePath", "ModifiedDate", "Name" },
                values: new object[] { 1, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(5681), "Łódzkie parki to nie tylko miejsca odpoczynku, ale także świadkowie bogatej historii miasta. Wiele z nich powstało z myślą o mieszkańcach, oferując przestrzeń do rekreacji i bliski kontakt z naturą. Zielone przestrzenie, pełne drzew i ścieżek, zachęcają do spacerów, pikników i aktywnego wypoczynku, a ich różnorodność sprawia, że każdy znajdzie coś dla siebie.", "Resources/Images/Categories/Park_Kategoria_01", new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(5684), "Parki" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "CategoryId", "Id", "CreatedDate", "Description", "ImagePath", "Latitude", "Longitude", "ModifiedDate", "Name", "ZoomLevel" },
                values: new object[] { 1, 1, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(5933), "Wybudowany w latach 1904-1910 wg. projektu Teodora Chrząńskiego pierwotnie nosił nazwę \"Ogród przy ul. Pańskiej\", a w roku 1917 otrzymał imię obecnego patrona Park charakteryzuje się bogatym doborem gatunków roślin oraz zróżnicowanym układem przestrzennym. Od lat 90. prowadzone są prace rewaloryzacyjne, mające na celu przywrócenie parku do jego dawnej świetności oraz dostosowanie go do potrzeb współczesnych użytkowników.", "Resources/Images/Locations/Poniatowski_Park_01", 51.754411400000002, 19.4423864, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(5934), "Park Poniatowski", 16.75f });

            migrationBuilder.InsertData(
                table: "PhysicalLocations",
                columns: new[] { "Id", "AlertEndEventMinutes", "AlertStartEventMinutes", "CategoryId", "CreatedDate", "Description", "EndDate", "ImagePath", "Latitude", "LocationId", "Longitude", "ModifiedDate", "Name", "Radius", "StartDate" },
                values: new object[,]
                {
                    { 1, 0, 0, 1, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6014), "Nowy tor, stworzony w 2008r. Przeznaczony dla rowerzystów, składa się z trzech sekcji z przeszkodami, w tym dużych skoków dla bardziej zaawansowanych. W zimę teren ten idealnie nadaje się do zjeżdżania na sankach.", null, "Resources/Images/PhysicalLocations/Bike_Park_01", 51.756849000000003, 1, 19.446677999999999, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6014), "Bike Park", 75f, null },
                    { 2, 0, 0, 1, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6100), "Most Zakochanych w parku Poniatowskiego stał się romantycznym miejscem, gdzie zakochane pary przypinają kłódki z imionami, symbolizując wieczną miłość. Zwyczaj ten, inspirowany tradycjami z innych miast Europy, polega na wrzuceniu kluczyka do wody po zawieszeniu kłódki, co ma przypieczętować związek. Jest to jedno z najbardziej urokliwych miejsc w Łodzi, idealne na sesję zdęciową.", null, "Resources/Images/PhysicalLocations/Most_Zakochanych_02", 51.753701, 1, 19.439156000000001, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6100), "Most Zakochanych", 75f, null },
                    { 3, 0, 0, 1, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6175), "Nowa fontanna w parku Poniatowskiego, typu dry-plaza, jest ceniona przez dzieci jako świetna okazja do schłodzenia się w gorące dni. Umożliwia tworzenie dynamicznych kolorowych efektów wodnych i świetlnych, dzięki podświetleniu LED RGB. Dostępna jest dla mieszkańców od wiosny 2018r. i czynna od maja do września.", null, "Resources /Images/PhysicalLocations/Fontanna_03", 51.754573999999998, 1, 19.447375000000001, new DateTime(2024, 10, 12, 17, 27, 54, 555, DateTimeKind.Utc).AddTicks(6176), "Most Zakochanych", 50f, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CategoryId",
                table: "Locations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalLocations_LocationId_CategoryId",
                table: "PhysicalLocations",
                columns: new[] { "LocationId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_UserId",
                table: "UserHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationLogging");

            migrationBuilder.DropTable(
                name: "UserHistory");

            migrationBuilder.DropTable(
                name: "PhysicalLocations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
