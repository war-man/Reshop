using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class answer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerToComment_CommentsForProduct_CommentId",
                table: "AnswerToComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerToComment",
                table: "AnswerToComment");

            migrationBuilder.RenameTable(
                name: "AnswerToComment",
                newName: "AnswerToComments");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerToComment_CommentId",
                table: "AnswerToComments",
                newName: "IX_AnswerToComments_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerToComments",
                table: "AnswerToComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerToComments_CommentsForProduct_CommentId",
                table: "AnswerToComments",
                column: "CommentId",
                principalTable: "CommentsForProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerToComments_CommentsForProduct_CommentId",
                table: "AnswerToComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerToComments",
                table: "AnswerToComments");

            migrationBuilder.RenameTable(
                name: "AnswerToComments",
                newName: "AnswerToComment");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerToComments_CommentId",
                table: "AnswerToComment",
                newName: "IX_AnswerToComment_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerToComment",
                table: "AnswerToComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerToComment_CommentsForProduct_CommentId",
                table: "AnswerToComment",
                column: "CommentId",
                principalTable: "CommentsForProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
