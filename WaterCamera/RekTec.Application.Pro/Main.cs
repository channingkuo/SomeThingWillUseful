using System;
using UIKit;
using RekTec.Corelib.Utils;

namespace RekTec.Chat.App
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception ex)
            {
                LoggingUtil.Exception(ex);
            }
        }
    }
}
