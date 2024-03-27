namespace Chinook.ClientModels
{
    public partial class GenreClientModel
    {
        public GenreClientModel()
        {
            Tracks = new HashSet<TrackClientModel>();
        }

        public long GenreId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TrackClientModel> Tracks { get; set; }
    }
}
