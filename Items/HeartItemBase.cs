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
        public virtual string GoldenTexture => Texture.Replace("Items", "Items/Golden");
        public int index = -2;
        public override bool CloneNewInstances{
			get { return true; }
		}
        public override bool Autoload(ref string name){
            if(name == "HeartItemBase")return false;
            return true;
        }
        public override void AutoStaticDefaults() {
            base.AutoStaticDefaults();
            if (!Main.dedServ && ModContent.TextureExists(GoldenTexture)) {
                Main.itemTexture[item.type].Tag = ModContent.GetTexture(GoldenTexture);
            }
        }
        public virtual void Damage(Player player, ref float damage, int heartIndex, int startIndex, bool crit = false, PlayerDeathReason reason = default){}
        public virtual void Heal(ref int healing, bool golden) { }
        public virtual void UpdateNaturalRegen(Player player, ref float regen, bool golden) { }
		/// <summary>
		/// allows a heart to modify health regeneration from buffs and accessories, returns the regen parameter by default
		/// </summary>
		/// <param name="player"></param>
		/// <param name="regen"></param>
		/// <returns>the amount of life the player will regenerate</returns>
		/// <param name="golden"></param>
        
		public virtual float ModifyLifeRegen(Player player, float regen, bool golden) {
            return regen;
        }
        public virtual void WhileInactive(Player player){}
        public virtual void WhileActive(Player player){}
        public virtual void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale){
            if (golden && Main.itemTexture[item.type].Tag is Texture2D goldenTexture) {
                spriteBatch.Draw(goldenTexture, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            } else {
                spriteBatch.Draw(Main.itemTexture[item.type], position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            }
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
        internal bool renderingInHealthbar = false;
		public override bool CanRightClick(){
            if (renderingInHealthbar) return false;
            HeartPlayer heartPlayer = Main.LocalPlayer.GetModPlayer<HeartPlayer>();
            int i = -1;
            for (i = 0; i < heartPlayer.MaxHearts; i++) {
                if (heartPlayer.hearts[i]?.IsAir??true) {
                    goto foundEmptySlot;
                }
            }
            i = -1;
            foundEmptySlot:
            if (i != -1) {
                Item clone = item.Clone();
                clone.stack = 1;
                heartPlayer.hearts[i] = clone;
                Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
                return true;
            }
			return false;
		}
    }
}