namespace Chinook.ClientModels
{
    public partial class ArtistClientModel
    {
        public ArtistClientModel()
        {
            Albums = new HashSet<AlbumClientModel>();
        }

        public long ArtistId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<AlbumClientModel> Albums { get; set; }
    }
}
