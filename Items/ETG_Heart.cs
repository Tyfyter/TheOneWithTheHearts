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
			Tooltip.SetDefault("2 HP\nReduces damage taken to 1\nReduces natural life regeneration by 100%");
		}
		public override void SetDefaults()
		{
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
        public override void Damage(Player player, ref int damage, bool crit = false, PlayerDeathReason reason = default){
			int defenseReduction = (int)Math.Min(player.statDefense * (Main.expertMode ? 0.75f : 0.5f), damage - 1);
			damage = (int)(((crit?2:1) * (damage>=60?2:1)) + defenseReduction);//Math.Ceiling((damage-defenseReduction)/60f)
		}
        public override void Heal(ref int healing) {
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
            	spriteBatch.Draw(mod.GetTexture("Items/"+name), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				case 1:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_Half"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
				default:
            	spriteBatch.Draw(mod.GetTexture("Items/"+name+"_Empty"), position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
				break;
			}
        }
	}
}
