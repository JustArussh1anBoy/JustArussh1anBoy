using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Newtonsoft.Json;
using Sys = Cosmos.System;

namespace KosmoConsole
{
    public class Kernel : Sys.Kernel
    {
        public string v = "0.1.1";
        public string username;
        public string uname;
        public string password;
        public Settings Settings = new Settings();
        public Explorer Explorer = new Explorer();
        public Internet Internet = new Internet();
        public Splash Splash = new Splash();
        public bool isNewBSoD = true;

        protected override void OnBoot()
        {
            base.OnBoot();
            Console.WriteLine("Starting up...");
        }

        public void InitFail(string fail, string reason)
        {
            Console.WriteLine($"Failed to initialize {fail}: {reason}");
            while (true)
            {
                Console.Write("[R]etry, [I]gnote, [F]ail? ");
                string a = Console.ReadLine()?.ToLower();
                if (a == "" || a.StartsWith("") || a == null)
                {
                    string t_fail = fail;
                    string t_reason = reason;
                    InitFail(t_fail, t_reason);
                }
                else if (a == "r" || a.StartsWith("r"))
                {
                    Initialize();
                    break;
                }
                else if (a == "i" || a.StartsWith("i"))
                {
                    break;
                }
                else if (a == "f" || a.StartsWith("f"))
                {
                    BSoD($"Failed to initialize {fail} - {reason}");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
        public void LogIn()
        {
            //username = "root";
            //uname = "KosmoConsole/Superuser";
            //password = "root";
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"User: ");
            string t_user = Console.ReadLine();
            if (t_user != username)
            {
                Console.WriteLine("Invalid user.");
                LogIn();
            }
            Console.Write($"Password: ");
            Console.ForegroundColor = ConsoleColor.Black;
            string t_pass = Console.ReadLine();
            if (t_pass == password)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Welcome back {username}!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Invalid password.");
                LogIn();
            }
        }

        private void Initialize()
        {
            Console.Write("Loading Kernel32... ");
            try
            {
                Kernel32.Initialize();
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("OK!");
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Failed!");
                InitFail("Kernel32", e.Message.ToString());
                return;
            }
            Console.Write("Loading ExplorerVFS... ");
            try
            {
                Explorer.Initialize();
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("OK!");
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Failed");
                InitFail("ExplorerVFS", e.Message.ToString());
            }
            Console.Write("Loading users data... ");
            try
            {
                Settings.LoadUserData();
                username = Settings.GetUserInfo("name");
                uname = Settings.GetUserInfo("nick");
                password = Settings.GetUserInfo("pass");
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(password))
                {
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine("Failed!");
                    InitFail("user data", "All data is empty/corrupted");
                    //username = "root";
                    //uname = "KosmoConsole/Superuser";
                    //password = "root";
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine("OK!");
                }
            }
            catch (Exception e)
            {
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Failed!");
                InitFail("Users", e.Message.ToString());
                username = "root";
                uname = "KosmoConsole/Superuser";
                password = "root";
            }
        }

        protected override void BeforeRun()
        {
            Initialize();
            Splash.Startup();
        }

