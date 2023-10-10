using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    class Program
    {
        public static List<Error> errors=new List<Error>();
        public static int MAX_IT = 0; 
        public static void Main(string[] args)
       {            
            while (true)//arreglar log y exp,arreglar funciones  y argumentos; no se confundan con variables en scope,posible solucion es que cada function tenga un scope propio para almacenar argumentos
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" :) >>>");
                string line = Console.ReadLine();      
                
                List<Token> tokens = new List<Token>();
                tokens = Lexer.Tokenizar(line);
                if (errors.Count > 0) {foreach (Error error in errors) { error.Evaluate();}CleanErrors();continue;}

                RecursiveParser recursive_parser = new RecursiveParser(tokens);

                var AST = recursive_parser.Parse();
                if (errors.Count > 0) { foreach (Error error in errors) { error.Evaluate(); } CleanErrors(); continue; }

                object result = AST.Evaluate();
                Console.WriteLine(result);
                Contexto.Reset();
                CleanErrors();
            }
        }
        public static void CleanErrors()
        {
            errors = new List<Error>();
        }
    }
}