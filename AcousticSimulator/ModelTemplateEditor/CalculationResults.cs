using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelLib;

namespace ModelEditor
{
    public partial class CalculationResults : Form
    {
        private Dictionary<calcpoint, Dictionary<NoiseSource, noise>> r;

        public CalculationResults(Dictionary<calcpoint, Dictionary<NoiseSource, noise>> r)
        {
            InitializeComponent();
            foreach (var sp in r)
                foreach (var np in sp.Value)
                {
                    string sname = (np.Key == null) ? "Structural" : np.Key.name;
                    dg.Rows.Add(sp.Key.name, sname, np.Value.lvl315.value, np.Value.lvl63.value,
                        np.Value.lvl125.value, np.Value.lvl250.value, np.Value.lvl500.value,
                        np.Value.lvl1000.value, np.Value.lvl2000.value, np.Value.lvl4000.value,
                        np.Value.lvl8000.value, np.Value.dBA.value, sp.Key.pos.X, sp.Key.pos.Y, sp.Key.pos.Z);
                }
        }
    }
}
