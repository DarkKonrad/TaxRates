using Microsoft.EntityFrameworkCore.Migrations;

namespace TaxRates.Data.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "TaxRates",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Rate = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TaxRates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "TaxCategories",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(nullable: true),
					RateId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TaxCategories", x => x.Id);
					table.ForeignKey(
						name: "FK_TaxCategories_TaxRates_RateId",
						column: x => x.RateId,
						principalTable: "TaxRates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_TaxCategories_RateId",
				table: "TaxCategories",
				column: "RateId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "TaxCategories");

			migrationBuilder.DropTable(
				name: "TaxRates");
		}
	}
}