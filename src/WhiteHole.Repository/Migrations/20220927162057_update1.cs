using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteHole.Repository.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Object1Name",
                table: "WhiteHoleObjectRelation",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectNName",
                table: "WhiteHoleObjectRelation",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Object1Name",
                table: "WhiteHoleObjectRelation");

            migrationBuilder.DropColumn(
                name: "ObjectNName",
                table: "WhiteHoleObjectRelation");
        }
    }
}
