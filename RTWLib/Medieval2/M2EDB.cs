using RTWLib.Functions;
using RTWLib.Objects.Buildings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
	class M2EDB : EDB
	{
		public M2EDB(bool log_on) : base(log_on)
		{


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
					string modified = Functions_General.RemoveFirstWord(trimmedLine);
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
								string firstWordRemoved = Functions_General.RemoveFirstWord(line.Trim());
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

											newBuilding.buildingName = Functions_General.GetFirstWord(line.Trim());
											//get factions
											string output = line.Trim().Substring(line.Trim().IndexOf('{') + 1);
											output = Functions_General.RemoveLastWord(output);
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

														else if (Functions_General.GetFirstWord(line.Trim()) == "{")
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

														if (Functions_General.GetFirstWord(line.Trim()) == "recruit" || Functions_General.GetFirstWord(line.Trim()) == "recruit_pool")
														{
															M2Recruit newRecruit = new M2Recruit();

															//get unit name
															string[] unitSplit = line.Trim().Split('"');
															newRecruit.name = unitSplit[1]; // unit name should always be here;
														
																//get unit experience
															string[] expSplit = unitSplit[2].Trim().Split(' ');
															newRecruit.startingPoints = Convert.ToInt32(expSplit[0].Trim());
															newRecruit.pointBuildingGains = Convert.ToDouble(expSplit[3].Trim());
															newRecruit.maximumPoints = Convert.ToDouble(expSplit[6].Trim());
															newRecruit.experience = Convert.ToInt32(expSplit[8].Trim());
			
															//get factions
															string output = line.Trim().Substring(line.Trim().IndexOf('{') + 1);
															output = Functions_General.RemoveLastWord(output);
															List<string> factionSplit = output.Split(',').ToList();

															factionSplit.Remove(factionSplit.Last());

															foreach (string faction in factionSplit)
															{
																newRecruit.requiresFactions.Add(faction);
															}

															newCapa.canRecruit.Add(newRecruit);
														}

														else if (Functions_General.GetFirstWord(line.Trim()) == "agent")
														{
															newCapa.agentList.Add(line.Trim());
														}

														else if (Functions_General.GetFirstWord(line.Trim()) == "{")
														{ }

														else if (!line.Trim().StartsWith("}"))
														{
															newCapa.effectList.Add(line.Trim());
														}

													}

												}

												if (line.Trim().StartsWith("material"))
												{
													newBuilding.material = Functions_General.RemoveFirstWord(line.Trim()).Trim();


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
															newConstruction.cost = Convert.ToInt32(Functions_General.RemoveFirstWord(line.Trim()).Trim());

														}

														if (line.Trim().StartsWith("settlement_min"))
														{
															newConstruction.settlement_min = Functions_General.RemoveFirstWord(line.Trim()).Trim();

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
	}

}
