using AccountManagementPortal.Repositories;

namespace AccountManagementPortal.Models;

public abstract class Customer
{
    public string? CustomerId { get; set; } 
    public string? FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? InstitutionId {get;set;}
    public string? AccountNumber { get; set; }
    public abstract CustomerRepository GetCustomer(string AccountNumber);
    public abstract string RegisterUser(CustomerRepository customer);
}