using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Default_Heart : HeartItemBase
	{
        public override string Texture => "Terraria/Images/Heart";
        public override int MaxLife => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart");
			Tooltip.SetDefault("20 HP");
		}
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = (int)(22*Item.scale);
			Item.width = (int)(22*Item.scale);
		}
		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.RottenChunk, 5);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.Vertebrae, 5);
			recipe.AddTile(TileID.Hellforge);
			recipe.Register();
		}
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
            spriteBatch.Draw((golden ? TextureAssets.Heart2 : TextureAssets.Heart).Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
        }
	}
}
