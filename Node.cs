namespace HVLK
{
	/*public interface IContext
	{
		bool IsDefined(string variable);
		bool IsDefined(string funcion, int args);
		bool Define(string variable);
		bool Define(string funcion, string[] args);
		IContext CreateChildContext();
	}

	public class Context : IContext
	{
		IContext parent;
		HashSet<string> variables = new HashSet<string>();
		Dictionary<string, string[]> functions = new Dictionary<string, string[]>();

		public bool IsDefined(string variable)//verifica si la variable ha sido definida 
		{
			if (variables.Contains(variable) || (parent != null && parent.IsDefined(variable)))
			{
				return true;
			}
			return false;
		}
		public bool IsDefined(string function, int args)//verifica si la funcion ya ha sido definida
		{
			if (functions.ContainsKey(function))
			{
				return true;
			}
			else return false;
		}
		public bool Define(string variable)
		{
			return variables.Add(variable);//definimos la variable
		}
		public bool Define(string function, string[] args)
		{
			if (functions.ContainsKey(function))//si no esta definida
			{
				return false;
			}

			functions[function] = args;//la definimos
			return true;

		}
		public IContext CreateChildContext()
		{
			return new Context() { parent = this };
		}
	}


	]
	*/

	public abstract class Node
	{
		protected List<Node> children=new List<Node>();

	}
	public abstract class Expression : Node
	{
		public abstract object Evaluate();

	}
	public class Letvar : Expression
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
		public override object Evaluate()
		{
			return inE.Evaluate();
		}

	}
	public class Print : Expression
	{
		public Expression expr;

		public Print(Expression _expr)
		{
			expr = _expr;
		}
		public override object Evaluate()
		{
			return expr.Evaluate();
		}
	}
	public class Def_Func : Expression
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
            return Body.Evaluate();
        }
    }
	public class BinaryExpr : Expression
	{
		public Lexer.TokenType Op;
		public Expression MI;
		public Expression MD;

		public BinaryExpr(Lexer.TokenType op, Expression L, Expression R)
		{
			Op = op;
			MI = L;
			MD = R;
		}
		public override object Evaluate()
		{
			switch (Op)
			{
				case Lexer.TokenType.sum: return (double)MI.Evaluate() + (double)MD.Evaluate();

				case Lexer.TokenType.minus: return (double)MI.Evaluate() - (double)MD.Evaluate();

				case Lexer.TokenType.mult: return (double)MI.Evaluate() * (double)MD.Evaluate();

				case Lexer.TokenType.divide: return (double)MI.Evaluate() / (double)MD.Evaluate();

				case Lexer.TokenType.residous: return (double)MI.Evaluate() % (double)MD.Evaluate();

				case Lexer.TokenType.power: return Math.Pow((double)MI.Evaluate() ,(double)MD.Evaluate());

				default:return 0;

			}
		}
	}
	public class UnaryExpression : Expression
    {
		Lexer.TokenType Op;
		Expression MD;
		public UnaryExpression(Lexer.TokenType op,Expression e)
        {
			Op = op;
			MD= e;

        }
        public override object Evaluate()
        {
            if (Op == Lexer.TokenType.minus)
            {
				return -(double)MD.Evaluate();
            }
			else if (Op == Lexer.TokenType.sum)
            {
				return MD;
            }
			return MD;
        }
    }
	/*public class Sum : BinaryExpr
    {
	public Sum(Expression L, Expression R) : base(Lexer.TokenType.sum, L, R)
        {
        }
    }
	*/
	public class Func_call : Expression
	{
		public string identifier;
		public List<Expression> args;

		public Func_call(string id, List<Expression> _args)
		{
			identifier = id;
			args = _args;
		}
		public override object Evaluate()//////////////////FALTA
		{
			throw new NotImplementedException();
		}
	}

	public class Variable : Expression
	{
		public string identifier;

		public Variable(string name)
		{
			identifier = name;
		}
		public override object Evaluate()
		{
			for (int i = Contexto.variables_scope.Count - 1; i >= 0; i--)
			{
				if (Contexto.variables_scope.ContainsKey(identifier))			
				{
					return Contexto.variables_scope[identifier].Evaluate();
				}
			}
			return null;
		}
	}
	public class Ctx:Expression
	{
		Dictionary<string, double> constantes = new Dictionary<string, double>();
		Dictionary<string, double> ctxs = new Dictionary<string, double>();
		string id;

		public Ctx(string _id)
		{
			ctxs["PI"] = 3.14;
			ctxs["E"] = 2.36;
			id = _id;
		}
		public double Get(string name)
		{
			double _value = ctxs[name];
			return _value;
		}
        public override object Evaluate()
        {
			return (double)Get(id);
        }
    }
	public class Number : Expression
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
	public class Lit_String : Expression
	{
		public string lit;

		public Lit_String(string content)
		{
			lit = content;
		}
		public override object Evaluate()
		{
			return (string)lit;
		}
	}
	public class ParenE : Expression
    {
		Expression inE;
		public ParenE(Expression inside)
        {
			inE= inside;
        }
        public override object Evaluate()
        {
            return inE.Evaluate();
        }
    }
	public class If_Else_Exp : Expression
	{
		Expression condition;
		Expression in_Exp;
		Expression else_Exp;
		public If_Else_Exp(Expression cond,Expression inE,Expression elseE)
        {
			condition = cond;
			in_Exp = inE;
			else_Exp = elseE;
        }
        public override object Evaluate()///////////////////Falta
        {
            throw new NotImplementedException();
        }
    }
	public class Math_Func : Expression
    {
		Expression inside;
		string operation;
		public Math_Func(string _operation,Expression inE)
        {
			operation = _operation;
			inside = inE;
        }
        public override object Evaluate()
        {
			Ctx E = new Ctx("E");
            switch (operation)
            {
				case "sen":return (double)Math.Sin((double)inside.Evaluate());

				case "cos": return (double)(Math.Cos((double)inside.Evaluate()));

				case "sqrt": return (double)Math.Sqrt((double)inside.Evaluate());

				case "exp": return (double)Math.Pow(E.Get("E"),(double)inside.Evaluate());

				default:return null;
			}
            
        }
    }
	public class Math_Log: Expression
    {
		Expression bas;
		Expression uper;
		public Math_Log(Expression _bas ,Expression up)
        {
			bas = _bas; 
			uper = up;
        }
        public override object Evaluate()
        {
			return (double)(Math.Log((double)uper.Evaluate(), (double)bas.Evaluate()));
        }
    }
}



