using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Ice_Heart : HeartItemBase {
        public override int MaxLife => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart of Ice");
			Tooltip.SetDefault("20 HP");
		}
		public override void SetDefaults() {
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.height = 22;
			item.width = 22;
		}
	}
	public class Ice_Heart_Useable : HeartItemBase {
        public override string Texture => "TheOneWithTheHearts/Items/Ice_Heart";
	}
}
