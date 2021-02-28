using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Logger;
using RTWLib.Objects;
using System.IO;
using RTWLib.Data;
using RTWLib.Objects.Buildings;
namespace RTWLib.Functions
{
	static class Helpers_EDB
	{
		public static readonly string[] EDBTabSpacers = { "    ", "        ", "            ", "                " };  // the edb uses spaces instead of tabs

	}

	public class EDB : FileBase, IFile
	{
		public List<string> hiddenResources = new List<string>();
		public List<CoreBuilding> buildingTrees = new List<CoreBuilding>();

		public EDB(bool log_on) 
			: base(FileNames.export_descr_buildings, @"data\export_descr_buildings.txt", "Building desscriptions and order")
		{
            is_on = log_on;
        }

		override public void Parse(string[] paths, out int lineNumber, out string currentLine)
		{
			lineNumber = 0;
			currentLine = "";
			if (!FileCheck(paths[0]))
			{
				DisplayLog();
				return;
			}
			string line;
			int counter = -1;

			StreamReader strat = new StreamReader(paths[0]);
			//get factions
			while ((line = strat.ReadLine()) != null)
			{
				lineNumber++;
				currentLine = line;
				string trimmedLine = line.Trim();

				if (trimmedLine.StartsWith("hidden_resources"))
				{
					string modified = LibFuncs.RemoveFirstWord(trimmedLine);
					string[] splitStr = modified.Split(' ');
					foreach (string str in splitStr)
					{
						hiddenResources.Add(str);
					}
				}

				if (trimmedLine.StartsWith("building"))
				{
					counter++;

					buildingTrees.Add(new CoreBuilding());

					//get building type
					string[] split = trimmedLine.Split(' ');
					buildingTrees[counter].buildingType = split[1].Trim();

					//start new while loop after every { and stop at }
					line = strat.ReadLine();

					if (line.Trim().StartsWith("{")) // start the while loop
					{
						bool whileOne = false;


						while (!whileOne)
						{
							line = strat.ReadLine();

							if (line.Trim().StartsWith("convert_to"))
							{
								string[] convSplit = line.Trim().Split(' ');

								buildingTrees[counter].CBconvert_to = convSplit[1];
							}

							if (line.Trim().StartsWith("}"))
							{
								whileOne = true; //break out of loop
								break;
							}

							if (line.Trim().StartsWith("levels"))
							{
								string firstWordRemoved = LibFuncs.RemoveFirstWord(line.Trim());
								string[] levelsSplit = firstWordRemoved.Split();

								foreach (string str in levelsSplit)
								{
									buildingTrees[counter].levels.Add(str);
								}

								line = strat.ReadLine(); //continue to next line

								if (line.Trim().StartsWith("{"))
								{
									bool whileTwo = false;

									//start another while loop (loop through the buildings foreach level)
									//line = strat.ReadLine(); //continue to next line
									bool buildingNext = true;

									Building newBuilding = new Building();


									while (!whileTwo)
									{
										line = strat.ReadLine(); //continue to next line

										if (line.Trim().StartsWith("}"))
										{
											whileTwo = true; //break out of loop
											break;
										}

										if (buildingNext)
										{
											newBuilding.factionsRequired = new List<string>();

											newBuilding.buildingName = LibFuncs.GetFirstWord(line.Trim(), new string[0]);
											//get factions
											string output = line.Trim().Substring(line.Trim().IndexOf('{') + 1);
											output = LibFuncs.RemoveLastWord(output);
											List<string> factionSplit = output.Split(',').ToList();

											factionSplit.Remove(factionSplit.Last());

											foreach (string faction in factionSplit)
											{
												newBuilding.factionsRequired.Add(faction.Trim());
											}


											buildingNext = false;
										}


										if (line.Trim().StartsWith("{"))
										{
											bool whileThree = false;

											//start looping through capabilitys of buildings, constuction and plugins
											while (!whileThree)
											{
												line = strat.ReadLine(); //continue to next line

												if (line.Trim().StartsWith("convert_to"))
												{
													string[] convSplit = line.Trim().Split(' ');

													newBuilding.Bconvert_to = convSplit[1];

												}

												if (line.Trim().StartsWith("}"))
												{
													whileThree = true; //break out of loop
													break;
												}

												if (line.Trim().StartsWith("faction_capability"))
												{
													bool whileSeven = false;

													Fcapability FnewCapa = new Fcapability();

													while (!whileSeven)
													{
														line = strat.ReadLine();

														if (line.Trim().StartsWith("}"))
														{

															newBuilding.fcapability = FnewCapa;

															whileSeven = true; //break out of loop
															break;
														}

														else if (LibFuncs.GetFirstWord(line.Trim(), new string[0]) == "{")
														{ }

														else if (!line.Trim().StartsWith("}"))
														{
															FnewCapa.effectList.Add(line.Trim());
														}


													}

												}

												if (line.Trim().StartsWith("capability")) //loop through capabilities
												{
													bool whileFour = false;

													Bcapability newCapa = new Bcapability();

													while (!whileFour)
													{
														line = strat.ReadLine();

														if (line.Trim().StartsWith("}"))
														{

															newBuilding.capability = newCapa;

															whileFour = true; //break out of loop
															break;
														}

														if (LibFuncs.GetFirstWord(line.Trim(), new string[0]) == "recruit" || LibFuncs.GetFirstWord(line.Trim(), new string[0]) == "recruit_pool")
														{
															Brecruit newRecruit = new Brecruit();

															//get unit name
															string[] unitSplit = line.Trim().Split('"');
															newRecruit.name = unitSplit[1]; // unit name should always be here;

															//get unit experience
															string[] expSplit = unitSplit[2].Trim().Split(' ');
															newRecruit.experience = Convert.ToInt32(expSplit[0].Trim());


															//get factions
															string output = line.Trim().Substring(line.Trim().IndexOf('{') + 1);
															output = LibFuncs.RemoveLastWord(output);
															List<string> factionSplit = output.Split(',').ToList();

															factionSplit.Remove(factionSplit.Last());

															foreach (string faction in factionSplit)
															{
																newRecruit.requiresFactions.Add(faction);
															}

															newCapa.canRecruit.Add(newRecruit);
														}

														else if (LibFuncs.GetFirstWord(line.Trim(), new string[0]) == "agent")
														{
															newCapa.agentList.Add(line.Trim());
														}

														else if (LibFuncs.GetFirstWord(line.Trim(), new string[0]) == "{")
														{ }

														else if (!line.Trim().StartsWith("}"))
														{
															newCapa.effectList.Add(line.Trim());
														}

													}

												}

												if (line.Trim().StartsWith("material"))
												{
													newBuilding.material = LibFuncs.RemoveFirstWord(line.Trim()).Trim();


												}

												if (line.Trim().StartsWith("construction"))
												{
													bool whileFive = false;

													Bconstruction newConstruction = new Bconstruction();

													//set initial construction value
													string[] constructionSplit = line.Trim().Split(' ');
													newConstruction.turnsToBuild = Convert.ToInt32(constructionSplit[2].Trim());


													while (!whileFive)
													{
														line = strat.ReadLine();

														if (line.Trim().StartsWith("}"))
														{
															whileFive = true; //break out of loop
															break;
														}


														if (line.Trim().StartsWith("cost"))
														{
															newConstruction.cost = Convert.ToInt32(LibFuncs.RemoveFirstWord(line.Trim()).Trim());

														}

														if (line.Trim().StartsWith("settlement_min"))
														{
															newConstruction.settlement_min = LibFuncs.RemoveFirstWord(line.Trim()).Trim();

														}

														if (line.Trim().StartsWith("upgrades"))
														{
															bool whileSix = false;

															while (!whileSix)
															{
																line = strat.ReadLine();

																if (line.Trim().StartsWith("}"))
																{
																	newBuilding.construction = newConstruction;
																	buildingTrees[counter].buildings.Add(new Building(newBuilding));
																	buildingNext = true;
																	whileFive = true; //break out of loop
																	break;
																}

																if (!line.Trim().StartsWith("{"))
																{
																	newConstruction.upgrades.Add(line.Trim());

																}
															}
														}

													}



												}

											}

										}


									}


								}




							}


						}


					}




				}
			}
			strat.Close();
		}
		override public string Output()
		{
			string a = "";

			a += "hidden_resources ";

			foreach (string resource in hiddenResources)
			{
				a += resource + " ";

			}

			a += "\r\n\r\n";

			foreach (CoreBuilding cBuilding in buildingTrees)
			{
				a += cBuilding.outputCoreBuilding() + "\r\n";
			}

			return a;
		}
		public string[] GetRandomBuildingFromChain(string type, Random rnd)
		{
			foreach (CoreBuilding cb in buildingTrees)
			{
				if (cb.buildingType == type)
				{
					string level = cb.levels[rnd.Next(cb.levels.Count())];

					Building building = cb.buildings.Find(x => x.buildingName == level);

					string settlement_min = building.construction.settlement_min;

					return new string[] { level, settlement_min };
				}
			}

			return null;
		}
		public string[] GetRandomBuildingFromChain(string type, string settlementLevel, Random rnd, string faction = null)
		{
			foreach (CoreBuilding cb in buildingTrees)
			{
				if (cb.buildingType == type)
				{

					List<Building> availableBuildings = new List<Building>();
					if (faction != null)
						availableBuildings = GetBuildingsAtLevel(settlementLevel, cb);
					else availableBuildings = GetBuildingsAtLevel(settlementLevel, cb, faction);

					if (availableBuildings.Count() == 0)
						return null;

					Building building = availableBuildings[rnd.Next(availableBuildings.Count())];
					
					return new string[]{ building.buildingName, cb.buildingType};
				}
			}

			return null;
		}

