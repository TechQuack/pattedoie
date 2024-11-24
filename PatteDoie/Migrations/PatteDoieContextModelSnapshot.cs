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

            modelBuilder.Entity("PatteDoie.Models.Platform.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<int>("MinPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlatformGame");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.HighScore", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("PlatformHighScore");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Lobby", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("PlatformLobby");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserUUID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LobbyId");

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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ScattergoriesGameId");

                    b.HasIndex("UserId");

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

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingPlayer", b =>
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

                    b.HasIndex("UserId");

                    b.ToTable("SpeedTypingPlayer");
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

            modelBuilder.Entity("PatteDoie.Models.Platform.HighScore", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.Game", null)
                        .WithMany("HighScores")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Lobby", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.User", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.Lobby", null)
                        .WithMany("Users")
                        .HasForeignKey("LobbyId");
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

                    b.HasOne("PatteDoie.Models.Platform.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingPlayer", b =>
                {
                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingGame", null)
                        .WithMany("Players")
                        .HasForeignKey("SpeedTypingGameId");

                    b.HasOne("PatteDoie.Models.Platform.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingTimeProgress", b =>
                {
                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingGame", null)
                        .WithMany("TimeProgresses")
                        .HasForeignKey("SpeedTypingGameId");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Game", b =>
                {
                    b.Navigation("HighScores");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Lobby", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesGame", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingGame", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("TimeProgresses");
                });
#pragma warning restore 612, 618
        }
    }
}
