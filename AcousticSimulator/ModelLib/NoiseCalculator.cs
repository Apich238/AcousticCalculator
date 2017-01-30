using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    class NoiseCalculator
    {
        public NoiseCalculator(Model m)
        {
            _m = m;
        }
        Model _m;

        public void calc()
        {
            traceres = new Dictionary<NoiseSource, List<pseudoNS>>();
            foreach (var s in _m.sources) tracesrc(s);


            res = new Dictionary<calcpoint, Dictionary<NoiseSource, noise>>();
            foreach (var c in _m.calcpts) reportcalcpt(c);
        }

        void tracesrc(NoiseSource s)//находит для заданного начального источника все конечные псевдоисточники
        {
            //init
            pseudoNS p = new pseudoNS();
            p.direction = Vector3d.Zero;
            p.pos = s.pos;
            if (s is LinearNoiseSrc) p.pos = ((s as LinearNoiseSrc).A + (s as LinearNoiseSrc).B) / 2;
            p.power = s.power;
            p.restrictedBord = new List<Border>();
            p.restrictedRefl = new List<Reflector>();
            p.room = s.room;
            List<pseudoNS> done = new List<pseudoNS>();
            Queue<pseudoNS> q = new Queue<pseudoNS>();
            q.Enqueue(p);
            //walk
            while (q.Count > 0)
            {
                if (q.Count == 0) return;
                var pp = q.Dequeue();
                done.Add(pp);
                if (pp.room == null) foreach (var ppp in getReflectedpns(pp)) q.Enqueue(ppp);
                List<Border> r = GetReachableBorders(pp);
                if (r.Count > 0) foreach (var b in r) q.Enqueue(getnewpns(pp, b));
            }
            traceres.Add(s, done);
        }

        private List<pseudoNS> getReflectedpns(pseudoNS pp)
        {
            return GetReachableReflectors(pp).Select(r => GetMirroredPNS(pp, r)).ToList();
        }

        private void reportcalcpt(calcpoint c)//создаёт сводку шумов для данной расчётной точки и всех источников
        {
            res.Add(c, new Dictionary<NoiseSource, noise>());
            var d = res[c];
            foreach (var p in _m.sources)
            {
                noise n;
                if (c.room == null && p.room == null) n = p.GetLevel(c.pos);//препятствия не учитываются
                else
                {
                    List<pseudoNS> ps = traceres[p].Where(q => object.ReferenceEquals(q.room, c.room)).ToList();
                    n = GetNoiseSummInRoom(ps, c);
                }
                d.Add(p, n);
            }
        }

        private noise GetNoiseSummInRoom(List<pseudoNS> ps, calcpoint c)
        {
            noise res;
            //фильтруем направленные в другие стороны, если источник - снаружи
            if (c.room == null)
            {
                ps = ps.Where(p => Vector3d.Dot(c.pos - p.pos, p.direction) > 0).ToList();
                return getnoisessummoutside(ps, c);
            }
            else
            {
                var n = ps.Where(qq => (qq.pos - c.pos).Length <= 5 * ps.Min(q => (q.pos - c.pos).Length)).ToList();//ближайшие
                res = getfullnoise(ps, c) + GetNearestInRoomNoise(n, c);
                return res;
            }
        }

        noise getnoisessummoutside(List<pseudoNS> n, calcpoint c)//считает суммарный шум для внешней расчётной точки и внутренних источников
        {
            noise res = null;

            foreach (var x in n)
            {
                double d = (c.pos - x.pos).Length;
                noise t = new noise();

                t.lvl315.value = x.power.lvl315.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.00 : 0);
                t.lvl63.value = x.power.lvl63.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.00 : 0);
                t.lvl125.value = x.power.lvl125.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.0007 : 0);
                t.lvl250.value = x.power.lvl250.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.0015 : 0);
                t.lvl500.value = x.power.lvl500.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.003 : 0);
                t.lvl1000.value = x.power.lvl1000.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.006 : 0);
                t.lvl2000.value = x.power.lvl2000.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.012 : 0);
                t.lvl4000.value = x.power.lvl4000.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.024 : 0);
                t.lvl8000.value = x.power.lvl8000.value - 20 * Math.Log10(d) + 10 * Math.Log10(1 / Math.PI) - ((d > 50) ? d * 0.048 : 0);
                if (res == null) res = t;
                else res += t;

            }
            return res;

        }

        noise getfullnoise(List<pseudoNS> n, calcpoint c)//сумма шума в комнате без учёта ближнего воздействия
        {
            noise res = null, t = new noise();
            double p = 4 / (k(c.room) * RoomConst(c.room));
            foreach (var x in n)
            {
                t = new noise();
                t.lvl315.value =10*Math.Log10(p * Math.Pow(10, 0.1 * x.power.lvl315.value));
                t.lvl63.value =  10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl63.value));
                t.lvl125.value = 10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl125.value));
                t.lvl250.value = 10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl250.value));
                t.lvl500.value = 10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl500.value));
                t.lvl1000.value =10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl1000.value));
                t.lvl2000.value =10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl2000.value));
                t.lvl4000.value =10*Math.Log10( p * Math.Pow(10, 0.1 * x.power.lvl4000.value));
                t.lvl8000.value = 10 * Math.Log10(p * Math.Pow(10, 0.1 * x.power.lvl8000.value));
                if (res == null) res = t;
                else res += t;
            }
            if (res == null) return new noise(); return res;
        }

        noise GetNearestInRoomNoise(List<pseudoNS> n, calcpoint c)
        {
            noise r = new noise();
            if (n.Count == 0) return r;
            r = nerarestnoise(n.First(), c);
            for (int i = 1; i < n.Count; i++)
                r += nerarestnoise(n[i], c);
            return r;
        }

        noise nerarestnoise(pseudoNS n, calcpoint c)
        {
            noise s = new noise();
            double r = (c.pos - n.pos).Length;
            double ks = chi(r / n.maxlinsz) / (Math.PI * Math.Pow(r, 2));
            s.lvl315.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl315.value));
            s.lvl63.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl63.value));
            s.lvl125.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl125.value));
            s.lvl250.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl250.value));
            s.lvl500.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl500.value));
            s.lvl1000.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl1000.value));
            s.lvl2000.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl2000.value));
            s.lvl4000.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl4000.value));
            s.lvl8000.value = 10 * Math.Log10(ks * Math.Pow(10, 0.1 * n.power.lvl8000.value));
            return s;
        }

        double RoomConst(Room r)//B
        {
            if (r == null) return 0;
            return EqualSquare(r) / (1 - AbsorptionMiddle(r));
        }

        private double AbsorptionMiddle(Room r)
        {
            var ss = SummSquare(r);
            if (ss == 0) return 0;
            return EqualSquare(r) / SummSquare(r);
        }

        double k(Room r)
        {
            return 1 / (1 - AbsorptionMiddle(r));
        }

        private Double SummSquare(Room r)
        {
            return _m.borders.Where(b => (object.ReferenceEquals(b.room1, r) || object.ReferenceEquals(b.room2, r))).Sum(b => b.Rectangle.Square);
        }

        private double EqualSquare(Room r)
        {
            return _m.borders.Where(b => (object.ReferenceEquals(b.room1, r) || object.ReferenceEquals(b.room2, r))).Sum(b => b.Rectangle.Square * absorption(b, r));//+штучные поглотители - потом
        }


        double chi(double x)
        {
            if (x <= 0) return 4;
            if (x >= 2) return 1;
            double[] X = new double[] { 0, 0.5, 1, 1.5, 2 };
            double[] Y = new double[] { 4, 3.75, 2, 1.3, 1 };
            double[] YP = new double[] { 0, -1, -1.5, -1, 0 };
            for (int i = 0; i < X.Length - 1; i++)
            {
                if (X[i] <= x && x < X[i + 1])
                {
                    double a = Y[i], b = YP[i], c = Y[i + 1], d = YP[i + 1];
                    double al = (d + b) / 0.25 - 2 * (c - a) / 0.125, be = 3 * (c - a) / 0.25 - (d + 2 * b) / 0.5;
                    return al * Math.Pow(x - X[i], 3) + be * Math.Pow(x - X[i], 2) + b * (x - X[i]) + a;
                }
            }
            return 0;
        }

        class pseudoNS
        {
            public Room room;
            public Vector3d pos;
            public noise power;
            public List<Border> restrictedBord;
            public List<Reflector> restrictedRefl;
            public Vector3d direction;
            public double maxlinsz;
        }
        bool BorderIsReachable(pseudoNS p, Border b)
        {
            return !p.restrictedBord.Contains(b) &&
                (p.direction == Vector3d.Zero ||
                 p.direction != Vector3d.Zero && Vector3d.Dot(p.direction, b.Rectangle.center - p.pos) > 0) &&
                ((object.ReferenceEquals(b.room1, p.room) && Vector3d.Dot(b.Rectangle.facenorm, (b.Rectangle.center - p.pos)) < 0) ||
                 (object.ReferenceEquals(b.room2, p.room) && Vector3d.Dot(b.Rectangle.facenorm, (b.Rectangle.center - p.pos)) > 0));
        }
        List<Border> GetReachableBorders(pseudoNS pns)//f-search reflections
        {
            return _m.borders.Where(p => BorderIsReachable(pns, p)).ToList();
        }

        pseudoNS GetMirroredPNS(pseudoNS pns, Reflector r)
        {
            pseudoNS p = new pseudoNS();
            p.restrictedBord = pns.restrictedBord;
            p.restrictedRefl = pns.restrictedRefl.Union(new Reflector[] { r }).ToList();
            p.pos = invplane(r.Rectangle, pns.pos);
            p.direction = invplane(r.Rectangle, pns.pos + pns.direction) - p.pos;
            p.maxlinsz = pns.maxlinsz;
            p.power = new noise();

            p.power.lvl315.value = pns.power.lvl315.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d315);
            p.power.lvl63.value = pns.power.lvl63.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d63);
            p.power.lvl125.value = pns.power.lvl125.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d125);
            p.power.lvl250.value = pns.power.lvl250.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d250);
            p.power.lvl500.value = pns.power.lvl500.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d500);
            p.power.lvl1000.value = pns.power.lvl1000.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d1000);
            p.power.lvl2000.value = pns.power.lvl2000.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d2000);
            p.power.lvl4000.value = pns.power.lvl4000.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d4000);
            p.power.lvl8000.value = pns.power.lvl8000.value + 10 * Math.Log10(1 - ReflMatColl.d[r.matid].d8000);


            p.room = null;
            return p;
        }

        bool ReflectorsIsReachable(pseudoNS p, Reflector b)
        {
            return !p.restrictedRefl.Contains(b) &&
                (p.direction == Vector3d.Zero
                || p.direction != Vector3d.Zero && Vector3d.Dot(p.direction, b.Rectangle.lb - p.pos) > 0
                || p.direction != Vector3d.Zero && Vector3d.Dot(p.direction, b.Rectangle.lt - p.pos) > 0
                || p.direction != Vector3d.Zero && Vector3d.Dot(p.direction, b.Rectangle.rb - p.pos) > 0
                || p.direction != Vector3d.Zero && Vector3d.Dot(p.direction, b.Rectangle.rt - p.pos) > 0);
        }
        List<Reflector> GetReachableReflectors(pseudoNS pns)
        {
            if (pns.room == null) return _m.reflectors.Where(p => ReflectorsIsReachable(pns, p) && ! pns.restrictedRefl.Contains(p)).ToList();
            else return new List<Reflector>();
        }

        Dictionary<NoiseSource, List<pseudoNS>> traceres;
        public Dictionary<calcpoint, Dictionary<NoiseSource, noise>> res;
        pseudoNS getnewpns(pseudoNS s, Border b)
        {
            var x = new pseudoNS();
            x.direction = b.Rectangle.facenorm * (object.ReferenceEquals(s.room, b.room1) ? -1 : 1);
            x.room = (object.ReferenceEquals(s.room, b.room1) ? b.room2 : b.room1);
            x.pos = b.Rectangle.center;
            x.restrictedBord = s.restrictedBord.Union(new Border[] { b }).ToList();
            x.power = new noise();
            x.restrictedRefl = s.restrictedRefl;

            if (x.room != null)
            {
                x.power = s.power.GetInDist((b.Rectangle.center - s.pos).Length);
                double d = -GetBorderisolation(b, s.room) + 10 * Math.Log10(b.Rectangle.Square) -
                    10 * Math.Log10(RoomConst(x.room)) - 10 * Math.Log10(k(x.room));
                double w = 10 * Math.Log10(4 / (k(x.room) * RoomConst(x.room)));
                x.power.lvl315.value += d - w;
                x.power.lvl63.value += d - w;
                x.power.lvl125.value += d - w;
                x.power.lvl250.value += d - w;
                x.power.lvl500.value += d - w;
                x.power.lvl1000.value += d - w;
                x.power.lvl2000.value += d - w;
                x.power.lvl4000.value += d - w;
                x.power.lvl8000.value += d - w;
            }
            else
            {
                x.power = s.power.GetInDist((b.Rectangle.center - s.pos).Length);
                double d = +GetBorderisolation(b, s.room)
                    - 10 * Math.Log10(b.Rectangle.Square)
                    + 10 * Math.Log10(RoomConst(s.room));
                x.power.lvl315.value -= d;
                x.power.lvl63.value -= d;
                x.power.lvl125.value -= d;
                x.power.lvl250.value -= d;
                x.power.lvl500.value -= d;
                x.power.lvl1000.value -= d;
                x.power.lvl2000.value -= d;
                x.power.lvl4000.value -= d;
                x.power.lvl8000.value -= d;
            }

            x.maxlinsz = b.Rectangle.maxlinsz;
            return x;
        }
        
        private double GetBorderisolation(Border b, Room room)//звукоизоляция перегородки по направлению к указанной комнате
        {
            return 10 * Math.Log10(1 / t(b, room));
        }

        private double t(Border b, Room room)//(Iпр+Iпогл)/Iпад
        {
            return 1 - refl(b, room);
        }

        private double refl(Border b, Room room)//Iотр/Iпад
        {
            if (object.ReferenceEquals(b.room2, room)) return refl_r(b.layers);
            else return refl_r(b.layers.Reverse<border_layer>().ToList());
        }
   
        
        double absorption(Border b, Room room)//коэффициент звукопоглощения с внутренней стороны комнаты
        {
            if (object.ReferenceEquals(b.room2, room)) return getabs(b.layers);
            else return getabs(b.layers.Reverse<border_layer>().ToList());
        }

        private Double getabs(List<border_layer> list)
        {
            return 1 - pass(list) - refl_r(list);
        }
        double refl_r(List<border_layer> bs)
        {
            if (bs.Count == 0) return 0;
            if (bs.Count == 1) return _m.mdb[bs[0].materialId].reflection;
            double ta = _m.mdb[bs[0].materialId].reflection, tb = refl_r(bs.Skip(1).ToList()), pa = pass(bs);
            return ta + pa * pa * tb / (1 - ta * tb);
        }

        private double pass(List<border_layer> list)
        {
            if (list.Count == 0) return 1;
            if (list.Count == 1) return Math.Min(1 - refl_r(list), _m.mdb[list[0].materialId].absorbparam * list[0].thickness);
            double pa = Math.Min(_m.mdb[list[0].materialId].absorbparam * list[0].thickness, 1 - refl_r(new List<border_layer>() { list[0] })), pb = pass(list.Skip(1).ToList()),
                ta = refl_r(new List<border_layer>() { list[0] }), tb = refl_r(list.Skip(1).ToList());
            return pa * pb / (1 - tb * ta);
        }


        Vector3d invplane(spacerect rc,Vector3d d)//считает зеркальную точку
        {
            return d - 2 * Vector3d.Dot(d - rc.ontop, rc.facenorm) * rc.facenorm;
        }





        /*
         * 1. для начального источника находятся все достижимые перегородки.
         * 1.0. -не входящие в список запрещённых
         * 1.1. -находящиеся в той же комнате
         * 1.2. -находящиеся также снаружи и внутренняя к помещению нормаль и вектор до центра перегородки от источника сонаправлены
         * 3. для каждой достижимой перегородки:
         * 3.1. в помещении расчитывается шум, создаваемый только через эту перегородку, с учётом постоянной помещения, звукопоглощения и звукоизоляции
         * 3.2. по этому шуму создаётся эквивалентный источник шума:
         * 3.2.1. -запрещено проходить через перегородку-источник и все запрещённые перегородки родительского источника
         * 3.2.2. -точка источника - в геометрическом центре перегородки
         * 3.2.3. -создаёт в помещении уровень шума как родительский через перегородку
         * 3.3. если источник в комнате с расчётной точкой, он запоминается и дальше не обрабатывается
         * 4. начальный источник забывается
         * 5. всё рекурсивно повторяется для всех источников шага 3
         * 6. в расчётной точке складываются все источники, запомненные на шаге 3.3.
         * 
         */



    }


}