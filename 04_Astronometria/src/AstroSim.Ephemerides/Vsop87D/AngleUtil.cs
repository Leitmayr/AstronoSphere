using System;

namespace AstroSim.Ephemerides.Vsop87D
{
    internal static class AngleUtil
    {
        public const double DegPerRad = 180.0 / Math.PI;

        // Self made method to wrap angles in [0, 360], e.g. for right ascensions. (ChatGPT hatte das nicht so hinbekommen, deshalb hier die Eigenentwicklung.)
        public static double WrapDegrees360(double deg)
        {
            /**
             *\brief method removing overflow in degrees ( >+360� and <0�)
            */
            double ret = deg;
            if (ret < 0.0)
            {
                while (ret < 0.0) ret += 360.0;
            }
            else
            {
                while (ret > 360.0) ret -= 360.0;
            }

            return (ret);
        }

        // Self made method to wrap angles in [-90, +90], e.g. for declinations. (ChatGPT hatte das nicht so hinbekommen, deshalb hier die Eigenentwicklung.)
        public static double WrapDegrees90(double deg)
        {
            /**
             * \brief method removing overflow in declinations ( >+90� and <-90�)
             */
            double ret = deg;
            if (ret < -90.0)
            {
                while (ret < -90.0) ret += 90.0;
            }
            else
            {
                while (ret > 90.0) ret -= 90.0;
            }

            return (ret);
        }

    }
}