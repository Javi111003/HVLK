using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVLK
{
     public class Scope
    {
        public Dictionary<string, object> Varguments;

        public Scope()
        {
            Varguments = new Dictionary<string, object>();
        }
        public Scope Child()
        {
            Scope child = new Scope();
            child.Parent = this;

            return child;
        }
        public Scope? Parent{ get;set; }
    }
}
