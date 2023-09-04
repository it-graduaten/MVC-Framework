using _09_01_InterimKantoor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Numerics;

namespace _09_01_InterimKantoor.Data
{
    public class InterimKantoorContext : IdentityDbContext<CustomUser>
    {
        public InterimKantoorContext(DbContextOptions<InterimKantoorContext> options) : base(options)
        {

        }

        public DbSet<Klant> Klanten { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<KlantJob> KlantJobs { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<KlantJob>().ToTable("KlantJob");

            modelBuilder.Entity<KlantJob>()
                .HasOne(p => p.Klant)
                .WithMany(x => x.klantjobs)
                .HasForeignKey(y => y.KlantId)
                .IsRequired();

            modelBuilder.Entity<KlantJob>()
                .HasOne(p => p.Job)
                .WithMany(x => x.KlantJobs)
                .HasForeignKey(y => y.JobId)
                .IsRequired();
        }
    }
}
