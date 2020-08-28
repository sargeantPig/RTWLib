using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Data
{
	[Flags]
	public enum Attributes
	{
		sea_faring = 1 << 0,
		hide_forest = 1 << 1,
		hide_improved_forest = 1 << 2,
		hide_long_grass = 1 << 3,
		hide_anywhere = 1 << 4,
		can_sap = 1 << 5,
		frighten_foot = 1 << 6,
		frighten_mounted = 1 << 7,
		can_run_amok = 1 << 8,
		general_unit = 1 << 9,
		general_unit_upgrade = 1 << 10,
		cantabrian_circle = 1 << 11,
		no_custom = 1 << 12,
		command = 1 << 13,
		mercenary_unit = 1 << 14,
		druid = 1 << 15,
		warcry = 1 << 16,
		hardy = 1 << 17,
		very_hardy = 1 << 18,
		screeching_women = 1 << 19,
		power_charge = 1 << 20,
		can_swim = 1 << 21,
		is_peasant = 1 << 22,
		//m2tw attributes only
		free_upkeep_unit = 1 << 23,
		can_withdraw = 1 << 24,
		knight = 1 << 25,
		gunpowder_unit = 1 << 26,
		start_not_skirmishing = 1 << 27,
		stakes = 1 << 28,
		fire_by_rank = 1 << 29,
		cannot_skirmish = 1 << 30 //kingdoms only?
	}
	[Flags]
	public enum FormationTypes
	{
		phalanx = 1 << 0,
		horde = 1 << 1,
		square = 1 << 2,
		testudo = 1 << 3,
		schiltrom = 1 << 4,
		wedge = 1 << 5,
		shield_wall = 1 << 6,
		none = 1 << 7
	};

	public enum MissileType
	{
		javelin,
		stone,
		pilum,
		arrow,
		arrow2,
		francisca,
		ballista,
		onager,
		heavy_onager,
		scorpion,
		repeating_ballista,
		head,
		bullet,
		boulder,
		big_boulder,
		harpoon,
		MT_no,
		//m2tw only
		arrow_fiery,
		composite_arrow,
		composite_arrow_fiery,
		cav_composite_arrow,
		cav_composite_arrow_fiery,
		bodkin_arrow,
		bodkin_arrow_fiery,
		cav_bodkin_arrow,
		cav_bodkin_arrow_fiery,
		catapult,
		fiery_catapult,
		trebuchet,
		fiery_trebuchet,
		tarred_rock,
		bolt,
		flaming_ballista,
		tower_ballista,
		tower_flaming_ballista,
		flaming_scorpion,
		bombard_shot,
		flaming_bombard_shot,
		ribault_shot,
		monster_ribault_shot,
		grand_bombard_shot,
		flaming_grand_bombard_shot,
		monster_bombard_shot,
		cannon_shot,
		exploding_cannon_shot,
		culverin_shot,
		exploding_culverin_shot,
		mortar_shot,
		basilisk_shot,
		exploding_basilisk_shot,
		serpentine_shot,
		elephant_cannon_shot,
		cow_carcass,
		rocket,
		elephant_rocket,
		test_cannon_ball,
		naphtha_bomb,
		hand_gun_bullet,
		arquebus_bullet,
		musket_bullet,
		pistol_bullet,
		camel_gun_bullet,
		crossbow_bolt,
		steel_crossbow_bolt,
		norman_catapult,
		fiery_norman_catapult,
	};

	public enum WeaponType
	{

		melee,
		thrown,
		missile,
		siege_missile,
		WT_no
	};

	public enum TechType
	{

		simple,
		other,
		blade,
		archery,
		siege,
		TT_no,
		//M2TW ONLY
		melee_simple, 
		melee_blade, 
		missile_mechanical, 
		missile_gunpowder, 
		artillery_mechanical,
		artillery_gunpowder
	};

	public enum DamageType
	{

		piercing,
		blunt,
		slashing,
		fire,
		DM_no
	};

	public enum SoundType
	{
		knife,
		mace,
		axe,
		sword,
		spear,
		ST_no
	};

	public enum ArmourSound
	{

		flesh = 1 << 0,
		leather = 1 << 1,
		metal = 1 << 2
	};

	public enum Statmental_discipline
	{

		normal = 1 << 0,
		low = 1 << 1,
		disciplined = 1 << 2,
		impetuous = 1 << 3,
		berserker = 1 << 4
	};

	public enum Statmental_training
	{

		untrained = 1 << 0,
		trained = 1 << 1,
		highly_trained = 1 << 2
	};

	[Flags]
	public enum Stat_pri_attr
	{
		ap = 1 << 0,
		bp = 1 << 1,
		pa_spear = 1 << 2,
		long_pike = 1 << 3,
		short_pike = 1 << 4,
		prec = 1 << 5,
		pa_thrown = 1 << 6,
		launching = 1 << 7,
		area = 1 << 8,
		fire = 1 << 9,
		spear_bonus_4 = 1 << 10,
		spear_bonus_6 = 1 << 11,
		spear_bonus_8 = 1 << 12,
		thrown_ap = 1 << 13,
		PA_no = 1 << 14,
		light_spear = 1 << 15,
		spear_bonus_1 = 1 << 16,
		spear_bonus_2 = 1 << 17,
		spear_bonus_3 = 1 << 18,
		spear_bonus_5 = 1 << 19,
		spear_bonus_7 = 1 << 20,
		spear_bonus_9 = 1 << 21,
		spear_bonus_10 = 1 << 22,
		spear_bonus_11 = 1 << 23,
		spear_bonus_12 = 1 << 24,
		spear = 1 << 25
	};

	public enum Cultures
	{
		none,
		carthaginian,
		eastern,
		parthian,
		egyptian,
		greek,
		roman,
		barbarian,
		numidian
	}

	[Flags]
	public enum FactionOwnership
	{
		romans_julii = 1 << 0,
		romans_brutii = 1 << 1,
		romans_scipii = 1 << 2,
		egypt = 1 << 3,
		seleucid = 1 << 4,
		carthage = 1 << 5,
		parthia = 1 << 6,
		gauls = 1 << 7,
		germans = 1 << 8,
		britons = 1 << 9,
		greek_cities = 1 << 10,
		romans_senate = 1 << 11,
		macedon = 1 << 12,
		pontus = 1 << 13,
		armenia = 1 << 14,
		dacia = 1 << 15,
		numidia = 1 << 16,
		scythia = 1 << 17,
		spain = 1 << 18,
		thrace = 1 << 19,
		slave = 1 << 20,
		roman = 1 << 21,
		carthaginian = 1 << 22,
		greek = 1 << 23,
		egyptian = 1 << 24,
		eastern = 1 << 25,
		barbarian = 1 << 26,
		nomad = 1 << 27, //Ahowls vanilla enhancement addition
		none = 1 << 28
		
	}

}
