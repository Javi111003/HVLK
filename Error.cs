using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
     public class Error:Expression
    {
        string message;
        int position;
        public Error(string _message, int _position)
        {
            message = _message;
            position = _position;
        }
        public override object Evaluate()
        {
            string result= this.message + $"{position}";
            Console.WriteLine(result);
            return this.message + $"{position}";
        }
    }
   
}
