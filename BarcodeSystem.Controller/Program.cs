using BarcodeSystem.Core;
using BarcodeSystem.UI;

namespace BarcodeSystem.Controller
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            IBarcodeSystemManager systemManager = new BarcodeSystemManager();
            IBarcodeSystemUI systemUI = new BarcodeSystemUI(systemManager);
            BarcodeSystemController systemController = new BarcodeSystemController(systemManager, systemUI);
            
            systemUI.Start();
        }
    }
}