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
using RTWLib.Logger;
namespace RTWLib.Functions
{
	public class SelectMaps : Logger.Logger
	{
		public string saveLocation;
		public string radarMapLocation;
		public const string DESCRIPTION = "Select Maps";
		MagickImage full_map;

		public SelectMaps(string saveLocation, string radarMapLocation)
		{
			this.saveLocation = saveLocation;
			this.radarMapLocation = radarMapLocation;
		}


		public Image CreateCompleteMap(Descr_Strat ds, Descr_Region dr, SM_Factions smf)
		{
			MagickImage regionMap = new MagickImage(dr.FilePathRegions); //use region map to map out regions
			MagickImage fullFactionMap = new MagickImage(radarMapLocation); // use radar map as a base
			
			var mag = new MagickGeometry(fullFactionMap.Width, fullFactionMap.Height);
			mag.FillArea = true;
			mag.IgnoreAspectRatio = true;

			regionMap.Alpha(AlphaOption.Remove);

			regionMap.AdaptiveResize(mag);
			var rpixels = regionMap.GetPixels();

			foreach (Faction f in ds.factions) // loop through faction
			{
				var fpixels = fullFactionMap.GetPixels(); //set up both maps
				MagickImage factionMap = new MagickImage(radarMapLocation);
				var fmag = new MagickGeometry(fullFactionMap.Width, fullFactionMap.Height);
				fmag.FillArea = true;
				fmag.IgnoreAspectRatio = true;

				factionMap.Alpha(AlphaOption.Remove);

				factionMap.AdaptiveResize(fmag);


				using (IPixelCollection fmPixels = factionMap.GetPixels())
				{
					foreach (Settlement s in f.settlements) //loop through settlements to get the regions
					{
						int[] regColour = dr.GetRGBValue(s.region); //get the colour of a region
						MagickColor regCol = MagickColor.FromRgb((byte)regColour[0], (byte)regColour[1], (byte)regColour[2]);

						Color facCol1 = smf.factionColours[f.name][0]; //get the faction colour primary
						Color facCol2 = smf.factionColours[f.name][1]; // secondary colour

						MagickColor priCol = MagickColor.FromRgb(facCol1.R, facCol1.G, facCol1.B); //convert the colours to magickcolour
						MagickColor secCol = MagickColor.FromRgb(facCol2.R, facCol2.G, facCol2.B);


						int channelsCount = fmPixels.Channels;
						for (int y = 0; y < factionMap.Height; y++)
						{
							List<ushort[]> regVert = new List<ushort[]>(); //create lists to store each pixel along the width of the image
							List<ushort[]> facVert = new List<ushort[]>();

							if (y - 1 >= 0)
							{
								regVert.Add(rpixels.GetArea(0, y - 1, regionMap.Width, 1));// get string of pixels across the image at current y value
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

							for (int x = 0; x < regVert[i].Length; x += channelsCount) // traverse each pixel across the image at the current y value
							{
								MagickColor pixCol = new MagickColor(regVert[i][x], regVert[i][x + 1], regVert[i][x + 2]);//create magickcolour using 
								MagickColor fCol = new MagickColor(facVert[i][x], facVert[i][x + 1], facVert[i][x + 2]);
								if (pixCol == regCol) //compare region colour
								{
									int bc = BorderCheck(x, i, regVert, regCol); //check if region is a border
									if (bc > 1 && bc < 4)
									{
										fpixels.SetPixel(x==0 ? x : x/3, y, Blend(secCol, fCol, 0.6)); ///divide x by 3 to account for the other channels
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

				Save(factionMap, f.name, saveLocation);
			}

			full_map = fullFactionMap;
			mag.Width = 277;
			mag.Height = 196;
			fullFactionMap.Resize(mag);
			return fullFactionMap.ToBitmap();
		}
		public MagickImage CreateDiplomacyMap(Descr_Strat ds, Descr_Region dr, SM_Factions smf, string factionName, string savepath)
		{
			MagickImage regionMap = new MagickImage(dr.FilePathRegions); //use region map to map out regions

			var rpixels = regionMap.GetPixels();

			//colours

			MagickColor fac1 = new MagickColor(Color.Gold); //get the faction colour primary
			MagickColor facCol2 = new MagickColor(Color.White); // secondary colour
			MagickColor allied = new MagickColor(Color.Green);
			MagickColor suspicous = new MagickColor(Color.Blue);
			MagickColor neutral = new MagickColor(Color.Aqua);
			MagickColor hostile = new MagickColor(Color.MediumVioletRed);
			MagickColor war = new MagickColor(Color.DarkRed);
			MagickColor currentColour1 = fac1;
			MagickColor currentColour2 = facCol2;
			MagickImage factionMap = new MagickImage(radarMapLocation);
			foreach (Faction f in ds.factions) // loop through faction
			{
				
				int relationval = (int)ds.factionRelationships.DoesFactionHaveRelations(factionName, f.name);
				if (f.name == factionName)
				{
					currentColour1 = fac1;
					currentColour2 = facCol2;
				}
				else if (relationval > -1)
				{
					DiplomaticPosition dp = ds.factionRelationships.GetDiplomaticPosition(relationval);
					if (dp == DiplomaticPosition.Allied)
						currentColour1 = allied;
					else if (dp == DiplomaticPosition.Hostile)
						currentColour1 = hostile;
					else if (dp == DiplomaticPosition.Neutral)
						currentColour1 = neutral;
					else if (dp == DiplomaticPosition.Suspicous)
						currentColour1 = suspicous;
					else if (dp == DiplomaticPosition.War)
						currentColour1 = war;
				}
				else continue;



				using (IPixelCollection fmPixels = factionMap.GetPixels())
				{
					foreach (Settlement s in f.settlements) //loop through settlements to get the regions
					{
						int[] regColour = dr.GetRGBValue(s.region); //get the colour of a region
						MagickColor regCol = MagickColor.FromRgb((byte)regColour[0], (byte)regColour[1], (byte)regColour[2]);

						int channelsCount = fmPixels.Channels;
						for (int y = 0; y < factionMap.Height; y++)
						{
							List<ushort[]> regVert = new List<ushort[]>(); //create lists to store each pixel along the width of the image
							List<ushort[]> facVert = new List<ushort[]>();

							if (y - 1 >= 0)
							{
								regVert.Add(rpixels.GetArea(0, y - 1, regionMap.Width, 1));// get string of pixels across the image at current y value
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

							for (int x = 0; x < regVert[i].Length; x += channelsCount) // traverse each pixel across the image at the current y value
							{
								MagickColor pixCol = new MagickColor(regVert[i][x], regVert[i][x + 1], regVert[i][x + 2]);//create magickcolour using 
								MagickColor fCol = new MagickColor(facVert[i][x], facVert[i][x + 1], facVert[i][x + 2]);
								if (pixCol == regCol) //compare region colour
								{
									int bc = BorderCheck(x, i, regVert, regCol); //check if region is a border
									if (bc > 1 && bc < 4)
									{
										fmPixels.SetPixel(x == 0 ? x : x / 3, y, Blend(currentColour2, fCol, 0.6));
									}

									else
									{
										fmPixels.SetPixel(x == 0 ? x : x / 3, y, Blend(currentColour1, fCol, 0.6));
									}
								}
							}
						}
					}
				}
			}



			return factionMap;
		}
		private void Save(MagickImage map, string name, string filepath)
		{
			string fileType = ".tga";
			map.Scale(384, 237);
			map.Write(filepath + "map_" + name + fileType);
			map.Dispose();
		}
		public void Save(string path)
		{
			if (FileCheck(@path))
				full_map.Write(@path, MagickFormat.Png);
			else PLog("Unable to write to " + path + " - does the directory exist?");
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
