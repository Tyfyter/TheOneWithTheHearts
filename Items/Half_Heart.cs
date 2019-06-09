using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Half_Heart : HeartItemBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Half Heart");
			Tooltip.SetDefault("It doesn't care.");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LifeCrystal);
			item.consumable = false;
			item.useStyle = 0;
			item.maxStack = 1;
			item.scale = 1/1.5f;
			item.height = (int)(44*item.scale);
			item.width = (int)(44*item.scale);
			life = max = 50;
			regentime = regentimemax = 60;
		}
		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.anyIronBar = true;
			recipe.AddRecipe();
		}
		public override void Damage(int damage, bool crit = false, PlayerDeathReason reason = default(PlayerDeathReason)){
			Player player = Main.player[item.owner];
			if(reason.SourceCustomReason!=null){
				reason.SourceCustomReason+=HeartPlayer.ignore.SourceCustomReason;
			}else{
				reason.SourceCustomReason = HeartPlayer.ignore.SourceCustomReason;
			}
			int oldlife = life;
			base.Damage((int)((damage-(player.statDefense*(Main.expertMode?0.75:0.5)))/2), crit, reason);
			player.Hurt(reason, damage-(oldlife-life), 0);
		}
		public override bool CanDoRegen(Player player){
			return regentime>=regentimemax;
		}
		public override bool CanDoRegenTime(bool active){
			return true;
		}
	}
}
