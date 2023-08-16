using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
 
    public class Token
    {
        private string _value=string.Empty;
        private  Lexer.TokenType _type;
   
        public Token(string valor,Lexer.TokenType tipo)
        {
            _value = valor;
            _type=tipo;

        }

        public string Value
        {
            get{ return _value; }
        }

        public  Lexer.TokenType Type
        {
            get { return _type; } 
        }
    }
}
