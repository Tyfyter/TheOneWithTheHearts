using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items {
	public class Mage_Heart : HeartItemBase {
		public override int MaxLife => 16;
		static ArmorShaderData shader;
		public override void AutoStaticDefaults() {
			base.AutoStaticDefaults();
			if (!Main.dedServ) {
				shader = new ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Effects/MageHeart", AssetRequestMode.ImmediateLoad).Value), "MageHeart");
				shader.UseNonVanillaImage(Mod.Assets.Request<Texture2D>("Items/Mage_Heart_Overlay", AssetRequestMode.ImmediateLoad));
				GameShaders.Armor.BindShader(Type, shader);
			}
		}
		public override void Unload() {
			shader = null;
		}
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Heart of Magic");
			Tooltip.SetDefault("16 HP\n");
		}
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LifeCrystal);
			Item.consumable = false;
			Item.useStyle = ItemUseStyleID.None;
			Item.maxStack = 1;
			Item.height = 22;
			Item.width = 22;
			Item.color = Color.DodgerBlue;
		}
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.VilePowder, 5);
			recipe.AddTile(TileID.DemonAltar);
			//recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Default_Heart>(), 1);
			recipe.AddIngredient(ItemID.LifeFruit, 1);
			recipe.AddIngredient(ItemID.ViciousPowder, 5);
			recipe.AddTile(TileID.DemonAltar);
			//recipe.Register();
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			int witheredHearts = Main.LocalPlayer.GetModPlayer<HeartPlayer>().oldWitheredHearts;
            for (int i = tooltips.Count-1; i > 0; i++) {
                if (tooltips[i].Name.Equals("Tooltip1")) {
					GetStatBoosts(witheredHearts, out float magicDamage, out float magicPen);
					string text = "+2% magic damage";
					if (magicDamage > 0.02f) {
						text = $"From equipped Hearts of Magic:\n+ {System.Math.Round(magicDamage * 100)}% magic damage";
					}
					if (magicPen > 0) {
						text += $"\n+ {magicPen} magic armor penetration";
                    }
					tooltips[i].Text = text;
					break;
                }
            }
        }
		public override void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale, int index) {
			golden = true;
			if (golden) {
				Main.spriteBatch.Restart(
					sortMode: SpriteSortMode.Immediate,
					transformMatrix: Main.UIScaleMatrix
				);
				DrawData data = new(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
				//shader.UseColor(new Color((int)(Math.Pow(Main.DiscoR / 255f, 2) * 255), (int)(Math.Pow(Main.DiscoG / 255f, 2) * 255), (int)(Math.Pow(Main.DiscoB/255f, 2) * 255)));
				//shader.UseColor(Main.DiscoColor);
				shader.Apply(Main.LocalPlayer, data);
				data.Draw(spriteBatch);
				Main.spriteBatch.Restart(transformMatrix: Main.UIScaleMatrix);
			} else {
				spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
			}
		}
		public static void GetStatBoosts(int mageHearts, out float magicDamage, out float magicPen) {
			magicPen = 0f;
			magicDamage = 0f;
            switch (mageHearts) {
				case 20:
				magicDamage += 0.02f;
				goto case 19;
						
				case 19:
				magicDamage += 0.02f;
				magicPen += 10f;
				goto case 18;
						
				case 18:
				magicDamage += 0.02f;
				goto case 17;
						
				case 17:
				magicDamage += 0.02f;
				goto case 16;
						
				case 16:
				magicDamage += 0.02f;
				goto case 15;
						
				case 15:
				magicPen += 7f;
				goto case 14;
						
				case 14:
				magicDamage += 0.02f;
				goto case 13;
						
				case 13:
				magicDamage += 0.02f;
				goto case 12;
						
				case 12:
				magicDamage += 0.02f;
				goto case 11;
						
				case 11:
				magicDamage += 0.02f;
				goto case 10;
						
				case 10:
				magicDamage += 0.02f;
				magicPen += 7f;
				goto case 9;
						
				case 9:
				magicDamage += 0.02f;
				goto case 8;
						
				case 8:
				magicDamage += 0.02f;
				goto case 7;
						
				case 7:
				magicDamage += 0.02f;
				goto case 6;
						
				case 6:
				magicDamage += 0.02f;
				goto case 5;
						
				case 5:
				magicPen += 7f;
				goto case 4;
						
				case 4:
				magicDamage += 0.02f;
				goto case 3;
						
				case 3:
				magicDamage += 0.02f;
				goto case 2;
						
				case 2:
				magicPen += 7f;
				goto case 1;

				case 1:
				magicDamage += 0.02f;
				break;
            }
        }
        public override void WhileActive(Player player) {
			player.GetModPlayer<HeartPlayer>().witheredHearts+=1;
		}
		public override void WhileInactive(Player player) {
			player.GetModPlayer<HeartPlayer>().witheredHearts+=1;
		}
        public override void UpdateNaturalRegen(Player player, ref float regen, bool golden) {
			regen += player.statMana * (golden ? 0.015f : 0.01f);
        }
	}
}
