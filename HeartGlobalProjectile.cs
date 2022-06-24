using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheOneWithTheHearts {
	public class HeartGlobalProjectile : GlobalProjectile {
		private delegate void OnSpawnDelegate(Projectile projectile, IEntitySource source);
		private static OnSpawnDelegate projectileLoaderOnSpawn;
		private static bool fromMultishot;
		public override void Load() {
			typeof(ProjectileLoader).GetMethod("OnSpawn", BindingFlags.NonPublic | BindingFlags.Static).CreateDelegate(typeof(OnSpawnDelegate));
		}
		public override void Unload() {
			projectileLoaderOnSpawn = null;
		}
		public override void OnSpawn(Projectile projectile, IEntitySource source) {
			if (!fromMultishot) {
				if (source is MultishotSource multishotSource) {
					fromMultishot = true;
					if(projectileLoaderOnSpawn is not null) projectileLoaderOnSpawn(projectile, multishotSource.baseSource);
					fromMultishot = false;
					return;
				}
				if (source is EntitySource_Parent parentSource && parentSource.Entity is Player player) {
					for (int i = Main.rand.RandomRound(player.GetModPlayer<HeartPlayer>().multishot); i-- > 0;) {
						Projectile.NewProjectile(
							new MultishotSource(source),
							projectile.Center,
							projectile.velocity.RotatedByRandom(0.1f),
							projectile.type,
							projectile.damage,
							projectile.knockBack,
							projectile.owner,
							projectile.ai[0],
							projectile.ai[1]
						);
					}
				}
			}
		}
	}
	public struct MultishotSource : IEntitySource {
		public string Context => "";
		public readonly IEntitySource baseSource;
		public MultishotSource(IEntitySource baseSource) {
			this.baseSource = baseSource;
		}
	}
}
