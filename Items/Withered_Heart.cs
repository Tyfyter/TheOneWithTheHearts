using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Withered_Heart : HeartItemBase
	{
        public override int MaxLife => 12;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Withered Heart");
			Tooltip.SetDefault("12 HP\n");
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
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			recipe.AddIngredient(ItemID.VilePowder, 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			recipe.AddIngredient(ItemID.ViciousPowder, 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			int witheredHearts = Main.LocalPlayer.GetModPlayer<HeartPlayer>().witheredHearts;
            for (int i = tooltips.Count-1; i > 0; i++) {
                if (tooltips[i].Name.Equals("Tooltip1")) {
					GetStatBoosts(witheredHearts, out int minionSlots, out float minionDamage);
					string text = "+1 minion slot";
                    if (minionSlots>1) {
						text = $"From equipped Withered Hearts:\n+{minionSlots} minion slots";
                    }
                    if (minionDamage>0) {
						text += $"\n+{System.Math.Round(minionDamage*100)}% minion damage";
                    }
					tooltips[i].text = text;
					break;
                }
            }
        }
		public static void GetStatBoosts(int witheredHearts, out int minionSlots, out float minionDamage) {
			minionSlots = 0;
			minionDamage = 0f;
            switch (witheredHearts) {
				case 20:
				minionDamage+=0.03f;
				minionSlots++;
				goto case 19;
						
				case 19:
				minionDamage+=0.03f;
				goto case 18;
						
				case 18:
				minionDamage+=0.03f;
				goto case 17;
						
				case 17:
				minionDamage+=0.03f;
				goto case 16;
						
				case 16:
				minionDamage+=0.03f;
				goto case 15;
						
				case 15:
				minionDamage+=0.02f;
				goto case 14;
						
				case 14:
				minionDamage+=0.02f;
				goto case 13;
						
				case 13:
				minionDamage+=0.02f;
				goto case 12;
						
				case 12:
				minionDamage+=0.02f;
				goto case 11;
						
				case 11:
				minionSlots++;
				goto case 10;
						
				case 10:
				minionDamage+=0.02f;
				goto case 9;
						
				case 9:
				minionDamage+=0.01f;
				goto case 8;
						
				case 8:
				minionDamage+=0.01f;
				goto case 7;
						
				case 7:
				minionSlots++;
				goto case 6;
						
				case 6:
				minionDamage+=0.01f;
				goto case 5;
						
				case 5:
				minionDamage+=0.01f;
				goto case 4;
						
				case 4:
				minionSlots++;
				goto case 3;
						
				case 3:
				minionDamage+=0.01f;
				goto case 2;
						
				case 2:
				minionSlots++;
				goto case 1;

				case 1:
				minionSlots++;
				break;
            }
        }
        public override void WhileActive(Player player){
			player.GetModPlayer<HeartPlayer>().witheredHearts+=1;
		}
		public override void WhileInactive(Player player){
			player.GetModPlayer<HeartPlayer>().witheredHearts+=1;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen) {
			regen *= 1.5f;
        }
	}
}