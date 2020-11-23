using BarcodeSystem.Controller;
using BarcodeSystem.Core;

namespace BarcodeSystem.UI
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            IBarcodeSystemController barcodeSystemController = new BarcodeSystemController();
            IBarcodeSystemUI ui = new BarcodeSystemUI(barcodeSystemController);
            
            ui.Start();
        }
    }
}