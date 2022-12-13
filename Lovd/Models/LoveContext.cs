using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AngleSharp.Common;

namespace Lovd.Models
{
    public partial class LoveContext : IdentityDbContext<IdentityUser>
    {
        public LoveContext()
        {
        }

        public LoveContext(DbContextOptions<LoveContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Article> Articles { get; set; } = null!;
      
        public virtual DbSet<Bait> Baits { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<KindOfFish> KindOfFishes { get; set; } = null!;
        public virtual DbSet<LikesWithDislike> LikesWithDislikes { get; set; } = null!;
        public virtual DbSet<Lure> Lures { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Pond> Ponds { get; set; } = null!;
        public virtual DbSet<TopicForum> TopicForums { get; set; } = null!;

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
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticle)
                    .HasName("PK__News__4559C72D29069D75");

                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.DateNews).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UserId).HasMaxLength(450);

               ;
            });


            modelBuilder.Entity<Bait>(entity =>
            {
                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComments)
                    .HasName("PK__Comments__0B5B73B1D6C9A202");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.Text).HasMaxLength(1000);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdArticle)
                    .HasConstraintName("R_19");

               
            });

            modelBuilder.Entity<KindOfFish>(entity =>
            {
                entity.ToTable("KindOfFish");

                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<LikesWithDislike>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.LikesWithDislikes)
                    .HasForeignKey(d => d.IdArticle)
                    .HasConstraintName("FK__LikesWith__UserI__412EB0B6");
            });

            modelBuilder.Entity<Lure>(entity =>
            {
                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessagesId)
                    .HasName("PK__Messages__F683BF1A578670B8");

                entity.Property(e => e.DateForumMessages).HasColumnType("datetime");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.Text).HasMaxLength(2000);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("R_20");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.IdNews);

                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.DateNews).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UserId).HasMaxLength(450);

            });

            modelBuilder.Entity<Pond>(entity =>
            {
                entity.Property(e => e.Announce).HasMaxLength(1000);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IdUser).HasMaxLength(450);

                entity.Property(e => e.Title).HasMaxLength(50);

        
            });

            modelBuilder.Entity<TopicForum>(entity =>
            {
                entity.HasKey(e => e.TopicId)
                    .HasName("PK__TopicFor__022E0F5DEE73C149");

                entity.ToTable("TopicForum");

                entity.Property(e => e.TopicName).HasMaxLength(200);

                entity.Property(e => e.UserId).HasMaxLength(450);

          
            });

             

            base.OnModelCreating(modelBuilder);
        }

  
    }
}
