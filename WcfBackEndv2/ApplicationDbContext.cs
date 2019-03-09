namespace WcfBackEndv2
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<ServiceCase> ServiceCases { get; set; }

        public ApplicationDbContext() : base("name=ApplicationDbContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Entity<ServiceCase>()
                .HasMany(c => c.Posts)
                .WithOptional()
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}