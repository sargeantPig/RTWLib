﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RTWLib.Objects;
using ImageMagick;
using System.Drawing;
using RTWLib.Data;
using System.Runtime.CompilerServices;
using RTWLib.Extensions;
namespace RTWLib.Functions
{
	public partial class BIDescr_Region : Descr_Region,  IFile
	{
		new public Dictionary<string, BIRegion> regions = new Dictionary<string, BIRegion>();

		public BIDescr_Region(bool log_on, string filePathImage, string filePathDR) 
			: base(log_on, filePathImage, filePathDR )
		{
            is_on = log_on;
        }

		public override string Output()
		{
			string str = ";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;".CRL();
			foreach (var region in regions)
				str += region.Value.Output();

			return str;
		}

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			currentLine = "";
			lineNumber = 0;
			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}
			if (!FileCheck(paths[1]))
			{
				DisplayLog();
				return;
			}

			StreamReader reg = new StreamReader(paths[0]);

			while ((currentLine = reg.ReadLine()) != null)
			{
				bool success = this.ParseLine(currentLine, regions);
				lineNumber++;
			}

			reg.Close();

			GetCityCoordinates(paths[1]);
			
			foreach(KeyValuePair<string, BIRegion> r in regions)
			{
				base.regions.Add(r.Key, r.Value);
			}
		}
		private string FindRegionByColour(int[] colour)
		{
			foreach (KeyValuePair<string, BIRegion> kv in regions)
			{
				MagickColor mg = MagickColor.FromRgb((byte)kv.Value.rgb[0], (byte)kv.Value.rgb[1], (byte)kv.Value.rgb[2]);
				if (CompareColour(colour, mg))
				{
					return kv.Key;
				}
			}

			return null;

		}
		private void GetCityCoordinates(string filepath)
		{
			MagickImage img = new MagickImage(filepath);

			Misc_Data.regionWater = new bool[img.Width, img.Height];
			Misc_Data.editRegionWater = new bool[img.Width, img.Height];

			var pixels = img.GetPixels();

			MagickColor black = MagickColor.FromRgb(0, 0, 0);
			MagickColor water = MagickColor.FromRgb(41, 140, 233);
			//img.Rotate(180);
			for (int x = 0; x < img.Width; x++)
				for (int y = 0; y < img.Height; y++)
				{
					int[] pixelCol = new int[] { pixels[x, y].ToColor().R, pixels[x, y].ToColor().G, pixels[x, y].ToColor().B };

					if (CompareColour(pixelCol, black)) //check for city
					{
						int tr, tg, tb;
						tr = pixels[x, y + 1].ToColor().R;
						tg = pixels[x, y + 1].ToColor().G;
						tb = pixels[x, y + 1].ToColor().B;

						if (CompareColour(new int[] { tr, tg, tb }, water)) // check for water
						{
							tr = pixels[x, y - 1].ToColor().R;
							tg = pixels[x, y - 1].ToColor().G;
							tb = pixels[x, y - 1].ToColor().B;
						}

						string index = FindRegionByColour(new int[] { tr, tg, tb });
						regions[index].x = x;
						regions[index].y = y;  //(img.Height - y) -1;
						Misc_Data.regionWater[x, y] = false;

					}

					else if (CompareColour(pixelCol, water))
					{
						Misc_Data.regionWater[x, y] = true; //(img.Height - y) - 1] = true;
					}

					else
					{
						Misc_Data.regionWater[x, y] = false; //(img.Height - y) - 1] = false;
					}
				}
		}
		private bool CompareColour(int[] col1, int[] col2)
		{
			if (col1[0] == col2[0] && col1[1] == col2[1] && col1[2] == col2[2])
			{
				return true;


			}

			else return false;

		}
		private bool CompareColour(int[] col1, MagickColor col2)
		{
			if (col1[0] == col2.R && col1[1] == col2.G && col1[2] == col2.B)
			{
				return true;


			}

			else return false;

		}
		override public int[] GetCityCoords(string name)
		{
			return new int[] {regions[name].x, regions[name].y };
		}
		override public int[] GetRGBValue(string name)
		{
			return new int[] { regions[name].rgb[0], regions[name].rgb[1], regions[name].rgb[2] };
		}
		public string GetFactoinCreator(string name)
		{
			return regions[name].factionCreator;
		}
		public string FilePathRegions
		{
			get { return FILEPATH_REGIONS; }
		}

		public Dictionary<string, BIRegion> BIRegions
		{
			get { return regions; }
		}
	}

}
