﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecordVoice.Persistence;

namespace RecordVoice.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32");

            modelBuilder.Entity("RecordVoice.Domain.Record", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("ApplicationOpened")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Connected")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IP")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Recording")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
