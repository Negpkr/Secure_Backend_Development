using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public partial class RobotContext : DbContext
{
    public RobotContext() { }

    public RobotContext(DbContextOptions<RobotContext> options)
        : base(options) { }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<RobotCommand> Robotcommands { get; set; }

    private string CONNECTION_STRING = DatabaseConfig.CONNECTION_STRING;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql(CONNECTION_STRING)
                .LogTo(Console.Write)
                // .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Name })
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Map>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("pk_map");
            entity.HasKey(e => e.Id);

            entity.ToTable("map");

            //entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

            entity.Property(e => e.Columns).HasColumnName("columns");

            entity
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");

            entity.Property(e => e.Description).HasMaxLength(800).HasColumnName("description");

            entity
                .Property(e => e.Issquare)
                .HasComputedColumnSql("((rows > 0) AND (rows = columns))", true)
                .HasColumnName("issquare");

            entity
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Rows).HasColumnName("rows");
        });

        modelBuilder.Entity<RobotCommand>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("pk_robotcommand");
            entity.HasKey(e => e.Id);

            entity.ToTable("robotcommand");

            //entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasColumnName("id");
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

            entity
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");

            entity.Property(e => e.Description).HasMaxLength(800).HasColumnName("description");

            entity.Property(e => e.IsMoveCommand).HasColumnName("ismovecommand");

            entity
                .Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
