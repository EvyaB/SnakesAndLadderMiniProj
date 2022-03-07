﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SnakesAndLadderEvyatar.Repositories;

namespace SnakesAndLadderEvyatar.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SnakesAndLadderEvyatar.Data.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("GameStartDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PlayerGameState")
                        .HasColumnType("int");

                    b.Property<string>("PlayerName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TurnNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SnakesAndLadderEvyatar.Data.Player", b =>
                {
                    b.OwnsOne("SnakesAndLadderEvyatar.Data.Cell", "CurrentCell", b1 =>
                        {
                            b1.Property<int>("PlayerId")
                                .HasColumnType("int");

                            b1.Property<int>("Column")
                                .HasColumnType("int");

                            b1.Property<int>("Row")
                                .HasColumnType("int");

                            b1.HasKey("PlayerId");

                            b1.ToTable("Players");

                            b1.WithOwner()
                                .HasForeignKey("PlayerId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
