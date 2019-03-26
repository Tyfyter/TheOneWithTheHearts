using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
		public override bool PreNPCLoot(NPC npc){
			if (npc.type==NPCID.SkeletronPrime||npc.type==NPCID.Retinazer||npc.type==NPCID.Spazmatism||npc.type==NPCID.TheDestroyer||npc.type==NPCID.TheDestroyerBody||npc.type==NPCID.TheDestroyerTail){
                switch (npc.type){
                    case 125:
                    if(!NPC.downedMechBoss2)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),1);//twins
                    break;
                    case 126:
                    if(!NPC.downedMechBoss2)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),1);//twins
                    break;
                    case 127:
                    if(!NPC.downedMechBoss3)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),2);//skelly
                    break;
                    case 134:
                    if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),3);//destroyer
                    break;
                    case 135:
                    if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),3);//destroyer
                    break;
                    case 136:
                    if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), mod.ItemType<Mech_Heart>(),3);//destroyer
                    break;
                    default:
                    break;
                }
            }
            return true;
		}
    }
}