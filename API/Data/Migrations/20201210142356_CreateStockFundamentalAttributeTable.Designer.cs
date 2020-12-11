﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201210142356_CreateStockFundamentalAttributeTable")]
    partial class CreateStockFundamentalAttributeTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("API.Entities.Stock", b =>
                {
                    b.Property<int>("stockID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("companyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("industry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<string>("stockSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("stockID");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("API.Entities.StockFundamentalAttributes", b =>
                {
                    b.Property<int>("StockFundamentalAttributeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Head")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RecordTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Statement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y0")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Y9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("stockID")
                        .HasColumnType("int");

                    b.HasKey("StockFundamentalAttributeID");

                    b.HasIndex("stockID");

                    b.ToTable("StockFundamentalAttributes");
                });

            modelBuilder.Entity("API.Entities.StockFundamentalAttributes", b =>
                {
                    b.HasOne("API.Entities.Stock", "stock")
                        .WithMany()
                        .HasForeignKey("stockID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("stock");
                });
#pragma warning restore 612, 618
        }
    }
}