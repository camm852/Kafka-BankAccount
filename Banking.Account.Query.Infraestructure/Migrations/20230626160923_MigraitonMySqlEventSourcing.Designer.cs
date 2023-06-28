﻿// <auto-generated />
using System;
using Banking.Account.Query.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Banking.Account.Query.Infraestructure.Migrations
{
    [DbContext(typeof(MysqlDbContext))]
    [Migration("20230626160923_MigraitonMySqlEventSourcing")]
    partial class MigraitonMySqlEventSourcing
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Banking.Account.Query.Domain.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountHolder")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("BankAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
