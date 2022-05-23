using RTWLib.Extensions;
using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions
{
    static class LibRandom
    {
        private static List<Settlement> CreateMissingSettlements(List<Settlement> settlements, Descr_Region dr)
        {
            foreach (KeyValuePair<string, Region> kv in dr.regions)
            {
                int index = settlements.FindIndex(x => x.region == kv.Key);
                if (index == -1)
                    settlements.Add(new Settlement("village", kv.Value.name, kv.Value.factionCreator, new List<DSBuilding>(), 0, 500));
            }

            return new List<Settlement>(settlements);
        }

        public static void RandomSettlements(Descr_Strat ds, Descr_Region dr, Random rnd, int minSettlements, int maxSettlements)
        {
            
            List<Settlement> tempSettlements = new List<Settlement>();

            Action<Faction> AddRSettlement = (f) =>
            {
                int index = rnd.Next(tempSettlements.Count());
                Settlement rndS = tempSettlements[index];
                f.settlements.Add(new Settlement(rndS));
                tempSettlements.RemoveAt(index);
            };

            Action<Settlement, Faction> AddSettlement = (s, f) =>
            {
                f.settlements.Add(new Settlement(s));
                tempSettlements.Remove(s);
            };

            //get all settlements
            foreach (Faction f in ds.factions)
            {
                foreach (Settlement s in f.settlements)
                {
                    tempSettlements.Add(new Settlement(s));
                }
            }

            tempSettlements = CreateMissingSettlements(tempSettlements, dr);

            //set capitals for each faction
            foreach (Faction f in ds.factions)
            {
                f.settlements.Clear();
                AddRSettlement(f); //add first settlement, (this will be the capital)
            }

            ds.ShuffleFactions(rnd);

            foreach (Faction f in ds.factions)
            {
                int maxrnd = rnd.Next((int)minSettlements, (int)maxSettlements + 1);
                int[] capitalCoords = dr.GetCityCoords(f.settlements.First().region);
                for (int i = 0; i < maxrnd - 1; i++)
                {
                    double distance = 100;
                    Settlement cityToAdd = null;

                    foreach (Settlement s in tempSettlements)
                    {
                        int[] cityCoords = dr.GetCityCoords(s.region);
                        double tempDis = cityCoords.DistanceTo(capitalCoords);

                        if (tempDis < distance)
                        {
                            distance = tempDis;
                            cityToAdd = s;

                        }
                    }

                    if (cityToAdd != null)
                        AddSettlement(cityToAdd, f);
                }

            }
        }

        public static void VoronoiSettlements(Descr_Strat ds, Descr_Region dr, Random rnd)
        {
            
            Dictionary<int[], List<ISettlement>> voronoiCoords = new Dictionary<int[], List<ISettlement>>();
            List<Settlement> tempSettlements = new List<Settlement>();

            ds.ShuffleFactions(rnd);

            //get all settlements
            foreach (Faction f in ds.factions)
            {
                foreach (Settlement s in f.settlements)
                {
                    tempSettlements.Add(new Settlement(s));
                }
            }

            tempSettlements = CreateMissingSettlements(tempSettlements, dr);

            while (!CheckVoronoiPoints(voronoiCoords))
            {
                //set voronoi points 
                for (int i = 0; i < ds.factions.Count; i++)
                {

                    int x = rnd.Next(20, 231);
                    int y = rnd.Next(20, 131);

                    while (voronoiCoords.ContainsKey(new int[] { x, y }))
                    {
                        x = rnd.Next(20, 231);
                        y = rnd.Next(20, 131);
                    }
                    voronoiCoords.Add(new int[] { x, y }, new List<ISettlement>());
                }

                //assign each settlement to closest voronoi point
                foreach (Settlement s in tempSettlements)
                {
                    int[] closestPoint = new int[2];
                    int distance = 10000;
                    int[] cityCoord = dr.GetCityCoords(s.region);
                    foreach (KeyValuePair<int[], List<ISettlement>> kv in voronoiCoords)
                    {
                        int tempDistance = (int)cityCoord.DistanceTo(kv.Key);
                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            closestPoint = kv.Key;
                        }
                    }

                    voronoiCoords[closestPoint].Add(s);
                }

                if (!CheckVoronoiPoints(voronoiCoords))
                {
                    voronoiCoords.Clear();
                }
            }

            //give factions settlements
            int counter = 0;
            foreach (KeyValuePair<int[], List<ISettlement>> kv in voronoiCoords)
            {
                ds.factions[counter].settlements = new List<ISettlement>(kv.Value);
                counter++;
            }
        }
        private static bool CheckVoronoiPoints(Dictionary<int[], List<ISettlement>> dic)
        {
            foreach (KeyValuePair<int[], List<ISettlement>> kv in dic)
            {
                if (kv.Value.Count == 0)
                    return false;
            }

            if (dic.Count == 0)
                return false;

            return true;
        }

    }
}
