﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserAuthProject.Models.DbContexts;

namespace UserAuthProject.Migrations
{
    [DbContext(typeof(GlobalDbContext))]
    [Migration("20201102202931_CreateUser")]
    partial class CreateUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("UserAuthProject.Models.Reward.Reward", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Command")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("MaxAmountRange")
                        .HasColumnType("int");

                    b.Property<int>("MinAmountRange")
                        .HasColumnType("int");

                    b.Property<int>("RewardRarity")
                        .HasColumnType("int");

                    b.Property<string>("Server")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("UserAuthProject.Models.Reward.RewardData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Verified")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RewardData");
                });

            modelBuilder.Entity("UserAuthProject.Models.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastRewardDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PasswordEncrypted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserAuthProject.Models.Reward.RewardData", b =>
                {
                    b.HasOne("UserAuthProject.Models.User.User", null)
                        .WithMany("Rewards")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
