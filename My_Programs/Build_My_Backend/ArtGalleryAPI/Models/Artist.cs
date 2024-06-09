namespace ArtGalleryAPI.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
    }
}
