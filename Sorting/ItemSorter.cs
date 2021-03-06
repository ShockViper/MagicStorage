using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace MagicStorage.Sorting
{
	public static class ItemSorter
	{
		public static IEnumerable<Item> SortAndFilter(IEnumerable<Item> items, SortMode sortMode, FilterMode filterMode,SubFilterMode subFilterMode, string modFilter, string nameFilter)
		{
			ItemFilter filter;
			switch (filterMode)
			{
			case FilterMode.All:
				filter = new FilterAll();
				break;
			case FilterMode.Weapons:
				filter = GetWeaponFilter(subFilterMode);
				break;
			case FilterMode.Tools:
				filter = GetToolFilter(subFilterMode);
                    break;
			case FilterMode.Equipment:
				filter = GetEquipmentFilter(subFilterMode);
				break;
			case FilterMode.Potions:
				filter = GetPotionFilter(subFilterMode);
				break;
			case FilterMode.Placeables:
				filter = GetPlaceableFilter(subFilterMode);
                    break;
			case FilterMode.Misc:
				filter = new FilterMisc();
				break;
			default:
				filter = new FilterAll();
				break;
			}
			IEnumerable<Item> filteredItems = items.Where((item) => filter.Passes(item) && FilterName(item, modFilter, nameFilter));
			CompareFunction func;
			switch (sortMode)
			{
			case SortMode.Default:
				func = new CompareDefault();
				break;
			case SortMode.Id:
				func = new CompareID();
				break;
			case SortMode.Name:
				func = new CompareName();
				break;
			case SortMode.Quantity:
				func = new CompareID();
				break;
			default:
				return filteredItems;
			}
			BTree<Item> sortedTree = new BTree<Item>(func);
			foreach (Item item in filteredItems)
			{
				sortedTree.Insert(item);
			}
			if (sortMode == SortMode.Quantity)
			{
				BTree<Item> oldTree = sortedTree;
				sortedTree = new BTree<Item>(new CompareQuantity());
				foreach (Item item in oldTree.GetSortedItems())
				{
					sortedTree.Insert(item);
				}
			}
			return sortedTree.GetSortedItems();
		}

		public static IEnumerable<Recipe> GetRecipes(SortMode sortMode, FilterMode filterMode, SubFilterMode subFilterMode, string modFilter, string nameFilter)
		{
			ItemFilter filter;
			switch (filterMode)
			{
			case FilterMode.All:
				filter = new FilterAll();
				break;
                case FilterMode.Weapons:
                    filter = GetWeaponFilter(subFilterMode);
                    break;
                case FilterMode.Tools:
                    filter = GetToolFilter(subFilterMode);
                    break;
                case FilterMode.Equipment:
                    filter = GetEquipmentFilter(subFilterMode);
                    break;
                case FilterMode.Potions:
                    filter = GetPotionFilter(subFilterMode);
                    break;
                case FilterMode.Placeables:
                    filter = GetPlaceableFilter(subFilterMode);
                    break;
                case FilterMode.Misc:
				filter = new FilterMisc();
				break;
			default:
				filter = new FilterAll();
				break;
			}
			IEnumerable<Recipe> filteredRecipes = Main.recipe.Where((recipe, index) => index < Recipe.numRecipes && filter.Passes(recipe) && FilterName(recipe.createItem, modFilter, nameFilter));
			CompareFunction func;
			switch (sortMode)
			{
			case SortMode.Default:
				func = new CompareDefault();
				break;
			case SortMode.Id:
				func = new CompareID();
				break;
			case SortMode.Name:
				func = new CompareName();
				break;
			default:
				return filteredRecipes;
			}
			BTree<Recipe> sortedTree = new BTree<Recipe>(func);
			foreach (Recipe recipe in filteredRecipes)
			{
				sortedTree.Insert(recipe);
				if (CraftingGUI.threadNeedsRestart)
				{
					return new List<Recipe>();
				}
			}
			return sortedTree.GetSortedItems();
		}

        private static ItemFilter GetWeaponFilter(SubFilterMode subFilterMode)
        {
            ItemFilter filter;
            switch (subFilterMode)
            {
                case SubFilterMode.All:
                    filter = new FilterWeapon();
                    break;
                case SubFilterMode.Melee:
                    filter = new FilterMelee();
                    break;
                case SubFilterMode.Ranged:
                    filter = new FilterRanged();
                    break;
                case SubFilterMode.Magic:
                    filter = new FilterMagic();
                    break;
                case SubFilterMode.Throwing:
                    filter = new FilterThrown();
                    break;
                case SubFilterMode.Summon:
                    filter = new FilterSummon();
                    break;
                case SubFilterMode.OtherWeapons:
                    filter = new FilterOtherWeapon();
                    break;
                default:
                    filter = new FilterWeapon();
                    break;
            }
            return filter;
        }

        private static ItemFilter GetToolFilter(SubFilterMode subFilterMode)
        {
            ItemFilter filter;
            switch (subFilterMode)
            {
                case SubFilterMode.All:
                    filter = new FilterTool();
                    break;
                case SubFilterMode.Axe:
                    filter = new FilterAxe();
                    break;
                case SubFilterMode.Hammer:
                    filter = new FilterHammer();
                    break;
                case SubFilterMode.Pickaxe:
                    filter = new FilterPickaxe();
                    break;
                default:
                    filter = new FilterTool();
                    break;
            }
            return filter;
        }

        private static ItemFilter GetEquipmentFilter(SubFilterMode subFilterMode)
        {
            ItemFilter filter;
            switch (subFilterMode)
            {
                case SubFilterMode.All:
                    filter = new FilterEquipment();
                    break;
                case SubFilterMode.Armor:
                    filter = new FilterArmor();
                    break;
                case SubFilterMode.Accessory:
                    filter = new FilterAccessory();
                    break;
                case SubFilterMode.Graple:
                    filter = new FilterGrapple();
                    break;
                case SubFilterMode.Mount:
                    filter = new FilterMount();
                    break;
                case SubFilterMode.Pet:
                    filter = new FilterPet();
                    break;
                case SubFilterMode.Dye:
                    filter = new FilterDye();
                    break;
                case SubFilterMode.Vanity:
                    filter = new FilterVanityItems();
                    break;
                default:
                    filter = new FilterEquipment();
                    break;
            }
            return filter;
        }

        private static ItemFilter GetPotionFilter(SubFilterMode subFilterMode)
        {
            ItemFilter filter;
            switch (subFilterMode)
            {
                case SubFilterMode.All:
                    filter = new FilterPotion();
                    break;
                case SubFilterMode.Recovery:
                    filter = new FilterRecovery();
                    break;
                case SubFilterMode.Food:
                    filter = new FilterFood();
                    break;
                case SubFilterMode.Buff:
                    filter = new FilterBuff();
                    break;
                case SubFilterMode.OtherPotions:
                    filter = new FilterOtherPotion();
                    break;
                default:
                    filter = new FilterPotion();
                    break;
            }
            return filter;
        }

        private static ItemFilter GetPlaceableFilter(SubFilterMode subFilterMode)
        {
            ItemFilter filter;
            switch (subFilterMode)
            {
                case SubFilterMode.All:
                    filter = new FilterPlaceable();
                    break;
                case SubFilterMode.Material:
                    filter = new FilterMaterial();
                    break;
                case SubFilterMode.Block:
                    filter = new FilterBlock();
                    break;
                case SubFilterMode.Ore:
                    filter = new FilterOre();
                    break;
                case SubFilterMode.RoomNeeds:
                    filter = new FilterRoomNeeds();
                    break;
                case SubFilterMode.Statue:
                    filter = new FilterStatue();
                    break;
                case SubFilterMode.Banner:
                    filter = new FilterBanner();
                    break;
                case SubFilterMode.Crate:
                    filter = new FilterCrate();
                    break;
                case SubFilterMode.OtherPlaceables:
                    filter = new FilterOtherPlaceable();
                    break;
                default:
                    filter = new FilterPlaceable();
                    break;
            }
            return filter;
        }

        private static bool FilterName(Item item, string modFilter, string filter)
		{
			string modName = "Terraria";
			if (item.modItem != null)
			{
				modName = item.modItem.mod.DisplayName;
			}
			return modName.ToLowerInvariant().IndexOf(modFilter.ToLowerInvariant()) >= 0 && item.Name.ToLowerInvariant().IndexOf(filter.ToLowerInvariant()) >= 0;
		}
	}
}
