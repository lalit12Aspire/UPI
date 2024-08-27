namespace AccountManagementPortal.Models;
public class Transaction 
{
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? AccountNumber { get; set; }

}