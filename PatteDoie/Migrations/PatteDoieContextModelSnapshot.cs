﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatteDoie;

#nullable disable

namespace PatteDoie.Migrations
{
    [DbContext(typeof(PatteDoieContext))]
    partial class PatteDoieContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Max_players")
                        .HasColumnType("int");

                    b.Property<int>("Min_players")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlatformGame");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformHighScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<Guid?>("PlatformGameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Player")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlatformGameId");

                    b.ToTable("PlatformHighScore");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformLobby", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("creatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("gameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("creatorId");

                    b.ToTable("PlatformLobby");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PlatformLobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlatformLobbyId");

                    b.ToTable("PlatformUser");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ScattergoriesGameId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ScattergoriesGameId");

                    b.ToTable("ScattergoriesCategory");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentLetter")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("CurrentRound")
                        .HasColumnType("int");

                    b.Property<int>("MaxRound")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ScattergoriesGame");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ScattergoriesGameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ScattergoriesGameId");

                    b.ToTable("ScattergoriesPlayer");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LaunchTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Words")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SpeedTypingGame");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<Guid?>("SpeedTypingGameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SpeedTypingGameId");

                    b.ToTable("SpeedTypingScore");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingTimeProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SpeedTypingGameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TimeProgress")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SpeedTypingGameId");

                    b.ToTable("SpeedTypingTimeProgress");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformHighScore", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.PlatformGame", null)
                        .WithMany("HighScores")
                        .HasForeignKey("PlatformGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformLobby", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.PlatformUser", "creator")
                        .WithMany()
                        .HasForeignKey("creatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("creator");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformUser", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.PlatformLobby", null)
                        .WithMany("users")
                        .HasForeignKey("PlatformLobbyId");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesCategory", b =>
                {
                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesGame", null)
                        .WithMany("Categories")
                        .HasForeignKey("ScattergoriesGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesPlayer", b =>
                {
                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesGame", null)
                        .WithMany("Players")
                        .HasForeignKey("ScattergoriesGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingScore", b =>
                {
                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingGame", null)
                        .WithMany("Scores")
                        .HasForeignKey("SpeedTypingGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingTimeProgress", b =>
                {
                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingGame", null)
                        .WithMany("TimeProgresses")
                        .HasForeignKey("SpeedTypingGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformGame", b =>
                {
                    b.Navigation("HighScores");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.PlatformLobby", b =>
                {
                    b.Navigation("users");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesGame", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingGame", b =>
                {
                    b.Navigation("Scores");

                    b.Navigation("TimeProgresses");
                });
#pragma warning restore 612, 618
        }
    }
}
