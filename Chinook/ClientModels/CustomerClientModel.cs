namespace Chinook.ClientModels
{
    public partial class CustomerClientModel
    {
        public CustomerClientModel()
        {
            Invoices = new HashSet<InvoiceClientModel>();
        }

        public long CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Company { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string Email { get; set; } = null!;
        public long? SupportRepId { get; set; }

        public virtual EmployeeClientModel? SupportRep { get; set; }
        public virtual ICollection<InvoiceClientModel> Invoices { get; set; }
    }
}
