﻿// <auto-generated />
using System;
using JobHunting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobHunting.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231102192141_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobHunting.Models.Entity.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("City")
                        .HasColumnType("int");

                    b.Property<int>("DecodeMethod")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)")
                        .HasColumnName("mobile");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("City");

                    b.ToTable("person", (string)null);
                });

            modelBuilder.Entity("JobHunting.Models.Entity.Resume", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsFullEmployment")
                        .HasColumnType("bit")
                        .HasColumnName("full_employment");

                    b.Property<bool>("IsPartTimeEmployment")
                        .HasColumnType("bit")
                        .HasColumnName("part_time_employment");

                    b.Property<bool>("IsTrainee")
                        .HasColumnType("bit")
                        .HasColumnName("trainee");

                    b.Property<bool>("IsVolunteering")
                        .HasColumnType("bit")
                        .HasColumnName("volunteering");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Salary");

                    b.ToTable("resumes");
                });

            modelBuilder.Entity("JobHunting.Models.Entity.Resume", b =>
                {
                    b.HasOne("JobHunting.Models.Entity.Person", null)
                        .WithMany("Resumes")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobHunting.Models.Entity.Person", b =>
                {
                    b.Navigation("Resumes");
                });
#pragma warning restore 612, 618
        }
    }
}