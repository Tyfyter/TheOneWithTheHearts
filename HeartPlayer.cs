using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using TheOneWithTheHearts.Items;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts {
    public class HeartPlayer : ModPlayer {
        public int MaxHearts => Math.Min(player.statLifeMax / 20, 20);
        public int GoldenHearts => Math.Min((player.statLifeMax - 400) / 5, 20);
        public float HealthMultiplier { get; private set; }
        public Item[] hearts = new Item[20];
        public int oldStatLife = 0;
        public int multishot = 1;
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            int rDamage = damage;
            int cHealth = player.statLife;
            int tDamage = 0;
            (HeartItemBase heart, int life) current;
            while (rDamage>0) {
                current = GetCurrentHeartWithHealth(cHealth);
                if (current.heart is null) {
                    return true;
                }
                current.heart.Damage(ref rDamage, crit, damageSource);
                if (rDamage > current.life) {
                    tDamage += current.life;
                    cHealth -= current.life;
                    rDamage -= current.life;
                } else {
                    tDamage += rDamage;
                    cHealth -= rDamage;
                    rDamage -= rDamage;
                }
            }
            damage = tDamage;
            return true;
        }
        public override void PostUpdateMiscEffects() {
            HealthMultiplier = player.statLifeMax2 / (float)player.statLifeMax;
            int health = 0;
            for (int i = 0; i < MaxHearts; i++){
                health += (hearts[i].modItem as HeartItemBase).MaxLife;
            }
            player.statLifeMax2 = health;
        }
        public override void PostUpdate(){
            if(!Main.gameInactive){
                multishot = 1;
                int currentHeart = GetCurrentHeart();
                for (int i = 0; i < MaxHearts; i++){
                    if (i == currentHeart) {
                        (hearts[i]?.modItem as HeartItemBase)?.WhileActive(player);
                    } else {
                        (hearts[i]?.modItem as HeartItemBase)?.WhileInactive(player);
                    }
                }
            }
            oldStatLife = player.statLife;
        }
        /// <summary>
        /// Gets the index of the heart that would be active at the specified health value
        /// </summary>
        /// <param name="health">leave at -1 to use the player's current health</param>
        /// <returns></returns>
        public int GetCurrentHeart(int health = -1){
            //int a = 0;
            if(health == -1)health = player.statLife;
            for (int i = 0; i < MaxHearts; i++){
                int heart = (hearts[i].modItem as HeartItemBase).MaxLife;
                if (health <= heart) {
                    return i;
                }
                health -= heart;
            }
            return -1;
        }
        /// <summary>
        /// Gets the heart that would be active at the specified health value, and the amount of health contained in that heart
        /// </summary>
        /// <param name="health">leave at -1 to use the player's current health</param>
        /// <returns></returns>
        public (HeartItemBase, int) GetCurrentHeartWithHealth(int health = -1){
            //int a = 0;
            if(health == -1)health = player.statLife;
            for (int i = 0; i < MaxHearts; i++){
                HeartItemBase heart = (hearts[i].modItem as HeartItemBase);
                if (health <= heart.MaxLife) {
                    return (heart, health);
                }
                health -= heart.MaxLife;
            }
            return (null, 0);
        }
        public override TagCompound Save(){
            if(hearts is null)return new TagCompound();
            TagCompound r = new TagCompound{
                {"hearts", hearts.ToList()}
            };
            return r;
        }
        public override void Load(TagCompound tag){
            Version lastSaveVersion = new Version(0, 1, 1, 0);
            if(tag.ContainsKey("saveVersion"))Version.TryParse(tag.Get<string>("saveVersion"), out lastSaveVersion);

            if (lastSaveVersion > new Version(0, 1, 1, 0)) {
                hearts = tag.GetList<Item>("hearts").ToArray();
            } else {
                try {
                    for (int i = 0; i < 20; i++) {
                        hearts[i] = tag.Get<Item>("heart"+i);
                    }
                    //for (int i = 0; i < TheOneWithTheHearts.mod.ui.heartSlots.Length; i++)if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.type==ModLoader.GetMod("ModLoader").ItemType("MysteryItem"))TheOneWithTheHearts.mod.ui.heartSlots[i].Item.TurnToAir();
                } catch (System.Exception) { }
            }
        }
    }
}