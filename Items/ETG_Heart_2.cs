using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheOneWithTheHearts.Items
{
	public class ETG_Heart_2 : HeartItemBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Four-chambered Heart");
			Tooltip.SetDefault(". → ⁛");
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
			life = max = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType<ETG_Heart>(), 1);
			recipe.AddIngredient(mod.ItemType<Mech_Heart>(), 1);
			recipe.AddIngredient(ItemID.BeetleHusk, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.anyIronBar = true;
			recipe.AddRecipe();
		}
		public override bool CanRightClick(){
			if(Main.mouseItem==null){
				Item i = item.Clone();
				i.stack = 1;
				Main.mouseItem = i;
				Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
				return true;
			}
			if(Main.mouseItem.IsAir){
				Item i = item.Clone();
				i.stack = 1;
				Main.mouseItem = i;
				Main.PlaySound(SoundID.MenuTick, Main.LocalPlayer.Center);
				return true;
			}
			return false;
		}
		public override void Damage(int damage, bool crit = false, PlayerDeathReason reason = default(PlayerDeathReason)){
			life = Math.Max(life-(crit?3:1),0);
            PlayerDeathReason ignore = new PlayerDeathReason();
            ignore.SourceCustomReason = "this_shouldn't_be_able_to_kill";
            int h = Main.player[item.owner].statLife;
            Main.player[item.owner].Hurt(ignore, 0, 0, false, false, crit);
            Main.player[item.owner].statLife = h;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            if(index == -2)return true;
            item.alpha = 0;
			switch (life)
			{
				case 4:
            	if(mod.TextureExists("Items/"+this.GetType().Name))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name), position-new Vector2(3,3), null, drawColor, 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				case 3:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_3Q"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_3Q"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				case 2:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Half"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Half"), position-new Vector2(3,3), null, drawColor, 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				case 1:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_1Q"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_1Q"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
				default:
            	if(mod.TextureExists("Items/"+this.GetType().Name+"_Empty"))spriteBatch.Draw(mod.GetTexture("Items/"+this.GetType().Name+"_Empty"), position-new Vector2(3,3), null, new Color(drawColor.R,drawColor.G,drawColor.B), 0, new Vector2(), scale*1.5f, SpriteEffects.None, 0);
				break;
			}
            index = -2;
            return false;
        }
        public override void Heal(int health, int overflow = 2, bool display = false){
            int plife = life;
            life = (int)(overflow == 1?life+(health/15f):Math.Min(life+(health/15f),max));
            if(overflow >= 2&&(life-plife)*15<health&&life-plife>0){
                if(!TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.IsAir)((HeartItemBase)TheOneWithTheHearts.mod.ui.heartSlots[index+1].Item.modItem).Heal(health-(life-plife),3);
            }
			if(!display)return;
            if(overflow == 2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, health);
            }else if (overflow<2){
                CombatText.NewText(Main.player[item.owner].Hitbox, CombatText.HealLife, life-plife);
            }
        }
		public override Color? GetAlpha(Color lightColor){item.alpha = 0;return null;}
		public override void WhileActive(Player player){
			player.endurance+=10;
			player.GetModPlayer<HeartPlayer>().multishot+=3;
			if(ModLoader.GetMod("RefTheGun")!=null)player.GetModPlayer(ModLoader.GetMod("RefTheGun"),"GunPlayer").Load(new TagCompound(){{"Multishot",4f}});
		}
		public override void WhileInactive(Player player){}
	}
}
