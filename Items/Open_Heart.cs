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
        public override int MaxLife => 80;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Open Heart");
			Tooltip.SetDefault("80 HP\nReduces natural life regeneration by 100%\nThis is not a modded sword.");
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
        public override void UpdateNaturalRegen(Player player, ref float regen) {
			regen = 0;
        }
        public override float ModifyLifeRegen(Player player, float regen) {
			return regen * 0.75f;
        }
        /*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
			string name = golden ? "Items/Golden/Open_Heart" : "Items/Open_Heart";
			spriteBatch.Draw(mod.GetTexture(name), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(mod.GetTexture(name+"_Inner"), position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
