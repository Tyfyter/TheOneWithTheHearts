using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts
{
	class TheOneWithTheHearts : Mod
	{
        public static TheOneWithTheHearts mod;
		public UserInterface heartUI;
		public HeartUI ui;
		public override void Load(){
            mod = this;
			if (!Main.dedServ){
				heartUI = new UserInterface();
			}
		}
		public TheOneWithTheHearts()
		{

		}
		public override void UpdateUI(GameTime gameTime) {
			heartUI?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"ExampleMod: Example Person UI",
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
