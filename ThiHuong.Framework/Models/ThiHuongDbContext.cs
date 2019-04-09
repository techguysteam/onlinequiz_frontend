using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ThiHuong.Framework.Models
{
    public partial class ThiHuongDbContext : DbContext
    {
        public ThiHuongDbContext()
        {
        }

        public ThiHuongDbContext(DbContextOptions<ThiHuongDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountInStage> AccountInStage { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<ExamDetail> ExamDetail { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<ResultDetail> ResultDetail { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Stage> Stage { get; set; }

        
    }
}
