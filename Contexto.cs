using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    public class Contexto
    {
        public static Dictionary<string,Expression>variables_scope=new Dictionary<string,Expression>();
        //public static Tuple<string, List<Expression>, Expression> function_scope = new();
        //Dictionary<string,int>functions=new Dictionary<string,int>();

        public static bool IsDefined(string id)
        {
            if (variables_scope.ContainsKey(id))
            {
                return true;
            }
            return false;
        }
        public static void Reset()
        {
            variables_scope = new Dictionary<string, Expression>();
        }
    }
   
}