        protected override void Run()
        {
            try
            {
                while (true)
                {
                    Console.Write($"{username}@KC {Kernel32.CurrentDirectory}: $ ");
                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                        continue;

                    string lowerInput = input.ToLower();

                    if (lowerInput == "quit")
                    {
                        Console.Write("Do you want to [S]hutdown, [R]estart, or [L]ogout? (default - R) ");
                        string a = Console.ReadLine().ToLower();
                        if (a.StartsWith("s") || a == "s")
                        {
                            base.AfterRun();
                            Console.WriteLine("Please wait...");
                            System.Threading.Thread.Sleep(200);
                            Splash.Shutdown();
                        }
                        else if (a.StartsWith("r") || a == "r")
                        {
                            base.AfterRun();
                            Console.WriteLine("Please wait...");
                            System.Threading.Thread.Sleep(200);
                            Splash.Restart();
                        }
                        else if (a.StartsWith("l") || a == "l")
                        {
                            Console.WriteLine("Logging out...");
                            LogIn();
                            continue;
                        }
                        else if (a.StartsWith("") || a == "" || a == null)
                        {
                            base.AfterRun();
                            Console.WriteLine("Please wait...");
                            System.Threading.Thread.Sleep(200);
                            Splash.Restart();
                        }
                        else
                        {
                            Console.WriteLine("Unknown input. Cancelled.");
                        }
                    }
                    else if (lowerInput == "yo mate can you please toggle bsod 4 me huh?" || lowerInput == "throw new 0x00")
                    {
                        throw new Exception("BSoD was triggered by user request.");
                    }
                    else if (lowerInput == "logout")
                    {
                        Console.WriteLine("Logging out...");
                        LogIn();
                        continue;
                    }
                    else if (lowerInput.StartsWith("about") && lowerInput != "about os" && lowerInput != "about this")
                    {
                        Console.WriteLine("Argument missing.");
                    }
                    else if (lowerInput == "about os")
                    {
                        Console.WriteLine($"KosmoConsole {v}");
                    }
                    else if (lowerInput == "about this")
                    {
                        Explorer.Specs();
                    }
                    else if (lowerInput == "help")
                    {
                        Console.WriteLine(
                            $"All commands:\n" +
                            $"  help - all commands\n" +
                            $"  quit - power options:\n" +
                            $"      logout - exit from an account,\n" +
                            $"      shutdown - shut dowm the system,\n" +
                            $"      restart - reboot the system\n" +
                            $"  clear - empty the line\n" +
                            $"  echo - rewrite the arguments\n" +
                            $"  date - show date\n" +
                            $"  time - show time\n" +
                            $"  whoami - show your nickname\n" +
                            $"  uname - show your username\n" +
                            $"  ls - show directory listing\n" +
                            $"  mkdir - create directory\n" +
                            $"  rm - remove file:\n" +
                            $"      rmdir - remove directory (if empty)\n" +
                            $"  touch - create file\n" +
                            $"  cat - view contents of the file"
                            );
                        Console.Write("Press any key to continue . . .");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(
                            $"  cd - change directory\n"+
                            $"  pwd - change password\n" +
                            $"  set - set value\n" +
                            $"  net - netword options:\n" +
                            $"      ping - send packet to server. Arguments:\n" +
                            $"          server domain name - where to send packets (default - google.com),\n" +
                            $"          packets count - how many packets to send (default - 5),\n" +
                            $"      ip - show your ip address.\n"
                            );
                    }
                    else if (lowerInput == "clear")
                    {
                        Console.Clear();
                    }
                    else if (lowerInput.StartsWith("echo "))
                    {
                        string text = input.Substring(5);
                        Console.WriteLine(text);
                    }
                    else if (lowerInput == "echo")
                    {
                        Console.WriteLine();
                    }
                    else if (lowerInput == "date")
                    {
                        Console.WriteLine($"{DateTime.Now.ToShortDateString()}");
                    }
                    else if (lowerInput == "time")
                    {
                        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}");
                    }
                    else if (lowerInput == "uname")
                    {
                        Console.WriteLine($"{uname}");
                    }
                    else if (lowerInput.StartsWith("cd "))
                    {
                        string dirName = input.Substring(3).Trim();
                        Explorer.Cd(dirName);
                    }
                    else if (lowerInput == "cd")
                    {
                        Console.WriteLine("No directory specified.");
                    }
                    else if (lowerInput == "ls")
                    {
                        Explorer.Ls(Kernel32.CurrentDirectory());
                    }
                    else if (lowerInput.StartsWith("ls "))
                    {
                        string dirName = input.Substring(3).Trim();
                        if (!string.IsNullOrEmpty(dirName))
                        {
                            try
                            {
                                Explorer.Ls(dirName);
                            }
                            catch (Exception e)
                            {
                                BSoD(e.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Directory name cannot be empty.");
                        }
                    }
                    else if (lowerInput.StartsWith("mkdir "))
                    {
                        string dirName = input.Substring(6).Trim();
                        if (!string.IsNullOrEmpty(dirName))
                        {
                            Explorer.Mkdir(dirName);
                        }
                        else
                        {
                            Console.WriteLine("Directory name cannot be empty.");
                        }
                    }
                    else if (lowerInput.StartsWith("touch "))
                    {
                        string fileName = input.Substring(6).Trim();
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            Explorer.Touch(fileName);
                        }
                        else
                        {
                            Console.WriteLine("File name cannot be empty.");
                        }
                    }
                    else if (lowerInput == "touch")
                    {
                        Console.WriteLine("File name cannot be empty.");
                    }
                    else if (lowerInput == "cat")
                    {
                        Console.Write("File name: ");
                        string fileName = Console.ReadLine();
                        Explorer.Cat(fileName);
                    }
                    else if (lowerInput == "pwd")
                    {
                        Console.Write("Current password: ");
                        string p = Console.ReadLine();
                        if (!string.IsNullOrEmpty(p))
                        {
                            Console.Write("New password: ");
                            string newPass = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newPass))
                            {
                                Console.Write("Comfirm password: ");
                                string retypePass = Console.ReadLine();
                                if (!string.IsNullOrEmpty(retypePass))
                                {
                                    if (newPass == retypePass)
                                    {
                                        Console.WriteLine("Password changed successfully.");
                                        password = newPass;
                                        Settings.PwdChange(newPass);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Passwords do not match.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid password.");
                        }
                    }
                    else if (lowerInput == "whoami")
                    {
                        Console.WriteLine($"{username}");
                    }
                    else if (lowerInput.StartsWith("set "))
                    {
                        string[] par1 = input.Substring(4).Split(' ');
                        if (par1 == null || par1.Length == 0)
                        {
                            Console.WriteLine("No parameters provided.");
                        }
                        else if (par1.Length == 1)
                        {
                            Console.WriteLine("No value provided for the parameter.");
                        }
                        else if (par1.Length > 2)
                        {
                            Console.WriteLine("Too many parameters provided.");
                        }
                        else
                        {
                            string key = par1[0];
                            string value = par1[1];
                            string boole = par1[2];
                            if (key == "username")
                            {
                                username = value;
                                Console.WriteLine($"Username set to {username}");
                            }
                            else if (key == "uname")
                            {
                                uname = value;
                                Console.WriteLine($"Uname set to {uname}");
                            }
                            else if (key == "bool")
                            {
                                if (value == "isNewBSoD")
                                {
                                    if (boole != null || boole != "")
                                    {
                                        if (boole.ToLower() == "true")
                                        {
                                            isNewBSoD = true;
                                            Console.WriteLine("Boolean isNewBSoD was set to true.");
                                        }
                                        else if (boole.ToLower() == "false")
                                        {
                                            isNewBSoD = false;
                                            Console.WriteLine("Boolean isNewBSoD was set to false.");
                                        }
                                        else if (boole.ToLower() == "show")
                                        {
                                            Console.WriteLine($"Current state of isNewBSoD: {isNewBSoD.ToString()}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Unknnowm parameter");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No value provided");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Unknown key: {key}");
                            }
                        }
                    }
                    else if (lowerInput == "set")
                    {
                        Console.WriteLine("No parameters provided.");
                    }
                    else if (lowerInput == "restart")
                    {
                        base.AfterRun();
                        Console.WriteLine("Please wait...");
                        System.Threading.Thread.Sleep(200);
                        Splash.Restart();
                    }
                    else if (lowerInput == "shutdown")
                    {
                        base.AfterRun();
                        Console.WriteLine("Please wait...");
                        System.Threading.Thread.Sleep(200);
                        Splash.Shutdown();
                    }
                    else if (lowerInput.StartsWith("net "))
                    {
                        string key = input.Substring(4).Trim();
                        if (key.StartsWith("ping "))
                        {
                            string[] paramet = key.Split(' ');
                            Internet.Ping(paramet[1], int.Parse(paramet[2]));
                        }
                        else if (key == "ping")
                        {
                            Internet.Ping("default", 0);
                        }
                        else if (key == "ip")
                        {
                            Internet.ShowIP();
                        }
                    }
                    else
                    {
                        if (lowerInput != null || lowerInput != "")
                        {
                            Console.WriteLine($"{lowerInput} is not registered as internal or external\n" +
                                $"command, file name or a component.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BSoD(e.Message.ToString());
            }
        }

        protected void BSoD(string e)
        {
            if (isNewBSoD)
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(e);
                    uint value = 0;
                    for (int i = 0; i < Math.Min(4, bytes.Length); i++)
                    {
                        value |= (uint)bytes[i] << (8 * (3 - i));
                    }
                    string hex = value.ToString("X8");
                    Console.Clear();
                    Console.Clear();
                    Console.WriteLine("[KERNEL PANIC!]");
                    Console.WriteLine("The system has encountered a fatal error and cannot continue.");
                    Console.WriteLine($"\n{e}");
                    Console.WriteLine("\nIf you see this for the first time, try rebooting your system. If you not, try doing the following:\n" +
                        "\n - Try restarting the system\n" +
                        " - Try reinstalling the system\n" +
                        " - Open utilities and check for any errors.\n");
                    Console.WriteLine("\nTechical information:\n" +
                        $"OS: KosmoConsole {v}\n" +
                        $"Stop code: 0x{hex}\n" +
                        $"Reason: {e}\n\n\n" +
                        $"0x00000000 0x{hex}");
                }
                catch
                {
                    isNewBSoD = false;
                    BSoD(e);
                }
            }
            else
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.WriteLine("An error occurred while processing a command and kernel has been halted to prevent any further corruption.");
                Console.WriteLine($"\n{e}");
                Console.WriteLine("\nIf you see this for the first time, try rebooting your system. If you not, try doing the following:\n" +
                    " - Try restarting the system\n" +
                    " - Try reinstalling the system\n" +
                    " - Open utilities and check for any errors.\n" +
                    "The system is now frozen. Press any key to reboot...");
                Console.ReadKey();
                Console.Clear();
                base.AfterRun();
                Sys.Power.Reboot();
            }
        }
    }
}
