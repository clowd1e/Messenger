using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupAndPrivateChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_chat_chat_chats_id",
                table: "user_chat");

            migrationBuilder.DropForeignKey(
                name: "FK_user_chat_user_users_id",
                table: "user_chat");

            migrationBuilder.RenameColumn(
                name: "users_id",
                table: "user_chat",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "chats_id",
                table: "user_chat",
                newName: "chat_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_chat_users_id",
                table: "user_chat",
                newName: "IX_user_chat_user_id");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "user",
                newName: "email_confirmed");

            migrationBuilder.CreateTable(
                name: "group_chat",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_chat", x => x.id);
                    table.ForeignKey(
                        name: "FK_group_chat_chat_id",
                        column: x => x.id,
                        principalTable: "chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "private_chat",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_private_chat", x => x.id);
                    table.ForeignKey(
                        name: "FK_private_chat_chat_id",
                        column: x => x.id,
                        principalTable: "chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_member",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    chat_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_member", x => new { x.user_id, x.chat_id });
                    table.ForeignKey(
                        name: "FK_group_member_group_chat_chat_id",
                        column: x => x.chat_id,
                        principalTable: "group_chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_member_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Copy data from chat to private_chat
            migrationBuilder.Sql(@"
                INSERT INTO private_chat (id)
                SELECT DISTINCT id
                FROM chat");

            migrationBuilder.CreateIndex(
                name: "IX_group_chat_id",
                table: "group_chat",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_member_chat_id",
                table: "group_member",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_private_chat_id",
                table: "private_chat",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_chat_chat_chat_id",
                table: "user_chat",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_chat_user_user_id",
                table: "user_chat",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_chat_chat_chat_id",
                table: "user_chat");

            migrationBuilder.DropForeignKey(
                name: "FK_user_chat_user_user_id",
                table: "user_chat");

            migrationBuilder.DropTable(
                name: "group_member");

            migrationBuilder.DropTable(
                name: "private_chat");

            migrationBuilder.DropTable(
                name: "group_chat");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_chat",
                newName: "users_id");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "user_chat",
                newName: "chats_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_chat_user_id",
                table: "user_chat",
                newName: "IX_user_chat_users_id");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "user",
                newName: "EmailConfirmed");

            migrationBuilder.AddForeignKey(
                name: "FK_user_chat_chat_chats_id",
                table: "user_chat",
                column: "chats_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_chat_user_users_id",
                table: "user_chat",
                column: "users_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
