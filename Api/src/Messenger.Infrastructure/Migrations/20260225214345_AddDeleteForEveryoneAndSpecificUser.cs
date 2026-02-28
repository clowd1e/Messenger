using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteForEveryoneAndSpecificUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_for_everyone_at",
                table: "message",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted_for_everyone",
                table: "message",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "deleted_for_users_messages",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deleted_for_users_messages", x => new { x.message_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_deleted_for_users_messages_message_message_id",
                        column: x => x.message_id,
                        principalTable: "message",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_deleted_for_users_messages_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_deleted_for_users_messages_user_id",
                table: "deleted_for_users_messages",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deleted_for_users_messages");

            migrationBuilder.DropColumn(
                name: "deleted_for_everyone_at",
                table: "message");

            migrationBuilder.DropColumn(
                name: "is_deleted_for_everyone",
                table: "message");
        }
    }
}
