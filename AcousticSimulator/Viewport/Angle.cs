using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewport
{
    public class Angle
    {
        #region fields_props_consts
        public Angle(float Radians = 0.0f) { this.Radians = Radians; }
        public float Radians;
        public const float pi = (float)Math.PI;
        #endregion
        public float Degrees
        {
            get { return DegreesFromRadians(Radians); }
            set { Radians = RadiansFromDegrees(value); }
        }
        #region conversion
        public static float DegreesFromRadians(float radians) { return radians * 180.0f / pi; }
        public static float RadiansFromDegrees(float degrees) { return degrees * pi / 180.0f; }
        public static Angle FromRadians(float R) { return new Angle(R); }
        public static Angle FromDegrees(float D) { return new Angle(RadiansFromDegrees(D)); }
        public static implicit operator Angle(float FloatValue) { return new Angle(FloatValue); }
        public static implicit operator float(Angle AngleValue) { return AngleValue.Radians; }
        #endregion
    }
}
