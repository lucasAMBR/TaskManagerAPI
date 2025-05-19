using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Equip> Equips { get; set; }
        public DbSet<Dev> Devs { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<InviteCode> InviteCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para relacionamentos
            modelBuilder.Entity<Equip>()
                .HasOne(e => e.Leader)
                .WithMany()
                .HasForeignKey(e => e.LeaderId);

            modelBuilder.Entity<Equip>()
                .HasOne(e => e.Project)
                .WithMany(p => p.Equips)
                .HasForeignKey(e => e.ProjectId);

            modelBuilder.Entity<Equip>()
                .HasMany(e => e.Members)
                .WithMany(d => d.Equips)
                .UsingEntity(j => j.ToTable("EquipDev"));

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Assignee)
                .WithMany(d => d.Tasks)
                .HasForeignKey(t => t.AssigneeId);
        }

    }
}
