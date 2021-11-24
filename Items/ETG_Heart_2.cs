using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheOneWithTheHearts.Items
{
	public class ETG_Heart_2 : HeartItemBase
	{
		public override int MaxLife => 4;
        public override bool GetsLifeBoosts => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Four-chambered Heart");
			Tooltip.SetDefault(". → ⁛");
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
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ETG_Heart>(), 1);
			//recipe.AddIngredient(ModContent.ItemType<Mech_Heart>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.anyIronBar = true;
			recipe.AddRecipe();
		}
		public override void Damage(ref int damage, bool crit = false, PlayerDeathReason reason = default){
			damage = crit?3:1;
		}
		public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
			switch (life) {
				case 4:
            	if(mod.TextureExists("Items/"+this.GetType().Name))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 3:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_3Q"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_3Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 2:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Half"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Half"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 1:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_1Q"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_1Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				default:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Empty"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Empty"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
