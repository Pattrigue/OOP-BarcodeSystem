using BarcodeSystem.Products;
using BarcodeSystem.Transactions;
using BarcodeSystem.Users;

namespace BarcodeSystem.UI
{
    public interface IBarcodeSystemUI
    {
        event BarcodeSystemEvent CommandEntered;
        void Start();
        void Close();
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(User user);
        void DisplayTooManyArgumentsError(string command);
        void DisplayNotEnoughArgumentsError(string command);
        void DisplayCommandNotFoundMessage(string command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count, BuyTransaction transaction);
        void DisplayInsufficientCash(User user, Product product);
        void DisplayMessage(string message);
        void DisplayError(string errorMessage);
    }
}