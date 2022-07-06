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
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
			Item.width = 22;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen *= golden ? 0.5f : 0;
        }
        public override float ModifyLifeRegen(Player player, float regen, bool golden) {
			return regen / (golden ? (regen > 0 ? 15f : 25f) : 20f);
        }
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ETG_Heart>(), 1);
			//recipe.AddIngredient(ModContent.ItemType<Mech_Heart>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = default) {
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			for (int i = heartIndex+1; i <= startIndex; i++) {
				ModItem heart = heartPlayer.hearts[i].ModItem;
                if (heart is ETG_Heart || heart is ETG_Heart_2) {
					return;
                }
            }
			damage = (((crit?2:1) * (damage>=35?3:1)));//Math.Ceiling((damage-defenseReduction)/60f)
		}
        public override void Heal(ref int healing, bool golden) {
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
        public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale, int index) {
			string name = golden ? "Golden/ETG_Heart_2": "ETG_Heart_2";
			switch (life) {
				case 4:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 3:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_3Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 2:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_Half"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 1:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_1Q"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				default:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_Empty"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
