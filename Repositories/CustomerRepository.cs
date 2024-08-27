using AccountManagementPortal.Data;
using AccountManagementPortal.Models;

namespace AccountManagementPortal.Repositories;

public class CustomerRepository : Customer
{
    private readonly Customers customers;
    private readonly Accounts accounts;

    private readonly BankRepository bankRepository;

    public CustomerRepository()
    {
        
    }
    public CustomerRepository( Customers customers, Accounts accounts,BankRepository bankRepository)
    {
        this.customers = customers;
        this.accounts = accounts;
        this.bankRepository = bankRepository;
    }

    public override CustomerRepository GetCustomer(string AccountNumber){
        return customers.CustomerList.Find(customer => customer.AccountNumber == AccountNumber);
    }
    public bool IsRegisteredCustomer(string number,string bankId)
    {
        if (customers.CustomerList.Find(customer => customer.ContactNumber == number && customer.InstitutionId == bankId) != null)
        {
            return true;
        }
        return false;
    }
    public override string RegisterUser(CustomerRepository customer)
    {
        if (IsRegisteredCustomer(customer.ContactNumber,customer.InstitutionId))
        {
            return "Account Already Exist, Login to continue";
        }
        else if (bankRepository.IsRegiteredInstitution(customer.InstitutionId) == null)
        {
            return "Please provide valid Financial Institution Id";
        }
        else
        {
            customers.CustomerList.Add(customer);
            accounts.AccountList.Add(new AccountsRepository{
                AccountNumber = customer.AccountNumber,
                Balance = 500,
                AccountCreationDate = DateTime.Now,
                InstitutionId = customer.InstitutionId
            });
            return $"Registered successfully \nYour Details: Customer Id - {customer.CustomerId} \nAccount Number - {customer.AccountNumber} \nContact Number - {customer.ContactNumber}";
        }
    }
    public Customer IsAccountExists(string AccountNumber, string ContactNumber,string customerId)
    {
        var customer = customers.CustomerList.Find(account => account.AccountNumber == AccountNumber && account.ContactNumber == ContactNumber && account.CustomerId == customerId);
        return customer;
    }

}