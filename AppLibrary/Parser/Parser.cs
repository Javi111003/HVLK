using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HVLK;

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
        Expression Parse();
    }
    public class RecursiveParser : IParser
    {
        Token[] tokens;
        int nextToken;
        public RecursiveParser(List<Token> tokens)
        {
            this.tokens = tokens.ToArray();
            nextToken = 0;
        }

        public Expression Parse()
        {
            var AST = ParseStat();
            if (!Match_value(";")) { Program.errors.Add(new Error("Missing ';' after statament in col:", tokens[nextToken - 1].Index)); }
            Match(Lexer.TokenType.token_EOF);
            if (AST == null) { Program.errors.Add(new Error("Is not a valid statament in HULK language in col:", tokens[nextToken - 1].Index)); return null; }

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
                case Lexer.TokenType.token_negation:
                    return 1;
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
                case Lexer.TokenType.concat:
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
            if (Match_value("if"))//Parea if_else
            {
                if (!Match(Lexer.TokenType.leftparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ( \"  before if_expression on col:", tokens[nextToken].Index)); nextToken -= 1; }
                var condition = ParseExp();
                if (!Match(Lexer.TokenType.rigthparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ) \"  after if_expression on col:", tokens[nextToken].Index)); nextToken -= 1; }
                var ifexpression = ParseStat();
                if (!Match_value("else")) { Program.errors.Add(new Error("Syntax ERROR:Missing \" else \"  on \"if_else\" expression on col:", tokens[nextToken].Index)); nextToken -= 1; }
                var elseexpression = ParseStat();

                return new If_Else_Exp(condition, ifexpression, elseexpression);
            }
            nextToken = currToken;
            if (Match_value("function"))//Parsea funcion
            {
                var a = BuildFunction();
                if (a != null) return a;

                else return null;
            }
            nextToken = currToken;
            var e = ParseExp();
            if (e != null) return e;

            return null;
        }
        Expression BuildFunction()
        {
            string name = tokens[nextToken++].Value;
            if (!Match(Lexer.TokenType.leftparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ( \"  before function name on col:", tokens[nextToken].Index)); nextToken -= 1; }
            List<Token> args = new List<Token>();
            if (!Match(Lexer.TokenType.rigthparenthesis)) { args.Add(tokens[nextToken - 1]); }
            else nextToken -= 1;
            while (!Match(Lexer.TokenType.rigthparenthesis))
            {
                nextToken -= 1;
                if (!Match(Lexer.TokenType.coma)) { { Program.errors.Add(new Error("Syntax ERROR:Missing \" , \" on col:", tokens[nextToken].Index)); nextToken -= 1; } }
                args.Add(tokens[nextToken++]);
            }
            if (!Match_value("=>")) { Program.errors.Add(new Error("Syntax ERROR:EXcepted \" => \"  before function_body on col:", tokens[nextToken].Index)); nextToken -= 1; }
            var Corpus = ParseStat();
            if (Contexto.IsFunctionDefined(name))
            {
                Program.errors.Add(new Error($"The function {name} is already defined on line: ", 1));
            }
            if (Program.errors.Count == 0) { Contexto.function_scope.Add(new Tuple<string, List<Token>, Expression>(name, args, Corpus)); }
            return new Def_Func(name, args, Corpus);
        }
        public Expression ParseExp(int parentPrecedence = 0)
        {
            Expression left;
            var unaryOperatorPrecedence = GetUnaryOperatorPrecedence(tokens[nextToken].Type);
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = tokens[nextToken++];
                var operand = ParseExp(unaryOperatorPrecedence);
                left = new UnaryExpression(operatorToken, operand);
            }
            else left = ParseExpAtomicLevel();
            while (true)
            {
                var precedence = GetBinaryOperatorPrecedence(tokens[nextToken].Type);
                if (precedence == 0 || precedence <= parentPrecedence) break;

                var operatorToken = tokens[nextToken++];
                var right = ParseExp(precedence);
                left = new BinaryExpr(operatorToken, left, right);
            }
            return left;
        }
        Expression ParseExpAtomicLevel()//agregar llamado a funcion matematica
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
                        if (Program.errors.Count == 0) Contexto.variables_scope.Add(new Tuple<string, Expression>(identifier_1, value_1));
                        else return null;
                        int currtoken = nextToken;
                        if (Match(Lexer.TokenType.coma))
                        {
                            var identifier_2 = tokens[nextToken++].Value;
                            Match_value("=");
                            var value_2 = ParseExp();
                            if (!Match_value("in")) { Program.errors.Add(new Error("Syntax ERROR:Missing \" in \" on let_in expression on col:", tokens[nextToken].Index)); nextToken -= 1; }
                            var in_E = ParseStat();

                            if (Program.errors.Count == 0) Contexto.variables_scope.Add(new Tuple<string, Expression>(identifier_2, value_2));
                            else return null;

                            return new Letvar(identifier_1, identifier_2, value_1, value_2, in_E);
                        }
                        nextToken = currtoken;
                        if (!Match_value("in")) { Program.errors.Add(new Error("Syntax ERROR:Missing \" in \" on let_in expression on col:", tokens[nextToken].Index)); nextToken -= 1; }
                        var inE = ParseStat();
                        return new Letvar(identifier_1, null, value_1, null, inE);
                    }
                }
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.number))//Parsea número
            {
                return new Number(tokens[nextToken - 1].Value);
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.leftparenthesis)) //Parsea (E)
            {
                var inE = ParseExp();
                if (!Match(Lexer.TokenType.rigthparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ) \" on col:", tokens[nextToken].Index)); nextToken -= 1; }
                return new ParenE(inE);
            }
            nextToken = currToken;
            if (Match(Lexer.TokenType.identifier))//Parsea variable o llamada a funcion
            {
                var name = tokens[nextToken - 1].Value;
                int curr = nextToken;
                if (Match(Lexer.TokenType.leftparenthesis))
                {
                    List<Expression> args = new List<Expression>();
                    if (!Match(Lexer.TokenType.rigthparenthesis)) { nextToken -= 1; args.Add(ParseExp()); }
                    else nextToken -= 1;
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
            nextToken = currToken;
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
                    var bas = ParseExp();
                    Match(Lexer.TokenType.coma);
                    var up = ParseExp();
                    Match(Lexer.TokenType.rigthparenthesis);
                    return new Math_Log(bas, up);
                }
                else
                {
                    string operation = tokens[nextToken - 1].Value;
                    Match(Lexer.TokenType.leftparenthesis);
                    var a = ParseExp();
                    Match(Lexer.TokenType.rigthparenthesis);
                    return new Math_Func(operation, a);
                }
            }
            nextToken = currToken;
            if (Match_value("print"))//Parsea PRINT
            {
                if (!Match(Lexer.TokenType.leftparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ( \" on col:", tokens[nextToken].Index)); nextToken -= 1; }
                var inE = ParseStat();
                if (!Match(Lexer.TokenType.rigthparenthesis)) { Program.errors.Add(new Error("Syntax ERROR:Missing \" ) \" on col:", tokens[nextToken].Index)); nextToken -= 1; }
                return new Print(inE);

            }
            nextToken = currToken;
            if (Match_value("PI"))//Parsea alguna de las constantes
            {
                return new Ctx("PI");
            }
            nextToken=currToken;
            if (Match_value("true"))
            {
                return new TokenBoolean("true");
            }
            nextToken = currToken;
            if (Match_value("false"))
            {
                return new TokenBoolean("false");
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
