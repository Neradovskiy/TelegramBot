﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramBot.Model;

#nullable disable

namespace TelegramBot.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230710145204_AddTraining")]
    partial class AddTraining
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramBot.Model.Abonement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Abonements");
                });

            modelBuilder.Entity("TelegramBot.Model.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AbonementId")
                        .HasColumnType("integer");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("EndTrainig")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTrainig")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("TrainingId")
                        .HasColumnType("integer");

                    b.Property<int?>("WorkerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AbonementId");

                    b.HasIndex("TrainingId");

                    b.HasIndex("WorkerId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TelegramBot.Model.PlanForDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("PlanForDay");
                });

            modelBuilder.Entity("TelegramBot.Model.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("TelegramBot.Model.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("PlanForDayId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PlanForDayId");

                    b.ToTable("Workout");
                });

            modelBuilder.Entity("TelegramBot.Model.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<int?>("PlanId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("TelegramBot.Model.Abonement", b =>
                {
                    b.HasOne("TelegramBot.Model.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("TelegramBot.Model.Client", b =>
                {
                    b.HasOne("TelegramBot.Model.Abonement", "Abonement")
                        .WithMany()
                        .HasForeignKey("AbonementId");

                    b.HasOne("TelegramBot.Model.Training", null)
                        .WithMany("Clients")
                        .HasForeignKey("TrainingId");

                    b.HasOne("TelegramBot.Model.Worker", null)
                        .WithMany("Clients")
                        .HasForeignKey("WorkerId");

                    b.Navigation("Abonement");
                });

            modelBuilder.Entity("TelegramBot.Model.Training", b =>
                {
                    b.HasOne("TelegramBot.Model.PlanForDay", null)
                        .WithMany("Trainings")
                        .HasForeignKey("PlanForDayId");
                });

            modelBuilder.Entity("TelegramBot.Model.Worker", b =>
                {
                    b.HasOne("TelegramBot.Model.PlanForDay", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("TelegramBot.Model.PlanForDay", b =>
                {
                    b.Navigation("Trainings");
                });

            modelBuilder.Entity("TelegramBot.Model.Training", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("TelegramBot.Model.Worker", b =>
                {
                    b.Navigation("Clients");
                });
#pragma warning restore 612, 618
        }
    }
}