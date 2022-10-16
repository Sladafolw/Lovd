using System;
using System.Collections.Generic;
using Lovd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lovd.Models
{
    public partial class LoveContext : LovdContext
    {
        

        public LoveContext(DbContextOptions<LoveContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<LikesWithDislike> LikesWithDislikes { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TopicForum> TopicForums { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-VVMACCF\\SQLEXPRESS;Database=Love;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComments)
                    .HasName("PK__Comments__0B5B73B1D6C9A202");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.Text).HasMaxLength(1000);

                entity.HasOne(d => d.IdNewsNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdNews)
                    .HasConstraintName("R_19");
            });

            modelBuilder.Entity<LikesWithDislike>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.IdNewsNavigation)
                    .WithMany(p => p.LikesWithDislikes)
                    .HasForeignKey(d => d.IdNews)
                    .HasConstraintName("FK__LikesWith__UserI__412EB0B6");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessagesId)
                    .HasName("PK__Messages__F683BF1A578670B8");

                entity.Property(e => e.DateForumMessages).HasColumnType("datetime");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.Photo)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.Text).HasMaxLength(2000);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("R_20");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews)
                    .HasName("PK__News__4559C72D29069D75");

                entity.Property(e => e.DateNews).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("R_12");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TopicForum>(entity =>
            {
                entity.HasKey(e => e.TopicId)
                    .HasName("PK__TopicFor__022E0F5DEE73C149");

                entity.ToTable("TopicForum");

                entity.Property(e => e.TopicName).HasMaxLength(200);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TopicForums)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("R_7");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534324A7A83")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LastOnline).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.Property(e => e.Password).HasMaxLength(30);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnUrl).HasMaxLength(30);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("R_9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
