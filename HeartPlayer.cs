using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts {
    public class HeartPlayer : ModPlayer {
        public int MaxHearts => Math.Min(Player.statLifeMax / 20, 20);
        public int GoldenHearts => Math.Min((Player.statLifeMax - 400) / 5, 20);
        public float HealthMultiplier { get; private set; }
        public Item[] hearts = new Item[20];
        public int oldStatLife = 0;
        public int multishot = 0;
        public int oldWitheredHearts = 0;
        public int witheredHearts = 0;
        public int oldMageHearts = 0;
        public int mageHearts = 0;
        public float partialRegen = 0;
        public bool frozenImmune = false;
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            //int defenseReduction = (int)Math.Min(player.statDefense * (Main.expertMode ? 0.75f : 0.5f), damage - 1);
            float reducedDamage = (float)Main.CalculateDamagePlayersTake(damage, Player.statDefense);
			float beetleDR = 0.15f * Player.beetleOrbs;
            float totalDM = (1f - Player.endurance) * (1f - beetleDR) * (Player.solarShields > 0 ? 0.7f : 1f);
            float rDamage = Math.Max(reducedDamage * totalDM, 1);//(damage - defenseReduction)
            float cHealth = Player.statLife;
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
                currentHeart = hearts[current.heart].ModItem as HeartItemBase;
                if (currentHeart is null) {
                    break;
                }
                currentHeart.Damage(Player, ref rDamage, current.heart, startIndex, crit, damageSource);
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
            int cHealth = Player.statLife;
            int tHealing = 0;
            (int heart, int life) current;
            HeartItemBase currentHeart;
            while (rHealing>0) {
                current = GetCurrentHeartWithHealth(cHealth+1);
                if (current.heart == -1) {
                    break;
                }
                currentHeart = hearts[current.heart].ModItem as HeartItemBase;
                currentHeart.Heal(ref rHealing, current.heart < GoldenHearts);
                float multipliers = 1f;
                if (currentHeart.GetsLifeBoosts) {
                    multipliers *= HealthMultiplier;
                    if (current.heart < GoldenHearts) {
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
            int index = GetCurrentHeart(Player.statLife + 1);
            (hearts[index]?.ModItem as HeartItemBase)?.UpdateNaturalRegen(Player, ref regen, index < GoldenHearts);
        }
        public int MultiplyLifeRegen(int regen) {
            int index = GetCurrentHeart(Player.statLife);
            if (index < 0) {
                return regen;
            }
            partialRegen += ((HeartItemBase)hearts[index]?.ModItem)?.ModifyLifeRegen(Player, regen, index < GoldenHearts)??0;
            int actualRegen = (int)(partialRegen - (partialRegen % 1));
            partialRegen -= actualRegen;
            return actualRegen;
        }
        public override void PostUpdateMiscEffects() {
            HealthMultiplier = Player.statLifeMax2 / (float)Player.statLifeMax;
            int currentHeart = GetCurrentHeart();
            int health = 0;
            for (int i = 0; i < MaxHearts; i++){
                if(hearts[i]?.ModItem is HeartItemBase heart) {
                    float multipliers = 1f;
                    if (heart.GetsLifeBoosts) {
                        multipliers *= HealthMultiplier;
                        if (i < GoldenHearts) {
                            multipliers *= 1.25f;
                        }
                    }
                    health += (int)(heart.MaxLife * multipliers);
                    if (i == currentHeart) {
                        heart.WhileActive(Player);
                    } else {
                        heart.WhileInactive(Player);
                    }
                }
            }
            Withered_Heart.GetStatBoosts(witheredHearts, out int minionSlots, out float minionDamage);
            Player.maxMinions += minionSlots;
            Player.GetDamage(DamageClass.Summon) += minionDamage;

            Mage_Heart.GetStatBoosts(witheredHearts, out float magicDamage, out float magicPen);
            Player.GetDamage(DamageClass.Magic) += magicDamage;
            Player.GetArmorPenetration(DamageClass.Magic) += magicPen;

            Player.statLifeMax2 = health > 0 ? health : 1;
            multishot = 0;
            oldWitheredHearts = witheredHearts;
            witheredHearts = 0;
        }
        public override void ResetEffects() {
            frozenImmune = false;
        }
        public override void PreUpdate() {
            if (frozenImmune && Player.frozen) {
                Player.frozen = false;
            }
        }
        public override void PostUpdate(){
            oldStatLife = Player.statLife;
        }
        /// <summary>
        /// Gets the index of the heart that would be active at the specified health value
        /// </summary>
        /// <param name="health">leave at -1 to use the player's current health</param>
        /// <returns></returns>
        public int GetCurrentHeart(int health = -1){
            //int a = 0;
            if(health == -1)health = Player.statLife;
            int highestIndex = 0;
            for (int i = 0; i < MaxHearts; i++){
                HeartItemBase heart = (hearts[i]?.ModItem as HeartItemBase);
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
            if(health == -1)health = Player.statLife;
            for (int i = 0; i < MaxHearts; i++){
                HeartItemBase heart = (hearts[i].ModItem as HeartItemBase);
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
        public override void SaveData(TagCompound tag) {
            tag.Add("hearts", hearts.Select(v => v ?? new Item()).ToList());
        }
        public override void LoadData(TagCompound tag){
            if(tag.ContainsKey("hearts")) {
                hearts = tag.GetList<Item>("hearts").ToArray();
            } else {
                try {
                    for (int i = 0; i < 20; i++) {
                        if (tag.ContainsKey("heart"+i)) {
                            hearts[i] = tag.Get<Item>("heart" + i);
                        }
                    }
                } catch (Exception) { }
            }
			if (hearts.Length != 20) {
                hearts = new Item[20];
			}
            retry:
            int airCount = 0;
            for (int i = 0; i < MaxHearts; i++) {
                if (hearts[i]?.IsAir??false) {
                    airCount++;
                } else if (hearts[i]?.ModItem is null) {
                    hearts[i] = new Item(ModContent.ItemType<Default_Heart>());
                    Mod.Logger.Info("added heart #" + (i + 1) + " on load");
                }
            }
			if (airCount >= 20) {
                hearts = new Item[20];
                goto retry;
			} else if(airCount >= MaxHearts) {
                hearts = hearts.ToList().OrderBy(h => h.IsAir ? 1 : 0).ToArray();
                goto retry;
            }
        }
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
            for (int i = 0; i < 20; i++) {
                hearts[i] = new Item(ModContent.ItemType<Default_Heart>());
                Mod.Logger.Info("added heart #" + (i + 1) + " on AddStartingItems");
            }
            return Array.Empty<Item>();
        }
    }
}