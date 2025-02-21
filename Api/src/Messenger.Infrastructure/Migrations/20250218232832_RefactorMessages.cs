using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "UsersChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chats",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "message");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Chats",
                newName: "chat");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "message",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "message",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "message",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "message",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "message",
                newName: "chat_id");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "user",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "user",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "user",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "IconUri",
                table: "user",
                newName: "icon_uri");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "user",
                newName: "IX_user_username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Id",
                table: "user",
                newName: "IX_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "user",
                newName: "IX_user_email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "chat",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "chat",
                newName: "creation_date");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_Id",
                table: "chat",
                newName: "IX_chat_id");

            migrationBuilder.AddColumn<Guid>(
                name: "new_id",
                table: "message",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropColumn(
                name: "id",
                table: "message");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "message",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_message",
                table: "message",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chat",
                table: "chat",
                column: "id");

            migrationBuilder.CreateTable(
                name: "user_chat",
                columns: table => new
                {
                    chats_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    users_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_chat", x => new { x.chats_id, x.users_id });
                    table.ForeignKey(
                        name: "FK_user_chat_chat_chats_id",
                        column: x => x.chats_id,
                        principalTable: "chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_chat_user_users_id",
                        column: x => x.users_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_chat_id",
                table: "message",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_id",
                table: "message",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_message_user_id",
                table: "message",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_chat_users_id",
                table: "user_chat",
                column: "users_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_chat_chat_id",
                table: "message",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_user_id",
                table: "message",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_message_chat_chat_id",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_user_id",
                table: "message");

            migrationBuilder.DropTable(
                name: "user_chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_message",
                table: "message");

            migrationBuilder.DropIndex(
                name: "IX_message_chat_id",
                table: "message");

            migrationBuilder.DropIndex(
                name: "IX_message_id",
                table: "message");

            migrationBuilder.DropIndex(
                name: "IX_message_user_id",
                table: "message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chat",
                table: "chat");

            migrationBuilder.RenameTable(
                name: "message",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "chat",
                newName: "Chats");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Message",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Message",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Message",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Message",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "Message",
                newName: "ChatId");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "icon_uri",
                table: "Users",
                newName: "IconUri");

            migrationBuilder.RenameIndex(
                name: "IX_user_username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "IX_user_id",
                table: "Users",
                newName: "IX_Users_Id");

            migrationBuilder.RenameIndex(
                name: "IX_user_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Chats",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "Chats",
                newName: "CreationDate");

            migrationBuilder.RenameIndex(
                name: "IX_chat_id",
                table: "Chats",
                newName: "IX_Chats_Id");

            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "Message",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "new_id",
                table: "Message",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                columns: new[] { "ChatId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsersChats",
                columns: table => new
                {
                    ChatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersChats", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UsersChats_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersChats_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersChats_UsersId",
                table: "UsersChats",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
