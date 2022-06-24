/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts.UI
{
	// This class represents the UIState for the only UI in the entire mod, that should be obvious enough to make this redundant. 
	public class HeartUI : UIState {
        public VanillaItemSlotWrapper[] heartSlots = new VanillaItemSlotWrapper[20];
        public override void OnInitialize(){
            for (int i = 0; i < 20; i++) {
                heartSlots[i] = new VanillaItemSlotWrapper(scale: 0.5f) {
                    Left = { Pixels = 1128 + ((i) % 10) * 26 },
                    Top = { Pixels = i > 9 ? -45 : -70 }
                };
                heartSlots[i].ValidItemFunc = item => 
                (!item.IsAir && item.ModItem is HeartItemBase) ||
                (item.IsAir && heartSlots.Except(new[] { heartSlots[i] }).Any((v)=>!(v?.Item?.IsAir ?? true)));

                if (Main.LocalPlayer.GetModPlayer<HeartPlayer>().hearts[i] != null) {
                    heartSlots[i].Item = Main.LocalPlayer.GetModPlayer<HeartPlayer>().hearts[i];
                }
                Append(heartSlots[i]);
            }
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            for (int i = 0; i < 20; i++) if (heartSlots[i]?.Item?.ModItem is HeartItemBase heartItem) {
                heartItem.index = i;
            }
        }
		public override void OnDeactivate(){
			UpdatePlayer();
		}
        public void UpdatePlayer() {
            HeartPlayer heartPlayer = Main.LocalPlayer.GetModPlayer<HeartPlayer>();
            for (int i = 0; i < 20; i++) {
                heartPlayer.hearts[i] = heartSlots[i].Item;
            }
		}
    }
}*/