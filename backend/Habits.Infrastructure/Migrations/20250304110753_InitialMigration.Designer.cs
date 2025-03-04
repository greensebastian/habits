﻿// <auto-generated />
using System;
using Habits.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Habits.Infrastructure.Migrations
{
    [DbContext(typeof(HabitsDbContext))]
    [Migration("20250304110753_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Habits.Core.Habit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.ToTable("Habits");
                });

            modelBuilder.Entity("Habits.Core.Habit", b =>
                {
                    b.OwnsMany("Habits.Core.LogEntry", "Entries", b1 =>
                        {
                            b1.Property<Guid>("HabitId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("Comment")
                                .HasColumnType("text");

                            b1.Property<DateTimeOffset>("DoneAt")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("HabitId", "Id");

                            b1.ToTable("LogEntries", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("HabitId");
                        });

                    b.Navigation("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
