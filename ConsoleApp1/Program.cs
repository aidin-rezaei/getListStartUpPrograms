using System;
using Microsoft.Win32;
// using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
// using System.Diagnostics.Eventing.Reader;
class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add program to startup");
            Console.WriteLine("2. Remove program from startup");
            Console.WriteLine("3. List startup programs");
            Console.WriteLine("4. List Windows users");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddProgram();
                    break;
                case "2":
                    RemoveProgram();
                    break;
                case "3":
                    ListStartupPrograms();
                    break;
                case "4":
                    ListWindowsUsers();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void AddProgram()
    {
        Console.Write("Enter program name: ");
        string appName = Console.ReadLine();

        Console.Write("Enter program path: ");
        string appPath = Console.ReadLine();

        RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        key.SetValue(appName, appPath);
        Console.WriteLine("Program added to startup.");

        key.Close();
    }

    static void RemoveProgram()
    {
        Console.Write("Enter program name to remove: ");
        string appName = Console.ReadLine();

        RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (key.GetValue(appName) != null)
        {
            key.DeleteValue(appName);
            Console.WriteLine("Program removed from startup.");
        }
        else
        {
            Console.WriteLine("Program not found in startup.");
        }

        key.Close();
    }

    static void ListStartupPrograms()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

        string[] valueNames = key.GetValueNames();

        Console.WriteLine("List of startup programs:");
        foreach (string valueName in valueNames)
        {
            Console.WriteLine("    -"+valueName);
        }

        key.Close();
    }

    static void ListWindowsUsers()
    {
        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList");

        Console.WriteLine("List of Windows users:");
        foreach (string subKeyName in key.GetSubKeyNames())
        {
            RegistryKey subKey = key.OpenSubKey(subKeyName);
            string profileImagePath = subKey.GetValue("ProfileImagePath")?.ToString();
            if (!string.IsNullOrEmpty(profileImagePath))
            {
                int lastIndex = profileImagePath.LastIndexOf("\\");
                string username = profileImagePath.Substring(lastIndex + 1);
                Console.WriteLine("    -"+username);
            }
            subKey.Close();
        }

        key.Close();
    }


}

