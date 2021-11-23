using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts {
	public class HeartWorld : ModWorld {
		public override void PostWorldGen() {
            List<int> DungeonChests = new List<int> {};
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 2 * 36) {
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++) {
						if (chest.item[inventoryIndex].IsAir) {
                            DungeonChests.Add(chestIndex);
							break;
						}
					}
				}
			}
			int count = Main.rand.Next(DungeonChests.Count);
			for (int i = 0; i < count; i++) {
				int chesta = Main.rand.Next(DungeonChests);
				Chest chestb = Main.chest[chesta];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++) {
					if (chestb.item[inventoryIndex].IsAir) {
						chestb.item[inventoryIndex].SetDefaults(ModContent.ItemType<ETG_Heart>());
						DungeonChests.Remove(chesta);
						break;
					}
				}
			}
		}
	}
}