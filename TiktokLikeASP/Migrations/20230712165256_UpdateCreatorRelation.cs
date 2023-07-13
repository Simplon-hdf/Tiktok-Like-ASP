using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokLikeASP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreatorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Persons_id",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "Posts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_creator_id",
                table: "Posts",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Persons_creator_id",
                table: "Posts",
                column: "creator_id",
                principalTable: "Persons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Persons_creator_id",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_creator_id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Persons_id",
                table: "Posts",
                column: "id",
                principalTable: "Persons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
