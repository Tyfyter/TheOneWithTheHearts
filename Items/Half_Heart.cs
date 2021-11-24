using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Half_Heart : HeartItemBase
	{
        public override int MaxLife => 50;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Half Heart");
			Tooltip.SetDefault("It doesn't care.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.height = (int)(22*item.scale);
			item.width = (int)(22*item.scale);
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
		public override void Damage(ref int damage, bool crit = false, PlayerDeathReason reason = default(PlayerDeathReason)){
			damage *= 2;
		}
	}
}
