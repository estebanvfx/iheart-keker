using Checker_IheartRadio.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Console = Colorful.Console;

namespace Checker_IheartRadio.Checker
{
    internal class Setup
    {

        public static void Star()
        {
            Console.Clear();
            Console.WriteLine();

            Console.Title = "IheartRadio Checker - Coded by Vizz";

            Console.WriteLine();

            Console.Write("    > [", Color.GhostWhite);
            Console.Write("+", Color.FromArgb(246, 120, 106));
            Console.Write("] How many ", Color.GhostWhite);
            Console.Write("threads ", Color.FromArgb(246, 120, 106));
            Console.Write("would you like to use? ", Color.GhostWhite);

            try
            {
                Variables.Threads = int.Parse(Console.ReadLine());
            }
            catch
            {
                Variables.Threads = 100;
            }

            for (; ; )
            {
                Console.Clear();
                Console.Write("\n    > [", Color.GhostWhite);
                Console.Write("+", Color.FromArgb(246, 120, 106));
                Console.Write("] Select Proxy Type: ", Color.GhostWhite);
                Console.Write("[HTTP, SOCKS4, SOCKS5]: ", Color.FromArgb(246, 120, 106));
                Variables.ProxyType = Console.ReadLine().ToUpper();
                if (Variables.ProxyType == "HTTP" || Variables.ProxyType == "SOCKS4" || Variables.ProxyType == "SOCKS5")
                {
                    break;
                }
                Console.Write("    > [", Color.GhostWhite);
                Console.Write("+", Color.FromArgb(255, 0, 0));
                Console.Write("] Please choose a valid proxy type. \n ", Color.GhostWhite);
                Thread.Sleep(2000);
            }

            Console.WriteLine();
            string FileName;
            OpenFileDialog openFile = new OpenFileDialog();

            do
            {
                Console.Write("    > [", Color.GhostWhite);
                Console.Write("+", Color.FromArgb(246, 120, 106));
                Console.Write("] Load your Proxy List: ", Color.GhostWhite);
                openFile.Title = "Select Proxy File";
                openFile.Filter = "Text Files|*.txt";
                openFile.RestoreDirectory = true;
                openFile.ShowDialog();
                FileName = openFile.FileName;
                Console.Write(FileName, Color.GhostWhite);

            } while (!File.Exists(FileName));

            Variables.Proxies = new List<string>(File.ReadAllLines(FileName));
            FileManager.ImportProxies(FileName);

            Console.Write("\n    > ", Color.FromArgb(246, 120, 106));
            //Console.Write(Variables.Total, Color.GhostWhite);
            Console.Write("Proxies Added\n", Color.FromArgb(246, 120, 106));

            do
            {
                Console.Write("    > [", Color.GhostWhite);
                Console.Write("+", Color.FromArgb(246, 120, 106));
                Console.Write("] Select Combo File: ", Color.GhostWhite);
                openFile.Title = "Select Combo File";
                openFile.Filter = "Text Files|*.txt";
                openFile.RestoreDirectory = true;
                openFile.ShowDialog();
                FileName = openFile.FileName;
                Variables.GetComboName = FileName;
                Console.Write(FileName, Color.GhostWhite);
            } while (!File.Exists(FileName));

            Variables.Combos = new List<string>(File.ReadAllLines(FileName));
            FileManager.ImportCombos(FileName);

            Console.Write("\n    > ", Color.FromArgb(246, 120, 106));
            //Console.Write(Variables.Total, Color.GhostWhite);
            Console.Write("Combos Loaded\n", Color.FromArgb(246, 120, 106));



            Console.Write("    > [", Color.GhostWhite);
            Console.Write("+", Color.FromArgb(246, 120, 106));
            Console.Write("] Starting Checker...\n\n", Color.GhostWhite);
            Thread.Sleep(50);

            Task.Factory.StartNew(delegate ()
            {
                Brute.UpdateUI();
            });

            for(int i = 1; i <= Variables.Threads; i++)
            {
                new Thread(new ThreadStart(Brute.Checker)).Start();
            }

            Console.ReadLine();
            Environment.Exit(0);

        }

        
    }
}
