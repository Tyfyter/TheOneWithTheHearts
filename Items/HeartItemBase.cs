using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items {
    public abstract class HeartItemBase : ModItem {
        public virtual int MaxLife => 20;
        public virtual bool GetsLifeBoosts => true;
        public virtual string GoldenTexture => Texture.Replace("Items", "Items/Golden");
        public int index = -2;
        protected override bool CloneNewInstances => true;
        public override void AutoStaticDefaults() {
            base.AutoStaticDefaults();
            if (!Main.dedServ && ModContent.HasAsset(GoldenTexture)) {
                TextureAssets.Item[Item.type].Value.Tag = (AutoCastingAsset<Texture2D>)ModContent.Request<Texture2D>(GoldenTexture);
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
        public virtual void DrawInHearts(SpriteBatch spriteBatch, Vector2 position, int life, bool golden, Color drawColor, Vector2 origin, float scale, int index) {
            if (golden && TextureAssets.Item[Item.type].Value.Tag is Texture2D goldenTexture) {
                spriteBatch.Draw(goldenTexture, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            } else {
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
            }
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI){
            Rectangle rect = new Rectangle((int)Item.position.X,(int)Item.position.Y,Item.width,Item.height);
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, Item.Center, rect, lightColor, 0, new Vector2(22,22), scale, SpriteEffects.None, 0);
            return false;
        }
        internal bool renderingInHealthbar = false;
		public override bool CanRightClick(){
            if (renderingInHealthbar) return false;
            HeartPlayer heartPlayer = Main.LocalPlayer.GetModPlayer<HeartPlayer>();
            int i = -1;
            for (i = 0; i < heartPlayer.MaxHearts; i++) {
                if (heartPlayer.hearts[i]?.IsAir ?? true) {
                    goto foundSlot;
                }
            }
            i = -1;
            if(Item.maxStack == 1 && Main.mouseRightRelease)for (i = 0; i < heartPlayer.MaxHearts; i++) {
                if (heartPlayer.hearts[i].type != Item.type) {
					for (int i1 = 0; i1 < Main.InventoryItemSlotsCount; i1++) {
						if (Main.LocalPlayer.inventory[i1] == Item) {
                            Utils.Swap(ref Main.LocalPlayer.inventory[i1], ref heartPlayer.hearts[i]);
                            SoundEngine.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
                            SoundEngine.PlaySound(SoundID.Grab, Main.LocalPlayer.Center);
                            return false;
						}
                    }
                }
            }
            foundSlot:
            if (i != -1) {
                Item clone = Item.Clone();
                clone.stack = 1;
                heartPlayer.hearts[i] = clone;
                SoundEngine.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
                return true;
            }
			return false;
		}
    }
}