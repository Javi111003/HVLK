﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    //Gramática libre de contexto 
    //Program P-> S_L
    //Statement_list S_L -> S | S ; S_L
    //Stat S -> l_v|d_f|p_s|if (Bo) else | E
    //Bo Boolean Bo-> EWZ| !EWZ  | EW | !EW
    //W -> < E | > E | <= E | >= E | ==E | !=E 
    //Z-> and Bo | or Bo
    //Argument List A_L-> id | id "," | E
    //Expression E-> T+E | T-E | T
    //Termino T-> F * T | F / T | F % T | F^T | F
    //Factor F -> -F | (E) | A
    //Atom A-> number|ID|func-call

    interface IParser
    {
        Expression Parse(List<Token> tokens);
    }
    public class RecursiveParser : IParser
    {
        Token[] tokens;
        int nextToken;

        public Expression Parse(List<Token> tokens)
        {
            this.tokens = tokens.ToArray();
            nextToken = 0;

            var AST = ParseStat();
            if (!Match_value(";")) { Console.WriteLine("Missing ';' after statament"); return null;}
            Match(Lexer.TokenType.token_EOF);
            if(AST==null)return null;
           
            else return AST;
        }
        bool Match(Lexer.TokenType type)
        {
            return nextToken + 1 <= tokens.Length && tokens[nextToken++].Type == type;
        }
        bool Match_value(string value)
        {
            return nextToken + 1 <= tokens.Length && tokens[nextToken++].Value == value;
        }
        void MoveBack(int n)
        {
            nextToken -= n;
        }
        public static int GetUnaryOperatorPrecedence(Lexer.TokenType op)
        {
            switch (op)
            {
                case Lexer.TokenType.sum:
                case Lexer.TokenType.minus:
                    return 3;
                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(Lexer.TokenType op)
        {
            switch (op)
            {
                case Lexer.TokenType.power:
                    return 5;
                case Lexer.TokenType.mult:
                case Lexer.TokenType.divide:
                case Lexer.TokenType.residous:
                    return 4;
                case Lexer.TokenType.sum:
                case Lexer.TokenType.minus:
                    return 3;
                case Lexer.TokenType.op:
                    return 2;
                case Lexer.TokenType.token_and:
                case Lexer.TokenType.token_or:
                    return 1;
                default:
                    return 0;
            }
        }
        Expression ParseStat()//falta booleans 
        {

            int currToken = nextToken;
            if (Match_value("let"))
            {
                if (Match(Lexer.TokenType.identifier))
                {
                    string identifier_1 = tokens[nextToken - 1].Value;
                    if (Match_value("="))
                    {
                        var value_1 = ParseExp();
                        if (Contexto.IsDefined(identifier_1))
                        {
                            Console.WriteLine("The var {0} is already defined", identifier_1); return null;
                        }
                        Contexto.variables_scope.Add(identifier_1, value_1);
                        int currtoken = nextToken;
                        if (Match(Lexer.TokenType.coma))
                        {
                            var identifier_2 = tokens[nextToken++].Value;
                            Match_value("=");
                            var value_2 = ParseExp();
                            Match(Lexer.TokenType.keyword);
                            var in_E = ParseStat();

                            if (Contexto.IsDefined(identifier_2))
                            {
                                Console.WriteLine("The var {0} is already defined",identifier_2);return null;
                            }
                            Contexto.variables_scope.Add(identifier_2, value_2);
                            return new Letvar(identifier_1, identifier_2, value_1, value_2, in_E);
                        }
                        nextToken = currtoken;
                        Match(Lexer.TokenType.keyword);
                        var inE = ParseStat();
                        return new Letvar(identifier_1, null, value_1, null, inE);
                        //Falta definirla(definirlas)
                    }
                }              
            }
            nextToken = currToken;
            if (Match_value("print"))//Parsea PRINT
            {
                if (Match(Lexer.TokenType.leftparenthesis))
                {
                    var inE = ParseStat();
                    Match(Lexer.TokenType.rigthparenthesis);
                    return new Print(inE);
                }
            }           
            nextToken = currToken;
            if ( Match_value("if"))//Parea if_else
            {
                Match(Lexer.TokenType.leftparenthesis);
                var condition = ParseExp();
                Match(Lexer.TokenType.rigthparenthesis);
                var ifexpression = ParseStat();
                Match_value("else");
                var elseexpression = ParseStat();

                return new If_Else_Exp(condition, ifexpression, elseexpression);
            }
            nextToken = currToken;
            if (Match_value("function"))//Parsea funcion
            {
                BuildFunction();

                return null;
            }
            nextToken = currToken;
            var e = ParseExp();
            if (e != null) return e;

            return null;
        }
        Expression BuildFunction()
        {
            string name = tokens[nextToken++].Value;
            Match(Lexer.TokenType.leftparenthesis);
            List<Token> args = new List<Token>();
            if(Match(Lexer.TokenType.identifier)) 
            { args.Add(tokens[nextToken - 1]); }
            while (!Match(Lexer.TokenType.rigthparenthesis))
            {
                nextToken -= 1;
                Match(Lexer.TokenType.coma);
                args.Add(tokens[nextToken - 1]);
            }
            Match(Lexer.TokenType.rigthparenthesis);
            Match_value("=>");
            var Corpus=ParseStat();

            return new Def_Func(name, args, Corpus);
            //Falta definirla 
        }
        private Expression ParseExp(int parentPrecedence=0)
        {
            Expression left;
            var unaryOperatorPrecedence = GetUnaryOperatorPrecedence(tokens[nextToken].Type);
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = tokens[nextToken++].Type;
                var operand = ParseExp(unaryOperatorPrecedence);
                left = new UnaryExpression(operatorToken, operand);
            }
            else left = ParseExpAtomicLevel();
            while (true)
            {
                var precedence = GetBinaryOperatorPrecedence(tokens[nextToken].Type);
                if (precedence == 0 || precedence <= parentPrecedence) break;

                var operatorToken = tokens[nextToken++].Type;
                var right = ParseExp(precedence);
                left = new BinaryExpr(operatorToken,left, right);
            }
            return left;
        }
   /*     Expression ParseExpLV1()
        {
            Expression newLeft = ParseExpLV2();
            Expression exp = ParseExpLV1_(newLeft);
            return exp;
        }
        Expression ParseExpLV1_(Expression left)
        {
            Expression exp = ParseAdd(left);
        }*/
        Expression ParseExpAtomicLevel()//agregar llamado a funcion matematica
        {
            int currToken = nextToken;
            if (Match(Lexer.TokenType.number))//Parsea número
            {
                return new Number(tokens[nextToken - 1].Value);
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.leftparenthesis)) //Parsea (E)
            {
                var inE = ParseExp();
                if (Match(Lexer.TokenType.rigthparenthesis))
                {
                    return new ParenE(inE);
                }
                nextToken = currToken;
                Console.WriteLine("Missing close parenthesis')'");
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.identifier))//Parsea variable o llamada a funcion
            {
                var name = tokens[nextToken - 1].Value;
                int curr = nextToken;
                if (Match(Lexer.TokenType.leftparenthesis))
                {
                    List<Expression> args = new List<Expression>();
                    args.Add(ParseExp());
                    int current = nextToken;
                    while (!Match(Lexer.TokenType.rigthparenthesis))
                    {
                        nextToken = current;
                        Match_value(",");
                        args.Add(ParseExp());
                    }
                    return new Func_call(name, args);
                }
                nextToken = curr;
                return new Variable(name);
            }
            nextToken=currToken;
            if (Match(Lexer.TokenType.lit_string))//Parsea string
            {
                return new Lit_String(tokens[nextToken - 1].Value);
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.Math_EXP))
            {
                if (tokens[nextToken - 1].Value == "log")
                {
                    Match(Lexer.TokenType.leftparenthesis);
                    var bas=ParseExp();
                    Match(Lexer.TokenType.coma);
                    var up =ParseExp();
                    Match(Lexer.TokenType.rigthparenthesis);
                    return new Math_Log(bas, up);
                }
                else
                {
                    string operation = tokens[nextToken - 1].Value;
                    Match(Lexer.TokenType.leftparenthesis);
                    var a = ParseExp();
                    Match(Lexer.TokenType.rigthparenthesis);
                    return new Math_Func(operation,a);
                }
            }
            nextToken = currToken;
            if (Match_value("PI"))
            {
                return new Ctx("PI");
            }
            nextToken = currToken;
            if (Match_value("E"))
            {
                return new Ctx("E");
            }
            nextToken = currToken;
            return null;
        }


    }
}
