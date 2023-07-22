using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Migrations
{
    /// <inheritdoc />
    public partial class AddTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Training_TrainingId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_PlanForDay_PlanForDayId",
                table: "Training");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Training",
                table: "Training");

            migrationBuilder.RenameTable(
                name: "Training",
                newName: "Workout");

            migrationBuilder.RenameIndex(
                name: "IX_Training_PlanForDayId",
                table: "Workout",
                newName: "IX_Workout_PlanForDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workout",
                table: "Workout",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Workout_TrainingId",
                table: "Clients",
                column: "TrainingId",
                principalTable: "Workout",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workout_PlanForDay_PlanForDayId",
                table: "Workout",
                column: "PlanForDayId",
                principalTable: "PlanForDay",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Workout_TrainingId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Workout_PlanForDay_PlanForDayId",
                table: "Workout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workout",
                table: "Workout");

            migrationBuilder.RenameTable(
                name: "Workout",
                newName: "Training");

            migrationBuilder.RenameIndex(
                name: "IX_Workout_PlanForDayId",
                table: "Training",
                newName: "IX_Training_PlanForDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Training",
                table: "Training",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Training_TrainingId",
                table: "Clients",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_PlanForDay_PlanForDayId",
                table: "Training",
                column: "PlanForDayId",
                principalTable: "PlanForDay",
                principalColumn: "Id");
        }
    }
}
