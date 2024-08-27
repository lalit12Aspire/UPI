using AccountManagementPortal.Data;
using AccountManagementPortal.Repositories;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AccountManagementPortal
{
    static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string InvalidOption = "Invalid option, please try again.";

        static void Main(string[] args)
        {
            // var config = new LoggingConfiguration();
            // var logfile = new FileTarget("file") { FileName = "../../../Logs/log.txt", Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}" };
            // var logconsole = new ConsoleTarget("console") { Layout = "${longdate} ${level} ${message}" };
            // config.AddTarget(logfile);
            // config.AddTarget(logconsole);
            // config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            // config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
            // LogManager.Configuration = config;

            LogManager.Setup().LoadConfigurationFromFile("../../../nlog.config");
            Logger.Info("Application started.");

            var customers = new Customers();
            var banks = new Banks();
            var accounts = new Accounts();
            var accountsRepository = new AccountsRepository(accounts, customers);
            var bankRepository = new BankRepository(banks, accounts);
            var customerRepository = new CustomerRepository(customers, accounts, bankRepository);

            var mainRepository = new MainRepository(customerRepository, accountsRepository, bankRepository);

            bool exit = false;
            bool isLoggedIn = false;
            bool isBank = false;
            bool isCustomer = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Account Management Portal ---");

                if (!isLoggedIn)
                {
                    Console.WriteLine("1. Register Institution");
                    Console.WriteLine("2. Enable UPI transaction");
                    Console.WriteLine("3. Login to continue");
                }

                if (isLoggedIn && isCustomer)
                {
                    Console.WriteLine("4. View Balance");
                    Console.WriteLine("5. Deposit Money");
                    Console.WriteLine("6. Withdraw Money");
                    Console.WriteLine("7. View Mini Statement");
                    Console.WriteLine("8. Change UPI Id");
                    Console.WriteLine("9. Update Pin");
                    Console.WriteLine("10. Transfer Money");
                    Console.WriteLine("11. View Today's Transactions");
                }

                if (isLoggedIn)
                {
                    Console.WriteLine("12. View Most Active Customer");
                    Console.WriteLine("13. View Top Institutions");
                    Console.WriteLine("14. LogOut");
                }

                if (isLoggedIn && isBank)
                {
                    Console.WriteLine("15. Create account");
                    Console.WriteLine("16. View Customers");
                }

                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                string? option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                        case "2":
                            if (!isLoggedIn)
                            {
                                if (option == "1") mainRepository.RegisterBank();
                                if (option == "2") mainRepository.EnableUPITransaction();
                            }
                            else
                            {
                                Logger.Warn("Invalid option selected while logged in.");
                                Console.WriteLine(InvalidOption);
                            }
                            break;

                        case "3":
                            if (!isLoggedIn)
                            {
                                Console.WriteLine("A. Login as Bank");
                                Console.WriteLine("B. Login to Customer account");
                                Console.Write("Select A or B : ");
                                string loginOption = Console.ReadLine();
                                string result = mainRepository.LoginCustomer(loginOption);
                                isLoggedIn = result != "";
                                isBank = result == "Bank";
                                isCustomer = result == "Customer";

                                if (isLoggedIn)
                                {
                                    Logger.Info($"{result} login successful.");
                                    Console.WriteLine("Login successful. Proceed with your actions.");
                                }
                                else
                                {
                                    Logger.Warn("Login failed.");
                                    Console.WriteLine("Invalid credentials. Please try again.");
                                }
                            }
                            else
                            {
                                Logger.Warn("Attempt to login while already logged in.");
                                Console.WriteLine("You are already logged in.");
                            }
                            break;

                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                            if (isLoggedIn && isCustomer)
                            {
                                switch (option)
                                {
                                    case "4":
                                        mainRepository.ViewBalance();
                                        break;
                                    case "5":
                                        mainRepository.DepositMoney();
                                        break;
                                    case "6":
                                        mainRepository.WithdrawMoney();
                                        break;
                                    case "7":
                                        mainRepository.ViewMiniStatement();
                                        break;
                                    case "8":
                                        mainRepository.ChangeUPIId();
                                        break;
                                    case "9":
                                        mainRepository.UpdatePin();
                                        break;
                                    case "10":
                                        mainRepository.TransferMoney();
                                        break;
                                    case "11":
                                        mainRepository.ViewTodayTransactions();
                                        break;
                                }
                                Logger.Info($"Customer action executed: Option {option}");
                            }
                            else
                            {
                                Logger.Warn("Invalid customer action attempted.");
                                Console.WriteLine(InvalidOption);
                            }
                            break;

                        case "12":
                        case "13":
                            if (isLoggedIn)
                            {
                                if (option == "12") mainRepository.ViewMostActiveCustomer();
                                if (option == "13") mainRepository.ViewTopPerformers();
                            }
                            else
                            {
                                Console.WriteLine("You must be logged in to perform this action.");
                            }
                            break;

                        case "14":
                            if (isLoggedIn)
                            {
                                mainRepository.LogOut();
                                isLoggedIn = false;
                                isBank = false;
                                isCustomer = false;
                                bankRepository.Id = "";
                                accountsRepository.UPI = "";
                                Logger.Info("User logged out successfully.");
                                Console.WriteLine("Logged out successfully.");
                            }
                            else
                            {
                                Logger.Warn("Attempt to log out while not logged in.");
                                Console.WriteLine(InvalidOption);
                            }
                            break;

                        case "15":
                        case "16":
                            if (isLoggedIn && isBank)
                            {
                                if (option == "15") mainRepository.RegisterCustomer();
                                if (option == "16") mainRepository.GetAccounts();
                            }
                            else
                            {
                                Logger.Warn("Invalid bank action attempted.");
                                Console.WriteLine(InvalidOption);
                            }
                            break;

                        case "0":
                            exit = true;
                            mainRepository.LogOut();
                            isLoggedIn = false;
                            Logger.Info("Application exited by user.");
                            Console.WriteLine("Exiting... Thank you!");
                            break;

                        default:
                            Logger.Warn("Invalid option selected.");
                            Console.WriteLine(InvalidOption);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "An error occurred while processing the option.");
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
