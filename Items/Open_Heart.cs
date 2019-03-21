using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneWithTheHearts.Items
{
	public class Open_Heart : HeartItemBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Open Heart");
			Tooltip.SetDefault("This is not a modded sword.");
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
			life = max = 80;
		}
		/*
		public override void ModifyTooltips(List<TooltipLine> tooltips){
			for (int i = 0; i < tooltips.Count; i++)if(tooltips[i].Name.ToLower().Contains("tooltip"))tooltips[i].text = life+"/"+max;
		}//*/
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowScale, 5);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TissueSample, 5);
			recipe.AddIngredient(ItemID.LifeCrystal, 2);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override void Heal(int health, int overflow = 2){
            int plife = life;
            life = (int)(overflow == 1?life+(health*1.25f):Math.Min(life+(health*1.25f),max));
            if(overflow >= 2&&(life-plife)/1.25f<health&&life-plife>0){
                if(!TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.IsAir)((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.modItem).Heal(health-(life-plife),3);
            }
            if(overflow == 2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, health);
            }else if (overflow<2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, life-plife);
            }
        }
		public override void WhileActive(Player player){}
		public override void WhileInactive(Player player){}
	}
}
