using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checker_IheartRadio.Utils
{
    internal class Functions
    {

        private static Random random = new Random();

        public static string RandomStringId()
        {

            string pattern = "ldldldld-dldd-dddd-ddld-lddldlddddld"; 

            StringBuilder cadena = new StringBuilder();

            foreach(char c in pattern)
            {
                switch (c)
                {
                    case 'l':
                        char letra = (char)random.Next('a', 'z' + 1);
                        cadena.Append(letra);
                        break;
                    case 'd':
                        char numero = (char)random.Next('0', '9' + 1);
                        cadena.Append(numero);
                        break;
                    case '-':
                        cadena.Append(c);
                        break;
                }
            }
            String cadenaFinal = cadena.ToString();
            return cadenaFinal;
        }
    }
}
