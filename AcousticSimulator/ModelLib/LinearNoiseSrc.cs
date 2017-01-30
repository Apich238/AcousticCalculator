using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ModelLib
{
    public class LinearNoiseSrc:NoiseSource
    {
        public Vector3d A, B;
        public override noise GetLevel(Vector3d posit)
        {

            double r = (double)(posit-A+ (B - A).Normalized()* Vector3d.Dot((B-A).Normalized(),posit-A)).Length;
            noise lres = new noise();

            lres.lvl315.value = power.lvl315.value - 15 * Math.Log10(r);
            lres.lvl63.value = power.lvl63.value - 15 * Math.Log10(r);
            lres.lvl125.value = power.lvl125.value - 15 * Math.Log10(r);
            lres.lvl250.value = power.lvl250.value - 15 * Math.Log10(r);
            lres.lvl500.value = power.lvl500.value - 15 * Math.Log10(r);
            lres.lvl1000.value = power.lvl1000.value - 15 * Math.Log10(r);
            lres.lvl2000.value = power.lvl2000.value - 15 * Math.Log10(r);
            lres.lvl4000.value = power.lvl4000.value - 15 * Math.Log10(r);
            lres.lvl8000.value = power.lvl8000.value - 15 * Math.Log10(r);

            return lres;
        }
    }
}
