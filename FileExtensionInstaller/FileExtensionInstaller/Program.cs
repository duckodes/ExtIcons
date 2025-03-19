using System.Diagnostics;
using System.Security.Principal;

namespace FileExtensionInstaller
{
    /// <summary>
    /// 加入副檔名圖標
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 要註冊的副檔名名稱
        /// </summary>
        private const string ExtensionName = "filexins";
        /// <summary>
        /// 圖標檔案名稱
        /// </summary>
        private const string IconName = "filexins.ico";
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

            string directoryPath = AppDomain.CurrentDomain.BaseDirectory + "Resource\\" + IconName;
            CheckRegistryIcon(directoryPath);
            Console.WriteLine("按任意鍵關閉此視窗…");
            Console.ReadKey();
        }
        private static bool IsRunAsAdmin()
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private static void CheckRegistryIcon(string path)
        {
            bool isRegistered = RegistryIcon.IsFileExtensionRegistered(ExtensionName);

            if (isRegistered)
            {
                Console.WriteLine("." + ExtensionName + " 已經註冊。");
            }
            else
            {
                Console.WriteLine("." + ExtensionName + " 未註冊。");
                if (File.Exists(path))
                {
                    RegistryIcon.SetFileExtensionIcon(ExtensionName, path);
                }
                else
                {
                    Console.WriteLine("檔案路徑不存在：" + path);
                }
            }
        }
    }
}