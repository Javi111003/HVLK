using System.Threading.Channels;
using System.Xml.Linq;
using HVLK;


namespace HVLK
{


    public abstract class Node//nodo raiz del AST
    {
        protected List<Node> children = new List<Node>();
    }
    public abstract class Expression : Node//Clase abstracta expresion 
    {
        public abstract object Evaluate();

    }
    public class Letvar : Expression//declaracion y uso de variables inline
    {
        string identifier1;
        string identifier2;
        Expression value1;
        Expression value2;
        Expression inE;

        public Letvar(string _id1, string _id2, Expression expr1, Expression expr2, Expression _inE)
        {
            identifier1 = _id1;
            identifier2 = _id2;
            value1 = expr1;
            value2 = expr2;
            inE = _inE;
        }
        public override object Evaluate()//Evaluando el cuerpo del "in"
        {
            return inE.Evaluate();
        }

    }
    public class Print : Expression//funcion Print de toda la vida
    {
        public Expression expr;

        public Print(Expression _expr)
        {
            expr = _expr;
        }
        public override object Evaluate()
        {
            var result = expr.Evaluate();
          
            return result;
        }
    }
    public class Def_Func : Expression//definicion de función
    {
        public string name;
        public List<Token> args;
        public Expression Body;

        public Def_Func(string _name, List<Token> _args, Expression expr)
        {
            name = _name;
            args = _args;
            Body = expr;
        }
        public override object Evaluate()
        {
            return $"The function \"{name}\" has been defined succesfully";
        }
    }
    public class BinaryExpr : Expression//Tipo que almacena todas las expresiones binarias ,tanto aritmeticas como booleanas o concatenacion de strings 
    {
        public Token Op;
        public Expression MI;
        public Expression MD;

