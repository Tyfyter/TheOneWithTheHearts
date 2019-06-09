using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items {
    public class HeartItemBase : ModItem {
        public int max = 20;
        public int life = 20;
        public int regen = 1;
        public int regencd = 20;
        public int regencdtime = 0;
        public int regentime = 0;
        public int regentimemax = 1;
        public virtual string ExtraTexture {
            get {return "";}
        }
        public virtual int Regen {
            get {return regen;}
        }
        public virtual int RegenCD {
            get {return regencd;}
        }
        public int index = -2;
        public override bool CloneNewInstances{
			get { return true; }
		}
        public override bool Autoload(ref string name){
            if(name == "HeartItemBase")return false;
            return true;
        }
        public override Color? GetAlpha(Color lightColor){
            item.alpha = 255-(int)(75+((life/(float)max)*180));
            return null;
        }
        public virtual void Damage(int damage, bool crit = false, PlayerDeathReason reason = default(PlayerDeathReason)){
            CombatText.NewText(Main.player[item.owner].Hitbox, crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly, damage);
            life = Math.Max(life-damage,0);
            regentime = 0;
        }
        public void AutoHeal(int health){
            //Main.NewText(item.Name+".Heal("+health+"-Math.Max("+Main.player[item.owner].statLife+"-"+Main.player[item.owner].GetModPlayer<HeartPlayer>().oldStatLife+",0))");
            Heal(health-Math.Max(Main.player[item.owner].statLife-Main.player[item.owner].GetModPlayer<HeartPlayer>().oldStatLife,0));
        }
        public virtual void Heal(int health, int overflow = 2, bool display = true){
            int plife = life;
            life = overflow == 1?life+health:Math.Min(life+health,max);
            if(overflow >= 2&&life-plife<health&&life-plife>0){
                if(!TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.IsAir)((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.modItem).Heal(health-(life-plife),3);
            }
            if(!display)return;
            if(overflow == 2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, health);
            }else if (overflow<2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, life-plife);
            }
        }
        public virtual void DoRegen(){
            life=Math.Min(life+Regen,max);regencdtime=0;
        }
        public virtual bool CanDoRegen(Player player){
            return Main.player[item.owner].statLife>=Main.player[item.owner].statLifeMax2;
        }
        public virtual bool CanDoRegenTime(bool active){
            return active;
        }
        public virtual void WhileActive(Player player){
            if(CanDoRegen(player))if(++regencdtime>RegenCD&&life<max){
                DoRegen();
            }
            if(regentime<regentimemax&&CanDoRegenTime(true))regentime++;
            if(life>=max){
                regencdtime = 0;
                regentime = 0;
            }
        }
        public virtual void WhileInactive(Player player){
            bool f = false;
            if(index<=0)f = true;
            if(!f)if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index-1].Item.modItem).life>=((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index-1].Item.modItem).max)f = true;
            if(CanDoRegen(player))if(f){
                if(++regencdtime>RegenCD*3){
                    if(life<max){life++;}regencdtime=0;
                }
            }
            if(regentime<regentimemax&&CanDoRegenTime(false))regentime++;
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			if(index!=-2){
                for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
            }
		}
        //*
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(index == -2)return true;
            item.alpha = 0;
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture+"_Pre"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture+"_Pre"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
            GetAlpha(drawColor);
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture), position-new Vector2(3,3), null, drawColor, 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
            item.alpha = 0;
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture+"_Extra"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture+"_Extra"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
            index = -2;
            return false;
        }//*/
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
            Rectangle rect = new Rectangle((int)item.position.X,(int)item.position.Y,item.width,item.height);
            if(mod.TextureExists("Items/"+this.GetType().Name)&&false){
                spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), item.Center, rect, lightColor, 0, new Vector2(22,22), scale, SpriteEffects.None, 0);
            }else{
                scale*=item.scale;
				return true;
			}
            return false;
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
    }
}