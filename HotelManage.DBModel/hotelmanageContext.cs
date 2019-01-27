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

        public virtual DbSet<Baiduapilog> Baiduapilog { get; set; }
        public virtual DbSet<Guest> Guest { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Hotelenum> Hotelenum { get; set; }
        public virtual DbSet<Hotelmanager> Hotelmanager { get; set; }
        public virtual DbSet<Operationlog> Operationlog { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Roomcheck> Roomcheck { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql("server=127.0.0.1;userid=root;pwd=ws1234!!;port=3306;database=hotelmanage;treattinyasboolean=true;SslMode=None;AllowPublicKeyRetrieval=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Baiduapilog>(entity =>
            {
                entity.ToTable("baiduapilog");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.HotelId)
                    .HasColumnName("hotelId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Type).HasColumnType("int(11)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.ToTable("guest");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CertId).HasColumnType("varchar(45)");

                entity.Property(e => e.CertType).HasColumnType("varchar(45)");

                entity.Property(e => e.CheckId).HasColumnType("bigint(20)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Mobile).HasColumnType("varchar(45)");

                entity.Property(e => e.Name).HasColumnType("varchar(45)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

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

                entity.Property(e => e.Role).HasColumnType("int(4)");

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

                entity.Property(e => e.IsDel)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.NewValue).HasColumnType("varchar(200)");

                entity.Property(e => e.OldValue).HasColumnType("varchar(200)");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

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

            modelBuilder.Entity<Roomcheck>(entity =>
            {
                entity.ToTable("roomcheck");

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

                entity.Property(e => e.PlanedCheckinTime).HasColumnType("datetime");

                entity.Property(e => e.PlanedCheckoutTime).HasColumnType("datetime");

                entity.Property(e => e.Prices).HasColumnType("decimal(8,2)");

                entity.Property(e => e.Remark).HasColumnType("varchar(100)");

                entity.Property(e => e.ReserveTime).HasColumnType("datetime");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status).HasColumnType("int(4)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });
        }
    }
}
