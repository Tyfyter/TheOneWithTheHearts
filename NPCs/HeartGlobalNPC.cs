using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheOneWithTheHearts.Items;

namespace TheOneWithTheHearts.NPCs {
    public class HeartGlobalNPC : GlobalNPC {
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;
        bool init = true;
        public override void SetDefaults(NPC npc) {
            if (npc.aiStyle == 1 && npc.ai[1] == 58) {
                //Main.NewText("SetDefaults works for slimey heart");
                if(Main.rand.NextBool(3))npc.ai[1] = ModContent.ItemType<Slime_Heart>();
            }
        }
        public override void AI(NPC npc){
            if (init){
                if (npc.aiStyle == 1 && npc.ai[1] == 58) {
                    //Main.NewText("slimey heart check");
                    if(Main.rand.NextBool(3))npc.ai[1] = ModContent.ItemType<Slime_Heart>();
                }
                init = false;
            }
        }
		public override bool PreKill(NPC npc){
            /*switch (npc.type) {
                case NPCID.Retinazer:
                if(!NPC.downedMechBoss2)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 1);//twins
                break;
                case NPCID.Spazmatism:
                if(!NPC.downedMechBoss2)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 1);//twins
                break;
                case NPCID.SkeletronPrime:
                if(!NPC.downedMechBoss3)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 2);//skelly
                break;
                case NPCID.TheDestroyer:
                if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 3);//destroyer
                break;
                case NPCID.TheDestroyerBody:
                if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 3);//destroyer
                break;
                case NPCID.TheDestroyerTail:
                if(!NPC.downedMechBoss1)Item.NewItem(npc.Center, new Vector2(), ModContent.ItemType<Mech_Heart>(), 3);//destroyer
                break;
                default:
                break;
            }*/
            return true;
		}
    }
}