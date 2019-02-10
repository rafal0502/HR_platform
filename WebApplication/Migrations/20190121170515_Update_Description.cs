using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class Update_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descritpion",
                table: "JobOffers",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "JobApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "JobOffers",
                newName: "Descritpion");
        }
    }
}
