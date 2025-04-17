using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class RenameSignaturePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignatureCode",
                table: "Signatures",
                newName: "SignaturePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignaturePath",
                table: "Signatures",
                newName: "SignatureCode");
        }
    }
}
