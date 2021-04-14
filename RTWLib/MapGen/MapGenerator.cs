using ImageMagick;
using LibNoise.Primitive;
using RTWLib.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RTWLib.MapGen
{
    public partial class MapGenerator
    {
       // public static MagickColor heightsWater = MagickColor.FromRgb(0, 0, 253);
       // public static MagickColor regionWater = MagickColor.FromRgb(41, 140, 233);

        LThreadManager threadManager;

        Map map;

        MagickImage mapHeights, mapFeatures, mapRegions,
            mapGroundTypes, mapClimate;

        public List<MagickImage> images { get; } = new List<MagickImage>();

        int width, height, sealevel, roughness, threads;
        float percentLandmass;
        float frequency, elevation;

        PictureBox picBox;

        string regionsImagePath, descrRegionPath;

        Random rnd;
        public MapGenerator(int width, int height, float frequency, int sealevel, int roughness, string drPath, string drImagePath,
            float elevation, LThreadManager tm, Random rand, int threads = 4, PictureBox pic = null)
        {
            if (elevation == 0)
                elevation = 0.1f;

            map = new Map(width, height);

            this.width = width;
            this.height = height;
            this.frequency = frequency;
            this.sealevel = sealevel;
            this.roughness = roughness;
            this.elevation = elevation;
            this.percentLandmass = elevation;
            this.picBox = pic;
            this.threads = threads;
            this.regionsImagePath = drImagePath;
            this.descrRegionPath = drPath;
            this.threadManager = tm;
            this.rnd = rand;
        }

        public void Generate(Random rnd)
        {
            threadManager.CreateThread(() => GenerateLandMasses(rnd));
            threadManager.Start();
            threadManager.Wait(10000);
            threadManager.ClearThreads();

            threadManager.CreateThread(() => GenerateRegions(rnd));
            threadManager.Start();
            threadManager.Wait(10000);
            threadManager.ClearThreads();

            threadManager.CreateThread(() => GenerateClimates(rnd));
            threadManager.Start();
            threadManager.Wait(10000);
            threadManager.ClearThreads();

            images.Add(mapHeights);
            images.Add(mapRegions);

            mapHeights.Write("map_heights.tga");
            mapRegions.Write("map_regions.tga");

        }

        public float[,] GenerateNoise(int passes, float frequency, float elevation, Func<float, int[], float> manipulator = null)
        {
            float[,] noise = new float[width, height];
            SimplexPerlin[] simplex = new SimplexPerlin[passes];

            for (int s = 0; s < passes; s++)
            {
                simplex[s] = new SimplexPerlin();
            }


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = 0;
                    float amplitudeSum = 0;
                    for(int i = 0; i < passes; i++)
                    {
                        float freq = frequency;
                        float amplitude = 1;
                        if (i != 0)
                        {
                            freq = freq * (i * 2);
                            int ii = i + 1; ;
                            amplitude = 1.0f / (float)(i+i);
                        }
                        value += amplitude * simplex[i].GetValue(freq * x, freq * y);
                        amplitudeSum += amplitude;
                    }

                    value = value / amplitudeSum;

                    if (value < 0)
                    {
                        value = -(float)Math.Pow(Math.Abs(value), elevation);
                    }
                    else value = (float)Math.Pow(Math.Abs(value), elevation);
                     if(manipulator != null)
                        value = manipulator(value, new int[] { x, y });
                    noise[x, y] = value;
                }

            }

            return noise;
        }



        public static object _lockImage = new object();

        public void UpdateImage(MagickImage image)
        {
            lock(_lockImage){ 
            if (picBox != null)
                picBox.Image = image.ToBitmap();
            picBox.Invoke(new MethodInvoker(picBox.Update));
            }
        }
    }
}
