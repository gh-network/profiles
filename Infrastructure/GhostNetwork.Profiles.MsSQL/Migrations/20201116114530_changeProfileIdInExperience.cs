using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GhostNetwork.Profiles.MsSQL.Migrations
{
    public partial class changeProfileIdInExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_Profiles_ProfileId1",
                table: "WorkExperience");

            migrationBuilder.DropIndex(
                name: "IX_WorkExperience_ProfileId1",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "ProfileId1",
                table: "WorkExperience");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProfileId",
                table: "WorkExperience",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_ProfileId",
                table: "WorkExperience",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_Profiles_ProfileId",
                table: "WorkExperience",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_Profiles_ProfileId",
                table: "WorkExperience");

            migrationBuilder.DropIndex(
                name: "IX_WorkExperience_ProfileId",
                table: "WorkExperience");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "WorkExperience",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId1",
                table: "WorkExperience",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_ProfileId1",
                table: "WorkExperience",
                column: "ProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_Profiles_ProfileId1",
                table: "WorkExperience",
                column: "ProfileId1",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
