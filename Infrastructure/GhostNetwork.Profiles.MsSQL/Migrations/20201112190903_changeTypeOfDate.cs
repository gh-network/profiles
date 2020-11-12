using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GhostNetwork.Profiles.MsSQL.Migrations
{
    public partial class changeTypeOfDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DateOfBirth",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Profiles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
