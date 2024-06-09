using ArtGalleryAPI.Models;
using ArtGalleryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtGalleryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistsController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var artists = await _artistService.GetAllArtistsAsync();
            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            if (artist == null) return NotFound();
            return Ok(artist);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Artist artist)
        {
            await _artistService.AddArtistAsync(artist);
            return CreatedAtAction(nameof(GetById), new { id = artist.Id }, artist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Artist artist)
        {
            if (id != artist.Id) return BadRequest();
            await _artistService.UpdateArtistAsync(artist);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _artistService.DeleteArtistAsync(id);
            return NoContent();
        }
    }
}
