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
			Tooltip.SetDefault("20 HP\nWhen active:\n+50% health regeneration\nIncreases fall resistance and falling speed\n'It kind of makes you question slime anatomy.'");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.scale = 1/2.5f;
			Item.height = 22;
			Item.width = 22;
		}
		public override void WhileActive(Player player){
			player.extraFall = 10;
			player.maxFallSpeed *= 1.1f;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen *= 1.5f;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
            scale*=Item.scale;
            return true;
        }
	}
}
