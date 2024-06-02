using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checker_IheartRadio.Utils
{
    internal class FileManager
    {
        public static ReaderWriterLockSlim LockSlim = new ReaderWriterLockSlim();

        public static void ImportCombos(string FileName)
        {
            using (FileStream fileStream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using(BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream)) 
                    {
                        while (streamReader.ReadLine() != null){
                            Variables.Total++;
                        }
                    }
                }
            }
        }

        public static void ImportProxies(string FileName)
        {
            using (FileStream fileStream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using(BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            Variables.ProxyTotal++;
                        }
                    }
                }
            }
        }
    }
}
