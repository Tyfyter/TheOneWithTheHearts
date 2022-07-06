using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Open_Heart : HeartItemBase
	{
        public override int MaxLife => 40;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Open Heart");
			Tooltip.SetDefault("40 HP\nWhen active:\nReduces natural life regeneration by 100%");
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
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen = 0;
        }
        public override float ModifyLifeRegen(Player player, float regen, bool golden) {
			return regen * 0.75f;
        }
        /*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale, int index) {
			string name = golden ? "Items/Golden/Open_Heart" : "Items/Open_Heart";
			spriteBatch.Draw(Mod.RequestTexture(name), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(Mod.RequestTexture(name+"_Inner"), position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}
