using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HVLK;


namespace HVLK
{
    public class Contexto
    {
        public static List<Tuple<string, Expression>> variables_scope = new();//almacena las variables
        public static List<Tuple<string, List<Token>, Expression>> function_scope = new();//almacena las funciones

        public static bool IsDefined(string id)
        {
            foreach (var item in variables_scope)
            {
                if (item.Item1 == id)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsFunctionDefined(string id)
        {
            foreach (var item in function_scope)
            {
                if (item.Item1 == id)
                {
                    return true;
                }
            }
            return false;
        }
        public static void Reset()//restablecer el contexto de variables ya que son "in_line"
        {
            variables_scope = new();
        }
    }

}

