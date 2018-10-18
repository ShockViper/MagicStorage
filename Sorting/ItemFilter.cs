using System;
using System.Collections.Generic;
using Terraria;

namespace MagicStorage.Sorting
{
	public abstract class ItemFilter
	{
		public abstract bool Passes(Item item);

		public bool Passes(object obj)
		{
			if (obj is Item)
			{
				return Passes((Item)obj);
			}
			if (obj is Recipe)
			{
				return Passes(((Recipe)obj).createItem);
			}
			return false;
		}
	}

	public class FilterAll : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return true;
		}
	}

	public class FilterMelee : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.melee && item.pick == 0 && item.axe == 0 && item.hammer == 0;
		}
	}

	public class FilterRanged : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.ranged;
		}
	}

	public class FilterMagic : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.magic;
		}
	}

	public class FilterSummon : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.summon;
		}
	}

	public class FilterThrown : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.thrown;
		}
	}

	public class FilterOtherWeapon : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return !item.melee && !item.ranged && !item.magic && !item.summon && !item.thrown && item.damage > 0;
		}
	}

	public class FilterWeapon : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.damage > 0 && item.pick == 0 && item.axe == 0 && item.hammer == 0;
		}
	}

	public class FilterPickaxe : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.pick > 0;
		}
	}

	public class FilterAxe : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.axe > 0;
		}
	}

	public class FilterHammer : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.hammer > 0;
		}
	}

	public class FilterTool : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.pick > 0 || item.axe > 0 || item.hammer > 0;
		}
	}

	public class FilterEquipment : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.headSlot >= 0 || item.bodySlot >= 0 || item.legSlot >= 0 || item.accessory || Main.projHook[item.shoot] || item.mountType >= 0 || (item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType]));
		}
	}

    public class FilterArmor : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return (item.headSlot >= 0 || item.bodySlot >= 0 || item.legSlot >= 0) && !item.vanity;
        }
    }

    public class FilterVanityArmor : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return (item.headSlot >= 0 || item.bodySlot >= 0 || item.legSlot >= 0) && item.vanity;
        }
    }

    public class FilterVanityItems : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return item.vanity;
        }
    }

    public class FilterAccessory : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return item.accessory;
        }
    }

    public class FilterGrapple : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return Main.projHook[item.shoot];
        }
    }

    public class FilterMount : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return item.mountType != -1 ;
        }
    }

    public class FilterPet : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType]);
        }
    }

    public class FilterDye : ItemFilter
    {
        public override bool Passes(Item item)
        {
            return item.dye > 0 || item.hairDye >= 0;
        }
    }

    public class FilterPotion : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.consumable && (item.healLife > 0 || item.healMana > 0 || item.buffType > 0);
		}
	}

	public class FilterPlaceable : ItemFilter
	{
		public override bool Passes(Item item)
		{
			return item.createTile >= 0 || item.createWall > 0;
		}
	}

	public class FilterMisc : ItemFilter
	{
		private static List<ItemFilter> blacklist = new List<ItemFilter> {
			new FilterWeapon(),
			new FilterTool(),
			new FilterEquipment(),
			new FilterPotion(),
			new FilterPlaceable()
		};

		public override bool Passes(Item item)
		{
			foreach (var filter in blacklist)
			{
				if (filter.Passes(item))
				{
					return false;
				}
			}
			return true;
		}
	}
}