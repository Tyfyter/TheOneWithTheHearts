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
			Tooltip.SetDefault("2 HP\nWhen active:\nReduces damage taken to 1\nReduces natural life regeneration by 100%");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
			Item.width = 22;
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			//recipe.AddIngredient(ModContent.ItemType<Mech_Heart>(), 1);
			recipe.AddIngredient(ItemID.MeteorShot, 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen *= golden ? 0.5f : 0;
        }
        public override float ModifyLifeRegen(Player player, float regen, bool golden) {
			return regen / (golden ? (regen > 0 ? 15f : 25f) : 20f);
        }
        public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = default){
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			for (int i = heartIndex+1; i <= startIndex; i++) {
				ModItem heart = heartPlayer.hearts[i].ModItem;
                if (heart is ETG_Heart || heart is ETG_Heart_2) {
					return;
                }
            }
			damage = (((crit?2:1) * (damage>=35?2:1)));//Math.Ceiling((damage-defenseReduction)/60f)
		}
        public override void Heal(ref int healing, bool golden) {
            if (healing >= 40) {
				healing -= 38;
            }else if (healing >= 30) {
				healing = 2;
            }else if (healing >= 10) {
				healing = 1;
            }
        }
		public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
			string name = golden ? "Golden/ETG_Heart": "ETG_Heart";
			switch (life) {
				case 2:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 1:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_Half"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				default:
            	spriteBatch.Draw(Mod.RequestTexture("Items/"+name+"_Empty"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
