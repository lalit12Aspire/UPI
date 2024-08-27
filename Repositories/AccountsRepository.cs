using AccountManagementPortal.Data;
using AccountManagementPortal.Models;
using System.Linq;

namespace AccountManagementPortal.Repositories
{
    public class AccountsRepository : Account
    {
        private const string WrongPin = "Please enter the correct Account number with a valid PIN";

        private readonly Accounts accounts;
        private readonly Customers customers;
        public string? UPI {get;set;}= "";

        public AccountsRepository()
        {
            
        }

        public AccountsRepository(Accounts accounts, Customers customers)
        {
            this.accounts = accounts;
            this.customers = customers;
        }

        public string GenerateAccountOrCardNumber(string type)
        {
            int length = 0;
            if(type == "Account")
            {length = 12;}
            else if(type == "Card")
            {length = 16;}
            Random random = new Random();
            string number = "";

            for (int i = 0; i < length; i++)
            {
                number += random.Next(0, 10).ToString();
            }

            if(accounts.AccountList.Find(account => account.AccountNumber == number) == null)
            return number;
            return GenerateAccountOrCardNumber(type);
        }
        public Account? CheckAccountDetails(string AccountNumber)
        {
            var account = accounts.AccountList?.Find(account => account.AccountNumber == AccountNumber);
            return account;
        }
        public Account? CheckPin(string Pin)
        {
            var account = accounts.AccountList?.Find(account => account.UPI_Id == UPI && account.Pin == Pin);
            return account;
        }
        public Account? GetAccount(string id){
            var customer = customers.CustomerList.Find(cust => cust.ContactNumber == id);
            var account = accounts.AccountList?.Find(acc => acc.UPI_Id == id || acc.AccountNumber == customer?.AccountNumber);
            return account;
        }
        public Account? CheckUPIId(string id)
        {
            var account = GetAccount(id);

            if (account!=null && string.IsNullOrEmpty(account.UPI_Id))
            {
                Console.WriteLine("Your account is not registered for UPI transactions.");
                return null;
            }

            else if (account == null)
            {
                Console.WriteLine("Invalid UPI ID or Mobile Number.");
                return null;
            }

            else{
            Console.Write("Enter your Pin: ");
            string pin = Console.ReadLine();
            
            if (account.Pin == pin)
            {
                UPI = account.UPI_Id;  
                return account;
            }
            else
            {
                Console.WriteLine("Invalid Pin.");
                return null;
            }
            }
        }
        public string DepositMoney(string accountNumber)
        {
            Account? account = CheckAccountDetails(accountNumber);

            if (account != null )
            {
                Console.Write("Enter the amount to deposit: ");
                decimal Amount = decimal.Parse(Console.ReadLine());
                account.Balance += Amount;
                account.Transactions ??= new List<Transaction>();
                account.Transactions?.Add(new Transaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    Amount = Amount,
                    Description = $"Rs.{Amount} deposited to {account.AccountNumber} on {DateTime.Now}",
                    Date = DateTime.Now,
                    AccountNumber = account.AccountNumber
                });
                return $"Amount {Amount} has been deposited to your Account\n Balance : {ViewBalance(account.Pin)}";
            }
            return WrongPin;
        }
        public string WithdrawMoney(string AccountNumber)
        {
            Account? account = CheckAccountDetails(AccountNumber);

            if (account != null)
            {
                Console.Write("Enter the amount to withdraw: ");
                decimal Amount = decimal.Parse(Console.ReadLine());
                if (account.Balance >= Amount)
                {
                    account.Balance -= Amount;
                    account.Transactions ??= new List<Transaction>();
                    account.Transactions?.Add(new Transaction
                    {
                        TransactionId = Guid.NewGuid().ToString(),
                        Amount = Amount,
                        Description = $"Rs.{Amount} withdrawn from {account.AccountNumber} on {DateTime.Now}",
                        Date = DateTime.Now,
                        AccountNumber = account.AccountNumber
                    });
                    return $"Amount {Amount} has been withdrawn from your Account \n Balance : {ViewBalance(account.Pin)}";
                }
                return $"Insufficient Balance : {ViewBalance(account.Pin)}";
            }
            return WrongPin;
        }
        public override object MiniStatement(string Pin)
        {
            Account? account = CheckPin(Pin);

            if (account != null)
            {
                Console.Write("Enter the start date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime FromDate) || FromDate > DateTime.Now)
                {
                    Console.WriteLine("Please enter a valid start date (must be on or before today).");
                    return null;
                }

                Console.Write("Enter the end date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime ToDate) || ToDate > DateTime.Now)
                {
                    Console.WriteLine("Please enter a valid end date (must be on or before today).");
                    return null;
                }

                if (FromDate > ToDate)
                {
                    Console.WriteLine("From Date should be before To Date.");
                    return null;
                }
                
                var transactionList = account.Transactions
                    .Where(transaction => transaction.Date.Date >= FromDate.Date && transaction.Date.Date <= ToDate.Date)
                    .ToList();

                if (transactionList.Count == 0)
                {
                    Console.WriteLine("No transactions found in the specified date range.");
                    return new List<Transaction>();
                }

                return transactionList;
            }

            return "Wrong Pin";
        }
        public override string ViewBalance(string Pin)
        {
            Account? account = CheckPin(Pin);

            if (account != null)
            {
                return $"Your Account {account.AccountNumber} Balance is {account.Balance}";
            }
            return WrongPin;
        }
        public override Dictionary<string,int>? GetMostActiveCustomer(string id)
        {
            var account = accounts.AccountList.Where(account => account.InstitutionId == id)
                .Select(account => new {
                    account.AccountNumber , 
                    TransactionCount = account.Transactions.Count()
                    })
                .OrderByDescending(account => account.TransactionCount)
                .FirstOrDefault();

            if (account != null)
            {
                var customer = customers.CustomerList?
                    .FirstOrDefault(customer => customer.AccountNumber == account.AccountNumber);
                return new Dictionary<string, int>
                    {
                        { customer.CustomerId, account.TransactionCount }
                    };
            }
            return null;
        }
        public override List<Transaction>? GetTodayTransactions(string Pin)
        {
            Account? account = CheckPin(Pin);

            if (account != null)
            {
                var todayTransactions = account.Transactions?
                .Where(transaction => transaction.Date.Date == DateTime.Today)
                .ToList();

             return todayTransactions ?? new List<Transaction>();
            }

            return null;
        }
        public string UpdatePin(string Pin){
            Account? account = CheckPin(Pin);

                if (account != null)
                {
                    Console.Write("Enter your New Pin: ");
                    string? newPin = Console.ReadLine();
                    account.Pin = newPin;
                    return "PIN updated successfully.";
                }
                else{
                return "Try again with valid Account and PIN";
                }
        }
        public string ChangeUPIId(string Pin){
            Account? account = CheckPin(Pin);

                if (account != null)
                {
                    Console.Write("Enter New UPI Id: ");
                    string? Id = Console.ReadLine();
                    account.UPI_Id = Id;
                    UPI = Id;
                    return $"UPI Id updated successfully. Your new UPI Id : {account.UPI_Id}";
                }
                else{
                return "Try again with valid Account and PIN";
                }
        }     
        public string TransferMoney(string UPI_Id)
        {
            Account? ToAccount = GetAccount(UPI_Id);
            Account? FromAccount = CheckUPIId(UPI);
            if(ToAccount.AccountNumber != FromAccount.AccountNumber)
            {
            if (ToAccount == null)
            {
                return "Recipient UPI Id not found";
            }

            if (ToAccount != null && FromAccount != null)
            {
                try
                {
                    Console.Write("Enter the amount to transfer: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal Amount) || Amount <= 0)
                    {
                        return "Invalid amount entered.";
                    }

                    if (FromAccount.Balance < Amount)
                    {
                        return $"Insufficient Balance: {ViewBalance(FromAccount.Pin)}";
                    }

                    FromAccount.Balance -= Amount;
                    ToAccount.Balance += Amount;

                    FromAccount.Transactions ??= new List<Transaction>();
                    ToAccount.Transactions ??= new List<Transaction>();

                    string transactionId = Guid.NewGuid().ToString().Substring(0, 15);

                    FromAccount.Transactions.Add(new Transaction
                    {
                        TransactionId = transactionId,
                        Amount = Amount,
                        Description = $"Ac/No {FromAccount.AccountNumber} debited Rs.{Amount} on {DateTime.Now} to {ToAccount.UPI_Id}",
                        Date = DateTime.Now,
                        AccountNumber = FromAccount.AccountNumber
                    });

                    ToAccount.Transactions.Add(new Transaction
                    {
                        TransactionId = transactionId,
                        Amount = Amount,
                        Description = $"Ac/No {ToAccount.AccountNumber} credited Rs.{Amount} on {DateTime.Now} from {FromAccount.UPI_Id}",
                        Date = DateTime.Now,
                        AccountNumber = ToAccount.AccountNumber
                    });

                    return $"Your amount Rs.{Amount} has been transferred to Ac/No : {ToAccount.AccountNumber}. \nTransaction Id is {transactionId} \nYour account balance is {ViewBalance(FromAccount.Pin)}";
                }
                catch (Exception ex)
                {
                    return $"An error occurred during the transaction: {ex.Message}";
                }
            }

            return WrongPin;
            }
            return "From and to UPIs can't be same";
        }
        public string GenerateUpiId(string fullName,string customerId,string institutionId)
        {
            string UPI_Id = $"{fullName?.Replace(" ", "").ToLower()}.{customerId.Substring(4,8).ToLower()}@{institutionId.Substring(0,4).ToLower()}upi";
            return UPI_Id;
        }
        public void LogOut(){
            UPI = "";
            Console.WriteLine("Logged out");
        }
    
    }
}
