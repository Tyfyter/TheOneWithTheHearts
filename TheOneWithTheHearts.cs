using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts {
	public class TheOneWithTheHearts : Mod {
        public static TheOneWithTheHearts mod;
		//public UserInterface heartUI;
		//public HeartUI ui;
		bool disableCombatText = false;
		public AutoCastingAsset<Texture2D> pipTexture;
		public override void Load() {
            mod = this;
			if (!Main.dedServ) {
				//heartUI = new UserInterface();
				pipTexture = Assets.Request<Texture2D>("UI/Pip");
			}
            On.Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet.DrawLife += Main_DrawInterface_Resources_Life;
            On.Terraria.Player.DropItems += Player_DropItems;
            On.Terraria.Player.UpdateLifeRegen += Player_UpdateLifeRegen;
            On.Terraria.CombatText.NewText_Rectangle_Color_int_bool_bool += CombatText_NewText;
			disableCombatText = false;
		}
        public override void Unload() {
			pipTexture = null;
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
					self.QuickSpawnClonedItem(self.GetSource_DropAsItem(), hearts[i]);
                }
            }
			orig(self);
        }

        private void Main_DrawInterface_Resources_Life(On.Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet.orig_DrawLife orig, Terraria.GameContent.UI.ResourceSets.ClassicPlayerResourcesDisplaySet self) {
			Player player = Main.LocalPlayer;
            /*if (player.controlSmart) {
				orig();
            }*/
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			if (heartPlayer is null) {
				orig(self);
				return;
			}
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
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(Lang.inter[0].Value + " " + player.statLifeMax2 + "/" + player.statLifeMax2);
			Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, new Vector2((float)(500 + 13 * rowWidth) - vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.MouseTextColorReal, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.DrawString(FontAssets.MouseText.Value, player.statLife + "/" + player.statLifeMax2, new Vector2((float)(500 + 13 * rowWidth) + vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.MouseTextColorReal, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(player.statLife + "/" + player.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
			int rHealth = player.statLife;
			bool canRemoveHearts = false;
			byte equippedHeartCount = 0;
			for (int i = 0; i < heartPlayer.MaxHearts; i++) {
				if ((heartPlayer.hearts[i]?.IsAir == false) && (++equippedHeartCount > 1)) {
					canRemoveHearts = true;
					break;
				}
			}
			for (int i = 0; i < heartPlayer.MaxHearts; i++) {
				HeartItemBase currentHeart = heartPlayer.hearts[i]?.ModItem as HeartItemBase;
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

				bool favHeld = Main.keyState.IsKeyDown(Main.FavoriteKey);
				if (currentMaxLife > 0) {
					currentHeart.DrawInHearts(Main.spriteBatch, position, heartLife, goldenHearts-- > 0, new Color(rgbValue, rgbValue, rgbValue, alpha), new Vector2(11), heartScale);
                } else if(favHeld){
					Main.spriteBatch.Draw(pipTexture, position, null, Color.White, 0, new Vector2(4), 0.75f, SpriteEffects.None, 0);
                }

                if (!PlayerInput.IgnoreMouseInterface) {
					Vector2 topLeft = position - new Vector2(11);
					Vector2 bottomRight = position + new Vector2(11);
                    if (Main.MouseScreen.X>topLeft.X && Main.MouseScreen.Y>topLeft.Y && Main.MouseScreen.X<bottomRight.X && Main.MouseScreen.Y<bottomRight.Y) {
                        if (heartPlayer.hearts[i] is null) {
							heartPlayer.hearts[i] = new Item();
                        }
						if(currentMaxLife > 0)currentHeart.renderingInHealthbar = true;
                        if (Main.playerInventory) {
							Main.LocalPlayer.mouseInterface = true;
							if (canRemoveHearts || !(ItemSlot.ShiftInUse || Main.mouseItem.IsAir)) {
								ItemSlot.Handle(ref heartPlayer.hearts[i], (ItemSlot.ShiftInUse && currentMaxLife > 0) ? ItemSlot.Context.EquipAccessory : ItemSlot.Context.BankItem);
							} else {
								ItemSlot.MouseHover(ref heartPlayer.hearts[i], ItemSlot.Context.InventoryItem);
							}
                        } else if(favHeld){
							Keys oldFav = Main.FavoriteKey;
							Main.FavoriteKey = Keys.None;
							try {
								ItemSlot.MouseHover(ref heartPlayer.hearts[i], ItemSlot.Context.InventoryItem);
							} finally {
								Main.FavoriteKey = oldFav;
							}
                        }
						if(currentMaxLife > 0)currentHeart.renderingInHealthbar = false;
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
	public struct AutoCastingAsset<T> where T : class {
		public bool HasValue => asset is not null;
		public bool IsLoaded => asset?.IsLoaded ?? false;
		public T Value => asset.Value;

		readonly Asset<T> asset;
		AutoCastingAsset(Asset<T> asset) {
			this.asset = asset;
		}
		public static implicit operator AutoCastingAsset<T>(Asset<T> asset) => new(asset);
		public static implicit operator T(AutoCastingAsset<T> asset) => asset.Value;
	}
	public static class HeartExtensions {
		public static AutoCastingAsset<Texture2D> RequestTexture(this Mod mod, string name) => mod.Assets.Request<Texture2D>(name);
		public static int RandomRound(this UnifiedRandom random, float value) {
			float amount = value % 1;
			value -= amount;
			if (random.NextFloat() < amount) {
				value++;
			}
			return (int)value;
		}
	}
}
