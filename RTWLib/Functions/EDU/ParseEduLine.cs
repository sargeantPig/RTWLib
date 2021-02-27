using RTWLib.Data;
using RTWLib.Medieval2;
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
        protected bool ParseLine(string line, ref int counter, int lineNumber, out KeyValuePair<EDULineEnums, object> commentPair)
        {
            EDULineEnums identifier;
            string comment = "";
            commentPair = new KeyValuePair<EDULineEnums, object>();
            string[] data = Functions_General.RemoveFirstWord(line, new string[] { "era", "banner" }, 1).Trim().DropAndOutComments(out comment).TrimEnd(',').Split(';', ',').CleanStringArray();
            string ident; 
               ident = Functions_General.GetFirstWord(line, new string[] {"era", "banner"} , 1).Capitalise(true).RemoveSpaces();
            bool isIdentifier = Enum.TryParse<EDULineEnums>(ident, out identifier);
             
            if (!isIdentifier)
                return false;

            try
            {
                switch (identifier)
                {
                    case EDULineEnums.Type:
                        if (this.GetType() == typeof(M2EDU))
                            units.Add(new M2Unit());
                        else units.Add(new Unit());
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
                        HandleGenericLine(ref units[counter].uClass, data);
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
                        HandleGenericInts(ref units[counter].health, data);
                        break;
                    case EDULineEnums.Stat_pri:
                        if(this.GetType() == typeof(M2EDU))
                             M2TWHandleStatPri(ref ((M2Unit)units[counter]).primaryWeapon, data);
                        else HandleStatPri(ref units[counter].primaryWeapon, data);
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
                    case EDULineEnums.Stat_ter:
                        M2TWHandleStatPri(ref ((M2Unit)units[counter]).stat_ter, data);
                        break;
                    case EDULineEnums.Stat_ter_ex:
                        HandleGenericLine(ref ((M2Unit)units[counter]).stat_ter_ex, data);
                        break;
                    case EDULineEnums.Stat_ter_attr:
                        HandleStatPriAttr(ref ((M2Unit)units[counter]).stat_ter_attr, data);
                        break;
                    case EDULineEnums.Stat_armour_ex:
                        HandleGenericLine(ref ((M2Unit)units[counter]).stat_armour_ex, data);
                        break;
                    case EDULineEnums.Stat_pri_ex:
                        HandleGenericLine(ref ((M2Unit)units[counter]).stat_pri_ex, data);
                        break;
                    case EDULineEnums.BannerFaction:
                        HandleGenericLine(ref ((M2Unit)units[counter]).bannerFaction, data);
                        break;
                    case EDULineEnums.BannerUnit:
                        HandleGenericLine(ref ((M2Unit)units[counter]).bannerUnit, data);
                        break;
                    case EDULineEnums.BannerHoly:
                        HandleGenericLine(ref ((M2Unit)units[counter]).bannerHoly, data);
                        break;
                    case EDULineEnums.Stat_sec_ex:
                        HandleGenericLine(ref ((M2Unit)units[counter]).stat_sec_ex, data);
                        break;
                    case EDULineEnums.Stat_stl:
                        HandleGenericLine(ref ((M2Unit)units[counter]).stat_stl, data);
                        break;
                    case EDULineEnums.Armour_ug_levels:
                        HandleOwnership(ref ((M2Unit)units[counter]).armour_ug_levels, data);
                        break;
                    case EDULineEnums.Armour_ug_models:
                        HandleOwnership(ref ((M2Unit)units[counter]).armour_ug_models, data);
                        break;
                    case EDULineEnums.Era0:
                        HandleOwnership(ref ((M2Unit)units[counter]).era0, data);
                        break;
                    case EDULineEnums.Era1:
                        HandleOwnership(ref ((M2Unit)units[counter]).era1, data);
                        break;
                    case EDULineEnums.Era2:
                        HandleOwnership(ref ((M2Unit)units[counter]).era2, data);
                        break;
                    case EDULineEnums.Info_pic_dir:
                        HandleGenericLine(ref ((M2Unit)units[counter]).info_pic_dir, data);
                        break;
                    case EDULineEnums.Card_pic_info:
                        HandleGenericLine(ref ((M2Unit)units[counter]).card_pic_info, data);
                        break;
                    case EDULineEnums.Unit_info:
                        HandleGenericLine(ref ((M2Unit)units[counter]).unit_info, data);
                        break;
                    case EDULineEnums.Accent:
                        HandleGenericLine(ref ((M2Unit)units[counter]).accent, data);
                        break;

                    default: return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog(ex, true);

                DisplayLog();
            }

            commentPair = new KeyValuePair<EDULineEnums, object>(identifier, comment);
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
            stat_pri.Missleattri[1] = Convert.ToInt32(data[4].Trim());
            stat_pri.missileType = data[2].Trim();
            stat_pri.WeaponFlags = lu.LookUpKey<WeaponType>(data[5].Trim());
            stat_pri.TechFlags =(TechType)Enum.Parse(typeof(TechType), data[6].Trim());
            stat_pri.DamageFlags = lu.LookUpKey<DamageType>(data[7].Trim());
            stat_pri.SoundFlags = data[8].Trim();
            stat_pri.attackdelay[0] = Convert.ToInt32(data[9].Trim());
            stat_pri.attackdelay[1] = (float)Convert.ToDouble(data[10].Trim());
        }
        private void M2TWHandleStatPri(ref M2StatWeapons stat_pri, string[] data)
        {
            if (data.Count() == 11)
            {

                LookUpTables lu = new LookUpTables();
                stat_pri.attack[0] = Convert.ToInt32(data[0].Trim());
                stat_pri.attack[1] = Convert.ToInt32(data[1].Trim());
                stat_pri.Missleattri[0] = Convert.ToInt32(data[3].Trim());
                stat_pri.Missleattri[1] = Convert.ToInt32(data[4].Trim());
                stat_pri.missileType = data[2].Trim();
                stat_pri.WeaponFlags = lu.LookUpKey<WeaponType>(data[5].Trim());
                stat_pri.TechFlags = (TechType)Enum.Parse(typeof(TechType), data[6].Trim());
                stat_pri.DamageFlags = lu.LookUpKey<DamageType>(data[7].Trim());
                stat_pri.SoundFlags = data[8].Trim();
                stat_pri.attackdelay[0] = Convert.ToInt32(data[9].Trim());
                stat_pri.attackdelay[1] = (float)Convert.ToDouble(data[10].Trim());
            }
            else if (data.Count() == 12)
             {
                LookUpTables lu = new LookUpTables();
                stat_pri.attack[0] = Convert.ToInt32(data[0].Trim());
                stat_pri.attack[1] = Convert.ToInt32(data[1].Trim());
                stat_pri.Missleattri[0] = Convert.ToInt32(data[3].Trim());
                stat_pri.Missleattri[1] = Convert.ToInt32(data[4].Trim());
                stat_pri.missileType = data[2].Trim();
                stat_pri.WeaponFlags = lu.LookUpKey<WeaponType>(data[5].Trim());
                stat_pri.TechFlags = (TechType)Enum.Parse(typeof(TechType), data[6].Trim());
                stat_pri.DamageFlags = lu.LookUpKey<DamageType>(data[7].Trim());
                stat_pri.SoundFlags = data[8].Trim();
                stat_pri.musket_shot_set = data[9].Trim();
                stat_pri.attackdelay[0] = Convert.ToInt32(data[10].Trim());
                stat_pri.attackdelay[1] = (float)Convert.ToDouble(data[11].Trim());
            }
        }
        private void HandleStatPriAttr(ref Stat_pri_attr stat_Pri_, string[] data)
        {
            LookUpTables lu = new LookUpTables();

            Stat_pri_attr temp = Stat_pri_attr.no;
            stat_Pri_ = temp;

            if (data[0] == lu.LookUpString<Stat_pri_attr>(Stat_pri_attr.no)) 
                return;

            foreach (string str in data)
            {
                stat_Pri_ |= lu.LookUpKey<Stat_pri_attr>(str.Trim());
            }

            stat_Pri_ &= ~Stat_pri_attr.no;
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
            Attributes attri = Attributes.no_custom;
            LookUpTables lookUp = new LookUpTables();
            bool set_noCustom = false;
            attributes = attri;
            foreach (string str in data)
            {
                if (str != " ")
                {
                    Attributes obj;
                    if (Enum.TryParse<Attributes>(str.Trim(), out obj))
                    {
                        if ((Attributes)obj == Attributes.no_custom)
                            set_noCustom = true;
                        else attributes |= obj;
                    }
                }
            }

            if (set_noCustom)
                attributes |= Attributes.no_custom;
            else attributes &= ~Attributes.no_custom;
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
                newCombined +=  full + " ";
            }

            string[] reSplit = newCombined.Split(' ').CleanStringArray();

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

        private void HandleOwnership(ref List<string> ownership, string[] data)
        {
            LookUpTables lookUp = new LookUpTables();
            
            foreach (string str in data)
            {
                //TODO add optional verification against smf file here
                ownership.Add(str.Trim());
            }
        }
    }
}
