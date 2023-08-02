using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public  class Lexer
    {
       public static string cadena=string.Empty;

        public static int idx_next_char = 0;//puntero que indica en que posicion de la cadena estamos
        public static char src_char;//almacena el caracter actual 
        public static char next_char;//almacena el caracter siguiente

        public static List<string> Tokenizar(string cad)
        {
            cadena = cad;
            List<string> tokens = new List<string>(); 
            
            string temp = string.Empty;//almacena el token hasta ser completado
            
          
            while (!next_is_EOL())//mientras no sea fin de linea
            {
                ReadChar();//leo el caracter actual
                NextChar();//paso al siguiente 

                if (Is_empty(src_char))
                {
                    tokens.Add(temp);temp = string.Empty; tokens.Add(temp);//añade el token anterior , el espacio vacio , y pasamos al proximo token
                }
                else if (Is_operator(src_char) && Is_operator(next_char))
                {
                    temp += src_char;
                }
                else if ((Is_num(src_char) && Is_operator(next_char)) || (Is_litlow(src_char) && Is_operator(next_char)) || (Is_litup(src_char) && Is_operator(next_char)) || (Is_operator(src_char)&&Is_num(next_char)) || (Is_operator(src_char)&&Is_litlow(next_char))|| (Is_operator(src_char) && Is_litup(next_char))) 
                {
                    temp += src_char; tokens.Add(temp); temp = string.Empty;
                }
                else
                {
                    temp += src_char;
                }
                
            }
            if(temp!=null)
                tokens.Add(temp);
            return tokens;
        }


        public static void NextChar()
        {
            idx_next_char++;
            if (!next_is_EOL())
            {
                next_char = cadena[idx_next_char];
            }
        }
        public static void ReadChar()
        {
            src_char = cadena[idx_next_char];
        }

            public static bool Is_empty(char c)
        {
            if (c == ' ')
            {
                return true;
            }
            return false;
        }

        public static bool Is_operator(char c)
        {
            char[] operadores = new char[] { '+','*','<','>','=','/','%'};
           
            for (int i = 0; i < operadores.Length; i++)
            {
                if (c == operadores[i])
                    return true;
            }
            return false;
        }
        public static bool Is_num(char c)
        {
            if(c >= 0 && c <= 9)
            {
                return true;
            }
            return false ;

        }
        public static bool next_is_EOL()
        {
            if (idx_next_char >= cadena.Length)
            {
                return true;
            }

            return false;
        }
        public static bool Is_litlow(char c)
        {
            if (c >= 'A' || c <= 'Z')
            {
                return true;
            }
            return false;
        }
        public static bool Is_litup(char c)
        {
            if (c >= 'a' || c <= 'z')
            {
                return true;
            }
            return false;
        }
    }
}
