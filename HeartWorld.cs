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
					"111000000_0000000000000000_000001111",
					"11000_________00000000_________00011",
					"1000___________000000___________0001",
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
					"100______________h_______________001",
					"1100_____________ss_____________0011",
					"11100____________ss____________00110",
					"111100________________________001111",
					"1111100______________________0011111",
					"11111100____________________00111111",
					"111111100__________________001111111",
					"1111111100________________0011111111",
					"11111111100______________00111111111",
					"111111111100____________001111111111",
					"1111111111100__________0011111111111",
					"11111111111100________00111111111111",
					"111111111111100______001111111111111",
					"1111111111111100____0011111111111111",
					"11111111111111100__00111111111111111",
				};
				Structure structure = new Structure(map,
					('0', new StructureTile(0, StructureTilePlacementType.Nothing)),
					('1', new StructureTile(0, StructureTilePlacementType.Nothing)),
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
                    if (CheckHeartPlacement(shrineX, shrineY, map) < 69 * 1.5f) {
						continue;
                    }
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
						WorldGen.PlacePot(shrineX+8, shrineY+10, 28, WorldGen.genRand.Next(4, 7));
						WorldGen.PlacePot(shrineX+25, shrineY+10, 28, WorldGen.genRand.Next(4, 7));
						remainingShrineCount--;
                    }
                }
			}));
        }
		public static int CheckHeartPlacement(int i, int j, string[] _map) {
            char[] line;
            int i1,j1;
            int changedTilesCount = 0;
            for(int j0 = 0; j0 < _map.Length; j0++) {
                j1 = j + j0;
                if (j1 >= Main.maxTilesY) {
					return 0;
                }
                line = _map[j0].ToCharArray();
                for(int i0 = 0; i0 < line.Length; i0++) {
                    i1 = i + i0;
					if (i1 >= Main.maxTilesX) {
						return 0;
					}
                    if (line[i0] == '0') {
                        switch (IsValidHeartShrineTile(i1, j1)) {
							case true:
							changedTilesCount++;
							break;
							case false:
							changedTilesCount--;
							break;
                        }
                    }
                }
            }
			return changedTilesCount;
        }
		public static bool? IsValidHeartShrineTile(int x, int y) {
            if (!Main.tile[x, y].active()) {
				return null;
            }
            switch (Main.tile[x, y].type) {
				case TileID.SnowBlock:
				case TileID.IceBlock:
				case TileID.Slush:
				return true;
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