using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HVLK;

namespace HVLK
{
    public class Error : Expression//Errores deriva de expresion para que pueda ser devuelto por cualquiera de los componentes del compilador y puedan ser evaluados
    {
        string message;
        int position;
        public Error(string _message, int _position)
        {
            message = _message;
            position = _position;
        }
        public override object Evaluate()//Evaluan mostrando en pantalla el error , su tipo y su localizacion
        {
            string result = message + $"{position}";
            Console.WriteLine(result);
            return message + $"{position}";
        }
    }

}
