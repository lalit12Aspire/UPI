using AccountManagementPortal.Models;
using AccountManagementPortal.Repositories;

namespace AccountManagementPortal.Data;

public class Accounts
{
    public List<AccountsRepository> AccountList { get; set; } = new List<AccountsRepository>
        {
            new ()
            {
                AccountNumber = "987654321098",
                Balance = 7500,
                AccountCreationDate = new DateTime(2021, 11, 20),
                UPI_Id = "john.doe@bank123",
                InstitutionId = "IDIB0001",
                Pin = "5678",
                Transactions = new List<Transaction>{
                                new ()
                                {
                                    TransactionId = "TXN004",
                                    Amount = 500,
                                    Description = "Grocery Purchase",
                                    Date = new DateTime(2023, 8, 3),
                                    AccountNumber = "987654321098"
                                },
                                new ()
                                {
                                    TransactionId = "TXN005",
                                    Amount = 2000,
                                    Description = "Rent Payment",
                                    Date = new DateTime(2023, 8, 10),
                                    AccountNumber = "987654321098"
                                }
                                }
            },
            new ()
            {
                AccountNumber = "123456789012",
                Balance = 15000,
                AccountCreationDate = new DateTime(2022, 1, 15),
                UPI_Id = "jane.smith@bank123",
                InstitutionId = "IDIB0001",
                Pin = "1234",
                Transactions = new List<Transaction>{
                                new ()
                                {
                                    TransactionId = "TXN001",
                                    Amount = 5000,
                                    Description = "Salary Deposit",
                                    Date = new DateTime(2024, 7, 5),
                                    AccountNumber = "123456789012"
                                },
                                new ()
                                {
                                    TransactionId = "TXN002",
                                    Amount = 1500,
                                    Description = "ATM Withdrawal",
                                    Date = new DateTime(2024, 5, 30),
                                    AccountNumber = "123456789012"
                                },
                                new ()
                                {
                                    TransactionId = "TXN003",
                                    Amount = 200,
                                    Description = "Online Shopping",
                                    Date = new DateTime(2023, 7, 15),
                                    AccountNumber = "123456789012"
                                }
                                }
            }
        };
}