using System;

namespace MagicStorage.Sorting
{
	public enum FilterMode
	{
		All,
		Weapons,
		Tools,
		Equipment,
		Potions,
		Placeables,
		Misc
	}
    public enum SubFilterMode
    {
        All=0,
        Melee=1,
        Ranged=2,
        Magic=3,
        Summon=4,
        Throwing=5,
        OtherWeapons=6,
        Axe=1,
        Hammer=2,
        Pickaxe=3,
        Armor = 1,
        Accessory = 2,
        Graple = 3,
        Mount = 4,
        Pet = 5,
        Dye = 6,
        Vanity = 7
    }
}