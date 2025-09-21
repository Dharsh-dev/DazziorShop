using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DazziorShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecificationToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specifictaion",
                table: "Product",
                newName: "Specification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specification",
                table: "Product",
                newName: "Specifictaion");
        }
    }
}
