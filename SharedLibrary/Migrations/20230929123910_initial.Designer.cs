﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedLibrary;

#nullable disable

namespace SharedLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230929123910_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("SharedLibrary.Entities.SettingsEntity", b =>
                {
                    b.Property<string>("ConnectionString")
                        .HasColumnType("TEXT");

                    b.HasKey("ConnectionString");

                    b.ToTable("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