        public BinaryExpr(Token op, Expression L, Expression R)
        {
            Op = op;
            MI = L;
            MD = R;
        }
        public override object Evaluate()//Realiza la evaluacion según el operador y controlando los errores semanticos
        {
            switch (Op.Type)
            {
                case Lexer.TokenType.sum:
                    try
                    {
                        return (double)MI.Evaluate() + (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '+' only can be used between two numbers on line:", Program.l));return null;
                    }

                case Lexer.TokenType.minus:
                    try
                    {
                        return (double)MI.Evaluate() - (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '-' only can be used between two numbers on line:", Program.l)); return null;
                    }

                case Lexer.TokenType.mult:
                    try
                    {
                        return (double)MI.Evaluate() * (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '*' only can be used between two numbers on line:", Program.l)); return null;
                    }

                case Lexer.TokenType.divide:
                    var MDeval = (double)MD.Evaluate();
                    if (MDeval == 0) { Program.errors.Add(new Error("Semantic Error:The divide '/' for zero is indefined,error on line:", Program.l)); return null; }
                    else 
                    { 
                        try
                        {
                            return (double)MI.Evaluate() / MDeval;
                        }
                        catch (Exception)
                        {
                            Program.errors.Add(new Error("Semantic Error:The operator '/' only can be used between two numbers on line:", Program.l)); return null;
                        }
                    }

                case Lexer.TokenType.residous:
                    try
                    {
                        return (double)MI.Evaluate() % (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '%' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case Lexer.TokenType.power:
                    try
                    {
                        return Math.Pow((double)MI.Evaluate(), (double)MD.Evaluate());
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '^' only can be used between two numbers on line:", Program.l)); return null;
                    }
                    
                case Lexer.TokenType.concat:
                    try
                    {
                        return (string)MI.Evaluate() + MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            return MI.Evaluate() + (string)MD.Evaluate();
                        }
                        catch (Exception)
                        {
                            Program.errors.Add(new Error("Semantic Error:One of elements not can be converted to string , on line:", Program.l)); return null;
                        }
                    }

                default: return Evaluateboolean();

            }
        }
        public object Evaluateboolean()//Realiza la evaluacion según el operador y controlando los errores semanticos
        {
            switch (Op.Value)
            {
                case ">":
                    try
                    {
                        return (double)MI.Evaluate() > (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '>' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case ">=":
                    try
                    {
                        return (double)MI.Evaluate() >= (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '>=' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case "<":
                    try
                    {
                        return (double)MI.Evaluate() < (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '<' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case "<=":
                    try
                    {
                        return (double)MI.Evaluate() <= (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '<=' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case "==":
                    try
                    {
                        return (double)MI.Evaluate() == (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '==' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case "!=":
                    try
                    {
                        return (double)MI.Evaluate() != (double)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '!=' only can be used between two numbers on line:", Program.l)); return null;
                    }
                case "|":
                    try
                    {
                        return (bool)MI.Evaluate() || (bool)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '|' only can be used between two booleans on line:", Program.l)); return null;
                    }
                case "&":
                    try
                    {
                        return (bool)MI.Evaluate() && (bool)MD.Evaluate();
                    }
                    catch (Exception)
                    {
                        Program.errors.Add(new Error("Semantic Error:The operator '&' only can be used between two booleans on line:", Program.l)); return null;
                    }

                default: return null;
            }
        }
    }
    public class UnaryExpression : Expression//usado para que expresiones de cualquier tipo ,pueda retornar su valor negativo si es lo que se quiere y asi el parser no detecte una expresion binaria incompleta
    {
        Token Op;
        Expression MD;
        public UnaryExpression(Token op, Expression e)
        {
            Op = op;
            MD = e;

        }
        public override object Evaluate()//Realiza la evaluacion según el operador y controlando los errores semanticos
        {
            if (Op.Type == Lexer.TokenType.minus)
            {
                try
                {
                    return -(double)MD.Evaluate();
                }
                catch(Exception)
                {
                    Program.errors.Add(new Error("Semantic Error:The operator '-' only can be used with numbers on line:", Program.l)); return null;
                }
            }
            else if (Op.Type == Lexer.TokenType.sum)
            {
                try
                {
                    return MD.Evaluate();
                }
                catch (Exception)
                {
                    Program.errors.Add(new Error("Semantic Error:The operator '+' only can be used with numbers on line:", Program.l)); return null;
                }
            }
            else if (Op.Value == "!")
            {
                try
                {
                    return !(bool)MD.Evaluate();
                }
                catch (Exception)
                {
                    Program.errors.Add(new Error("Semantic Error:The operator '!' only can be used with booleans on line:", Program.l)); return null;
                }
                
            }
            return MD.Evaluate();
        }
    }
    public class Func_call : Expression//Almacena y evalúa llamados a funciones , adjudicando a cada llamado sus argumentos e introduciondelos en el contexto para luego ser usados y desechados al retornar el valor 
    {
        public string identifier;
        public List<Expression> args;

        public Func_call(string id, List<Expression> _args)
        {
            identifier = id;
            args = _args;
        }
        public override object Evaluate()
        {

            object result = null;
            foreach (var item in Contexto.function_scope)
            {
                List<Tuple<string, Expression>> aux = new List<Tuple<string, Expression>>();
                if (item.Item1 == identifier)
                {
                    if (item.Item2.Count != args.Count)
                    {
                        Program.errors.Add(new Error($"Still you don´t give all the arguments for function {identifier} on line: ", 1)); return null;
                    }
                    for (int i = 0; i < args.Count; i++)//Garantiza aplicar cambios a los argumentos en los llamados recursivos
                    {
                        var a = args[i].Evaluate();
                        var toks = Lexer.Tokenizar(a.ToString());
                        var l = new RecursiveParser(toks);
                        var exp = l.ParseExp();
                        aux.Add(new Tuple<string, Expression>(item.Item2[i].Value, exp));
                    }
                    Program.MAX_IT++;//controlando stackoverflow
                    /*if(Program.MAX_IT == 25000)
					{
						Program.errors.Add(new Error("Stack Overflow:the stack is disborded for +25000 function calls on line :", 1));break;
					}*/
                    for (int i = 0; i < aux.Count; i++)
                    {
                        Contexto.variables_scope.Add(aux[i]);
                    }
                    aux.Clear();
                    result = item.Item3.Evaluate();
                    if (Contexto.variables_scope.Count != 0) Contexto.variables_scope.RemoveAt(Contexto.variables_scope.Count - 1);//Eliminando las variables que se crean en los llamados recursivos
                    return result;
                }
            }
            if (Program.errors.Count > 0) return null;
            else
            {
                Contexto.Reset(); return result;//Contexto limpio para retornar el resultado y esperar la proxima entrada 
            }
        }
    }

    public class Variable : Expression
    {
        public string identifier;

        public Variable(string name)
        {
            identifier = name;
        }
        public override object Evaluate()//retorna el valor de  la expresion asignada a la variable en el momento del Analisis sintactico y construccion del AST
        {
            for (int i = Contexto.variables_scope.Count - 1; i >= 0; i--)
            {
                if (Contexto.variables_scope[i].Item1 == identifier)//Si existe la variable , retornamos el valor
                {
                    var value = Contexto.variables_scope[i].Item2.Evaluate();
                   
                    return value;
                }
            }
            return new Error($"Doesn't exist the var {identifier} on the actual context on line: ", 0).Evaluate();//si la variable no existe 
        }
    }
    public class Ctx : Expression//Almacenar las constantes
    {
        Dictionary<string, double> ctxs = new Dictionary<string, double>();
        string id;

        public Ctx(string _id)
        {
            ctxs["PI"] = Math.PI;
            ctxs["E"] = Math.E;
            id = _id;
        }
        public double Get(string name)
        {
            return ctxs[name];
        }
        public override object Evaluate()//obtener el valor de la constante invocada por su nombre
        {
            return Get(id);
        }
    }
    public class Number : Expression//Manejo de números
    {
        public double value;

        public Number(string _value)
        {
            value = double.Parse(_value);
        }

        public override object Evaluate()
        {
            return value;
        }
    }
    public class Lit_String : Expression//Manejo de strings
    {
        public string lit;

        public Lit_String(string content)
        {
            lit = content;
        }
        public override object Evaluate()
        {
            return $"\"{lit}\"";
        }
    }
    public class ParenE : Expression
    {
        Expression inE;
        public ParenE(Expression inside)
        {
            inE = inside;
        }
        public override object Evaluate()
        {
            return inE.Evaluate();
        }
    }
    public class If_Else_Exp : Expression
    {
        Expression condition;
        Expression if_Exp;
        Expression else_Exp;
        public If_Else_Exp(Expression cond, Expression inE, Expression elseE)
        {
            condition = cond;
            if_Exp = inE;
            else_Exp = elseE;
        }
        public override object Evaluate()
        {
            if ((bool)condition.Evaluate())
            {
                return if_Exp.Evaluate();
            }
            return else_Exp.Evaluate();
        }
    }
    public class Math_Func : Expression
    {
        Expression inside;
        string operation;
        public Math_Func(string _operation, Expression inE)//Funciones trigonometricas exponencial y raiz cuadrada
        {
            operation = _operation;
            inside = inE;
        }
        public override object Evaluate()
        {
            switch (operation)
            {
                case "sen": return Math.Sin((double)inside.Evaluate());

                case "cos": return Math.Cos((double)inside.Evaluate());

                case "sqrt": return Math.Sqrt((double)inside.Evaluate());

                case "exp": return Math.Pow(Math.E, (double)inside.Evaluate());

                default: return null;
            }

        }
    }
    public class Math_Log : Expression//Funcion logaritmica
    {
        Expression bas;
        Expression uper;
        public Math_Log(Expression _bas, Expression up)
        {
            bas = _bas;
            uper = up;
        }
        public override object Evaluate()
        {
            return Math.Log((double)uper.Evaluate(), (double)bas.Evaluate());
        }
    }
    public class TokenBoolean : Expression//Manejo de true or false 
    {
        string v;
        public TokenBoolean(string _v)
        {
            v= _v;
        }
        public override object Evaluate()
        {
            if (v == "true") return true;

            else return false;

        }
    }
}



