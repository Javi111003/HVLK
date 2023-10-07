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
        private int _index;
       

        public Token(string valor,Lexer.TokenType tipo,int i)
        {
            _value = valor;
            _type=tipo;
            _index = i;

        }

        public string Value
        {
            get{ return _value; }
        }

        public  Lexer.TokenType Type
        {
            get { return _type; } 
        }
        public int Index
        {
            get { return _index; }
        }
    }
}
