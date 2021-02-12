using Microsoft.EntityFrameworkCore.Migrations;

namespace Reshop.Infrastructure.Migrations
{
    public partial class chilscategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildCategories_Categories_CategoryId",
                table: "ChildCategories");

            migrationBuilder.DropIndex(
                name: "IX_ChildCategories_CategoryId",
                table: "ChildCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ChildCategories");

            migrationBuilder.CreateTable(
                name: "ChildCategoryToCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCategoryToCategories", x => new { x.CategoryId, x.ChildCategoryId });
                    table.ForeignKey(
                        name: "FK_ChildCategoryToCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildCategoryToCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategoryToCategories_ChildCategoryId",
                table: "ChildCategoryToCategories",
                column: "ChildCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildCategoryToCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ChildCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategories_CategoryId",
                table: "ChildCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildCategories_Categories_CategoryId",
                table: "ChildCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
