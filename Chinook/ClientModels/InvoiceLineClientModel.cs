namespace Chinook.ClientModels
{
    public partial class InvoiceLineClientModel
    {
        public long InvoiceLineId { get; set; }
        public long InvoiceId { get; set; }
        public long TrackId { get; set; }
        public byte[] UnitPrice { get; set; } = null!;
        public long Quantity { get; set; }

        public virtual InvoiceClientModel Invoice { get; set; } = null!;
        public virtual TrackClientModel Track { get; set; } = null!;
    }
}
