using Microsoft.Win32;
using System.Diagnostics;

namespace FileExtensionInstaller
{
    public class RegistryIcon
    {
        /// <summary>
        /// 存放圖標的註冊資料夾名稱
        /// </summary>
        private const string DefaultIcon = "ExtensionIcon";
        /// <summary>
        /// 設置副檔名圖標
        /// </summary>
        public static void SetFileExtensionIcon(string extension, string iconPath)
        {
            try
            {
                // 創建或打開HKEY_CLASSES_ROOT\.extension
                using (RegistryKey fileExtKey = Registry.ClassesRoot.CreateSubKey("." + extension))
                {
                    if (fileExtKey != null)
                    {
                        fileExtKey.SetValue(null, DefaultIcon);

                        using (RegistryKey iconKey = Registry.ClassesRoot.CreateSubKey("." + extension + "\\" + DefaultIcon))
                        {
                            if (iconKey != null)
                            {
                                iconKey.SetValue(null, iconPath);
                                Console.WriteLine($".{extension} 完成註冊。");
                            }
                            else
                            {
                                Console.WriteLine("無法創建或打開圖標註冊表項。");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("無法創建或打開文件擴展名註冊表項。");
                    }
                }

                // 更新文件關聯
                Process.Start("cmd.exe", $"/C assoc {extension}={DefaultIcon}");
                Process.Start("cmd.exe", $"/C ftype {DefaultIcon}=\"{iconPath}\"");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("發生錯誤：" + ex.Message);
            }
        }
        /// <summary>
        /// 檢查副檔名是否已註冊
        /// </summary>
        public static bool IsFileExtensionRegistered(string extension)
        {
            try
            {
                using RegistryKey fileExtKey = Registry.ClassesRoot.OpenSubKey("." + extension);
                return fileExtKey != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤：" + ex.Message);
                return false;
            }
        }
    }
}
