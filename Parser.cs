using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    //Gramática libre de contexto 
    //Expression E-> T | T+E | T-E
    //Termino T-> F * T | F / T | F % T | F
    //Factor F -> -F | (S) | num    
              
        public class Parser
        {
            Token[] tokens;
            int nextToken;

            bool Match(Lexer.TokenType type)
            {              
                return tokens[nextToken++].Type == type;
            }
            
             bool E()
            {
                //Parsea un no-terminaal E
                int currToken = nextToken;
                if(E1()) return true;

                nextToken = currToken;
                if (E2()) return true;

                nextToken=currToken;
                if (E3()) return true;


                else return false;
                
            }
             bool E1()//parsea E->T
            {
                return T();   
            }
            bool E2()//Parsea E->T+E
            {
                return T() && Match(Lexer.TokenType.sum) && E();
            }
            bool E3()
            {
                return T() && Match(Lexer.TokenType.minus) && E();
            }
            bool T()
            {
                int currToken = nextToken;
                if (T1()) return true;

                nextToken = currToken;
                if (T2()) return true;

                nextToken = currToken;
                if (T3()) return true;

                nextToken = currToken;
                if (T4()) return true;

                else return false;
                
            }
            bool T1()//Parsea F
            {
                return F();
            }
            bool T2()//Parsea F*T
            {
                return F() && Match(Lexer.TokenType.mult) && T();
            }
            bool T3()//Parsea F/T
            {
                return F() && Match(Lexer.TokenType.divide) && T();
            }
            bool T4()//Parsea F%T
            {
                return F() && Match(Lexer.TokenType.residous) && T();
            }

            bool F()
            {
                int currToken = nextToken;
                if (F1()) return true;

                nextToken = currToken;
                if (F2()) return true;

                nextToken = currToken;
                if (F3()) return true;

                else return false;
            }

            bool F3()//Parsea (E)
            {
                return Match(Lexer.TokenType.leftparenthesis) && E() && Match(Lexer.TokenType.rigthparenthesis);
            }
            bool F2()//Parsea -F
            {
                return Match(Lexer.TokenType.minus) && F();
            }
            bool F1()//Parsea num
            {
                return Match(Lexer.TokenType.number);
            }
           public bool Parse(Token[] tokens)
            {
                this.tokens = tokens;
                nextToken = 0;

                return E() && nextToken == tokens.Length;
            }
        }

    
}
