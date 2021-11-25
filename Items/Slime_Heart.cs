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
        public override int MaxLife => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimey Heart");
			Tooltip.SetDefault("20 HP\n+50% health regeneration\nIncreases fall resistance and falling speed\n'It kind of makes you question slime anatomy.'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.scale = 1/2.5f;
			item.height = 22;
			item.width = 22;
		}
		public override void WhileActive(Player player){
			player.extraFall = 10;
			player.maxFallSpeed *= 1.1f;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen) {
			regen *= 1.5f;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
            if(mod.TextureExists("Items/"+this.GetType().Name)&&false){
				spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), item.position-Main.screenPosition, null, Color.White, rotation, new Vector2(22,22), scale/2.5f, SpriteEffects.None, 0);
			}else{
                scale*=item.scale;
				return true;
			}
            return false;
        }
	}
}
