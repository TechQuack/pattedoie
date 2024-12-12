﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PatteDoie;

#nullable disable

namespace PatteDoie.Migrations
{
    [DbContext(typeof(PatteDoieContext))]
    [Migration("20241207102341_AddLobbyNameField")]
    partial class AddLobbyNameField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("dcdbaa94-a09f-440f-800a-ea6258ca5b4c"),
                            MaxPlayers = 8,
                            MinPlayers = 2,
                            Name = "Scattergories"
                        },
                        new
                        {
                            Id = new Guid("3ecbba82-d7bb-4981-b3ac-9166b1c09d19"),
                            MaxPlayers = 5,
                            MinPlayers = 1,
                            Name = "SpeedTyping"
                        });
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

                    b.Property<string>("LobbyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("GameId");

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

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ScattergoriesPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ScattergoriesPlayerId");

                    b.ToTable("ScattergoriesAnswer");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ScattergoriesCategory");

                    b.HasData(
                        new
                        {
                            Id = new Guid("39fa4494-76b3-4dc8-80f5-2cd76f6ae92e"),
                            Name = "country"
                        },
                        new
                        {
                            Id = new Guid("028bf727-7e59-41e3-9489-317d5ffce472"),
                            Name = "animal"
                        },
                        new
                        {
                            Id = new Guid("a992d0e1-f945-47d4-8fdd-a96e44925950"),
                            Name = "firstname"
                        },
                        new
                        {
                            Id = new Guid("2bfe903c-1dc1-44a3-b2fc-2207b3d6db5f"),
                            Name = "clothes"
                        },
                        new
                        {
                            Id = new Guid("720e88c2-886a-4062-89d9-2146ca613e39"),
                            Name = "sport"
                        },
                        new
                        {
                            Id = new Guid("b45eb4a8-1726-4e55-b99c-f3f70995c46d"),
                            Name = "occupation"
                        },
                        new
                        {
                            Id = new Guid("d7b5b9ef-a0d6-409a-8e43-6d25f02fcaef"),
                            Name = "fruit or vegetable"
                        },
                        new
                        {
                            Id = new Guid("8cc453db-d1d1-425d-bc7e-0ed1b55d80b7"),
                            Name = "brand"
                        },
                        new
                        {
                            Id = new Guid("5f463de8-d058-45fb-ba73-35b138219b70"),
                            Name = "famous person"
                        },
                        new
                        {
                            Id = new Guid("b686db24-1357-4bca-9379-0545f899c1b0"),
                            Name = "game"
                        },
                        new
                        {
                            Id = new Guid("9e842717-555b-40b9-89b8-65edfad64a7d"),
                            Name = "movie/tv show"
                        });
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

                    b.Property<bool>("IsHostCheckingPhase")
                        .HasColumnType("bit");

                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxRound")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LobbyId");

                    b.ToTable("ScattergoriesGame");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsHost")
                        .HasColumnType("bit");

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

                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Words")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LobbyId");

                    b.ToTable("SpeedTypingGame");
                });

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("SecondsToFinish")
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

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SpeedTypingGameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TimeProgress")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SpeedTypingGameId");

                    b.ToTable("SpeedTypingTimeProgress");
                });

            modelBuilder.Entity("ScattergoriesCategoryScattergoriesGame", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GamesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "GamesId");

                    b.HasIndex("GamesId");

                    b.ToTable("ScattergoriesCategoryScattergoriesGame");
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

                    b.HasOne("PatteDoie.Models.Platform.Game", "Game")
                        .WithMany("Lobbys")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.User", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.Lobby", null)
                        .WithMany("Users")
                        .HasForeignKey("LobbyId");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesAnswer", b =>
                {
                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesPlayer", null)
                        .WithMany("Answers")
                        .HasForeignKey("ScattergoriesPlayerId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesGame", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.Lobby", "Lobby")
                        .WithMany()
                        .HasForeignKey("LobbyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lobby");
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

            modelBuilder.Entity("PatteDoie.Models.SpeedTyping.SpeedTypingGame", b =>
                {
                    b.HasOne("PatteDoie.Models.Platform.Lobby", "Lobby")
                        .WithMany()
                        .HasForeignKey("LobbyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lobby");
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
                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingPlayer", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PatteDoie.Models.SpeedTyping.SpeedTypingGame", null)
                        .WithMany("TimeProgresses")
                        .HasForeignKey("SpeedTypingGameId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("ScattergoriesCategoryScattergoriesGame", b =>
                {
                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesCategory", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PatteDoie.Models.Scattergories.ScattergoriesGame", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Game", b =>
                {
                    b.Navigation("HighScores");

                    b.Navigation("Lobbys");
                });

            modelBuilder.Entity("PatteDoie.Models.Platform.Lobby", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesGame", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("PatteDoie.Models.Scattergories.ScattergoriesPlayer", b =>
                {
                    b.Navigation("Answers");
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
