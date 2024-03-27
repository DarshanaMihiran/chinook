namespace Chinook.ClientModels
{
    public partial class MediaTypeClientModel
    {
        public MediaTypeClientModel()
        {
            Tracks = new HashSet<TrackClientModel>();
        }

        public long MediaTypeId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TrackClientModel> Tracks { get; set; }
    }
}
