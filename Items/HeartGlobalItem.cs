using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items {
    public class HeartGlobalItem : GlobalItem {
        /*public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        public override bool OnPickup(Item item, Player player){
            if(item.type == ItemID.Heart){
                HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
                if(heartPlayer.GetCurrentHeart()>=0)if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()].Item.IsAir){
                    if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()].Item.modItem).life<((HeartItemBase)heartPlayer.hearts[heartPlayer.GetCurrentHeart()].modItem).maxLife){
                        ((HeartItemBase)heartPlayer.hearts[heartPlayer.GetCurrentHeart()].modItem).AutoHeal(20);
                    }else if(heartPlayer.GetCurrentHeart()<19){
                        if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()+1].Item.IsAir){
                            ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()+1].Item.modItem).AutoHeal(20);
                        }
                    }
                }
            }
            return base.OnPickup(item, player);
        }
        public override bool UseItem(Item item, Player player){
            //run BEFORE healing
            if(item.healLife>0){
                HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
                if(heartPlayer.GetCurrentHeart()>=0){
                    if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.GetCurrentHeart()].Item.IsAir){
                        int h = item.healLife;
                        h = h-(player.statLifeMax2-player.statLife);
                        if(h>0)((HeartItemBase)heartPlayer.hearts[heartPlayer.GetCurrentHeart()].modItem).Heal(h, display:true);
                    }
                }else if(item.healLife>(player.statLifeMax2-player.statLife))if(!TheOneWithTheHearts.mod.ui.heartSlots[0].Item.IsAir){
                    int h = item.healLife;
                    h = h-(player.statLifeMax2-player.statLife);
                    if(h>0)((HeartItemBase)heartPlayer.hearts[0].modItem).Heal(h, display:true);
                }
            }
            return base.UseItem(item, player);
        }*/
    }
}