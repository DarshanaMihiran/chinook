namespace Chinook.ClientModels
{
    public partial class TrackClientModel
    {
        public TrackClientModel()
        {
            InvoiceLines = new HashSet<InvoiceLineClientModel>();
            Playlists = new HashSet<Playlist>();
        }

        public long TrackId { get; set; }
        public string Name { get; set; } = null!;
        public long? AlbumId { get; set; }
        public long MediaTypeId { get; set; }
        public long? GenreId { get; set; }
        public string? Composer { get; set; }
        public long Milliseconds { get; set; }
        public long? Bytes { get; set; }
        public byte[] UnitPrice { get; set; } = null!;

        public virtual AlbumClientModel? Album { get; set; }
        public virtual GenreClientModel? Genre { get; set; }
        public virtual MediaTypeClientModel MediaType { get; set; } = null!;
        public virtual ICollection<InvoiceLineClientModel> InvoiceLines { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
