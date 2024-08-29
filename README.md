This version is no longer supported see here for the current repository: [RTWLibTools](https://github.com/sargeantPig/RTWLibTools)

# RTWLib

This library contains features to parse/output RTW files.

It's extensively utilised in my randomiser, so check that out for more examples of usage.

This library makes use of the fantastic [MAGICK.NET](https://github.com/dlemstra/Magick.NET) library.

## Features

1. descr_region.txt
    1. can parse region name, colour, cityname, faction_creator, resources and citycoordinates
    2. find region by colour
2. descr_strat.txt 
    1. can parse the entire descr_strat into a simple data structure.
    2. can output the entire file
    3. Remove SPQR function to quickly remove them from the game
    4. shuffle factions (useful if doing randomisation)
3. export_descr_buildings.txt 
    1. can parse the building types, their levels, and all the building details. (exception hidden resources are currently not          handled)
    2. can output the entire file
    3. functions to return a specific or random building
    4. functions to return buildings at a certain settlement level
4. export_descr_unit.txt
    1. can parse the entire file
    2. can output the entire file
    3. functions to find specific units and return the unit(s)
    4. function to check a unit belongs to a faction
5.  names.txt
    1. can parse all the names and ownership of the names
    2. functions to get random names from the files
    3. function to check for non ASCII characters
6. Select Map creation
    1. can paint faction maps showing the settlements a faction owns.
    2. can paint a full map showing all factions and their associated colour.
    3. can paint diplomacy maps showing whos at war with who for each faction.
7. descr_sm_factions
    1. can parse the factions and their associated colours.


## Examples of what this library can achieve (ui is using winforms for c#)

![Example from the RTWRandomiser](https://media.discordapp.net/attachments/230357533980753921/724720371022168214/unknown.png)

![Example from the RTWRandomiser](https://media.discordapp.net/attachments/230357533980753921/724720608646135899/unknown.png)

![Example from the RTWRandomiser](https://media.discordapp.net/attachments/230357533980753921/724720776833532004/unknown.png)


## Code Examples

Parsing the export_descr_unit.txt
```csharp
EDU edu = new EDU(true); //create an object of the edu with logging on. 
edu.parse(new string[] {"path"}, out linenumber, out linetext); // needs a string array currently, line vars are for logging
```

Accessing and modifying a value within export_descr_unit.txt
```csharp
Unit unit = edu.FindUnit("peasant");
unit.attack[0] = 3; // modify attack factor
unit.morale = 1; // modify morale
unit.discipline = StatMental_discipline.berserker; //modify discipline
unit.ownership |=  carthage; // add carthage as an owner of the unit
```

Outputting the export_descr_unit.txt
```csharp
edu.ToFile("path");
```

