using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheOneWithTheHearts.Items {
	public class ETG_Heart_2 : HeartItemBase {
		public override int MaxLife => 4;
        public override bool GetsLifeBoosts => false;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Four-chambered Heart");
			Tooltip.SetDefault("4 HP\nWhen active:\nReduces damage taken to 1\nReduces natural life regeneration by 100%\n'. → ⁛'");
		}
		public override void SetDefaults() {
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.height = 22;
			item.width = 22;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen) {
			regen = 0;
        }
        public override float ModifyLifeRegen(Player player, float regen) {
			return regen / 20f;
        }
		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ETG_Heart>(), 1);
			//recipe.AddIngredient(ModContent.ItemType<Mech_Heart>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.anyIronBar = true;
			recipe.AddRecipe();
		}
		public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = default) {
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			for (int i = heartIndex+1; i <= startIndex; i++) {
				ModItem heart = heartPlayer.hearts[i].modItem;
                if (heart is ETG_Heart || heart is ETG_Heart_2) {
					return;
                }
            }
			damage = (((crit?2:1) * (damage>=35?3:1)));//Math.Ceiling((damage-defenseReduction)/60f)
		}
        public override void Heal(ref int healing) {
            if (healing >= 80) {
				healing -= 76;
            } else if (healing >= 70) {
				healing = 4;
            } else if (healing >= 50) {
				healing = 3;
            } else if (healing >= 30) {
				healing = 2;
            } else if (healing >= 10) {
				healing = 1;
            }
        }
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale) {
			string name = golden ? "Golden/ETG_Heart_2": "ETG_Heart_2";
			switch (life) {
				case 4:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 3:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_3Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 2:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_Half"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 1:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_1Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				default:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_Empty"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
