namespace AccountManagementPortal.Models;
public abstract class Account{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public DateTime AccountCreationDate { get; set; }
    public string? CardNumber{get;set;}
    public string? UPI_Id {get;set;}
    public string? Pin { get; set; }
    public string? InstitutionId {get;set;}
    public List<Transaction>? Transactions { get; set;}
    public abstract object MiniStatement(string Pin);     
    public abstract string ViewBalance(string Pin);
    public abstract Dictionary<string,int>? GetMostActiveCustomer(string id);
    public abstract List<Transaction>? GetTodayTransactions(string Pin);

}