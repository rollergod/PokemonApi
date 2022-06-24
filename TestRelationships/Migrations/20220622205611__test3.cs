using Microsoft.EntityFrameworkCore.Migrations;

namespace TestRelationships.Migrations
{
    public partial class _test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "CityId", "Name" },
                values: new object[] { 1, 1, "Rus" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
