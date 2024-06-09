using ArtGalleryAPI.Models;
using ArtGalleryAPI.Repositories;

namespace ArtGalleryAPI.Services
{
    public class ArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _artistRepository.GetAllAsync();
        }

        public async Task<Artist> GetArtistByIdAsync(int id)
        {
            return await _artistRepository.GetByIdAsync(id);
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _artistRepository.AddAsync(artist);
        }

        public async Task UpdateArtistAsync(Artist artist)
        {
            await _artistRepository.UpdateAsync(artist);
        }

        public async Task DeleteArtistAsync(int id)
        {
            await _artistRepository.DeleteAsync(id);
        }
    }
}
