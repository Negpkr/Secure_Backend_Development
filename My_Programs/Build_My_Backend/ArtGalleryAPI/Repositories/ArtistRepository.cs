using ArtGalleryAPI.Data;
using ArtGalleryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtGalleryAPI.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ArtGalleryContext _context;

        public ArtistRepository(ArtGalleryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists.Include(a => a.Artifacts).ToListAsync();
        }

        public async Task<Artist> GetByIdAsync(int id)
        {
            return await _context.Artists.Include(a => a.Artifacts).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
