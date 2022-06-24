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
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Iron Heart");
			Tooltip.SetDefault("15 HP\nWhen active:\nReduces damage taken by 5\nReduces natural life regeneration by 15%\n'This is not a modded sword.'");
		}
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
			Item.height = 22;
            Item.width = 22;
		}
        public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = null) {
			damage -= 5;
        }
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen *= 0.85f;
        }
        /*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 2);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 6);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
