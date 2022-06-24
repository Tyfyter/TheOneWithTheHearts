using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Half_Heart : HeartItemBase
	{
        public override int MaxLife => 10;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Half Heart");
			Tooltip.SetDefault("10 HP\n'It doesn't care.'");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
            Item.width = 22;
		}
		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			recipe.AddTile(TileID.Sawmill);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = default(PlayerDeathReason)){
			damage -= Math.Min(damage, 20)/2;
		}
	}
}
