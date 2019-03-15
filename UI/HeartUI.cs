using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace TheOneWithTheHearts.UI
{
	// This class represents the UIState for the only UI in the entire mod, that should be obvious enough to make this redundant. 
	internal class HeartUI : UIState {
        VanillaItemSlotWrapper[] heartSlots = new VanillaItemSlotWrapper[15];
        public override void OnInitialize(){
            for (int i = 0; i < 15; i++){
                heartSlots[i] = new VanillaItemSlotWrapper(ItemSlot.Context.ChestItem, 0.85f){
				Left = { Pixels = 50+i*10 },
                Top = { Pixels = 270 },
                    ValidItemFunc = item => item.IsAir || !item.IsAir && (item.modItem!=null?item.modItem.mod==TheOneWithTheHearts.mod:true)
                };
                Append(heartSlots[i]);
            }
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(!Main.playerInventory)TheOneWithTheHearts.mod.heartUI.SetState(null);
        }protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			// Here we have a lot of code. This code is mainly adapted from the vanilla code for the reforge option.
			// This code draws "Place an item here" when no item is in the slot and draws the reforge cost and a reforge button when an item is in the slot.
			// This code could possibly be better as different UIElements that are added and removed, but that's not the main point of this example.
			// If you are making a UI, add UIElements in OnInitialize that act on your ItemSlot or other inputs rather than the non-UIElement approach you see below.

			for (int i = 0; i < 15; i++)if (!heartSlots[i].Item.IsAir) {
                int slotX = 50+i*10;
                int slotY = 270;
				int awesomePrice = Item.buyPrice(0, 1, 0, 0);

				string costText = Language.GetTextValue("LegacyInterface.46") + ": ";
				string coinsText = "";
				int[] coins = Utils.CoinsSplit(awesomePrice);
				if (coins[3] > 0) {
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Language.GetTextValue("LegacyInterface.15") + "] ";
				}
				if (coins[2] > 0) {
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Language.GetTextValue("LegacyInterface.16") + "] ";
				}
				if (coins[1] > 0) {
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Language.GetTextValue("LegacyInterface.17") + "] ";
				}
				if (coins[0] > 0) {
					coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Language.GetTextValue("LegacyInterface.18") + "] ";
				}
				ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2(slotX + 50 + Main.fontMouseText.MeasureString(costText).X, (float)slotY), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
				int reforgeX = slotX + 70;
				int reforgeY = slotY + 40;
				bool hoveringOverReforgeButton = Main.mouseX > reforgeX - 15 && Main.mouseX < reforgeX + 15 && Main.mouseY > reforgeY - 15 && Main.mouseY < reforgeY + 15 && !PlayerInput.IgnoreMouseInterface;
				Texture2D reforgeTexture = Main.reforgeTexture[hoveringOverReforgeButton ? 1 : 0];
				Main.spriteBatch.Draw(reforgeTexture, new Vector2(reforgeX, reforgeY), null, Color.White, 0f, reforgeTexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);
			}
			else {
                int slotX = 50+i*10;
                int slotY = 270;
				string message = "Place";
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			}
}
    }
}