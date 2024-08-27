using AccountManagementPortal.Repositories;

namespace AccountManagementPortal.Data;

public class Banks{
    public List<BankRepository> InstitutionList {get;set;} = new List<BankRepository>
        {
            new() {
                InstitutionId = "IDIB0001",
                Type = "Commercial Bank",
                Name = "ABC Bank",
                Address = "123 Main St, Cityville",
                ContactNumber = "123-456-7890",
                ServicesOffered = new List<string> { "Savings Accounts", "Loans", "Credit Cards" },
                Password = "IDIB0001"
            },
            new()
            {
                InstitutionId = "IDIB0002",
                Type = "Credit Union",
                Name = "XYZ Credit Union",
                Address = "789 Maple St, Townsville",
                ContactNumber = "9876543210",
                ServicesOffered = new List<string> { "Checking Accounts", "Mortgages", "Auto Loans" },
                Password = "IDIB0002"
            }
        };

}