using AccountManagementPortal.Data;
using AccountManagementPortal.Models;
using Microsoft.VisualBasic;

namespace AccountManagementPortal.Repositories;

public class BankRepository : FinancialInstitutions
{
    private readonly Banks banks;
    public string? Id {get;set;} = "";
    private readonly Accounts accounts;

    public BankRepository()
    {
        
    }
    public BankRepository(Banks banks, Accounts accounts)
    {
        this.banks = banks;
        this.accounts = accounts;
    }
    public BankRepository? IsRegiteredInstitution(string id)
    {
        var bank = banks.InstitutionList.Find(bank => bank.InstitutionId.ToLower() == id.ToLower());
        Id = bank?.InstitutionId ?? "";
        return bank;
    }
    public override string RegisterFinancialInstitution(BankRepository financialInstitutions)
    {
        if (IsRegiteredInstitution(financialInstitutions.InstitutionId) != null)
        {
            return "Financial Institution Id is already registered";
        }
        else
        {
            banks.InstitutionList.Add(financialInstitutions);
            return $"Your Financial institution has been registered successfully \n Your Institution Id : {financialInstitutions.InstitutionId} and Password : {financialInstitutions.Password}";
        }
    }
    public override void TopPerformers()
    {
        var topInstitutions = accounts.AccountList
            .GroupBy(account => account.InstitutionId)
            .Select(group => new
            {
                InstitutionId = group.Key,
                TransactionCount = group.Sum(account => account.Transactions?.Count ?? 0)
            })
            .OrderByDescending(g => g.TransactionCount)
            .Take(5)
            .ToList();

        var result = topInstitutions
            .Select(institution => new
            {
                Institution = banks.InstitutionList
                    .FirstOrDefault(bank => bank.InstitutionId == institution.InstitutionId),
                institution.TransactionCount
            })
            .Where(performer => performer.Institution != null)
            .ToList();

        result.ForEach(institution => 
            Console.WriteLine($"Institution: {institution.Institution.InstitutionId}-{institution.Institution.Name}, Transactions: {institution.TransactionCount} \n"+
            $"{institution.Institution.Address},{institution.Institution.ContactNumber}")
        );
    }
    public List<AccountsRepository> GetAccounts()
    {
        return accounts.AccountList.Where(account => account.InstitutionId == Id).ToList();
    }

}