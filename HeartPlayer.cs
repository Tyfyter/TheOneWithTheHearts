using Terraria;
using Terraria.ModLoader;
using TheOneWithTheHearts.UI;

namespace TheOneWithTheHearts {
    public class HeartPlayer : ModPlayer {
        public override void PreUpdate(){
            if(Main.playerInventory){
                if(TheOneWithTheHearts.mod.heartUI.CurrentState!=null)if(TheOneWithTheHearts.mod.heartUI.CurrentState.ToString().ToLower().Contains("heart"))return;
                TheOneWithTheHearts.mod.heartUI.SetState(new HeartUI());
                TheOneWithTheHearts.mod.heartUI.CurrentState.Height.Pixels=100;
                TheOneWithTheHearts.mod.heartUI.CurrentState.Width.Pixels=100;
                TheOneWithTheHearts.mod.heartUI.CurrentState.Top.Pixels=100;
                TheOneWithTheHearts.mod.heartUI.CurrentState.Left.Pixels=100;
                Main.NewText(TheOneWithTheHearts.mod.heartUI.CurrentState.Left.Pixels.ToString()+"");
            }
        }
    }
}