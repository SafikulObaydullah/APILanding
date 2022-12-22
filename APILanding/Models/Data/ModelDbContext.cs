using System;
using System.Collections.Generic;
using APILanding.Models.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace APILanding.Models.Data;

public partial class ModelDbContext : DbContext
{
    public ModelDbContext()
    {
    }

    public ModelDbContext(DbContextOptions<ModelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblItem> TblItems { get; set; }

    public virtual DbSet<TblPartner> TblPartners { get; set; }

    public virtual DbSet<TblPartnerType> TblPartnerTypes { get; set; }

    public virtual DbSet<TblPurchase> TblPurchases { get; set; }

    public virtual DbSet<TblPurchaseDetail> TblPurchaseDetails { get; set; }

    public virtual DbSet<TblSale> TblSales { get; set; }

    public virtual DbSet<TblSalesDetail> TblSalesDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-GRS0037;Initial Catalog=GEODB; Trusted_Connection =True; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblItem>(entity =>
        {
            entity.HasKey(e => e.IntItemId);

            entity.ToTable("tblItem");

            entity.Property(e => e.IntItemId).HasColumnName("intItemId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
            entity.Property(e => e.NumStockQuantity).HasColumnName("numStockQuantity");
            entity.Property(e => e.StrItemName)
                .HasMaxLength(50)
                .HasColumnName("strItemName");
        });

        modelBuilder.Entity<TblPartner>(entity =>
        {
            entity.HasKey(e => e.IntPartnerId);

            entity.ToTable("tblPartner");

            entity.Property(e => e.IntPartnerId).HasColumnName("intPartnerId");
            entity.Property(e => e.IntPartnerTypeId).HasColumnName("intPartnerTypeId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
            entity.Property(e => e.StrPartnerName)
                .HasMaxLength(50)
                .HasColumnName("strPartnerName");
        });

        modelBuilder.Entity<TblPartnerType>(entity =>
        {
            entity.HasKey(e => e.IntPartnerTypeId);

            entity.ToTable("tblPartnerType");

            entity.Property(e => e.IntPartnerTypeId).HasColumnName("intPartnerTypeId");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
            entity.Property(e => e.StrPartnerTypeName)
                .HasMaxLength(50)
                .HasColumnName("strPartnerTypeName");
        });

        modelBuilder.Entity<TblPurchase>(entity =>
        {
            entity.HasKey(e => e.IntPurchaseId);

            entity.ToTable("tblPurchase");

            entity.Property(e => e.IntPurchaseId).HasColumnName("intPurchaseId");
            entity.Property(e => e.DtePurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("dtePurchaseDate");
            entity.Property(e => e.IntSupplierId).HasColumnName("intSupplierId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
        });

        modelBuilder.Entity<TblPurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.IntDetailsId);

            entity.ToTable("tblPurchaseDetails");

            entity.Property(e => e.IntDetailsId).HasColumnName("intDetailsId");
            entity.Property(e => e.IntItemId).HasColumnName("intItemId");
            entity.Property(e => e.IntPurchaseId).HasColumnName("intPurchaseId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
            entity.Property(e => e.NumItemQuantity).HasColumnName("numItemQuantity");
            entity.Property(e => e.NumUnitPrice).HasColumnName("numUnitPrice");
        });

        modelBuilder.Entity<TblSale>(entity =>
        {
            entity.HasKey(e => e.IntSalesId);

            entity.ToTable("tblSales");

            entity.Property(e => e.IntSalesId).HasColumnName("intSalesId");
            entity.Property(e => e.DteSalesDate)
                .HasColumnType("datetime")
                .HasColumnName("dteSalesDate");
            entity.Property(e => e.IntCustomerId).HasColumnName("intCustomerId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
        });

        modelBuilder.Entity<TblSalesDetail>(entity =>
        {
            entity.HasKey(e => e.IntDetailsId);

            entity.ToTable("tblSalesDetails");

            entity.Property(e => e.IntDetailsId).HasColumnName("intDetailsId");
            entity.Property(e => e.IntItemId).HasColumnName("intItemId");
            entity.Property(e => e.IntSalesId).HasColumnName("intSalesId");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("isActive");
            entity.Property(e => e.NumItemQuantity).HasColumnName("numItemQuantity");
            entity.Property(e => e.NumUnitPrice).HasColumnName("numUnitPrice");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
