using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class FAQs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FAQs",
                table: "Booking",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FAQs",
                table: "Booking");
        }
    }
}
