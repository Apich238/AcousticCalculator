using OpenTK;
using System;

namespace ModelLib
{
    public class NoiseSource:pointModelPart
    {
        public NoiseSource()
        {
            power = new noise();
            pos = Vector3d.Zero;
        }
        public noise power;
        public NoiseSource clone()
        {
            var A = new NoiseSource();
            A.power = power.clone();
            A.pos = pos;
            A.room = room;
            A.name = name;
            return A;
        }
        public virtual noise GetLevel(Vector3d posit)//даёт уровень звуковой мощности от этого источника в указанной точке без учёта препятствий
        {
            double r = (double)(pos - posit).Length;
            noise lres = new noise();

            lres.lvl315.value = power.lvl315.value - 20 * Math.Log10(r);
            lres.lvl63.value = power.lvl63.value - 20 * Math.Log10(r);
            lres.lvl125.value = power.lvl125.value - 20 * Math.Log10(r);
            lres.lvl250.value = power.lvl250.value - 20 * Math.Log10(r);
            lres.lvl500.value = power.lvl500.value - 20 * Math.Log10(r);
            lres.lvl1000.value = power.lvl1000.value - 20 * Math.Log10(r);
            lres.lvl2000.value = power.lvl2000.value - 20 * Math.Log10(r);
            lres.lvl4000.value = power.lvl4000.value - 20 * Math.Log10(r);
            lres.lvl8000.value = power.lvl8000.value - 20 * Math.Log10(r);

            return lres;
        }
    }

    public class noise//представляет шум как набор уровней шума на нескольких октавных полосах
    {
        public noise()
        {
            lvl315 = new noiseoctlvl(0);
            lvl63 = new noiseoctlvl(0);
            lvl125 = new noiseoctlvl(0);
            lvl250 = new noiseoctlvl(0);
            lvl500 = new noiseoctlvl(0);
            lvl1000 = new noiseoctlvl(0);
            lvl2000 = new noiseoctlvl(0);
            lvl4000 = new noiseoctlvl(0);
            lvl8000 = new noiseoctlvl(0);

        }
        public noise clone()
        {
            noise A = new noise();
           A.lvl315.value =lvl315.value;
           A.lvl63.value = lvl63.value;
           A.lvl125.value = lvl125.value;
           A.lvl250.value = lvl250.value;
           A.lvl500.value = lvl500.value;
           A.lvl1000.value = lvl1000.value;
           A.lvl2000.value = lvl2000.value;
           A.lvl4000.value = lvl4000.value;
           A.lvl8000.value = lvl8000.value;
            return A;
        }
        public noiseoctlvl  lvl315, lvl63, lvl125, lvl250, lvl500, lvl1000, lvl2000, lvl4000, lvl8000;//уровни соотв. октавных полос
        public noiseoctlvl dBA//возвращает уровень шума в децибелах по шкале А
        {
            get
            {
                noiseoctlvl a = new noiseoctlvl(0);
                a.value = 10 * Math.Log10( Math.Pow(10, 0.1 * (lvl315.value - 42)) +
                Math.Pow(10, 0.1 * (lvl63.value - 26.3)) +
                Math.Pow(10, 0.1 * (lvl125.value - 16.1)) +
                Math.Pow(10, 0.1 * (lvl250.value - 8.6)) +
                Math.Pow(10, 0.1 * (lvl500.value - 3.2)) +
                Math.Pow(10, 0.1 * (lvl1000.value - 0)) +
                Math.Pow(10, 0.1 * (lvl2000.value + 1.2)) +
                Math.Pow(10, 0.1 * (lvl4000.value + 1)) +
                Math.Pow(10, 0.1 * (lvl8000.value + 1.1)));
                return a;
            }
        }
        public static noise operator +(noise A, noise B)//складывает 2 шума
        {
            noise r = new noise();
            r.lvl315 = A.lvl315 + B.lvl315;
            r.lvl63 = A.lvl63 + B.lvl63;
            r.lvl125 = A.lvl125 + B.lvl125;
            r.lvl250 = A.lvl250 + B.lvl250;
            r.lvl500 = A.lvl500 + B.lvl500;
            r.lvl1000 = A.lvl1000 + B.lvl1000;
            r.lvl2000 = A.lvl2000 + B.lvl2000;
            r.lvl4000 = A.lvl4000 + B.lvl4000;
            r.lvl8000 = A.lvl8000 + B.lvl8000;
            return r;
        }

        public noise GetInDist(double dist)
        {
            noise lres = new noise();
            lres.lvl315.value =lvl315.value - 20 * Math.Log10(dist);
            lres.lvl63.value = lvl63.value - 20 * Math.Log10(dist);
            lres.lvl125.value =lvl125.value - 20 * Math.Log10(dist);
            lres.lvl250.value =lvl250.value - 20 * Math.Log10(dist);
            lres.lvl500.value =lvl500.value - 20 * Math.Log10(dist);
            lres.lvl1000.value =lvl1000.value - 20 * Math.Log10(dist);
            lres.lvl2000.value =lvl2000.value - 20 * Math.Log10(dist);
            lres.lvl4000.value =lvl4000.value - 20 * Math.Log10(dist);
            lres.lvl8000.value =lvl8000.value - 20 * Math.Log10(dist);

            return lres;
        }
    }

    public class noiseoctlvl//представляет октавный уровень шума
    {
        public noiseoctlvl(double c)
        { value = c; }
        public double value;
        public static noiseoctlvl operator +(noiseoctlvl a, noiseoctlvl b)
        {
            return new noiseoctlvl(10 * Math.Log10(Math.Pow(10, 0.1 * a.value) + Math.Pow(10, 0.1 * b.value)));
        }
    }
}