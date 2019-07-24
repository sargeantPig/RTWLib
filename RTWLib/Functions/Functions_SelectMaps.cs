using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using RTWLib.Objects;
using RTWLib.Data;
using RTWLib.Objects.Descr_strat;
namespace RTWLib.Functions
{
	public class SelectMaps
	{
		public const string SAVELOC = @"randomiser\data\world\maps\campaign\imperial_campaign\";
		public const string RADARMAP = @"randomiser\van_data\world\maps\campaign\imperial_campaign\radar_map1.tga";
		public const string DESCRIPTION = "Select Maps";

		MagickImage full_map;

		public Image CreateCompleteMap(Descr_Strat ds, Descr_Region dr, SM_Factions smf)
		{
			LookUpTables lt = new LookUpTables();
			MagickImage regionMap = new MagickImage(dr.FilePathRegions);
			MagickImage fullFactionMap = new MagickImage(RADARMAP);

			var rpixels = regionMap.GetPixels();

			foreach (Faction f in ds.factions)
			{
				var fpixels = fullFactionMap.GetPixels();
				MagickImage factionMap = new MagickImage(RADARMAP);
				using (IPixelCollection fmPixels = factionMap.GetPixels())
				{
					foreach (Settlement s in f.settlements)
					{
						int[] regColour = dr.GetRGBValue(s.region);
						MagickColor regCol = MagickColor.FromRgb((byte)regColour[0], (byte)regColour[1], (byte)regColour[2]);

						Color facCol1 = smf.factionColours[lt.LookUpKey<FactionOwnership>(f.name)][0];
						Color facCol2 = smf.factionColours[lt.LookUpKey<FactionOwnership>(f.name)][1];
						MagickColor priCol = MagickColor.FromRgb(facCol1.R, facCol1.G, facCol1.B);
						MagickColor secCol = MagickColor.FromRgb(facCol2.R, facCol2.G, facCol2.B);

						int channelsCount = fmPixels.Channels;
						for (int y = 0; y < factionMap.Height; y++)
						{
							List<ushort[]> regVert = new List<ushort[]>();
							List<ushort[]> facVert = new List<ushort[]>();

							if (y - 1 >= 0)
							{
								regVert.Add(rpixels.GetArea(0, y - 1, regionMap.Width, 1));
								facVert.Add(fmPixels.GetArea(0, y - 1, factionMap.Width, 1));
							}
		
							regVert.Add(rpixels.GetArea(0, y, regionMap.Width, 1));
							facVert.Add(fmPixels.GetArea(0, y, factionMap.Width, 1));

							if (y + 1 < regionMap.Height)
							{
								facVert.Add(fmPixels.GetArea(0, y + 1, factionMap.Width, 1));
								regVert.Add(rpixels.GetArea(0, y + 1, regionMap.Width, 1));

							}

							int i = 0;

							if (regVert.Count == 2)
								i = 0;
							else i = 1;

							for (int x = 0; x < regVert[i].Length; x += channelsCount)
							{
								MagickColor pixCol = new MagickColor(regVert[i][x], regVert[i][x + 1], regVert[i][x + 2]);
								MagickColor fCol = new MagickColor(facVert[i][x], facVert[i][x + 1], facVert[i][x + 2]);
								if (pixCol == regCol)
								{
									int bc = BorderCheck(x, i, regVert, regCol);
									if (bc > 1 && bc < 4)
									{
										fpixels.SetPixel(x==0 ? x : x/3, y, Blend(secCol, fCol, 0.6));
										fmPixels.SetPixel(x==0 ? x : x/3, y, Blend(secCol, fCol, 0.6));
									}

									else
									{
										fpixels.SetPixel(x == 0 ? x : x / 3, y, Blend(priCol, fCol, 0.6));
										fmPixels.SetPixel(x == 0 ? x : x / 3, y, Blend(priCol, fCol, 0.6));
									}
								}
								
							}

							
						}


					}
				}

				Save(factionMap, f.name, SAVELOC);
			}

			full_map = fullFactionMap;
			

			return fullFactionMap.ToBitmap();
		}

		private void Save(MagickImage map, string name, string filepath)
		{
			string fileType = ".tga";
			map.Scale(384, 237);
			map.Write(filepath + "map_" + name + fileType);
		}

		public void Save(string path)
		{
			full_map.Write(@path, MagickFormat.Png);
		}

		private int BorderCheck(int x, int y, IPixelCollection pixels, MagickImage regionMap, MagickColor mc)
		{
			int bcount = 0;

			if (x + 1 < regionMap.Width)
				if (pixels[x + 1, y].ToColor() == mc)
					bcount++;
			if (x - 1 > -1)
				if (pixels[x - 1, y].ToColor() == mc)
					bcount++;
			if (y + 1 < regionMap.Height)
				if (pixels[x, y + 1].ToColor() == mc)
					bcount++;
			if (y - 1 > -1)
				if (pixels[x, y - 1].ToColor() == mc)
					bcount++;

			return bcount;

		}

		private int BorderCheck(int x, int i, List<ushort[]> vp, MagickColor mc)
		{
			int bcount = 0;

			MagickColor left = null;
			MagickColor right = null;
			MagickColor up = null;
			MagickColor down = null;

			if(x - 3 >= 0)
				left =  new MagickColor(vp[i][x - 3], vp[i][x - 2], vp[i][x - 1]);
			if (x + 3 < vp[i].Length)
				right = new MagickColor(vp[i][x + 3], vp[i][x + 4], vp[i][x + 5]);
			if(i - 1 >= 0)
				up = new MagickColor(vp[i - 1][x], vp[i - 1][x + 1], vp[i - 1][x + 2]);
			if(i + 1 < vp.Count)
				down = new MagickColor(vp[i + 1][x], vp[i + 1][x + 1], vp[i + 1][x + 2]);
			
			if (right != null)
				if (right == mc)
					bcount++;
			if (left != null)
				if (left == mc)
					bcount++;
			if (down != null)
				if (down == mc)
					bcount++;
			if (up != null)
				if (up == mc)
					bcount++;

			return bcount;

		}

		private static ushort[] Blend(MagickColor color, MagickColor backColor, double amount)
		{
			ushort R = (ushort)((color.R * amount) + backColor.R * (1 - amount));
			ushort G = (ushort)((color.G * amount) + backColor.G * (1 - amount));
			ushort B = (ushort)((color.B * amount) + backColor.B * (1 - amount));

			return new ushort[] { R, G, B };
		}

	}


}
