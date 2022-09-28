using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteHole.Repository.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhiteHoleObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectName = table.Column<string>(type: "varchar(255)", nullable: false),
                    ObjectJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhiteHoleObject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhiteHoleKV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<int>(type: "int", nullable: false),
                    ObjectName = table.Column<string>(type: "varchar(255)", nullable: false),
                    ObjectKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ObjectValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhiteHoleKV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhiteHoleKV_WhiteHoleObject_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "WhiteHoleObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhiteHoleObjectRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Object1Id = table.Column<int>(type: "int", nullable: false),
                    ObjectNId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhiteHoleObjectRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhiteHoleObjectRelation_WhiteHoleObject_Object1Id",
                        column: x => x.Object1Id,
                        principalTable: "WhiteHoleObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_WhiteHoleObjectRelation_WhiteHoleObject_ObjectNId",
                        column: x => x.ObjectNId,
                        principalTable: "WhiteHoleObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhiteHoleKV_ObjectId",
                table: "WhiteHoleKV",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteHoleObjectRelation_Object1Id",
                table: "WhiteHoleObjectRelation",
                column: "Object1Id");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteHoleObjectRelation_ObjectNId",
                table: "WhiteHoleObjectRelation",
                column: "ObjectNId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhiteHoleKV");

            migrationBuilder.DropTable(
                name: "WhiteHoleObjectRelation");

            migrationBuilder.DropTable(
                name: "WhiteHoleObject");
        }
    }
}
