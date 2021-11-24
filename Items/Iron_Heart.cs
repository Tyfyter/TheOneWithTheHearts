using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Iron_Heart : HeartItemBase
	{
        public override int MaxLife => 15;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iron Heart");
			Tooltip.SetDefault("This is not a modded sword.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.scale = 1/1.5f;
			item.height = (int)(44*item.scale);
			item.width = (int)(44*item.scale);
		}
		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.anyIronBar = true;
			recipe.AddRecipe();
		}
		public override void WhileActive(Player player){
			base.WhileActive(player);
			player.statDefense+=10;
		}
	}
}
