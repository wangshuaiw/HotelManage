using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelManage.DBModel
{
    public partial class hotelmanageContext : DbContext
    {
        public hotelmanageContext()
        {
        }

        public hotelmanageContext(DbContextOptions<hotelmanageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Hotelenum> Hotelenum { get; set; }
        public virtual DbSet<Hotelmanager> Hotelmanager { get; set; }
        public virtual DbSet<Operationlog> Operationlog { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Roomhistorycheckin> Roomhistorycheckin { get; set; }
        public virtual DbSet<Roomorder> Roomorder { get; set; }
        public virtual DbSet<Roomstatus> Roomstatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=127.0.0.1;userid=root;pwd=;port=3306;database=hotelmanage;treattinyasboolean=true;sslmode=none;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("hotel");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address).HasColumnType("varchar(200)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.HotelPassword).HasColumnType("varchar(50)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Region).HasColumnType("varchar(50)");

                entity.Property(e => e.Remark).HasColumnType("varchar(100)");

                entity.Property(e => e.Salt).HasColumnType("varchar(50)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Hotelenum>(entity =>
            {
                entity.ToTable("hotelenum");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.EnumClass).HasColumnType("varchar(20)");

                entity.Property(e => e.FullKey)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Name).HasColumnType("varchar(20)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Hotelmanager>(entity =>
            {
                entity.ToTable("hotelmanager");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.HotelId)
                    .HasColumnName("HotelID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.WxOpenId)
                    .IsRequired()
                    .HasColumnName("WxOpenID")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.WxUnionId)
                    .HasColumnName("WxUnionID")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<Operationlog>(entity =>
            {
                entity.ToTable("operationlog");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(16)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.FieldName).HasColumnType("varchar(20)");

                entity.Property(e => e.ForeignKey)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.NewValue).HasColumnType("varchar(200)");

                entity.Property(e => e.OldValue).HasColumnType("varchar(200)");

                entity.Property(e => e.Type).HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("room");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.HotelId)
                    .HasColumnName("HotelID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Remark).HasColumnType("varchar(50)");

                entity.Property(e => e.RoomNo)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.RoomType).HasColumnType("varchar(20)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Roomhistorycheckin>(entity =>
            {
                entity.ToTable("roomhistorycheckin");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.OccupantCertType).HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantId)
                    .HasColumnName("OccupantID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantMobile).HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantName).HasColumnType("varchar(20)");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Roomorder>(entity =>
            {
                entity.ToTable("roomorder");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CheckinTime).HasColumnType("datetime");

                entity.Property(e => e.CheckoutTime).HasColumnType("datetime");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Deposit).HasColumnType("decimal(8,2)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.OccupantCertType).HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantId)
                    .HasColumnName("OccupantID")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantMobile).HasColumnType("varchar(20)");

                entity.Property(e => e.OccupantName).HasColumnType("varchar(20)");

                entity.Property(e => e.ReserveTime).HasColumnType("datetime");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Roomstatus>(entity =>
            {
                entity.ToTable("roomstatus");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Prices).HasColumnType("decimal(8,2)");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });
        }
    }
}
