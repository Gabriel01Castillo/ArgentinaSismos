using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEarthquake.Utils
{
    public class UraniaDMS
    {
        private long glDegrees;
        private long glMinutes;
        private double gfSeconds;
        private long glSign;

        public UraniaDMS()
        {

        }

        public void setSign(double pfNum)
        {
            if (pfNum < 0)
            {
                glSign = -1;
            }
            else
            {
                glSign = 1;
            }
        }

        public int IsPositive()
        {
            if (glSign == -1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void setDegree(long plDegrees)
        {
            setSign(plDegrees);
            glDegrees = Math.Abs(plDegrees);
        }

        public void setMinutes(long plMinutes)
        {
            if (glDegrees == 0)
            {
                setSign(plMinutes);
            }

            glMinutes = Math.Abs(plMinutes);
            while (glMinutes >= 60)
            {
                glMinutes = glMinutes - 60;
                glDegrees = glDegrees + 1;
            }
        }

        public void setSeconds(double pfSeconds)
        {
            if ((glDegrees == 0) && (glMinutes == 0))
            {
                setSign(pfSeconds);
            }
            gfSeconds = Math.Abs(pfSeconds);

            while (gfSeconds >= 60)
            {
                gfSeconds = gfSeconds - 60;
                glMinutes = glMinutes + 1;
            }

            while (glMinutes >= 60)
            {
                glMinutes = glMinutes - 60;
                glDegrees = glDegrees + 1;
            }
        }

        public void setDMS(long plDegrees, long plMinutes, double pfSeconds)
        {
            setSign(plDegrees);
            glDegrees = Math.Abs(plDegrees);
            glMinutes = Math.Abs(plMinutes);
            gfSeconds = Math.Abs(pfSeconds);

            while (gfSeconds >= 60)
            {
                gfSeconds = gfSeconds - 60;
                glMinutes = glMinutes + 1;
            }
            while (glMinutes >= 60)
            {
                glMinutes = glMinutes - 60;
                glDegrees = glDegrees + 1;
            }
        }

        public void setDec(double pfDec)
        {
            double fTmp;
            setSign(pfDec);

            glDegrees = (long)Math.Floor(Math.Abs(pfDec));
            fTmp = (Math.Abs(pfDec) - glDegrees) * 60;
            glMinutes = (long)Math.Floor(fTmp);
            fTmp = (fTmp - glMinutes) * 60;
            gfSeconds = fTmp;
            while (gfSeconds >= 60)
            {
                gfSeconds = gfSeconds - 60;
                glMinutes = glMinutes + 1;
            }
            while (glMinutes >= 60)
            {
                glMinutes = glMinutes - 60;
                glDegrees = glDegrees + 1;
            }
        }

        public long getDegrees()
        {
            return glDegrees * glSign;
        }

        public long getMinutes()
        {
            if (glDegrees == 0)
            {
                return glMinutes * glSign;
            }
            else
            {
                return glMinutes;
            }
        }

        public double getSeconds()
        {
            if ((glDegrees == 0) && (glMinutes == 0))
            {
                return gfSeconds * glSign;
            }
            else
            {
                return gfSeconds;
            }
        }

        public double getDecFormat()
        {
            double fDec;
            fDec = ((double)glDegrees + (glMinutes / 60.0) + ((gfSeconds / 60.0 / 60.0))) * (double)glSign;
            return fDec;
        }

        public string getString(string sFormat)
        {
            string sTmp;

            sTmp = glDegrees.ToString("00") + "° " + glMinutes.ToString("00") + "' " + (gfSeconds.ToString(sFormat)) + "\"";

            if (glSign == -1)
            {
                sTmp = "-" + sTmp;
            }
            return sTmp;
        }
    }  
}