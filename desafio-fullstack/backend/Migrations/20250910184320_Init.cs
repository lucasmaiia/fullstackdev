using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadsApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "Id", "Category", "DateCreated", "Description", "Email", "FirstName", "LastName", "Phone", "Price", "Status", "Suburb" },
                values: new object[,]
                {
                    { 1, "Plumbing", new DateTime(2025, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Kitchen leak", "ana@example.com", "Ana", "Silva", "(31) 99999-1111", 650.00m, 0, "Centro" },
                    { 2, "Electrical", new DateTime(2025, 9, 2, 15, 30, 0, 0, DateTimeKind.Utc), "Outlet repair", "bruno@example.com", "Bruno", "Souza", "(31) 98888-2222", 300.00m, 0, "Savassi" },
                    { 3, "Cleaning", new DateTime(2025, 9, 3, 9, 45, 0, 0, DateTimeKind.Utc), "Post-renovation", "carla@example.com", "Carla", "Lima", "(31) 97777-3333", 800.00m, 1, "Funcionários" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leads");
        }
    }
}
