using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoApi_TypeGen_NSwag_Starter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNo = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Active", "Name", "StudentNo" },
                values: new object[,]
                {
                    { 1, true, "Alice Johnson", "S0001001" },
                    { 2, true, "Bob Smith", "S0001002" },
                    { 3, false, "Charlie Brown", "S0001003" },
                    { 4, true, "Diana Prince", "S0001004" },
                    { 5, true, "Edward Norton", "S0001005" },
                    { 6, false, "Fiona Davis", "S0001006" },
                    { 7, true, "George Wilson", "S0001007" },
                    { 8, true, "Hannah Moore", "S0001008" },
                    { 9, true, "Ivan Taylor", "S0001009" },
                    { 10, false, "Julia Anderson", "S0001010" }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "City", "Country", "PostalCode", "Province", "Street", "StudentId" },
                values: new object[,]
                {
                    { 1, "New York", "USA", "10001", "NY", "123 Main St", 1 },
                    { 2, "Boston", "USA", "02101", "MA", "456 Oak Ave", 1 },
                    { 3, "Chicago", "USA", "60601", "IL", "789 Pine Rd", 2 },
                    { 4, "Seattle", "USA", "98101", "WA", "321 Elm Dr", 2 },
                    { 5, "Austin", "USA", "73301", "TX", "654 Maple Ln", 3 },
                    { 6, "Denver", "USA", "80201", "CO", "987 Cedar Blvd", 3 },
                    { 7, "Portland", "USA", "97201", "OR", "147 Birch St", 4 },
                    { 8, "Miami", "USA", "33101", "FL", "258 Willow Way", 4 },
                    { 9, "Phoenix", "USA", "85001", "AZ", "369 Spruce Ct", 5 },
                    { 10, "Atlanta", "USA", "30301", "GA", "741 Ash Pl", 5 },
                    { 11, "Dallas", "USA", "75201", "TX", "852 Cherry Ter", 6 },
                    { 12, "Houston", "USA", "77001", "TX", "963 Walnut Dr", 6 },
                    { 13, "Philadelphia", "USA", "19101", "PA", "159 Poplar Ave", 7 },
                    { 14, "San Diego", "USA", "92101", "CA", "357 Hickory Rd", 7 },
                    { 15, "San Francisco", "USA", "94101", "CA", "486 Sycamore St", 8 },
                    { 16, "Las Vegas", "USA", "89101", "NV", "624 Magnolia Ln", 8 },
                    { 17, "Detroit", "USA", "48201", "MI", "735 Redwood Blvd", 9 },
                    { 18, "Minneapolis", "USA", "55401", "MN", "846 Cypress Way", 9 },
                    { 19, "Tampa", "USA", "33601", "FL", "957 Dogwood Ct", 10 },
                    { 20, "Baltimore", "USA", "21201", "MD", "168 Beech Pl", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StudentId",
                table: "Addresses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNo",
                table: "Students",
                column: "StudentNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
