using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Upload_API.Models
{
    public partial class UploadDBContext : DbContext
    {     
        public UploadDBContext(DbContextOptions<UploadDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Upload> Uploads { get; set; }     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.ToTable("Upload");

                entity.Property(e => e.FileName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath).IsUnicode(false);

                entity.Property(e => e.FileSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("GUID");

                entity.Property(e => e.LocalFileName).IsUnicode(false);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
