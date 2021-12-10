using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using TheOneWithTheHearts.Items;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts {
	public class TheOneWithTheHearts : Mod {
        public static TheOneWithTheHearts mod;
		public UserInterface heartUI;
		public HeartUI ui;
		bool disableCombatText = false;
		public override void Load() {
            mod = this;
			if (!Main.dedServ) {
				heartUI = new UserInterface();
			}
            On.Terraria.Main.DrawInterface_Resources_Life += Main_DrawInterface_Resources_Life;
            On.Terraria.Player.DropItems += Player_DropItems;
            On.Terraria.Player.UpdateLifeRegen += Player_UpdateLifeRegen;
            On.Terraria.CombatText.NewText_Rectangle_Color_int_bool_bool += CombatText_NewText;
			disableCombatText = false;
		}

        private void Player_UpdateLifeRegen(On.Terraria.Player.orig_UpdateLifeRegen orig, Player self) {
			int life = self.statLife;
			disableCombatText = true;
			orig(self);
			disableCombatText = false;
			self.statLife = life + self.GetModPlayer<HeartPlayer>().MultiplyLifeRegen(self.statLife - life);
            if (self.statLife < life) {
				CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), CombatText.LifeRegen, life - self.statLife, dramatic: false, dot: true);
            }
        }

        private int CombatText_NewText(On.Terraria.CombatText.orig_NewText_Rectangle_Color_int_bool_bool orig, Rectangle location, Color color, int amount, bool dramatic, bool dot) {
            if (disableCombatText) {
				return 100;
            }
			return orig(location, color, amount, dramatic, dot);
        }

        private void Player_DropItems(On.Terraria.Player.orig_DropItems orig, Player self) {
			Item[] hearts = self.GetModPlayer<HeartPlayer>().hearts;
            for (int i = 0; i < 20; i++) {
                if (hearts[i].type != ModContent.ItemType<Default_Heart>()) {
					self.QuickSpawnClonedItem(hearts[i]);
                }
            }
			orig(self);
        }

        private void Main_DrawInterface_Resources_Life(On.Terraria.Main.orig_DrawInterface_Resources_Life orig) {
			Player player = Main.LocalPlayer;
            /*if (player.controlSmart) {
				orig();
            }*/
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			float UI_ScreenAnchorX = Main.screenWidth - 800;
			//float halfHeartWidth = 11;
			if (player.ghost) {
				return;
			}
			int hearts = heartPlayer.MaxHearts;
			int goldenHearts = heartPlayer.GoldenHearts;
			float healthMultiplier = heartPlayer.HealthMultiplier;
			if (goldenHearts < 0) {
				goldenHearts = 0;
			}
			int rowWidth = hearts >= 10 ? 10 : hearts;
			Vector2 vector = Main.fontMouseText.MeasureString(Lang.inter[0].Value + " " + player.statLifeMax2 + "/" + player.statLifeMax2);
			Main.spriteBatch.DrawString(Main.fontMouseText, Lang.inter[0].Value, new Vector2((float)(500 + 13 * rowWidth) - vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.mouseTextColorReal, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.DrawString(Main.fontMouseText, player.statLife + "/" + player.statLifeMax2, new Vector2((float)(500 + 13 * rowWidth) + vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.mouseTextColorReal, 0f, new Vector2(Main.fontMouseText.MeasureString(player.statLife + "/" + player.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
			int rHealth = player.statLife;
			for (int i = 0; i < heartPlayer.MaxHearts; i++) {
				HeartItemBase currentHeart = heartPlayer.hearts[i]?.modItem as HeartItemBase;
				int currentMaxLife = currentHeart?.MaxLife ?? 0;
                if (currentHeart is null || heartPlayer.hearts[i].IsAir) {
					currentMaxLife = 0;
                }
                float multipliers = 1f;
                if (currentHeart?.GetsLifeBoosts??false) {
                    multipliers *= healthMultiplier;
                    if (goldenHearts > 0) {
                        multipliers *= 1.25f;
                    }
					currentMaxLife = (int)(currentMaxLife * multipliers);
                }
				int rgbValue;
				float heartScale = 1f;
				bool beating = false;
				int heartLife;

				if (rHealth >= currentMaxLife) {
					rgbValue = 255;
					heartLife = currentMaxLife;
					if (rHealth == currentMaxLife) {
						beating = true;
					}
				} else {
					heartLife = rHealth > 0 ? rHealth : 0;
					float currentPercent = rHealth / (float)currentMaxLife;
					rgbValue = (int)(30f + 225f * currentPercent);
					if (rgbValue < 30) {
						rgbValue = 30;
					}
					heartScale = currentPercent / 4f + 0.75f;
					if (heartScale < 0.75) {
						heartScale = 0.75f;
					}
					if (currentPercent > 0f) {
						beating = true;
					}
				}
				rHealth -= currentMaxLife;
				if (beating) {
					heartScale += Main.cursorScale - 1f;
				}
				int xOffset = 0;
				int yOffset = 0;
				if (i >= 10) {
					xOffset -= 260;
					yOffset += 26;
				}
				int alpha = (int)(rgbValue * 0.9);
				Vector2 position = new Vector2(500 + 26 * i + xOffset + UI_ScreenAnchorX + 11, 32f + (22 - 22 * heartScale) / 2f + (float)yOffset + 11);
				if(currentMaxLife>0)currentHeart.DrawInHearts(Main.spriteBatch, position, heartLife, goldenHearts-->0, new Color(rgbValue, rgbValue, rgbValue, alpha), new Vector2(11), heartScale);
				
                if (!PlayerInput.IgnoreMouseInterface && Main.keyState.IsKeyDown(Main.FavoriteKey)) {
					Vector2 topLeft = position - new Vector2(11);
					Vector2 bottomRight = position + new Vector2(11);
                    if (Main.MouseScreen.X>topLeft.X && Main.MouseScreen.Y>topLeft.Y && Main.MouseScreen.X<bottomRight.X && Main.MouseScreen.Y<bottomRight.Y) {
						Keys oldFav = Main.FavoriteKey;
                        Main.FavoriteKey = Keys.None;
                        try {
                            if (heartPlayer.hearts[i] is null) {
								heartPlayer.hearts[i] = new Item();
                            }
                            if (Main.playerInventory) {
								Main.LocalPlayer.mouseInterface = true;
								ItemSlot.Handle(ref heartPlayer.hearts[i], ItemSlot.Context.InventoryItem);
                            } else {
								ItemSlot.MouseHover(ref heartPlayer.hearts[i], ItemSlot.Context.InventoryItem);
                            }
                        } finally {
							Main.FavoriteKey = oldFav;
                        }
                    }
                }
			}
        }

        /*public TheOneWithTheHearts() {

		}
		public override void UpdateUI(GameTime gameTime) {
			heartUI?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"TheOneWithTheHearts: TheOnlyUIInTheMod",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						heartUI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}*/
	}
}
