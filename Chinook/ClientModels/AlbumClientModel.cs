namespace Chinook.ClientModels
{
    public partial class AlbumClientModel
    {
        public AlbumClientModel()
        {
            Tracks = new HashSet<TrackClientModel>();
        }

        public long AlbumId { get; set; }
        public string Title { get; set; } = null!;
        public long ArtistId { get; set; }

        public virtual ArtistClientModel Artist { get; set; } = null!;
        public virtual ICollection<TrackClientModel> Tracks { get; set; }
    }
}
