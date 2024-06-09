namespace ArtGalleryAPI.Models
{
    public class Artifact
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public int ExhibitionId { get; set; }
        public Exhibition Exhibition { get; set; }
    }
}
