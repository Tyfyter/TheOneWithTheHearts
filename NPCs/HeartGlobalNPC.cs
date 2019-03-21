using Terraria;
using Terraria.ModLoader;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts.NPCs {
    public class HeartGlobalNPC : GlobalNPC {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
        bool init = true;
        public override void AI(NPC npc){
            if (init){
                if(npc.aiStyle==1&&npc.ai[1]==58)if(Main.rand.Next(10)==0)npc.ai[1] = mod.ItemType<Slime_Heart>();
                init = false;
            }
        }
    }
}