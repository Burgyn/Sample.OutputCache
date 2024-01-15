public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal AmountInStock { get; set; }

    public bool IsDiscontinued { get; set; }

    public DateTime LastModified { get; set; }

    public DateTime Now => DateTime.Now;

    public int TenantId { get; set; }
}
