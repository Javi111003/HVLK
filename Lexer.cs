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
        public static List<string> Palabras_reservadas = new List<string>() { "true", "false", "let", "number", "print", "sqrt", "rand", "sin", "cos", "exp", "PI", "E", "in", "function", "if","else" };
        public static string temp = string.Empty;//almacena el token hasta ser completado
        public enum TokenType
        {

            lit_string,
            number,
            identifier,
            op,
            comment,
            sum,
            mult,
            divide,
            minus,
            power,
            leftparenthesis,
            rigthparenthesis,
            keyfunction,
            puntuation,
            unknowed,
            keyword,
            residous,
            token_EOF,
            token_EOL,

        }
        public static List<Token> Tokenizar(string _cadena)
        {
            cadena = _cadena;
            List<Token> tokens = new List<Token>();

            while (!next_is_EOL())//mientras no sea fin de linea
            {
                ReadChar();//leo el caracter actual

                if (Is_empty(src_char))
                {
                    NextChar(); temp = string.Empty;
                }
                else if (src_char == '\"')
                {
                    tokens.Add(new Token(Extract_String(), TokenType.lit_string)); temp = string.Empty;
                }
                else if (src_char == '(')
                {
                    tokens.Add(new Token("(", TokenType.leftparenthesis)); NextChar();
                }
                else if (src_char == ')')
                {
                    tokens.Add(new Token(")", TokenType.rigthparenthesis)); NextChar();
                }
                else if (src_char == '+')
                {
                    tokens.Add(new Token("+", TokenType.sum)); NextChar();
                }
                else if (src_char == '-')
                {
                    tokens.Add(new Token("-", TokenType.minus)); NextChar();
                }
                else if (src_char == '/')
                {
                    NextChar();
                    if (next_char == '/')
                    {
                        tokens.Add(new Token("//", TokenType.comment));
                    }
                    tokens.Add(new Token("/", TokenType.divide));
                }
                else if (src_char == '\n')
                {
                    tokens.Add(new Token("\n", TokenType.token_EOL)); NextChar();
                }
                else if (src_char == '*')
                {
                    tokens.Add(new Token("*", TokenType.mult)); NextChar();
                }
                else if (src_char == '^')
                {
                    tokens.Add(new Token("^", TokenType.power)); NextChar();
                }
                else if (src_char == '%')
                {
                    tokens.Add(new Token("%", TokenType.residous)); NextChar();
                }
                else if (Is_operator(src_char))
                {
                    tokens.Add(new Token(Extract_Operator(), TokenType.op)); temp = string.Empty;
                    NextChar();
                }
                else if (Is_num(src_char))
                {
                    tokens.Add(new Token(Extract_Number(), TokenType.number)); temp = string.Empty;

                }
                else if (Is_litup(src_char) || Is_litlow(src_char))
                {
                    temp = Extract_Identifier();

                    if (!Restricted_word(temp))
                    {
                        tokens.Add(new Token(temp, TokenType.identifier)); temp = string.Empty;

                    }
                    else
                    {
                        tokens.Add(new Token(temp, TokenType.keyword)); temp = string.Empty;

                    }

                }
                else//aun no se ha completado el token para ser add
                {
                    temp += src_char;
                    tokens.Add(new Token(temp, TokenType.unknowed)); temp = string.Empty;
                    NextChar();
                }

            }
            
            
            tokens.Add(new Token("", TokenType.token_EOF)); cadena= string.Empty;idx_next_char= 0;
            return tokens;
        }


        public static void NextChar()
        {
            idx_next_char += 1;
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
            char[] operadores = new char[] { '+', '*', '<', '>', '=', '/', '%','^' };

            for (int i = 0; i < operadores.Length; i++)
            {
                if (c == operadores[i])
                    return true;
            }
            return false;
        }
        public static bool Is_num(char c)
        {
            char[] num = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < num.Length; i++)
            {
                if (c == num[i])
                {
                    return true;
                }
            }
            return false;

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

            for (int i = 0; i < Litup.Length; i++)
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
            char[] Litlow = new char[] { '_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

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
            return false;
        }
        public static string Extract_Identifier()
        {
            while (Is_litup(src_char) || Is_litlow(src_char)||Is_num(src_char))
            {
                temp += src_char;
                NextChar();
                if (!next_is_EOL())
                {
                    ReadChar();
                }
                else
                {
                    return temp;
                }
            }
            return temp;
        }
        public static string Extract_Operator()//arreglar dobles operadores
        {

            switch (src_char)
            {
                case '<':
                    temp += src_char;
                    NextChar();
                    if (!next_is_EOL())
                    {
                        ReadChar();

                        if (src_char == '=')
                        {

                            temp += src_char;
                            return temp;
                        }
                        else
                        {
                            idx_next_char = idx_next_char - 1;
                            return temp;
                        }
                    }
                    else
                    {
                        return temp;
                    }


                case '>':
                    temp += src_char;
                    NextChar();
                    if (!next_is_EOL())
                    {
                        ReadChar();

                        if (src_char == '=')
                        {

                            temp += src_char;
                            return temp;
                        }
                        else
                        {
                            idx_next_char = idx_next_char - 1;
                            return temp;
                        }
                    }
                    else
                    {
                        return temp;
                    }


                case '=':
                    temp += src_char;
                    NextChar();
                    if (!next_is_EOL())
                    {
                        ReadChar();

                        if (src_char == '=' || src_char == '>')
                        {

                            temp += src_char;
                            return temp;
                        }
                        else
                        {
                            idx_next_char = idx_next_char - 1;
                            return temp;
                        }
                    }
                    else
                    {
                        return temp;
                    }

                default:
                    temp += src_char;
                    return temp;

            }
        }
        public static string Extract_Number()
        {
            int c = 0;
            while (Is_num(src_char) || (src_char == '.' && c != 1))
            {
               
                temp += src_char;
                NextChar();
                if (!next_is_EOL())
                {                   
                    if (src_char == '.')
                    {
                        c++;
                    }
                    ReadChar();
                }
                else
                {
                    return temp;
                }
            }
            return temp;
        }
        public static string Extract_String()
        {
            NextChar();
            if (!next_is_EOL())
            {
                ReadChar();
            }
            while (src_char != '\"') 
            {
                temp += src_char;
                NextChar();

                if (!next_is_EOL())
                {
                    ReadChar();
                }
                else
                {
                    return temp;
                }

            }
          NextChar();
            return temp;
        }
    }
}
