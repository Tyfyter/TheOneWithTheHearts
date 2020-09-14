using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;
using TheOneWithTheHearts.Items;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts {
    public class HeartPlayer : ModPlayer {
        public List<Item> hearts = new List<Item>(20){null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null};
        public int oldStatLife = 0;
        public int multishot = 1;
        public static PlayerDeathReason ignore = new PlayerDeathReason();
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            ignore.SourceCustomReason = "this_shouldn't_be_able_to_kill";
            if(damageSource.SourceCustomReason!=null)if(damageSource.SourceCustomReason.Equals(ignore.SourceCustomReason)){
                damageSource.SourceCustomReason = damageSource.SourceCustomReason.Replace(ignore.SourceCustomReason, "");
                return true;
            }
            if(getCurrentHeart()>=0)if(TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.type>0)if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).GetType().IsSubclassOf(typeof(HeartItemBase))){
                if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).life<=0)return true;
                int dmg = damage - Math.Max(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).life,0);
                ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).Damage(damage, crit, damageSource);
                //((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).life=Math.Max(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).life-damage,0);
                damage = dmg;
                if (damage>0){
                    player.Hurt(damageSource, damage, hitDirection, pvp, quiet, crit);
                    return false;
                }
                int h = player.statLife;
                player.Hurt(ignore, 0, hitDirection, pvp, quiet, crit);
                player.statLife = h;
                return false;
            }
            return true;
        }
        public override void PreUpdate(){
            if(TheOneWithTheHearts.mod.heartUI.CurrentState!=null)if(TheOneWithTheHearts.mod.heartUI.CurrentState.ToString().ToLower().Contains("heart"))return;
            TheOneWithTheHearts.mod.ui = new HeartUI();
            TheOneWithTheHearts.mod.ui.Activate();
            TheOneWithTheHearts.mod.heartUI.SetState(TheOneWithTheHearts.mod.ui);
            TheOneWithTheHearts.mod.heartUI.CurrentState.Height.Pixels = 100;
            TheOneWithTheHearts.mod.heartUI.CurrentState.Width.Pixels = 100;
            TheOneWithTheHearts.mod.heartUI.CurrentState.Top.Pixels = 100;
            TheOneWithTheHearts.mod.heartUI.CurrentState.Left.Pixels = 100;
            TheOneWithTheHearts.mod.heartUI.IsVisible = true;
        }
        public override void PostUpdate(){
            string a = "!";
            if(!Main.gameInactive){
                multishot = 1;
                if(getCurrentHeart()>=0)if(TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem!=null)if(TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem.mod.Name==mod.Name)if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).GetType().IsSubclassOf(typeof(HeartItemBase))){
                    ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[getCurrentHeart()].Item.modItem).WhileActive(player);
                }
                for (int i = 0; i < 20; i++)if(i!=getCurrentHeart())if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem!=null)if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem.mod.Name==mod.Name)if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).GetType().IsSubclassOf(typeof(HeartItemBase))){
                    ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).WhileInactive(player);
                    //a+=i+"i";
                }
                //player.chatOverhead.NewMessage("",5);
                //player.chatOverhead.chatText=a;
            }
            try {
            for (int i = 0; i < TheOneWithTheHearts.mod.ui.heartSlots.Length; i++)if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.type==ModLoader.GetMod("ModLoader").ItemType("MysteryItem")){
                if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.type==ModLoader.GetMod("ModLoader").ItemType("MysteryItem")){
                    TagCompound ynopublic = ((MysteryItem)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).Save();
                    if(!(ynopublic.GetString("mod").Equals("")&&ynopublic.GetString("name").Equals("")))player.QuickSpawnClonedItem(TheOneWithTheHearts.mod.ui.heartSlots[i].Item,TheOneWithTheHearts.mod.ui.heartSlots[i].Item.stack);
                }
                TheOneWithTheHearts.mod.ui.heartSlots[i].Item.TurnToAir();
                }
            }
            catch (System.Exception){}
            oldStatLife = player.statLife;
        }
        public override void OnRespawn(Player player){
            /*
            for (int i = 0; i < 5; i++){
                ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).life=((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).max;
            }//*/
            if(!TheOneWithTheHearts.mod.ui.heartSlots[0].Item.IsAir)((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[0].Item.modItem).Heal(100);
        }
        public int getCurrentHeart(){
            //int a = 0;
            for (int i = 19; i >= 0; i--){
                if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.IsAir)continue;
                //a++;
                if(!(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem.GetType().IsSubclassOf(typeof(HeartItemBase))))continue;
                if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[i].Item.modItem).life>0)return i;
            }
            //if(a==0)Main.NewText("found nothing");
            return -1;
        }
        public override TagCompound Save(){
            if(player == null||hearts==null)return new TagCompound();
            TagCompound r = new TagCompound{
                {"heart0",hearts[0]},
                {"heart1",hearts[1]},
                {"heart2",hearts[2]},
                {"heart3",hearts[3]},
                {"heart4",hearts[4]},
                {"heart5",hearts[5]},
                {"heart6",hearts[6]},
                {"heart7",hearts[7]},
                {"heart8",hearts[8]},
                {"heart9",hearts[9]},
                {"heart10",hearts[10]},
                {"heart11",hearts[11]},
                {"heart12",hearts[12]},
                {"heart13",hearts[13]},
                {"heart14",hearts[14]},
                {"heart15",hearts[15]},
                {"heart16",hearts[16]},
                {"heart17",hearts[17]},
                {"heart18",hearts[18]},
                {"heart19",hearts[19]},
                {"bheart0",TheOneWithTheHearts.mod.ui.heartSlots[0].Item},
                {"bheart1",TheOneWithTheHearts.mod.ui.heartSlots[1].Item},
                {"bheart2",TheOneWithTheHearts.mod.ui.heartSlots[2].Item},
                {"bheart3",TheOneWithTheHearts.mod.ui.heartSlots[3].Item},
                {"bheart4",TheOneWithTheHearts.mod.ui.heartSlots[4].Item},
                {"bheart5",TheOneWithTheHearts.mod.ui.heartSlots[5].Item},
                {"bheart6",TheOneWithTheHearts.mod.ui.heartSlots[6].Item},
                {"bheart7",TheOneWithTheHearts.mod.ui.heartSlots[7].Item},
                {"bheart8",TheOneWithTheHearts.mod.ui.heartSlots[8].Item},
                {"bheart9",TheOneWithTheHearts.mod.ui.heartSlots[9].Item},
                {"bheart10",TheOneWithTheHearts.mod.ui.heartSlots[10].Item},
                {"bheart11",TheOneWithTheHearts.mod.ui.heartSlots[11].Item},
                {"bheart12",TheOneWithTheHearts.mod.ui.heartSlots[12].Item},
                {"bheart13",TheOneWithTheHearts.mod.ui.heartSlots[13].Item},
                {"bheart14",TheOneWithTheHearts.mod.ui.heartSlots[14].Item},
                {"bheart15",TheOneWithTheHearts.mod.ui.heartSlots[15].Item},
                {"bheart16",TheOneWithTheHearts.mod.ui.heartSlots[16].Item},
                {"bheart17",TheOneWithTheHearts.mod.ui.heartSlots[17].Item},
                {"bheart18",TheOneWithTheHearts.mod.ui.heartSlots[18].Item},
                {"bheart19",TheOneWithTheHearts.mod.ui.heartSlots[19].Item}
            };
            TheOneWithTheHearts.mod.heartUI.SetState(null);
            return r;
        }
        public override void Load(TagCompound tag){
            hearts = new List<Item>(){};
            hearts.Add(tag.GetTag<Item>("heart0"));
            hearts.Add(tag.GetTag<Item>("heart1"));
            hearts.Add(tag.GetTag<Item>("heart2"));
            hearts.Add(tag.GetTag<Item>("heart3"));
            hearts.Add(tag.GetTag<Item>("heart4"));
            hearts.Add(tag.GetTag<Item>("heart5"));
            hearts.Add(tag.GetTag<Item>("heart6"));
            hearts.Add(tag.GetTag<Item>("heart7"));
            hearts.Add(tag.GetTag<Item>("heart8"));
            hearts.Add(tag.GetTag<Item>("heart9"));
            hearts.Add(tag.GetTag<Item>("heart10"));
            hearts.Add(tag.GetTag<Item>("heart11"));
            hearts.Add(tag.GetTag<Item>("heart12"));
            hearts.Add(tag.GetTag<Item>("heart13"));
            hearts.Add(tag.GetTag<Item>("heart14"));
            hearts.Add(tag.GetTag<Item>("heart15"));
            hearts.Add(tag.GetTag<Item>("heart16"));
            hearts.Add(tag.GetTag<Item>("heart17"));
            hearts.Add(tag.GetTag<Item>("heart18"));
            hearts.Add(tag.GetTag<Item>("heart19"));
            try
            {
            TheOneWithTheHearts.mod.ui.heartSlots[0].Item=tag.GetTag<Item>("bheart0");
            TheOneWithTheHearts.mod.ui.heartSlots[1].Item=tag.GetTag<Item>("bheart1");
            TheOneWithTheHearts.mod.ui.heartSlots[2].Item=tag.GetTag<Item>("bheart2");
            TheOneWithTheHearts.mod.ui.heartSlots[3].Item=tag.GetTag<Item>("bheart3");
            TheOneWithTheHearts.mod.ui.heartSlots[4].Item=tag.GetTag<Item>("bheart4");
            TheOneWithTheHearts.mod.ui.heartSlots[5].Item=tag.GetTag<Item>("bheart5");
            TheOneWithTheHearts.mod.ui.heartSlots[6].Item=tag.GetTag<Item>("bheart6");
            TheOneWithTheHearts.mod.ui.heartSlots[7].Item=tag.GetTag<Item>("bheart7");
            TheOneWithTheHearts.mod.ui.heartSlots[8].Item=tag.GetTag<Item>("bheart8");
            TheOneWithTheHearts.mod.ui.heartSlots[9].Item=tag.GetTag<Item>("bheart9");
            TheOneWithTheHearts.mod.ui.heartSlots[10].Item=tag.GetTag<Item>("bheart10");
            TheOneWithTheHearts.mod.ui.heartSlots[11].Item=tag.GetTag<Item>("bheart11");
            TheOneWithTheHearts.mod.ui.heartSlots[12].Item=tag.GetTag<Item>("bheart12");
            TheOneWithTheHearts.mod.ui.heartSlots[13].Item=tag.GetTag<Item>("bheart13");
            TheOneWithTheHearts.mod.ui.heartSlots[14].Item=tag.GetTag<Item>("bheart14");
            TheOneWithTheHearts.mod.ui.heartSlots[15].Item=tag.GetTag<Item>("bheart15");
            TheOneWithTheHearts.mod.ui.heartSlots[16].Item=tag.GetTag<Item>("bheart16");
            TheOneWithTheHearts.mod.ui.heartSlots[17].Item=tag.GetTag<Item>("bheart17");
            TheOneWithTheHearts.mod.ui.heartSlots[18].Item=tag.GetTag<Item>("bheart18");
            TheOneWithTheHearts.mod.ui.heartSlots[19].Item=tag.GetTag<Item>("bheart19");
            //for (int i = 0; i < TheOneWithTheHearts.mod.ui.heartSlots.Length; i++)if(TheOneWithTheHearts.mod.ui.heartSlots[i].Item.type==ModLoader.GetMod("ModLoader").ItemType("MysteryItem"))TheOneWithTheHearts.mod.ui.heartSlots[i].Item.TurnToAir();
            }
            catch (System.Exception){}
        }
    }
}