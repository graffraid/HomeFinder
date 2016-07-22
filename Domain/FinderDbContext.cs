namespace Domain
{
    using System.Data.Entity;
    using Entities;

    public class FinderDbContext : DbContext
    {
        public FinderDbContext()
            : base("name=HomeFinderConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Advert> Adverts { get; set; }

        public DbSet<AdvertImage> AdvertImages { get; set; }

        public DbSet<Building> Buildings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertImage>()
                .HasRequired(img => img.Advert)
                .WithMany(adv => adv.AdvertImages)
                .HasForeignKey(img => img.AdvertId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Advert>()
                .HasRequired(adv => adv.Building)
                .WithMany(bld => bld.Adverts)
                .HasForeignKey(adv => adv.BuildingId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Advert>()
                .HasOptional(adv => adv.ChangedAdvert)
                .WithRequired(adv => adv.InitialAdvert)
                .WillCascadeOnDelete(false);
        }
    }
}