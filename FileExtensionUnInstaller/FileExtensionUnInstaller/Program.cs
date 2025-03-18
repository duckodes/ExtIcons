using System.Diagnostics;
using System.Security.Principal;

namespace FileExtensionUnInstaller
{
    internal class Program
    {
        /// <summary>
        /// 移除註冊的副檔名名稱
        /// </summary>
        private const string ExtensionName = "filexins";
        static void Main(string[] args)
        {
            if (!IsRunAsAdmin())
            {
                var procInfo = new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                try
                {
                    Process.Start(procInfo);
                }
                catch
                {
                    Console.WriteLine("無法以管理員權限運行！");
                }
                return;
            }
            Console.WriteLine("程序已以管理員權限運行！");

            Unregistry();
            Console.WriteLine("按任意鍵關閉此視窗…");
            Console.ReadKey();
        }
        private static bool IsRunAsAdmin()
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private static void Unregistry()
        {
            UnregistryIcon.UnregisterFileExtension(ExtensionName);
        }
    }
}