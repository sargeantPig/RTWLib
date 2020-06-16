using System;
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

namespace RTWLib.Functions
{
	public class Descr_Region : Logger.Logger,  IFile
	{
        FileNames name = FileNames.descr_regions;
		const string DESCRIPTION = "Regions";
		public const string FILEPATH_REGIONS = @"randomiser\data\world\maps\base\map_regions.tga";
		public const string FILEPATH_DR = @"randomiser\data\world\maps\base\descr_regions.txt";
		public Dictionary<string, Objects.Region> rgbRegions = new Dictionary<string, Objects.Region>();

		public Descr_Region(bool log_on)
		{
            is_on = log_on;
        }

		public Descr_Region(Descr_Region _Region)
		{
			rgbRegions = new Dictionary<string, Objects.Region>(_Region.rgbRegions);
		}

		public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			if (!FileCheck(paths[0]))
				DisplayLogExit();

			if (!FileCheck(paths[1]))
				DisplayLogExit();

			//add an output for this in the tool section
			string line;

			StreamReader reg = new StreamReader(paths[0]);

			int counter = -1;
			string name = "";
			string cityName = "";
			string faction_creator = "";
			currentLine = "";
			lineNumber = 0;
			while ((line = reg.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				if (!line.Contains("\t") && !line.Contains(";") && !line.Contains(" ") && line != "")
				{
					if (counter >= 0)
					{
						PLog("Loaded -- " + name + ", " + cityName);
					}

					counter++;
					name = line.Trim();
					rgbRegions.Add(name, new Objects.Region());
					rgbRegions[name].name = name;
					Output("\r\nRegion found: " + name);

					line = reg.ReadLine();
					cityName = line.Trim();
					rgbRegions[name].cityName = cityName;
					line = reg.ReadLine();
					faction_creator = line.Trim();
					rgbRegions[name].faction_creator = faction_creator;
				}

				else if (line.Split(' ').Count() == 3)
				{
					decimal num;

					string[] split = line.Split(' ');

					var result = Decimal.TryParse(split[0].Trim(), out num);
					if (result)
					{
						rgbRegions[name].rgb[0] = Convert.ToInt32(split[0].Trim());
						rgbRegions[name].rgb[1] = Convert.ToInt32(split[1].Trim());
						rgbRegions[name].rgb[2] = Convert.ToInt32(split[2].Trim());

						line = reg.ReadLine();
						string res = line.Trim();
						string[] resArr = res.Split(',');

						resArr.CleanStrings();

						rgbRegions[name].resources = resArr;

					}

					

				}
			}

			reg.Close();

			GetCityCoordinates(paths[1]);
		}

		private string FindRegionByColour(int[] colour)
		{
			foreach (KeyValuePair<string, Objects.Region> kv in rgbRegions)
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
			var pixels = img.GetPixels();

			MagickColor black = MagickColor.FromRgb(0, 0, 0);
			MagickColor water = MagickColor.FromRgb(41, 140, 233);
			img.Rotate(180);
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
						rgbRegions[index].x = x;
						rgbRegions[index].y = (img.Height - y) -1;
						Misc_Data.regionWater[x, (img.Height - y) - 1] = false;

					}

					else if (CompareColour(pixelCol, water))
					{
						Misc_Data.regionWater[x, (img.Height - y) - 1] = true;
					}

					else
					{
						Misc_Data.regionWater[x, (img.Height - y) - 1] = false;
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

		public int[] GetCityCoords(string name)
		{
			return new int[] {rgbRegions[name].x, rgbRegions[name].y };
		}

		public int[] GetRGBValue(string name)
		{
			return new int[] { rgbRegions[name].rgb[0], rgbRegions[name].rgb[1], rgbRegions[name].rgb[2] };
		}

		public string GetFactoinCreator(string name)
		{
			return rgbRegions[name].faction_creator;
		}

		public string Description
		{
			get { return DESCRIPTION; }
		}

		public string Log(string txt)
		{
			return base.PLog(txt);
		}

		public string Output()
		{
			return null;
		}

        public FileNames Name
        {
            get { return name; }
        }

        public string FilePath
		{
			get { return FILEPATH_DR; }
		}

		public string FilePathRegions
		{
			get { return FILEPATH_REGIONS; }
		}
	}

}
