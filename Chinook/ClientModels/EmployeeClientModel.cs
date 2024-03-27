namespace Chinook.ClientModels
{
    public partial class EmployeeClientModel
    {
        public EmployeeClientModel()
        {
            Customers = new HashSet<CustomerClientModel>();
            InverseReportsToNavigation = new HashSet<EmployeeClientModel>();
        }

        public long EmployeeId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Title { get; set; }
        public long? ReportsTo { get; set; }
        public byte[]? BirthDate { get; set; }
        public byte[]? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }

        public virtual EmployeeClientModel? ReportsToNavigation { get; set; }
        public virtual ICollection<CustomerClientModel> Customers { get; set; }
        public virtual ICollection<EmployeeClientModel> InverseReportsToNavigation { get; set; }
    }
}
