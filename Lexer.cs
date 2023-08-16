using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    public class Lexer
    {
        public static string cadena = string.Empty;

        public static int idx_next_char = 0;//puntero que indica en que posicion de la cadena estamos
        public static char src_char;//almacena el caracter actual 
        public static char next_char;//almacena el caracter siguiente
        public static List<string> Palabras_reservadas = new List<string>() { "true","false","let","number","print","sqrt","rand","sin","cos","exp","PI","E" };
        public enum TokenType
        {
            whitespace,
            vartoken,
            lit,
            num,
            op,
            token_sum,
            token_plus,
            token_divide,
            token_minus,
            leftparenthesis,
            rigthparenthesis,
            keyvar,
            keyfunction,
            puntuation,
        }
        public static List<Token> Tokenizar(string cad)
        {
            cadena = cad;
            List<Token> tokens = new List<Token>(); 
            
            string temp = string.Empty;//almacena el token hasta ser completado
            
          
            while (!next_is_EOL())//mientras no sea fin de linea
            {
                ReadChar();//leo el caracter actual
                NextChar();//paso al siguiente para tener visualizacion de el en la toma de decisiones
           
                    if (src_char == '(')
                    {
                    tokens.Add(new Token("(", TokenType.leftparenthesis));
                    }
                    else if (src_char == ')')
                    {
                    tokens.Add(new Token(")", TokenType.rigthparenthesis));
                    }
                    else if (src_char == '+')
                    {
                    tokens.Add(new Token("+", TokenType.token_sum));
                    }
                    else if (src_char == '-')
                    {
                    tokens.Add(new Token("-", TokenType.token_minus));
                    }
                    else if (src_char == '/')
                    {
                    tokens.Add(new Token("/", TokenType.token_divide));
                    }
                    else if (src_char == '*')
                    {
                    tokens.Add(new Token("*", TokenType.token_plus));
                    }
                    else if (Is_operator(src_char) && Is_operator(next_char))
                    {
                        temp += src_char;
                    }
                    else if((Is_litlow(src_char) && next_char == '(')|| (Is_litup(src_char) && next_char == '('))
                    {
                    temp += src_char;tokens.Add(new Token(temp, TokenType.lit)); temp = string.Empty;
                    }
                    else if ((Is_num(src_char) && next_char == '('))
                    {
                    temp += src_char; tokens.Add(new Token(temp, TokenType.num)); temp = string.Empty;
                    }
                    else if ((Is_num(src_char) && next_is_EOL()) || Is_num(src_char) && Is_operator(next_char) || (Is_num(src_char) && Is_empty(next_char)))//cuando el token es num
                    {
                        temp += src_char; tokens.Add(new Token(temp, TokenType.num)); temp = string.Empty;
                    }
                    else if ((Is_litlow(src_char) && next_char==')' ) || (Is_litlow(src_char) && next_is_EOL()) || (Is_litup(src_char) && Is_empty(next_char)) || (Is_litlow(src_char) && Is_empty(next_char)) || (Is_litlow(src_char) && Is_operator(next_char)) || (Is_litup(src_char) && Is_operator(next_char)))  //es literal   
                    {
                        temp += src_char; tokens.Add(new Token(temp, TokenType.lit)); temp = string.Empty;
                    }
                    else if ((Is_litlow(src_char) && next_char == ')') || (Is_operator(src_char) && next_is_EOL()) || (Is_operator(src_char) && Is_empty(next_char)) || (Is_operator(src_char) && Is_num(next_char)) || (Is_operator(src_char) && Is_litlow(next_char)) || (Is_operator(src_char) && Is_litup(next_char)))//es operador 
                    {
                        temp += src_char; tokens.Add(new Token(temp, TokenType.op)); temp = string.Empty;
                    }
                    else if (Is_empty(src_char))
                    {
                    tokens.Add(new Token(temp, TokenType.whitespace)); temp = string.Empty;//añade el espacio vacio , y pasamos al proximo token
                    }
                    else//aun no se ha completado el token para ser add
                    {
                        temp += src_char;
                    }
                
            }
  
            return tokens;
        }
       

        public static void NextChar()
        {
            idx_next_char+=1;
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
            char[] num = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

            for (int i = 0; i < num.Length; i++)
            {
                if (c == num[i])
                {
                    return true;
                }
            }
            return false ;

        }
        public static bool next_is_EOL()//si es fin de linea
        {
            if (idx_next_char >= cadena.Length)
            {
                return true;
            }

            return false;
        }
        public static bool Is_litup(char c)//Si es letra en mayuscula
        {
            char[] Litup = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            for (int i = 0; i < Litup.Length ; i++)
            {
                if (c == Litup[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Is_litlow(char c)//si es letra en minuscula
        {
            char[] Litlow = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            for (int i = 0; i < Litlow.Length; i++)
            {
                if (c == Litlow[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Restricted_word(string tok)
        {
            if (Palabras_reservadas.Contains(tok))
            {
                return true;
            }
            return false ;
        }
    }
}
