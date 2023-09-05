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
            while (true)
            {
                string cadena = Console.ReadLine();

                List<Token> tokens = new List<Token>();
                tokens = Lexer.Tokenizar(cadena);
                Parser recursive_parser = new Parser();

                if (recursive_parser.Parse(tokens))
                {
                    Console.WriteLine("cadena correcta");
                }
                else
                {
                    Console.WriteLine("bad");
                }
            }
        }
    }
}