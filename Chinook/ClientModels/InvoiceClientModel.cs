namespace Chinook.ClientModels
{
    public partial class InvoiceClientModel
    {
        public InvoiceClientModel()
        {
            InvoiceLines = new HashSet<InvoiceLineClientModel>();
        }

        public long InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public byte[] InvoiceDate { get; set; } = null!;
        public string? BillingAddress { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingCountry { get; set; }
        public string? BillingPostalCode { get; set; }
        public byte[] Total { get; set; } = null!;

        public virtual CustomerClientModel Customer { get; set; } = null!;
        public virtual ICollection<InvoiceLineClientModel> InvoiceLines { get; set; }
    }
}
