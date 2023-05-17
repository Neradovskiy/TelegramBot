using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicFieldToPlanForDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanForDayId",
                table: "Clients",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PlanForDayId",
                table: "Clients",
                column: "PlanForDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_PlanForDay_PlanForDayId",
                table: "Clients",
                column: "PlanForDayId",
                principalTable: "PlanForDay",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_PlanForDay_PlanForDayId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PlanForDayId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PlanForDayId",
                table: "Clients");
        }
    }
}
