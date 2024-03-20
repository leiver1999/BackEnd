﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectAPI.Context;

#nullable disable

namespace ProjectAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240318203855_Usuario con Requisicion")]
    partial class UsuarioconRequisicion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectAPI.Models.Factura", b =>
                {
                    b.Property<int>("codigo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("codigo"));

                    b.Property<string>("Fecha")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreCliente")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumFactura")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pedido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RequisicionId")
                        .HasColumnType("int");

                    b.Property<int>("codigoFactura")
                        .HasColumnType("int");

                    b.HasKey("codigo");

                    b.HasIndex("RequisicionId");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("ProjectAPI.Models.Requisicion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Requisiciones");
                });

            modelBuilder.Entity("ProjectAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cedula")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ResetPasswordExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Telefono")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VehiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehiculoId")
                        .IsUnique()
                        .HasFilter("[VehiculoId] IS NOT NULL");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ProjectAPI.Models.Vehiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EstadoVehiculo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<long>("Kilometraje")
                        .HasColumnType("bigint");

                    b.Property<string>("NumeroPlaca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoVehiculo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("ProjectAPI.Models.Factura", b =>
                {
                    b.HasOne("ProjectAPI.Models.Requisicion", "Requisicion")
                        .WithMany("Facturas")
                        .HasForeignKey("RequisicionId");

                    b.Navigation("Requisicion");
                });

            modelBuilder.Entity("ProjectAPI.Models.Requisicion", b =>
                {
                    b.HasOne("ProjectAPI.Models.User", "User")
                        .WithMany("Requisiciones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectAPI.Models.User", b =>
                {
                    b.HasOne("ProjectAPI.Models.Vehiculo", "Vehiculo")
                        .WithOne()
                        .HasForeignKey("ProjectAPI.Models.User", "VehiculoId");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("ProjectAPI.Models.Requisicion", b =>
                {
                    b.Navigation("Facturas");
                });

            modelBuilder.Entity("ProjectAPI.Models.User", b =>
                {
                    b.Navigation("Requisiciones");
                });
#pragma warning restore 612, 618
        }
    }
}
