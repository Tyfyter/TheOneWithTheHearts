using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Slime_Heart : HeartItemBase
	{
        public override int MaxLife => 25;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimey Heart");
			Tooltip.SetDefault("It kind of makes you question slime anatomy.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.scale = 1/2.5f;
			item.height = (int)(44*item.scale);
			item.width = (int)(44*item.scale);
		}
		public override void WhileActive(Player player){
			base.WhileActive(player);
			player.noFallDmg = true;
			player.maxFallSpeed*=1.1f;
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
            Rectangle rect = new Rectangle((int)item.position.X,(int)item.position.Y,item.width,item.height);
            if(mod.TextureExists("Items/"+this.GetType().Name)&&false){
				spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), item.position-Main.screenPosition, rect, Color.White, rotation, new Vector2(22,22), scale/2.5f, SpriteEffects.None, 0);
			}else{
                scale*=item.scale;
				return true;
			}
            return false;
        }
	}
}
