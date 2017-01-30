using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialsDatabase
{
    //dictionary inetrface
    //for easy use of database abstraction class
    //in other applications
    //and for ability to use custom databases
    public interface IMatDBDict:IDictionary<Int32,Material>
    {
    }
}
