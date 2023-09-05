using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    class Program
    {
        public static void Main(string[] args)
       {
            string cadena = " let x= 2.5 in print(x)";


            Lexer.Tokenizar(cadena);
            
            
        }
    }
}