using RTWLib.Data;
using RTWLib.Objects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.EDU
{
    public partial class EDU
    {
        private bool ParseLine(string line, ref int counter, int lineNumber)
        {
            EDULineEnums identifier;
            string[] data = Functions_General.RemoveFirstWord(line).Trim().DropComments().TrimEnd(',').Split(';', ',');
            bool isIdentifier = Enum.TryParse<EDULineEnums>(Functions_General.GetFirstWord(line).Capitalise(), out identifier);

            if (!isIdentifier)
                return false;

            try
            {
                switch (identifier)
                {
                    case EDULineEnums.Type:
                        units.Add(new Unit());
                        counter++;
                        HandleGenericLine(ref units[counter].type, data);
                        break;
                    case EDULineEnums.Dictionary:
                        HandleGenericLine(ref units[counter].dictionary, data);
                        break;
                    case EDULineEnums.Category:
                        HandleGenericLine(ref units[counter].category, data);
                        break;
                    case EDULineEnums.Class:
                        HandleGenericLine(ref units[counter].unitClass, data);
                        break;
                    case EDULineEnums.Voice_type:
                        HandleGenericLine(ref units[counter].voiceType, data);
                        break;
                    case EDULineEnums.Ship:
                        HandleGenericLine(ref units[counter].naval, data);
                        break;
                    case EDULineEnums.Soldier:
                        HandleSoldier(ref units[counter].soldier, data);
                        break;
                    case EDULineEnums.Officer:
                        HandleGenericLine(ref units[counter].officer, data);
                        break;
                    case EDULineEnums.Mount:
                        HandleGenericLine(ref units[counter].mount, data);
                        break;
                    case EDULineEnums.Animal:
                        HandleGenericLine(ref units[counter].animal, data);
                        break;
                    case EDULineEnums.Engine:
                        HandleGenericLine(ref units[counter].engine, data);
                        break;
                    case EDULineEnums.Mount_effect:
                        HandleMountEffect(ref units[counter].mountEffect, data);
                        break;
                    case EDULineEnums.Attributes:
                        HandleAttributes(ref units[counter].attributes, data);
                        break;
                    case EDULineEnums.Formation:
                        HandleFormation(ref units[counter].formation, data);
                        break;
                    case EDULineEnums.Stat_health:
                        HandleGenericInts(ref units[counter].heatlh, data);
                        break;
                    case EDULineEnums.Stat_pri:
                        HandleStatPri(ref units[counter].primaryWeapon, data);
                        break;
                    case EDULineEnums.Stat_pri_attr:
                        HandleStatPriAttr(ref units[counter].priAttri, data);
                        break;
                    case EDULineEnums.Stat_sec:
                        HandleStatPri(ref units[counter].secondaryWeapon, data);
                        break;
                    case EDULineEnums.Stat_sec_attr:
                        HandleStatPriAttr(ref units[counter].secAttri, data);
                        break;
                    case EDULineEnums.Stat_pri_armour:
                        HandleStatPriArmour(ref units[counter].primaryArmour, data);
                        break;
                    case EDULineEnums.Stat_sec_armour:
                        HandleStatPriArmour(ref units[counter].secondaryArmour, data);
                        break;
                    case EDULineEnums.Stat_heat:
                        HandleGenericLine(ref units[counter].heat, data);
                        break;
                    case EDULineEnums.Stat_ground:
                        HandleGenericInts(ref units[counter].ground, data);
                        break;
                    case EDULineEnums.Stat_mental:
                        HandleMental(ref units[counter].mental, data);
                        break;
                    case EDULineEnums.Stat_charge_dist:
                        HandleGenericLine(ref units[counter].chargeDistance, data);
                        break;
                    case EDULineEnums.Stat_fire_delay:
                        HandleGenericLine(ref units[counter].fireDelay, data);
                        break;
                    case EDULineEnums.Stat_food:
                        HandleGenericInts(ref units[counter].food, data);
                        break;
                    case EDULineEnums.Ownership:
                        HandleOwnership(ref units[counter].ownership, data);
                        break;
                    case EDULineEnums.Stat_cost:
                        HandleGenericInts(ref units[counter].cost, data);
                        break;
                    default: return false;
                }
            }
            catch (Exception ex)
            {
                PLog(ex.Message + "\r\n" +
                    "Error at Line: " + lineNumber.ToString() + "\r\n" +
                    line + "\r\n");
            }

            return true;
        }

        private void HandleGenericLine<T>(ref T unitVar, string[] data)
        {
            if (typeof(T) == typeof(string)) 
                    unitVar = (T)(object)data[0];
            else if (typeof(T) == typeof(List<string>))
                ((List<string>)(object)unitVar).Add(data[0]);
            else if (typeof(T) == typeof(int)) 
                unitVar = (T)(object)Convert.ToInt32(data[0]);
        }

        private void HandleSoldier(ref Soldier soldier, string[] data)
        {
            soldier.name = data[0].Trim();
            soldier.number = Convert.ToInt32(data[1].Trim());
            soldier.extras = Convert.ToInt32(data[2].Trim());
            soldier.collisionMass = data[3].Trim().UniversalParse();
        }

        private void HandleStatPri(ref StatWeapons stat_pri, string[] data)
        {
            LookUpTables lu = new LookUpTables();
            stat_pri.attack[0] = Convert.ToInt32(data[0].Trim());
            stat_pri.attack[1] = Convert.ToInt32(data[1].Trim());
            stat_pri.Missleattri[0] = Convert.ToInt32(data[3].Trim());
            stat_pri.missletypeFlags = lu.LookUpKey<MissileType>(data[2].Trim());
            stat_pri.WeaponFlags = lu.LookUpKey<WeaponType>(data[5].Trim());
            stat_pri.TechFlags = lu.LookUpKey<TechType>(data[6].Trim());
            stat_pri.DamageFlags = lu.LookUpKey<DamageType>(data[7].Trim());
            stat_pri.SoundFlags = lu.LookUpKey<SoundType>(data[8].Trim());
        }

        private void HandleStatPriAttr(ref Stat_pri_attr stat_Pri_, string[] data)
        {
            LookUpTables lu = new LookUpTables();
            foreach (string str in data)
            {
                stat_Pri_ |= lu.LookUpKey<Stat_pri_attr>(str.Trim());
            }
        }

        private void HandleStatPriArmour<T>(ref T statPriArmour, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();
            if (statPriArmour is StatPriArmour)
            {
                ((StatPriArmour)(object)statPriArmour).stat_pri_armour[0] = Convert.ToInt32(data[0].Trim());
                ((StatPriArmour)(object)statPriArmour).stat_pri_armour[1] = Convert.ToInt32(data[1].Trim());
                ((StatPriArmour)(object)statPriArmour).stat_pri_armour[2] = Convert.ToInt32(data[2].Trim());
                ((StatPriArmour)(object)statPriArmour).armour_sound = lookUp.LookUpKey<ArmourSound>(data[3].Trim());
            }
            else if (statPriArmour is StatSecArmour)
            {
                ((StatSecArmour)(object)statPriArmour).stat_sec_armour[0] = Convert.ToInt32(data[0].Trim());
                ((StatSecArmour)(object)statPriArmour).stat_sec_armour[1] = Convert.ToInt32(data[1].Trim());
                ((StatSecArmour)(object)statPriArmour).sec_armour_sound = lookUp.LookUpKey<ArmourSound>(data[2].Trim());
            }
        }

        private void HandleAttributes(ref Attributes attributes, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();
            foreach (string str in data)
            {
                if (str != " ")
                {
                    object obj;
                    if ((obj = lookUp.LookUpKey<Attributes>(str)) != null)
                        attributes |= lookUp.LookUpKey<Attributes>(str.Trim());
                }
            }
        }

        private void HandleGenericInts(ref int[] values, string[] data)
        {
            for(int i = 0; i < data.Count(); i++)
            {
                 values[i] = Convert.ToInt32(data[i]);
            }
        }

        private void HandleMountEffect(ref MountEffect mountEffect, string[] data)
        {
            string newCombined = "";

            foreach (string full in data)
            {
                newCombined += full;
            }

            string[] reSplit = newCombined.Split(' ');

            for (int index = 0; index < reSplit.Count(); index += 2)
            {
                mountEffect.mountType.Add(reSplit[index]);
                mountEffect.modifier.Add(Convert.ToInt32(reSplit[index+1]));
            }
        }

        private void HandleFormation(ref Formation formation, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();


            int i = 0;
            int a = 0;
            int b = 0;

            foreach (string STRING in data)
            {
                if (i < 2)
                {
                    formation.FormationTight[i] = STRING.Trim().UniversalParse();
                }
                else if (a < 2)
                {
                    formation.FormationSparse[a] = STRING.Trim().UniversalParse();
                    a++;
                }
                else if (b < 1)
                {
                    formation.FormationRanks = Convert.ToInt32(STRING.Trim());
                    b++;
                }

                dynamic ft = lookUp.LookUpKey<FormationTypes>(STRING.Trim());

                if (ft != null)
                    formation.FormationFlags |= ft;

                i++;
            }
        }

        private void HandleMental(ref Mentality mental, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();
            mental.morale = Convert.ToInt32(data[0].Trim());
            mental.discipline = lookUp.LookUpKey<Statmental_discipline>(data[1].Trim());
            mental.training = lookUp.LookUpKey<Statmental_training>(data[2].Trim());
        }

        private void HandleOwnership(ref FactionOwnership ownership, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();
            foreach (string STRING in data)
            {
                var a = lookUp.LookUpKey<FactionOwnership>(STRING.Trim());

                if (a != null)
                    ownership |= a;
                else ownership |= lookUp.LookUpKey<FactionOwnership>(STRING.Trim());

            }
        }
    }
}
