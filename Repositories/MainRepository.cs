using AccountManagementPortal.Models;

namespace AccountManagementPortal.Repositories
{
    public class MainRepository : IMainRepository
    {
        private readonly CustomerRepository _customerRepository;
        private readonly AccountsRepository _accountsRepository;
        private readonly BankRepository _bankRepository;

        private string? InstitutionId = "";
        public MainRepository(CustomerRepository customerRepository, AccountsRepository accountsRepository, BankRepository bankRepository)
        {
            _customerRepository = customerRepository;
            _accountsRepository = accountsRepository;
            _bankRepository = bankRepository;
        }

        public void RegisterCustomer()
        {
            try
            {
                Console.Write("Enter your Full Name: ");
                string? fullName = Console.ReadLine();
                Console.Write("Enter your Date of Birth (yyyy-mm-dd): ");
                DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter your Contact Number: ");
                string? contactNumber = Console.ReadLine();
                Console.Write("Enter your Email: ");
                string? email = Console.ReadLine();
                Console.Write("Enter your Address: ");
                string? address = Console.ReadLine();

                CustomerRepository customer = new CustomerRepository()
                {
                    CustomerId = $"CUST{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    FullName = fullName,
                    DateOfBirth = dateOfBirth,
                    ContactNumber = contactNumber,
                    Email = email,
                    Address = address,
                    InstitutionId = _bankRepository.Id,
                    AccountNumber = _accountsRepository.GenerateAccountOrCardNumber("Account")
                };

                string result = _customerRepository.RegisterUser(customer);
                LogRepository.LogInfo(result);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void RegisterBank(){

            Console.Write("Enter the type of institution (e.g., Bank, Credit Union): ");
            string Type = Console.ReadLine();
            Console.Write("Enter the institution name: ");
            string Name = Console.ReadLine();
            Console.Write("Enter the address: ");
            string Address = Console.ReadLine();
            Console.Write("Enter the contact number: ");
            string ContactNumber = Console.ReadLine();
            List<string> ServicesOffered = new List<string>();
            Console.WriteLine("Enter the services offered (type 'done' when finished):");
            string? service;
            while (true)
            {
                service = Console.ReadLine();
                if (service?.ToLower() == "done")
                    break;

                if (!string.IsNullOrEmpty(service))
                    ServicesOffered.Add(service);
            }
            
            string id = $"{Name.Substring(0,4)}{Guid.NewGuid().ToString().ToUpper().Substring(0, 4)}";
            var financialInstitutions = new BankRepository{
                InstitutionId = id,
                Type = Type,
                Name = Name,
                Address = Address,
                ContactNumber = ContactNumber,
                ServicesOffered = ServicesOffered,
                Password = id
            };

            var result = _bankRepository.RegisterFinancialInstitution(financialInstitutions);
            Console.WriteLine(result);
        }
        public void EnableUPITransaction()
        { 
            try
            {
                Console.Write("Enter your Account Number: ");
                string? accountNumber = Console.ReadLine();
                var account = _accountsRepository.CheckAccountDetails(accountNumber);
                if(account != null)
                {
                if(string.IsNullOrEmpty(account.UPI_Id))
                {
                Console.Write("Enter your Customer ID: ");
                string? customerId = Console.ReadLine();
                Console.Write("Enter your Contact Number: ");
                string? contactNumber = Console.ReadLine();
                var customer = _customerRepository.IsAccountExists(accountNumber,contactNumber,customerId);
                if(customer != null ){
                Console.Write("Enter your new UPI Pin: ");
                string? newUpiPin = Console.ReadLine();
                account.UPI_Id = _accountsRepository.GenerateUpiId(customer.FullName,customerId,account.InstitutionId);
                account.Pin = newUpiPin;
                Console.WriteLine("UPI transaction enabled successfully.");
                Console.WriteLine($"Your UPI Id : {account.UPI_Id} and Pin : {account.Pin}");
                }
                else Console.WriteLine("Invalid Customer Id or Contact number");
                }
                else Console.WriteLine("UPI_Id already exists for this account");
                }
                else Console.WriteLine("Provide valid Account Number");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enabling UPI transaction: {ex.Message}");
            }
        }
        public string LoginCustomer(string loginOption)
        {
            if(loginOption.ToLower() == "a"){
                Console.Write("Enter Your Bank's Institution Id: ");
                string Id = Console.ReadLine();
                var bank =_bankRepository.IsRegiteredInstitution(Id);
                if(bank != null){
                    Console.Write("Enter Password: ");
                    string Password = Console.ReadLine();
                    if(bank.Password == Password)
                    {
                        InstitutionId = Id;
                        return "Bank";
                    }
                    else{
                        Console.WriteLine("Imvalid Password");
                        return "";
                    }
                }
                else{
                    Console.WriteLine("Institution with given Institution Id is not found.\nRegister to continue.");
                    return "";
                }
            }
            else if(loginOption.ToLower() =="b"){
            Console.Write("Enter Your UPI ID or Mobile Number: ");
            string Id = Console.ReadLine();
            var account = _accountsRepository.CheckUPIId(Id);
            if(account!=null){
                InstitutionId = account.InstitutionId;
                return "Customer";
            }
            else{
                Console.WriteLine("Invalid Id or Pin");
                return "";
            }
            }
            return "";
        }
        public void ViewBalance()
        {
            try
            {
                Console.Write("Enter your Pin: ");
                string? pin = Console.ReadLine();
                string result = _accountsRepository.ViewBalance(pin);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void DepositMoney()
        {
            try
            {
                Console.Write("Enter Your Account Number: ");
                string? number = Console.ReadLine();
                string result = _accountsRepository.DepositMoney(number);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void WithdrawMoney()
        {
             try
            {
                Console.Write("Enter your Account Number: ");
                string? accountNumber = Console.ReadLine();
                string result = _accountsRepository.WithdrawMoney(accountNumber);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void ViewMiniStatement()
        {
            try
            {
                Console.Write("Enter your Pin: ");
                string? Pin = Console.ReadLine();

                var transactions = _accountsRepository.MiniStatement(Pin);

                if (transactions is List<Transaction> transactionList)
                {
                    foreach (var transaction in transactionList)
                    {
                        Console.WriteLine($"{transaction.Date}: {transaction.Description} Amount: {transaction.Amount}");
                    }
                }
                else
                {
                    Console.WriteLine(transactions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void UpdatePin()
        {
            Console.Write("Enter your Old Pin: ");
            string? Pin = Console.ReadLine();
            var result = _accountsRepository.UpdatePin(Pin);
            Console.WriteLine(result);
        }
        public void ChangeUPIId()
        {
            Console.Write("Enter your Pin: ");
            string? Pin = Console.ReadLine();
            var result = _accountsRepository.ChangeUPIId(Pin);
            Console.WriteLine(result);
        }
        public void TransferMoney(){
            try
            {
                Console.Write("Enter UPI Id or Contact number of a recepient: ");
                string? UPI_Id = Console.ReadLine();
                string result = _accountsRepository.TransferMoney(UPI_Id);
                Console.WriteLine(result);
                // LogRepository.LogInfo(result);
            }
            catch (Exception ex)
            {
                // LogRepository.LogError(ex);
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void ViewMostActiveCustomer()
        {
            try
            {
                var activeCustomer = _accountsRepository.GetMostActiveCustomer(InstitutionId!);

                if (activeCustomer != null)
                {
                    Console.WriteLine($"Most Active Customer: {activeCustomer.First().Key}, Transactions: {activeCustomer.First().Value}");
                }
                else
                {
                    Console.WriteLine("No transactions found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void ViewTodayTransactions()
        {
            try
            {
                Console.Write("Enter your Pin: ");
                string? Pin = Console.ReadLine();

                var transactions = _accountsRepository.GetTodayTransactions(Pin);

                if (transactions != null && transactions.Count > 0)
                {
                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine($"{transaction.Date}: {transaction.Description}");
                    }
                }
                else
                {
                    Console.WriteLine("No transactions found for today.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void ViewTopPerformers()
        {
            _bankRepository.TopPerformers();
        }
        public void LogOut(){
            _accountsRepository.LogOut();
        }
        public void GetAccounts(){
            var account =_bankRepository.GetAccounts();
            if(account.Count > 0){
                account.ForEach(account => {
                    var customer = _customerRepository.GetCustomer(account.AccountNumber);
                    Console.WriteLine($"Account Number : {account.AccountNumber} - Customer Name : {customer.FullName} - Customer Id : {customer.CustomerId}");
                    Console.WriteLine($" Phone : {customer.ContactNumber} - Account Created On : {account.AccountCreationDate}");
                    });
            }
            else{
                Console.WriteLine("No accounts created till in this Bank.");
            }
        }
    }
}
