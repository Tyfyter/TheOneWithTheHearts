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
        public int witheredHearts = 0;
        public float partialRegen = 0;
        public bool frozenImmune = false;
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            //int defenseReduction = (int)Math.Min(player.statDefense * (Main.expertMode ? 0.75f : 0.5f), damage - 1);
            float reducedDamage = (float)Main.CalculatePlayerDamage(damage, player.statDefense);
			float beetleDR = 0.15f * player.beetleOrbs;
            float totalDM = (1f - player.endurance) * (1f - beetleDR) * (player.solarShields > 0 ? 0.7f : 1f);
            float rDamage = Math.Max(reducedDamage * totalDM, 1);//(damage - defenseReduction)
            float cHealth = player.statLife;
            float tDamage = 0;
            (int heart, int life) current;
            HeartItemBase currentHeart;
            int startIndex = -1;
            while (rDamage>0) {
                current = GetCurrentHeartWithHealth((int)cHealth);
                if (current.heart == -1) {
                    break;
                }
                if (startIndex == -1) {
                    startIndex = current.heart;
                }
                currentHeart = hearts[current.heart].modItem as HeartItemBase;
                if (currentHeart is null) {
                    break;
                }
                currentHeart.Damage(player, ref rDamage, current.heart, startIndex, crit, damageSource);
                if (rDamage > current.life) {
                    tDamage += current.life;
                    cHealth -= current.life;
                    rDamage -= current.life;
                } else if(rDamage > 0){
                    tDamage += rDamage;
                    cHealth -= rDamage;
                    rDamage -= rDamage;
                } else {
                    rDamage = 0;
                }
                if (current.life <= 0) {
                    tDamage += rDamage;
                    break;
                }
            }
            damage = (int)Math.Ceiling(tDamage / (totalDM > 0 ? totalDM : 1));// + defenseReduction;
            customDamage = true;
            return damage > 0;
        }
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue) {
            int rHealing = healValue;
            int cHealth = player.statLife;
            int tHealing = 0;
            (int heart, int life) current;
            HeartItemBase currentHeart;
            while (rHealing>0) {
                current = GetCurrentHeartWithHealth(cHealth+1);
                if (current.heart == -1) {
                    break;
                }
                currentHeart = hearts[current.heart].modItem as HeartItemBase;
                currentHeart.Heal(ref rHealing);
                float multipliers = 1f;
                if (currentHeart.GetsLifeBoosts) {
                    multipliers *= HealthMultiplier;
                    if (current.heart<GoldenHearts) {
                        multipliers *= 1.25f;
                    }
                }
                int maxLife = (int)(currentHeart.MaxLife * multipliers);
                int currentLife = current.life - 1;
                if (rHealing > maxLife-currentLife) {
                    tHealing += maxLife-currentLife;
                    cHealth += maxLife-currentLife;
                    rHealing -= maxLife-currentLife;
                    if (maxLife-currentLife <= 0) {
                        cHealth += 1;
                    }
                } else {
                    tHealing += rHealing;
                    cHealth += rHealing;
                    rHealing -= rHealing;
                }
                if (rHealing <= 1) {
                    break;
                }
            }
            healValue = tHealing;
        }
        public override void NaturalLifeRegen(ref float regen) {
            (hearts[GetCurrentHeart(player.statLife+1)]?.modItem as HeartItemBase)?.UpdateNaturalRegen(player, ref regen);
        }
        public int MultiplyLifeRegen(int regen) {
            int index = GetCurrentHeart(player.statLife);
            if (index < 0) {
                return regen;
            }
            partialRegen += ((HeartItemBase)hearts[index].modItem).ModifyLifeRegen(player, regen);
            int actualRegen = (int)(partialRegen - (partialRegen % 1));
            partialRegen -= actualRegen;
            return actualRegen;
        }
        public override void PostUpdateMiscEffects() {
            HealthMultiplier = player.statLifeMax2 / (float)player.statLifeMax;
            int currentHeart = GetCurrentHeart();
            int health = 0;
            for (int i = 0; i < MaxHearts; i++){
                if(hearts[i]?.modItem is HeartItemBase heart) {
                    float multipliers = 1f;
                    if (heart.GetsLifeBoosts) {
                        multipliers *= HealthMultiplier;
                        if (i<GoldenHearts) {
                            multipliers *= 1.25f;
                        }
                    }
                    health += (int)(heart.MaxLife * multipliers);
                    if (i == currentHeart) {
                        heart.WhileActive(player);
                    } else {
                        heart.WhileInactive(player);
                    }
                }
            }
            Withered_Heart.GetStatBoosts(witheredHearts, out int minionSlots, out float minionDamage);
            player.maxMinions += minionSlots;
            player.minionDamage += minionDamage;
            player.statLifeMax2 = health;
            multishot = 0;
            witheredHearts = 0;
        }
        public override void ResetEffects() {
            frozenImmune = false;
        }
        public override void PreUpdate() {
            if (frozenImmune && player.frozen) {
                player.frozen = false;
            }
        }
        public override void PostUpdate(){
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
            int highestIndex = 0;
            for (int i = 0; i < MaxHearts; i++){
                HeartItemBase heart = (hearts[i]?.modItem as HeartItemBase);
                int currentMaxLife = heart?.MaxLife ?? 0;
                float multipliers = 1f;
                if (heart?.GetsLifeBoosts??false) {
                    multipliers *= HealthMultiplier;
                    if (i<GoldenHearts) {
                        multipliers *= 1.25f;
                    }
					currentMaxLife = (int)(currentMaxLife * multipliers);
                }
                if (currentMaxLife > 0) {
                    highestIndex = i;
                }
                if (health <= currentMaxLife) {
                    return i;
                }
                health -= currentMaxLife;
            }
            return highestIndex;
        }
        /// <summary>
        /// Gets the heart that would be active at the specified health value, and the amount of health contained in that heart
        /// </summary>
        /// <param name="health">leave at -1 to use the player's current health</param>
        /// <returns></returns>
        public (int, int) GetCurrentHeartWithHealth(int health = -1){
            //int a = 0;
            if(health == -1)health = player.statLife;
            for (int i = 0; i < MaxHearts; i++){
                HeartItemBase heart = (hearts[i].modItem as HeartItemBase);
                int currentMaxLife = heart?.MaxLife ?? 0;
                float multipliers = 1f;
                if (heart?.GetsLifeBoosts??false) {
                    multipliers *= HealthMultiplier;
                    if (i<GoldenHearts) {
                        multipliers *= 1.25f;
                    }
					currentMaxLife = (int)(currentMaxLife * multipliers);
                }
                if (health <= currentMaxLife) {
                    return (i, health);
                }
                health -= currentMaxLife;
            }
            return (-1, 0);
        }
        public override TagCompound Save(){
            if(hearts is null)return new TagCompound();
            TagCompound r = new TagCompound{
                {"hearts", hearts.Select(v=>v??new Item()).ToList()}
            };
            return r;
        }
        public override void Load(TagCompound tag){
            try {
                hearts = tag.GetList<Item>("hearts").ToArray();
            } catch (Exception) {
                try {
                    for (int i = 0; i < 20; i++) {
                        try {
                            hearts[i] = tag.Get<Item>("heart" + i);
                        } catch (Exception) {
                            hearts[i] = new Item();
                            hearts[i].SetDefaults(ModContent.ItemType<Default_Heart>());
                        }
                    }
                } catch (Exception) { }
            }
        }
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath) {
            for (int i = 0; i < 20; i++) {
                hearts[i] = new Item();
                hearts[i].SetDefaults(ModContent.ItemType<Default_Heart>());
            }
        }
    }
}