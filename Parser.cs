using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    //Gramática libre de contexto 
    //Expression E-> T+E | T-E | T
    //Termino T-> F * T | F / T | F % T | F
    //Factor F -> -F | (S) | num    
              
        public class Parser
        {
            List<Token> tokens;
            int nextToken;

        public bool Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            nextToken = 0;

            return E() && Match(Lexer.TokenType.token_EOF);
        }

        bool Match(Lexer.TokenType type)
            {              
                return nextToken+1<=tokens.Count && tokens[nextToken++].Type == type;
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
             bool E3()//parsea E->T
            {
                return T();   
            }
            bool E1()//Parsea E->T+E
            {
                return T() && Match(Lexer.TokenType.sum) && E();
            }
            bool E2()
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
            bool T4()//Parsea F
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
            bool T1()//Parsea F%T
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

            bool F2()//Parsea (E)
            {
                return Match(Lexer.TokenType.leftparenthesis) && E() && Match(Lexer.TokenType.rigthparenthesis);
            }
            bool F1()//Parsea -F
            {
                return Match(Lexer.TokenType.minus) && F();
            }
            bool F3()//Parsea num
            {
            
                return Match(Lexer.TokenType.number);
            }
         
        }

    
}
