﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mottu.Api.Infrastructure.Identity;

#nullable disable

namespace Mottu.Api.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.DeliveryMan", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<string>("CompanyRegistrationNumber")
                        .IsRequired()
                        .HasColumnType("varchar(14)")
                        .HasColumnName("company_registration_number");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<string>("RentId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("rent_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("update_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("delivery_men", (string)null);
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.Motorcycle", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("model");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("varchar(7)")
                        .HasColumnName("plate");

                    b.Property<string>("RentId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("rent_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("update_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("year");

                    b.HasKey("Id");

                    b.ToTable("motorcycles", (string)null);
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.Rent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("DeliveryManId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("delivery_man_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("datetime")
                        .HasColumnName("expected_end_date");

                    b.Property<string>("MotorcycleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("motorcycle_id");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime")
                        .HasColumnName("return_date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime")
                        .HasColumnName("start_date");

                    b.Property<decimal?>("TotalAmountPayable")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total_amount_payable");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime")
                        .HasColumnName("update_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryManId")
                        .IsUnique();

                    b.HasIndex("MotorcycleId")
                        .IsUnique();

                    b.ToTable("rents", (string)null);
                });

            modelBuilder.Entity("Mottu.Api.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Mottu.Api.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Mottu.Api.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mottu.Api.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Mottu.Api.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.DeliveryMan", b =>
                {
                    b.HasOne("Mottu.Api.Infrastructure.Identity.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("Mottu.Api.Domain.Entities.DeliveryMan", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Mottu.Api.Domain.Entities.DriverLicense", "DriverLicense", b1 =>
                        {
                            b1.Property<string>("DeliveryManId")
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("ImagePath")
                                .HasColumnType("varchar(255)")
                                .HasColumnName("driver_license_image_path");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("varchar(11)")
                                .HasColumnName("driver_license_number");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("char(1)")
                                .HasColumnName("driver_license_type");

                            b1.HasKey("DeliveryManId");

                            b1.ToTable("delivery_men");

                            b1.WithOwner()
                                .HasForeignKey("DeliveryManId");
                        });

                    b.Navigation("DriverLicense")
                        .IsRequired();
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.Rent", b =>
                {
                    b.HasOne("Mottu.Api.Domain.Entities.DeliveryMan", "DeliveryMan")
                        .WithOne("Rent")
                        .HasForeignKey("Mottu.Api.Domain.Entities.Rent", "DeliveryManId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mottu.Api.Domain.Entities.Motorcycle", "Motorcycle")
                        .WithOne("Rent")
                        .HasForeignKey("Mottu.Api.Domain.Entities.Rent", "MotorcycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Mottu.Api.Domain.Entities.Plan", "Plan", b1 =>
                        {
                            b1.Property<string>("RentId")
                                .HasColumnType("varchar(255)");

                            b1.Property<decimal>("DailyRate")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("plan_daily_rate");

                            b1.Property<string>("Days")
                                .IsRequired()
                                .HasColumnType("varchar(255)")
                                .HasColumnName("plan_days");

                            b1.HasKey("RentId");

                            b1.ToTable("rents");

                            b1.WithOwner()
                                .HasForeignKey("RentId");
                        });

                    b.Navigation("DeliveryMan");

                    b.Navigation("Motorcycle");

                    b.Navigation("Plan")
                        .IsRequired();
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.DeliveryMan", b =>
                {
                    b.Navigation("Rent");
                });

            modelBuilder.Entity("Mottu.Api.Domain.Entities.Motorcycle", b =>
                {
                    b.Navigation("Rent");
                });
#pragma warning restore 612, 618
        }
    }
}
