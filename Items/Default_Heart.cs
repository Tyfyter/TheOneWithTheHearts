using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Default_Heart : HeartItemBase
	{
        public override string Texture => "Terraria/Heart";
        public override int MaxLife => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart");
			Tooltip.SetDefault("20 HP");
		}
		public override void SetDefaults() {
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
		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
            spriteBatch.Draw(golden ? Main.heart2Texture : Main.heartTexture, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
        }
	}
}
