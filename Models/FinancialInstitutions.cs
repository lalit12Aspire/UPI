using AccountManagementPortal.Repositories;

namespace AccountManagementPortal.Models;
public abstract class FinancialInstitutions{

    public string? InstitutionId { get; set; }
    public string? Type {get;set;}
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? ContactNumber { get; set; }
    public List<string>? ServicesOffered {get ; set;}
    public string? Password {get;set;}
    public abstract string RegisterFinancialInstitution(BankRepository financialInstitutions);
    public abstract void TopPerformers();

}



