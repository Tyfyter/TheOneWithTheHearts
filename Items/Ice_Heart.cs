using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Tyfyter.Utils;

namespace TheOneWithTheHearts.Items
{
	public class Ice_Heart : HeartItemBase {
        public override int MaxLife => 20;
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Heart of Ice");
			Tooltip.SetDefault("20 HP\nWhen active:\nCold debuffs will strengthen you instead");
		}
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
			Item.width = 22;
		}
        public override void WhileActive(Player player) {
			player.resistCold = true;
			if (player.frozen) {
				player.frozen = false;
				player.endurance += (1-player.endurance) * 0.2f;
            }
			if (player.chilled) {
				player.chilled = false;
				player.moveSpeed /= 0.75f;
			}
			player.GetModPlayer<HeartPlayer>().frozenImmune = true;
        }
        public override float ModifyLifeRegen(Player player, float regen, bool golden) {
            if (player.HasBuff(BuffID.Frostburn)) {
				return MathHelper.Max(regen * 2, 0) + 0.15f;
            }
            if (player.HasAnyBuff(new HashSet<int> {BuffID.Frozen, BuffID.Chilled})) {
				return MathHelper.Max(regen * 2, 0);
            }
            return (regen>0)^player.HasAnyBuff(new HashSet<int> {BuffID.OnFire, BuffID.CursedInferno, BuffID.Burning})? regen * 2 : regen * 0.5f;
        }
        public override void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = null) {
			SoundEngine.PlaySound(SoundID.Item27, player.Center);
        }
    }
	public class Ice_Heart_Useable : Ice_Heart {
        public override string Texture => "TheOneWithTheHearts/Items/Ice_Heart";
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			Tooltip.SetDefault("Permanently increases maximum life by 20 when used\n"+Tooltip.GetDefault());
		}
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.height = 22;
			Item.width = 22;
		}
        public override bool ConsumeItem(Player player) {
            return player.statLifeMax < 400;
        }
        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */ {
			if (player.statLifeMax >= 400) {
				return true;
			}
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			heartPlayer.hearts[heartPlayer.MaxHearts].SetDefaults(ModContent.ItemType<Ice_Heart>());
			player.statLifeMax += 20;
			player.statLifeMax2 += 20;
			player.statLife += 20;
			if (Main.myPlayer == player.whoAmI) {
				player.HealEffect(20);
			}
			AchievementsHelper.HandleSpecialEvent(player, 0);
            return true;
        }
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddTile(TileID.IceMachine);
			recipe.Register();
		}
    }
}
