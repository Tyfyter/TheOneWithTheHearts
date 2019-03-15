using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TheOneWithTheHearts
{
	class TheOneWithTheHearts : Mod
	{
        public static TheOneWithTheHearts mod;
		public UserInterface heartUI;
		public override void Load(){
            mod = this;
			if (!Main.dedServ){
				heartUI = new UserInterface();
			}
		}
		public TheOneWithTheHearts()
		{

		}
	}
}
