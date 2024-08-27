using AccountManagementPortal.Models;

namespace AccountManagementPortal.Repositories;

public interface IMainRepository{
        void RegisterCustomer();
        void RegisterBank();
        void EnableUPITransaction();
        string LoginCustomer(string loginOption);
        void ViewBalance();
        void DepositMoney();
        void WithdrawMoney();
        void ViewMiniStatement();
        void UpdatePin();
        void ChangeUPIId();
        void TransferMoney();
        void ViewMostActiveCustomer();
        void ViewTodayTransactions();
        void ViewTopPerformers();
        void LogOut();
        void GetAccounts();

}