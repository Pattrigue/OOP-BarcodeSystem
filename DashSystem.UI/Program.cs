using DashSystem.Controller;
using DashSystem.Core;

namespace DashSystem.UI
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            IDashSystemController dashSystemController = new DashSystemController();
            DashSystemUI ui = new DashSystemUI(dashSystemController);
            
            ui.Start();
        }
    }
}