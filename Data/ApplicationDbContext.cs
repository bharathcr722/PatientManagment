using Microsoft.EntityFrameworkCore;
using PatientManagment.Models;

namespace PatientManagment.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<Admin> Admin { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Observation>()
                .HasOne(o => o.Patient)
                .WithMany(p => p.Observations)
                .HasForeignKey(o => o.PatientId);
        }
    }
}
