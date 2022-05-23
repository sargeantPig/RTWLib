using RTWLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RTWLib.MapGen.Voronoi
{
    public class LibVoronoi
    {
        public List<VoronoiPoint> points { get; set; }

        public Random rnd;
        public LibVoronoi(Random rnd)
        {
            this.rnd = rnd;
        }

        public void GeneratePoints(int xmin, int xmax, int ymin, int ymax, int distanceBetween, int count, int[,] mask = null, int mScale =1, int maskValue = 0, Func<int, int, bool> op = null, bool traceBounds = false)
        {
            if (traceBounds)
                points = TraceBoundaries(xmin, xmax, ymin, ymax, mask, mScale, maskValue, (x, y) => x < y);
            else points = new List<VoronoiPoint>();

            count = count - points.Count();

            if (mask != null)
            {
                for (int i = 0; i < count; i++)
                {
                    int rx = (int)(rnd.Next(xmin, xmax));
                    int ry = (int)(rnd.Next(ymin, ymax));

                    VoronoiPoint point = new VoronoiPoint(rx, ry);
                    double squeezeFactor = 0.1;

                    while (!op(mask[rx* mScale, ry* mScale], maskValue) || !IsUnique(point) || ClosestDis(point.GetXY) < distanceBetween-squeezeFactor)
                    {
                        rx = (int)(rnd.Next(xmin, xmax));
                        ry = (int)(rnd.Next(ymin, ymax));
                        point = new VoronoiPoint(rx, ry);
                        squeezeFactor += 0.1;
                    }
                    points.Add(new VoronoiPoint(rx, ry));
                    squeezeFactor = 0.1;
                }
                return;
            }
           
        
            for (int i = 0; i < count; i++)
            {
                points.Add(new VoronoiPoint(rnd.Next(xmin, xmax), rnd.Next(ymin, ymax)));
            }
        
        }

        protected List<VoronoiPoint> TraceBoundaries(int xmin, int xmax, int ymin, int ymax, int[,] mask = null, int mScale = 1, int maskValue = 0, Func<int, int, bool> op = null )
        {
            List<VoronoiPoint> boundaries = new List<VoronoiPoint>();
            for (int x = xmin + 2; x < xmax; x += 1)
            {
                for (int y = ymin + 2; y < ymax; y += 1)
                {
                    VoronoiPoint vp = new VoronoiPoint(x, y);
                    bool res = op(mask[x* mScale, y* mScale], maskValue);
                    if (res && ClosestDis(vp.GetXY, boundaries) > 25 && mask.GetSurroundingPoints(new HashSet<int[]>(), vp.GetXY, xmax, ymax, op, 0).Count > 0)
                    {
                        boundaries.Add(new VoronoiPoint(x, y));
                    }
                }
            }
            return boundaries;
        }

        public void LinearDistribution(int xmin, int xmax, int ymin, int ymax, int count, int mScale, int maskValue = 0, int[,] mask = null, Func<int, int, bool> op = null, Func<int, int, int> formula = null)
        {
            points = new List<VoronoiPoint>();

            if (mask != null)
            {
                int index = 0;
                for (int x = xmin + 2; x < xmax; x+=1)
                {
                    for (int y = ymin + 2; y < ymax; y+=1)
                    {
                        VoronoiPoint vp = new VoronoiPoint(x, y);
                        bool res = op(mask[x*mScale, y*mScale], maskValue);
                        if ( res && ClosestDis(vp.GetXY) > 50  && mask.GetSurroundingPoints(new HashSet<int[]>(), vp.GetXY, xmax, ymax, op, 0).Count == 0)
                        {
                            points.Add(new VoronoiPoint(x, y));
                            index++;

                            if (index >= count)
                                return;

                        }
                    }
                }
            }

            return;
        }
        
        
        /// <summary>
        /// sets value of grid (b) points to index of caller
        /// moves (b) points away by distanceFromClosest
        /// </summary>
        /// <param name="b"></param>
        public void LinkVoronoiGrid(LibVoronoi b, int distanceFromClosest = 0)
        {
            for(int i = 0; i < b.points.Count(); i++)
            {
                int ind = ClosestIndex(b.points[i].GetXY);
                double dis;
                /* while((dis = this.points[ind].GetXY.DistanceTo(b.points[i].GetXY)) < distanceFromClosest)
                 {
                     double dirTo = b.points[i].GetXY.DirectionTo(this.points[ind].GetXY);
                     int move = (int)(10 * -dirTo);
                     int xdif = this.points[ind].x - b.points[i].x;
                     int ydif = this.points[ind].y - b.points[i].y;
                     int mx = (distanceFromClosest) - xdif;
                     int my = (distanceFromClosest) - ydif;
                     b.points[i].x = b.points[i].x +  (int)((mx * -dirTo) * 100);
                     b.points[i].y = b.points[i].y + (int)((my * -dirTo) * 100);
                 }*/
                b.points[i].value = ind;
            }
        }

        protected bool IsUnique(VoronoiPoint p)
        {
            if (!points.ContainsItem(p))
                return true;
            else return false;
        }

        public void SetPoints(int[] values, int[] weights)
        {
            bool breakOut = false;
            foreach (var p in points)
            {
                int num = rnd.Next(0, 101);
                for (int i = 0; i < weights.Count(); i++)
                {
                    int prevWeight = i -1;

                    if (prevWeight < 0)
                        prevWeight = 0;
                    else prevWeight = weights[i - 1];

                    if (num  <= weights[i] && num >  prevWeight)
                    {
                        p.value = values[i];
                        breakOut = true;
                    }
                    if (breakOut)
                        break;
                }
                breakOut = false;
            }
        
        }

        public double ClosestDis(int[] vp, List<VoronoiPoint> _points)
        {
            double dis = double.MaxValue;
            foreach (var p in _points)
            {
                if (p == null)
                    continue;
                double pdis = vp.DistanceTo(p.GetXY);
                if (pdis < dis)
                {
                    dis = pdis;
                }
            }
            return dis;
        }


        public double ClosestDis(int[] vp)
        {
            double dis = double.MaxValue;
            foreach (var p in points)
            {
                if (p == null)
                    continue;
                double pdis = vp.DistanceTo(p.GetXY);
                if (pdis < dis)
                {
                    dis = pdis;
                }
            }
            return dis;
        }

        public int ClosestValue(int[] vp, out double dis)
        {
            dis = double.MaxValue;
            int cval = 0;
            foreach(var p in points)
            {
                double pdis = vp.DistanceTo(p.GetXY);
                if (pdis < dis)
                {
                    dis = pdis;
                    cval = p.value;
                }    
            }
            return cval;
        }

        public int ClosestIndex(int[] vp)
        {
            double dis = double.MaxValue;
            int currind = 0;
            int ind = 0;
            int i = 0;
            foreach (var p in points)
            {
                double pdis = vp.DistanceToNorm(p.GetXY);
                double sinDis = pdis; 
                if (sinDis < dis)
                {
                    dis = sinDis;
                    ind = currind;
                }
                currind++;
                i++;
            }
            return ind;
        }
    }

    public class VoronoiPoint
    {
        public int x { get; set;}
        public int y { get; set; }
        public int value { get; set; } 
        public VoronoiPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public int[] GetXY
        {
           get { return new int[] { x, y }; }
        }
    }

    public static class ExtVoronoi
    {
        public static bool ContainsPoint(this List<VoronoiPoint> voronoiPoints, VoronoiPoint search)
        {
            int find = voronoiPoints.FindIndex(x => x.x == search.x && x.y == search.y);
            if (find == -1)
                return false;
            else return true;
        }
    }
}
