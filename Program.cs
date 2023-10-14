using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HVLK;


namespace HVLK
{
    class Program
    {
        public static List<Error> errors=new List<Error>();
        public static int MAX_IT = 0; 
        public static int  l=1;
        public static void Main(string[] args)
       {            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            string v2 = "\r\n                                                         )         (         )  \r\n (  (           (                             )       ( /(         )\\ )   ( /(  \r\n )\\))(   '   (  )\\             )      (    ( /(       )\\())    (  (()/(   )\\()) \r\n((_)()\\ )   ))\\((_) (   (     (      ))\\   )\\()) (   ((_)\\     )\\  /(_))|((_)\\  \r\n_(())\\_)() /((_)_   )\\  )\\    )\\  ' /((_) (_))/  )\\   _((_) _ ((_)(_))  |_ ((_) \r\n\\ \\((_)/ /(_)) | | ((_)((_) _((_)) (_))   | |_  ((_) | || || | | || |   | |/ /  \r\n \\ \\/\\/ / / -_)| |/ _|/ _ \\| '  \\()/ -_)  |  _|/ _ \\ | __ || |_| || |__   ' <   \r\n  \\_/\\_/  \\___||_|\\__|\\___/|_|_|_| \\___|   \\__|\\___/ |_||_| \\___/ |____| _|\\_\\  \r\n                                                                                \r\n";
            Console.WriteLine(v2);
            Console.WriteLine("Let's get started---------------:");
            while (true)
            {
                Console.Write(" :) >>>");
                string line = Console.ReadLine();      
                
                List<Token> tokens = new List<Token>();
                tokens = Lexer.Tokenizar(line);
                if (errors.Count > 0) {foreach (Error error in errors) { error.Evaluate();}CleanErrors();continue;}

                RecursiveParser recursive_parser = new RecursiveParser(tokens);

                var AST = recursive_parser.Parse();
                if (errors.Count > 0) { foreach (Error error in errors) { error.Evaluate(); } CleanErrors(); continue; }

                object result = AST.Evaluate();
                if(errors.Count>0&&result==null) { foreach (Error error in errors) { error.Evaluate(); } CleanErrors(); continue; }
                else Console.WriteLine(result);
                Contexto.Reset();
                CleanErrors();
                l += 2;
            }
        }
        public static void CleanErrors()
        {
            errors = new List<Error>();
        }
    }
}