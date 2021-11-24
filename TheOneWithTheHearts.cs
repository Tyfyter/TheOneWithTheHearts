using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TheOneWithTheHearts.Items;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts {
	public class TheOneWithTheHearts : Mod {
        public static TheOneWithTheHearts mod;
		public UserInterface heartUI;
		public HeartUI ui;
		public override void Load() {
            mod = this;
			if (!Main.dedServ) {
				heartUI = new UserInterface();
			}
            On.Terraria.Main.DrawInterface_Resources_Life += Main_DrawInterface_Resources_Life;
		}

        private void Main_DrawInterface_Resources_Life(On.Terraria.Main.orig_DrawInterface_Resources_Life orig) {
			Player player = Main.LocalPlayer;
			HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
			float UI_ScreenAnchorX = Main.screenWidth - 800;
			float halfHeartWidth = 11;
			if (player.ghost) {
				return;
			}
			int hearts = heartPlayer.MaxHearts;
			int goldenHearts = heartPlayer.GoldenHearts;
			if (goldenHearts < 0) {
				goldenHearts = 0;
			}
			int rowWidth = hearts >= 10 ? 10 : hearts;
			Vector2 vector = Main.fontMouseText.MeasureString(Lang.inter[0].Value + " " + player.statLifeMax2 + "/" + player.statLifeMax2);
			Main.spriteBatch.DrawString(Main.fontMouseText, Lang.inter[0].Value, new Vector2((float)(500 + 13 * rowWidth) - vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.mouseTextColorReal, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.DrawString(Main.fontMouseText, player.statLife + "/" + player.statLifeMax2, new Vector2((float)(500 + 13 * rowWidth) + vector.X * 0.5f + UI_ScreenAnchorX, 6f), Main.mouseTextColorReal, 0f, new Vector2(Main.fontMouseText.MeasureString(player.statLife + "/" + player.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
			int rHealth = player.statLife;
			for (int i = 0; i < heartPlayer.MaxHearts; i++) {
				HeartItemBase currentHeart = heartPlayer.hearts[i].modItem as HeartItemBase;
				int rgbValue = 255;
				float heartScale = 1f;
				bool flag = false;
				if ((float)player.statLife >= (float)i * UIDisplay_LifePerHeart) {
					rgbValue = 255;
					if ((float)player.statLife == (float)i * UIDisplay_LifePerHeart) {
						flag = true;
					}
				} else {
					float currentPercent = ((float)player.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
					rgbValue = (int)(30f + 225f * currentPercent);
					if (rgbValue < 30) {
						rgbValue = 30;
					}
					heartScale = currentPercent / 4f + 0.75f;
					if (heartScale < 0.75) {
						heartScale = 0.75f;
					}
					if (currentPercent > 0f) {
						flag = true;
					}
				}
				if (flag) {
					heartScale += Main.cursorScale - 1f;
				}
				int xOffset = 0;
				int yOffset = 0;
				if (i >= 10) {
					xOffset -= 260;
					yOffset += 26;
				}
				int alpha = (int)(rgbValue * 0.9);
				if (!player.ghost) {
					if (goldenHearts > 0) {
						goldenHearts--;
						Main.spriteBatch.Draw(heart2Texture, new Vector2(500 + 26 * i + xOffset + UI_ScreenAnchorX + 11, 32f + (22 - 22 * heartScale) / 2f + (float)yOffset + 11), new Rectangle(0, 0, 22, 22), new Color(rgbValue, rgbValue, rgbValue, alpha), 0f, new Vector2(11), heartScale, SpriteEffects.None, 0f);
					} else {
						Main.spriteBatch.Draw(heartTexture, new Vector2(500 + 26 * i + xOffset + UI_ScreenAnchorX + 11, 32f + (22 - 22 * heartScale) / 2f + (float)yOffset + 11), new Rectangle(0, 0, 22, 2), new Color(rgbValue, rgbValue, rgbValue, alpha), 0f, new Vector2(11), heartScale, SpriteEffects.None, 0f);
					}
				}
			}
        }

        public TheOneWithTheHearts() {

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
		}
	}
}
