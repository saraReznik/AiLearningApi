using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class FixUnicodeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__Categor__440B1D61",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__SubCate__44FF419A",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__UserId__4316F928",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__SubCatego__Categ__3E52440B",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Users__1788CC4CB41357ED",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SubCateg__26BE5B192D0120D4",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Prompts__3214EC0726753AC7",
                table: "Prompts");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Categori__19093A0BB1F11D74",
                table: "Categories");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__A9D10534C333071A",
                table: "Users",
                newName: "UQ__Users__A9D10534");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Users__1788CC4C",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SubCateg__26BE5B19",
                table: "SubCategories",
                column: "SubCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Prompts__3214EC07",
                table: "Prompts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Categori__19093A0B",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__Categor",
                table: "Prompts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__SubCate",
                table: "Prompts",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__UserId__",
                table: "Prompts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__SubCatego__Categ",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__Categor",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__SubCate",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__Prompts__UserId__",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK__SubCatego__Categ",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Users__1788CC4C",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SubCateg__26BE5B19",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Prompts__3214EC07",
                table: "Prompts");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Categori__19093A0B",
                table: "Categories");

            migrationBuilder.RenameIndex(
                name: "UQ__Users__A9D10534",
                table: "Users",
                newName: "UQ__Users__A9D10534C333071A");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubCategories",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Users__1788CC4CB41357ED",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SubCateg__26BE5B192D0120D4",
                table: "SubCategories",
                column: "SubCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Prompts__3214EC0726753AC7",
                table: "Prompts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Categori__19093A0BB1F11D74",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__Categor__440B1D61",
                table: "Prompts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__SubCate__44FF419A",
                table: "Prompts",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Prompts__UserId__4316F928",
                table: "Prompts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK__SubCatego__Categ__3E52440B",
                table: "SubCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
