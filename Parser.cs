using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    //Gramática libre de contexto 
    //Program P-> S_L
    //Statement_list S_L -> S | S ; S_L
    //Stat S -> l_v|d_f|p_s
    //Argument List A_L-> id | id ","
    //Expression E-> T+E | T-E | T
    //Termino T-> F * T | F / T | F % T | F^T | F
    //Factor F -> -F | (E) | A
    //Atom A-> number|ID|func-call
              
    interface IParser
    {
        bool Parse(List<Token>tokens);
    }
        public class RecursiveParser:IParser
        {
            List<Token> tokens;
            int nextToken;

        public bool Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            nextToken = 0;

            return P() && Match(Lexer.TokenType.token_EOF);
        }

        bool Match(Lexer.TokenType type)
            {              
                return nextToken+1<=tokens.Count && tokens[nextToken++].Type == type;
            }
        bool Match_value(string value)
        {
            return nextToken + 1 <= tokens.Count && tokens[nextToken++].Value == value;
        }
            
        bool P()
        {
            return S_L();
        }
        bool S_L()
        {
            int currToken = nextToken;
            if (S_L1()) return true;

            nextToken = currToken;
            if (S_L2()) return true;


            else return false;

        }
        bool S_L2()
        {
            return S() && Match_value(";");
        }
        bool S_L1()
        {
            return S() && Match_value(";") && S_L();
        }
        bool S()
        {
            int currToken = nextToken;
            if (S1()) return true;

            nextToken = currToken;
            if (S2()) return true;

            nextToken = currToken;
            if (S3()) return true;

            nextToken = currToken;
            if (S4()) return true;


            else return false;

        }
        bool S1()
        {
            return Match_value("let") && Match(Lexer.TokenType.identifier) && Match_value("=") && E() && Match_value("in") && S();
        }
        bool S2()
        {
            return Match_value("function") && Match(Lexer.TokenType.identifier) && Match(Lexer.TokenType.leftparenthesis) && A_L() && Match(Lexer.TokenType.rigthparenthesis) && Match_value("=>") && E();
        }
        bool S3()
        {
            return Match_value("print") && Match(Lexer.TokenType.leftparenthesis) && E() && Match(Lexer.TokenType.rigthparenthesis);
        }
        bool S4()
        {
            return E();
        }

        bool A_L()
        {
            int currToken = nextToken;
            if (A_L1()) return true;

            nextToken = currToken;
            if (A_L2()) return true;

            else return false;
        }
        bool A_L1()
        {
            return Match(Lexer.TokenType.identifier) && Match_value(",") && A_L();
        }
        bool A_L2()
        {
            return Match(Lexer.TokenType.identifier);
        }
        bool E()
            {
                //Parsea un no-terminal E
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
            bool E2()//Parsea E->T-E
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

                nextToken = currToken;
                if (T5()) return true;

            else return false;
                
            }
            bool T5()//Parsea F
            {
                return F();
            }
            bool T4()//Parsea F^T
            {
            return F() && Match(Lexer.TokenType.power) && T();
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
            bool F3()//Parsea Atom
            {
            
                return A();
            }
            bool A()
            {
            int currToken = nextToken;
            if (A1()) return true;//Parsea number

            nextToken = currToken;
            if (A2()) return true;//Parsea func_call

            nextToken = currToken;
            if (A3()) return true;//Parsea id

            nextToken = currToken;
            if (A4()) return true;//Parsea string

            else return false;
            }
        bool A1()
        {
            return Match(Lexer.TokenType.number);
        }
        bool A3()
        {
            return Match(Lexer.TokenType.identifier);
        }
        bool A2()
        {
            return Match(Lexer.TokenType.identifier) && Match(Lexer.TokenType.leftparenthesis) && E_L() && Match(Lexer.TokenType.rigthparenthesis);
        }
        bool A4()
        {
            return Match(Lexer.TokenType.lit_string);
        }
        bool E_L()
        {
            int currToken = nextToken;
            if (E_L1()) return true;

            nextToken = currToken;
            if (E_L2()) return true;

            else return false;
        }
        bool E_L1()
        {
            return E() && Match_value(",") && E_L();
        }
        bool E_L2()
        {
            return E();
        }


    }

    
}
