/*using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Mech_Heart : HeartItemBase
	{
        public override int MaxLife => 60;
		public override string ExtraTexture => "_Extra";
		public override int RegenCD => (int)(regencd*(((life/(float)maxLife)*0.75f)+0.25f));
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Heart");
			Tooltip.SetDefault("Torn from a great foe.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.scale = 1/1.5f;
			item.height = (int)(44*item.scale);
			item.width = (int)(44*item.scale);
		}
		/ *
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//* /
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(index == -2)return true;
            GetAlpha(drawColor);
            if(mod.TextureExists("Items/"+this.GetType().Name))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), position-new Vector2(3,3), null, drawColor, 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
            item.alpha = 0;
			drawColor = Color.Lerp(Color.Red,Color.Blue,(life/(float)maxLife))*10;
            if(mod.TextureExists("Items/"+this.GetType().Name+"_Extra"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Extra"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
            index = -2;
            return false;
        }
		public override void WhileActive(Player player){
			base.WhileActive(player);
			player.statDefense+=20;
		}
		public override void WhileInactive(Player player){
			base.WhileInactive(player);
			if(life>=maxLife){
				player.statDefense+=2;
				player.endurance+=(1-player.endurance)*0.03f;
			}else{
				HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
				if(heartPlayer.GetCurrentHeart()>=0){
					if(++regencdtime>RegenCD*6){
						((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()].Item.modItem).Heal(1, 2, false);
						regencdtime=0;
					}
				}
			}
			player.lifeRegen++;
		}
	}
}
*/