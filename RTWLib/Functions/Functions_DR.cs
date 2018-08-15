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
namespace RTWLib.Functions
{
	public class Functions_DR : Logger.Logger
	{
		private Dictionary<string, Objects.Region> rgbRegions = new Dictionary<string, Objects.Region>();
		
		private void GetCityCoordinates(string filepath)
		{
			MagickImage img = new MagickImage(filepath);
			var pixels = img.GetPixels();
			img.Flip();

			MagickColor white = MagickColor.FromRgb(0, 0, 0);
			MagickColor water = MagickColor.FromRgb(41, 140, 233);

			for (int x = 0; x < img.Width; x++)
				for (int y = 0; y < img.Height; y++)
				{
					int[] pixelCol = new int[] {pixels[x,y].ToColor().R, pixels[x, y].ToColor().G, pixels[x, y].ToColor().B };

					if (CompareColour(pixelCol, white )) //check for city
					{
						int tr, tg, tb;
						tr = pixels[x, y + 1].ToColor().R;
						tg = pixels[x, y + 1].ToColor().G;
						tb = pixels[x, y + 1].ToColor().B;

						if (CompareColour(new int[] {tr, tg, tb}, water)) // check for water
						{
							tr = pixels[x, y - 1].ToColor().R;
							tg = pixels[x, y - 1].ToColor().G;
							tb = pixels[x, y - 1].ToColor().B;

						}
						string index = FindRegionByColour(new int[] { tr, tg, tb });
						rgbRegions[index].x = x;
						rgbRegions[index].y = y;
						Misc_Data.regionWater[x, y] = false;
						
					}


					else if (CompareColour(pixelCol, water))
					{
						Misc_Data.regionWater[x, y] = true;
					}

					else
					{
						Misc_Data.regionWater[x, y] = false;
					}

				}


		}

		public Dictionary<string, Objects.Region> ParseVanRegions(string filePath, string regions_image_path)
		{
			//add an output for this in the tool section
			string line;

			StreamReader reg = new StreamReader(filePath);


			int counter = -1;
			string name = "";
			string cityName = "";
			while ((line = reg.ReadLine()) != null)
			{
				if (!line.Contains("\t") && !line.Contains(";") && !line.Contains(" ") && line != "")
				{
					counter++;
					name = line.Trim();
					rgbRegions.Add(name, new Objects.Region());
					rgbRegions[name].name = name;
					Output("\r\nRegion found: " + name);

					line = reg.ReadLine();
					cityName = line.Trim();
					rgbRegions[name].cityName = cityName;
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
					}


				}
			}

			reg.Close();

			GetCityCoordinates(regions_image_path);

			return rgbRegions;

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
	}

}
