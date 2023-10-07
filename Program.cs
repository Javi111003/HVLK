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
            while (true)//arreglar log y exp,ver booleans y como evaluan
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" :) >>>");
                string line = Console.ReadLine();      
                
                List<Token> tokens = new List<Token>();
                tokens = Lexer.Tokenizar(line);
                RecursiveParser recursive_parser = new RecursiveParser();
                object result = recursive_parser.Parse(tokens).Evaluate();
                Console.WriteLine(result);
                Contexto.Reset();
            }
        }
    }
}