		override public void ToFile(string filepath)
		{
			StreamWriter sw = new StreamWriter(filepath);
			sw.Write(Output());
			sw.Close();
		}

		public string[] GetSpecificBuildingFromChain(string type, string settlementLevel)
		{
			foreach (CoreBuilding cb in buildingTrees)
			{
				if (cb.buildingType == type)
				{
					int realLevel = GetRealLevel(settlementLevel);

					if (realLevel > cb.buildings.Count - 1)
						realLevel = cb.buildings.Count - 1;

					if ((type == "core_building" || type == "defenses") && settlementLevel != "huge_city")
						realLevel--;

					Building newb = new Building();

					foreach (Building b in cb.buildings)
					{
						if (GetRealLevel(b.construction.settlement_min) <= realLevel)
							newb = new Building(b);
					}
					return new string[] { newb.buildingName, cb.buildingType };
				}
			}

			return null;
		}
		public int GetRealLevel(string settlementLevel)
		{
			switch(settlementLevel){
				case "village":
					return 0;
				case "town":
					return 1;
				case "large_town":
					return 2;
				case "city":
					return 3;
				case "large_city":
					return 4;
				case "huge_city":
					return 5;
				default:
					return 0;
			}
		}
		public List<Building> GetBuildingsAtLevel(string settlementLevel, string faction = null)
		{
			int level = GetRealLevel(settlementLevel);
			List<Building> buildings = new List<Building>();
			foreach (CoreBuilding cb in buildingTrees)
			{
				foreach (Building b in cb.buildings)
				{
					if (GetRealLevel(b.construction.settlement_min) <= level && faction == null)
					{
						buildings.Add(b);
					}

					else if (GetRealLevel(b.construction.settlement_min) <= level && b.factionsRequired.Contains(faction))
					{
						buildings.Add(b);
					}
				}
			}

			return buildings;

		}
		public List<Building> GetBuildingsAtLevel(string settlementLevel, CoreBuilding coreBuilding, string faction = null)
		{
			int level = GetRealLevel(settlementLevel);
			List<Building> buildings = new List<Building>();
			foreach (Building b in coreBuilding.buildings)
			{
				if (GetRealLevel(b.construction.settlement_min) <= level && faction == null)
				{
					buildings.Add(b);
				}

				else if (GetRealLevel(b.construction.settlement_min) <= level && b.factionsRequired.Contains(faction))
				{
					buildings.Add(b);
				}
			}

			return buildings;

		}

		public CoreBuilding GetBuildingTree(string treeName)
		{
			foreach (CoreBuilding b in buildingTrees)
			{
				if (b.buildingType == treeName)
					return b;
			}
			return null;
		}
	
	}
}
