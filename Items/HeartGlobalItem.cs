using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items {
    public class HeartGlobalItem : GlobalItem {
        public override bool OnPickup(Item item, Player player){
            if(item.type == ItemID.Heart){
                HeartPlayer heartPlayer = player.GetModPlayer<HeartPlayer>();
                if(heartPlayer.getCurrentHeart()>=0)if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.getCurrentHeart()].Item.IsAir){
                    if(((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.getCurrentHeart()].Item.modItem).life<((HeartItemBase)heartPlayer.hearts[heartPlayer.getCurrentHeart()].modItem).max){
                        ((HeartItemBase)heartPlayer.hearts[heartPlayer.getCurrentHeart()].modItem).AutoHeal(20);
                    }else if(heartPlayer.getCurrentHeart()<19){
                        if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.getCurrentHeart()+1].Item.IsAir){
                            ((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.getCurrentHeart()+1].Item.modItem).AutoHeal(20);
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
                if(heartPlayer.getCurrentHeart()>=0)if(!TheOneWithTheHearts.mod.ui.heartSlots[heartPlayer.getCurrentHeart()].Item.IsAir){
                    int h = item.healLife;
                    h = h-(player.statLifeMax2-player.statLife);
                    ((HeartItemBase)heartPlayer.hearts[heartPlayer.getCurrentHeart()].modItem).AutoHeal(item.healLife);
                }
            }
            return base.UseItem(item, player);
        }
    }
}