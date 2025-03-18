using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace FileExtensionUnInstaller
{
    public class UnregistryIcon
    {
        /// <summary>
        /// 存放圖標的註冊資料夾名稱
        /// </summary>
        private const string DefaultIcon = "ExtensionIcon";
        /// <summary>
        /// 取消註冊副檔名圖標
        /// </summary>
        public static void UnregisterFileExtension(string extension)
        {
            try
            {
                // 刪除HKEY_CLASSES_ROOT\.yourfileextension
                Registry.ClassesRoot.DeleteSubKeyTree("." + extension, false);

                // 刪除關聯的默認圖標項(如果存在)
                Registry.ClassesRoot.DeleteSubKeyTree(DefaultIcon, false);
                Registry.ClassesRoot.DeleteSubKeyTree(extension, false);

                // 更新文件關聯
                NativeMethods.SHChangeNotify(NativeMethods.SHCNE_ASSOCCHANGED, NativeMethods.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
                Console.WriteLine($".{extension} 取消註冊。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤：" + ex.Message);
            }
        }
    }

    internal class NativeMethods
    {
        public const uint SHCNE_ASSOCCHANGED = 0x08000000;
        public const uint SHCNF_IDLIST = 0x0000;

        [DllImport("Shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
