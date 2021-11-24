using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class ETG_Heart : HeartItemBase
	{
		public override int MaxLife => 2;
        public override bool GetsLifeBoosts => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Familliar Heart");
			Tooltip.SetDefault("");
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
		public override void Damage(ref int damage, bool crit = false, PlayerDeathReason reason = default){
			damage = (crit?2:1);
		}
		public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
			switch (life) {
				case 2:
            	if(mod.TextureExists("Items/"+this.GetType().Name))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), position, null, drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
				break;
				case 1:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Half"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Half"), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale, SpriteEffects.None, 0);
				break;
				default:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Empty"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Empty"), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
