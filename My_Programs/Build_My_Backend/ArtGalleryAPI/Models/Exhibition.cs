namespace ArtGalleryAPI.Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
    }
}
