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
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            if(!item.ranged)return true;
            if(item?.modItem?.mod?.Name?.Equals("RefTheGun")??false)return true;
            int multishot = player.GetModPlayer<HeartPlayer>().multishot;
            for (int i = multishot; i > 0; i--) {
                if (item?.modItem?.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack) ?? false) {
                    Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(0.1f), type, damage, knockBack, player.whoAmI);
                }
            }
            return true;
        }
    }
}