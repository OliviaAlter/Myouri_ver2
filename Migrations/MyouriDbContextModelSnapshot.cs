﻿// <auto-generated />
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnotherMyouri.Migrations
{
    [DbContext(typeof(MyouriDbContext))]
    partial class MyouriDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.1.21102.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AnotherMyouri.DatabaseEntities.Entities.Server", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

                    b.Property<decimal>("EventLogChannel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<bool>("InviteToggle")
                        .HasColumnType("bit");

                    b.Property<decimal>("LeaveChannel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("LeaveMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MessageLogChannel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Prefix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RoleMentionToggle")
                        .HasColumnType("bit");

                    b.Property<bool>("UserMentionToggle")
                        .HasColumnType("bit");

                    b.Property<decimal>("UserUpdateChannel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("WelcomeChannel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("WelcomeMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("WelcomeUrl")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });
#pragma warning restore 612, 618
        }
    }
}