using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.EDU
{
    public partial class EDU
    {
        public static FileScheme edu_scheme = new FileScheme(new Dictionary<string, Dictionary<string, int>>()
        {

            {"soldier", new Dictionary<string, int>(){

                {"name", 0},
                {"number", 1 },
                {"extras", 2},
                {"collisionMass", 3}
            
            } },

            {"formation", new Dictionary<string, int>(){

                {"tight_side", 0 },
                {"tight_back", 1 },
                {"sparse_side", 0 },
                {"sparse_back", 1 }
            } },

            {"stat_pri_armour", new Dictionary<string, int>(){

                {"armour", 0 },
                {"defence", 1 },
                {"shield", 2 },
                {"sound", 3 }
            } },

            {"stat_pri", new Dictionary<string, int>(){

                {"attack", 0 },
                {"chargeBonus", 1 },
                {"missileType", 2},
                {"missileRange", 3 },
                {"missileAmmo", 4},
                {"weaponFlag", 5},
                {"techFlag", 6},
                {"damageType", 7},
                {"soundType", 8},
                {"attkDelay1", 9},
                {"attkDelay2", 10}
            } },

            {"stat_sec", new Dictionary<string, int>(){

                {"attack", 0 },
                {"chargeBonus", 1 },
                {"missileType", 2},
                {"missileRange", 3 },
                {"missileAmmo", 4},
                {"weaponFlag", 5},
                {"techFlag", 6},
                {"damageType", 7},
                {"soundType", 8},
                {"attkDelay1", 9},
                {"attkDelay2", 10}
            } },

            {"stat_sec_armour", new Dictionary<string, int>(){

                {"armour", 0 },
                {"defence", 1 },
                {"sound", 2}
            } },

            {"stat_ground", new Dictionary<string, int>(){

                {"scrub", 0 },
                {"sand", 1 },
                {"forest", 2 },
                {"snow", 3 }
            } },

            {"stat_mental", new Dictionary<string, int>(){

                {"morale", 0 },
                {"discipline", 1 },
                {"training", 2 }
            } },

            {"stat_cost", new Dictionary<string, int>(){

                {"turns", 0 },
                {"construct", 1 },
                {"upkeep", 2},
                {"weaponUpgrade", 3 },
                {"armourUpgrade", 4},
                {"custom", 5}
            } },
        });

        public static FileData<EDULineEnums> fileData = new FileData<EDULineEnums>(new Dictionary<EDULineEnums, int>() {

            {EDULineEnums.Type, 1 },
            {EDULineEnums.Dictionary, 1},
            {EDULineEnums.Animal, 1},
            {EDULineEnums.Attributes, 1 },
            {EDULineEnums.Category, 1},
            {EDULineEnums.Class, 1},
            {EDULineEnums.Engine, 1},
            {EDULineEnums.Formation, 6},
            {EDULineEnums.Mount, 1},
            {EDULineEnums.Mount_effect, 2},
            {EDULineEnums.Officer, 1},
            {EDULineEnums.Ownership, 1},
            {EDULineEnums.Ship, 1},
            {EDULineEnums.Soldier, 4},
            {EDULineEnums.Stat_charge_dist, 1},
            {EDULineEnums.Stat_cost, 6},
            {EDULineEnums.Stat_fire_delay, 1},
            {EDULineEnums.Stat_food, 2},
            {EDULineEnums.Stat_ground, 4},
            {EDULineEnums.Stat_health, 2},
            {EDULineEnums.Stat_heat, 1},
            {EDULineEnums.Stat_mental, 3},
            {EDULineEnums.Stat_pri, 11},
            {EDULineEnums.Stat_sec, 11},
            {EDULineEnums.Stat_sec_armour, 3},
            {EDULineEnums.Stat_sec_attr, 1},
            {EDULineEnums.Stat_pri_armour, 4},
            {EDULineEnums.Stat_pri_attr, 1},
        });
    }
}
