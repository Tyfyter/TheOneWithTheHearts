using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class ETG_Heart : HeartItemBase
	{
		public int heal = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Familliar Heart");
			Tooltip.SetDefault("This seems vaguely familliar.");
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
			life = max = 2;
		}
		public override bool CanRightClick(){
			if(Main.mouseItem==null){
				Item i = item.Clone();
				i.stack = 1;
				Main.mouseItem = i;
				Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
				return true;
			}
			if(Main.mouseItem.IsAir){
				Item i = item.Clone();
				i.stack = 1;
				Main.mouseItem = i;
				Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
				return true;
			}
			return false;
		}
		public override void Damage(int damage, bool crit = false){
			life = Math.Max(life-(crit?2:1),0);
            PlayerDeathReason ignore = new PlayerDeathReason();
            ignore.SourceCustomReason = "this_shouldn't_be_able_to_kill";
            int h = Main.player[item.owner].statLife;
            Main.player[item.owner].Hurt(ignore, 0, 0, false, false, crit);
            Main.player[item.owner].statLife = h;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(index == -2)return true;
            item.alpha = 0;
			switch (life)
			{
				case 2:
            	if(mod.TextureExists("Items/"+this.GetType().Name))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), position-new Vector2(3,3), null, drawColor, 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				case 1:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Half"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Half"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				default:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Empty"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Empty"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
			}
            index = -2;
            return false;
        }
        public override void Heal(int health, int overflow = 2, bool display = false){
            int plife = life;
			if(health%10!=0){
				heal+=health%10;
				if(heal>30)Heal(10, display:true);
			}
            life = (int)(overflow == 1?life+(health/10f):Math.Min(life+(health/10f),max));
            if(overflow >= 2&&(life-plife)*10<health&&life-plife>0){
                if(!TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.IsAir)((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.modItem).Heal(health-(life-plife),3);
            }
			if(!display)return;
            if(overflow == 2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, health);
            }else if (overflow<2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, life-plife);
            }
        }
		public override Color? GetAlpha(Color lightColor){item.alpha = 0;return null;}
		public override void WhileActive(Player player){player.endurance+=10;}
		public override void WhileInactive(Player player){heal=0;}
	}
}
