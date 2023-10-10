using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
    public class Contexto
    {
        public static List<Tuple<string,Expression>>variables_scope= new();
        public static List<Tuple<string, List<Token>, Expression,Scope>> function_scope = new();
        //Dictionary<string,int>functions=new Dictionary<string,int>();

        public static bool IsDefined(string id)
        {
            foreach (var item in variables_scope)
            {
                if (item.Item1==id)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsFunctionDefined(string id)
        {
            foreach(var item in Contexto.function_scope)
            {
                if (item.Item1 == id)
                {
                    return true;
                }
            }
            return false;
        }
        public static void Reset()
        {
            Contexto.variables_scope = new();
        }
    }
   
}

