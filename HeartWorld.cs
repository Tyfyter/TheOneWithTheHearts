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
using static Tyfyter.Utils.StructureUtils;

namespace TheOneWithTheHearts {
	public class HeartWorld : ModWorld {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
			tasks.Insert(tasks.Count - 1, new PassLegacy("Heart Shrine", (GenerationProgress progress) => {
				string[] map = new string[] {
					"000000000_0000000000000000_000000000",
					"00000_________00000000_________00000",
					"0000___________000000___________0000",
					"000_____________0000_____________000",
					"00_______________00_______________00",
					"0__________________________________0",
					"0__________________________________0",
					"0__________________________________0",
					"0__________________________________0",
					"____________________________________",
					"0_______p________________p_________0",
					"0_______sst_____________tss________0",
					"0_______ss_______________ss________0",
					"0__________________________________0",
					"00________________________________00",
					"000_____________h________________000",
					"0000____________ss______________0000",
					"00000___________ss_____________00000",
					"000000________________________000000",
					"0000000______________________0000000",
					"00000000____________________00000000",
					"000000000__________________000000000",
					"0000000000________________0000000000",
					"00000000000______________00000000000",
					"000000000000____________000000000000",
					"0000000000000__________0000000000000",
					"00000000000000________00000000000000",
					"000000000000000______000000000000000",
					"0000000000000000____0000000000000000",
					"00000000000000000__00000000000000000",
				};
				Structure structure = new Structure(map,
					('0', new StructureTile(0, StructureTilePlacementType.Nothing)),
					('_', new StructureTile(0, StructureTilePlacementType.ReplaceOld|StructureTilePlacementType.Deactivate)),
					('s', new StructureTile(TileID.SnowBlock, StructureTilePlacementType.ReplaceOld)),
					('p', new StructureTile(TileID.Pots, StructureTilePlacementType.ReplaceOld|StructureTilePlacementType.MultiTile, style:WorldGen.genRand.Next(4, 7))),
					('t', new StructureTile(TileID.Torches, StructureTilePlacementType.ReplaceOld|StructureTilePlacementType.MultiTile, style:9)),
					('h', new StructureTile(TileID.Containers, StructureTilePlacementType.ReplaceOld|StructureTilePlacementType.MultiTile, style:22))
				);
				int remainingShrineCount = Main.maxTilesX / 2100 - 1;
				int shrineX;
				int shrineY;
				int iceHeartType = ModContent.ItemType<Ice_Heart_Useable>();
                while (remainingShrineCount>0) {
					shrineX = WorldGen.genRand.Next(0, Main.maxTilesX-30);
					shrineY = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY-30);
					int validAnchors = 0;
                    switch(IsValidHeartShrineTile(shrineX, shrineY)) {
						case true:
						validAnchors++;
						break;
						case null:
						break;
						case false:
						continue;
                    }
                    switch(IsValidHeartShrineTile(shrineX+36, shrineY)) {
						case true:
						validAnchors++;
						break;
						case null:
						break;
						case false:
						continue;
                    }
                    switch(IsValidHeartShrineTile(shrineX, shrineY+30)) {
						case true:
						validAnchors++;
						break;
						case null:
						break;
						case false:
						continue;
                    }
                    switch(IsValidHeartShrineTile(shrineX+36, shrineY+30)) {
						case true:
						validAnchors++;
						break;
						case null:
						break;
						case false:
						continue;
                    }
                    if (validAnchors < 3)continue;
                    if (structure.Place(shrineX, shrineY) > 0) {
						while (structure.createdChests.Count > 0) {
							int chestIndex = structure.createdChests.Dequeue();
							Chest chest = Main.chest[chestIndex];
							chest.item[0].SetDefaults(iceHeartType);
							chest.item[1].SetDefaults(WorldGen.genRand.NextBool()?ItemID.HeartLantern:ItemID.HeartStatue);
							chest.item[2].SetDefaults(ItemID.SilverCoin);
							chest.item[2].stack = 69;
							chest.item[3].SetDefaults(ItemID.CopperCoin);
							chest.item[3].stack = WorldGen.genRand.Next(0, 69);
                        }
						remainingShrineCount--;
                    }
                }
			}));
        }
		public static bool? IsValidHeartShrineTile(int x, int y) {
            if (Main.tile[x, y].active()) {
				return null;
            }
            switch (Main.tile[x, y].type) {
				case TileID.SnowBlock:
				case TileID.IceBlock:
				return true;
				case TileID.Slush:
				return null;
            }
			return false;
        }
        public override void PostWorldGen() {
            List<int> DungeonChests = new List<int> {};
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the nth chest is the ___ Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
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