﻿// <auto-generated />
using System;
using CounselWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CounselWeb.Migrations
{
    [DbContext(typeof(CounselContext))]
    [Migration("20201009152055_issuesList")]
    partial class issuesList
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CounselWeb.Models.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Issues");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Smoking"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Career and course choices"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Project choice"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Depression and suicidal thoughts"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Constant failing of courses"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Prostitution"
                        });
                });

            modelBuilder.Entity("CounselWeb.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("MessageBody")
                        .HasColumnType("text");

                    b.Property<int>("RequestId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("CounselWeb.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AdminId")
                        .HasColumnType("integer");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MatricNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Problem")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("CounselWeb.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MatricNo")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@counsel.com",
                            FirstName = "Admin",
                            IsAdmin = true,
                            LastName = "SuperAdmin",
                            Password = "admin123"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
