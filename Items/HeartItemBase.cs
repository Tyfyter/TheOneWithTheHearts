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
        public virtual int MaxLife => 20;
        public virtual bool GetsLifeBoosts => true;
        public virtual string ExtraTexture {
            get {return "";}
        }
        public int index = -2;
        public override bool CloneNewInstances{
			get { return true; }
		}
        public override bool Autoload(ref string name){
            if(name == "HeartItemBase")return false;
            return true;
        }
        public virtual void Damage(ref int damage, bool crit = false, PlayerDeathReason reason = default){}
        public virtual void WhileInactive(Player player){}
        public virtual void WhileActive(Player player){}
        public virtual void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture+"_Pre"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture+"_Pre"), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale, SpriteEffects.None, 0);
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture), position, null, drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
            if(mod.TextureExists("Items/"+this.GetType().Name+ExtraTexture+"_Extra"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+ExtraTexture+"_Extra"), position, null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale, SpriteEffects.None, 0);
        }

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
			if(Main.mouseItem?.IsAir??true){
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