/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   HVLK
{
     class Expression
    {
        public virtual int Evaluate(Expression U)
        {
            return 0;
        }
        
    }
    class Bexpression : Expression
    {
        protected Expression M_I;
        protected Expression M_D;

        public Bexpression(Expression _mi, Expression _md)
        {
            M_I = _mi; M_D = _md;
        }

    }
    class Uexpression : Expression
    {
        protected Expression U;

        public Uexpression(Expression _U) 
        { 
            U = _U;
        }

    }
    class Sum : Bexpression
    {

        public Sum(Expression M_I, Expression M_D)
        {
            base(M_I,M_D);
        }      
            
        

        public override int Evaluate(Expression U)
        {
            return Evaluate(MI)+Evaluate(MD);
        }

    }

}
*/