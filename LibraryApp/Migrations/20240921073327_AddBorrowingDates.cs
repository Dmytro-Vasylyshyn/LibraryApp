using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookId",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Users_UserId",
                table: "Borrowings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowings",
                table: "Borrowings");

            migrationBuilder.RenameTable(
                name: "Borrowings",
                newName: "Borrowing");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Borrowing",
                newName: "BorrowedDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Borrowing",
                newName: "ReturnedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowings_UserId",
                table: "Borrowing",
                newName: "IX_Borrowing_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowings_BookId",
                table: "Borrowing",
                newName: "IX_Borrowing_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowing",
                table: "Borrowing",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowing_Books_BookId",
                table: "Borrowing",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowing_Users_UserId",
                table: "Borrowing",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowing_Books_BookId",
                table: "Borrowing");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowing_Users_UserId",
                table: "Borrowing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowing",
                table: "Borrowing");

            migrationBuilder.RenameTable(
                name: "Borrowing",
                newName: "Borrowings");

            migrationBuilder.RenameColumn(
                name: "ReturnedDate",
                table: "Borrowings",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "BorrowedDate",
                table: "Borrowings",
                newName: "StartDate");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowing_UserId",
                table: "Borrowings",
                newName: "IX_Borrowings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowing_BookId",
                table: "Borrowings",
                newName: "IX_Borrowings_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowings",
                table: "Borrowings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookId",
                table: "Borrowings",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Users_UserId",
                table: "Borrowings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
