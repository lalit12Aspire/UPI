using AccountManagementPortal.Models;

namespace AccountManagementPortal.Data;
public class Transactions{
    public List<Transaction> TransactionList {get;set;} = new List<Transaction>
        {
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
        },
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
    };
}
