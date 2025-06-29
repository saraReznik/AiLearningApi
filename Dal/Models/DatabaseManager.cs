using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.Models;

public partial class DatabaseManager : DbContext
{
    public DatabaseManager() { }

    public DatabaseManager(DbContextOptions<DatabaseManager> options)
        : base(options) { }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Prompt> Prompts { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(true); // ✅ תמיכה בעברית

            entity.Property(e => e.Description)
                .HasColumnType("nvarchar(max)"); // ✅ תמיכה בעברית
        });

        modelBuilder.Entity<Prompt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Prompts__3214EC07");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Prompt1).HasColumnName("Prompt");

            entity.HasOne(d => d.Category).WithMany(p => p.Prompts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prompts__Categor");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Prompts)
                .HasForeignKey(d => d.SubCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prompts__SubCate");

            entity.HasOne(d => d.User).WithMany(p => p.Prompts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prompts__UserId__");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__SubCateg__26BE5B19");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(true); // ✅ תמיכה בעברית

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__SubCatego__Categ");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C");

            entity.Property(e => e.UserId).ValueGeneratedOnAdd();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
