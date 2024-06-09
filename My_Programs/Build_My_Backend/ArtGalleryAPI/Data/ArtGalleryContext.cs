using Microsoft.EntityFrameworkCore;
using ArtGalleryAPI.Models;

namespace ArtGalleryAPI.Data
{
    public class ArtGalleryContext : DbContext
    {
        public ArtGalleryContext(DbContextOptions<ArtGalleryContext> options) : base(options) { }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
    }
}
