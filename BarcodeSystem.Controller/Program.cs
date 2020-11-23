using BarcodeSystem.Core;
using BarcodeSystem.UI;
using BarcodeSystem.UI.Commands;

namespace BarcodeSystem.Controller
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            IBarcodeSystemManager systemManager = new BarcodeSystemManagerManager();
            IBarcodeSystemUI ui = new BarcodeSystemUI(systemManager);
            BarcodeSystemController sc = new BarcodeSystemController(systemManager, ui);
            
            ui.Start();
        }
    }
}