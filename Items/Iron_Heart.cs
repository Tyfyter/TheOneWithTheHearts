using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
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
			Tooltip.SetDefault("Reduces damage taken by 5\nReduces natural life regeneration by 25%\nThis is not a modded sword.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.height = 22;
            item.width = 22;
		}
        public override void Damage(Player player, ref int damage, bool crit = false, PlayerDeathReason reason = null) {
			damage -= 5;
        }
        public override void UpdateNaturalRegen(Player player, ref float regen) {
			regen *= 0.75f;
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
	}
}
