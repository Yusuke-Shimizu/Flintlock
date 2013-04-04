/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Framework;


namespace Sample
{
	//@j あたり判定をおこなうクラス。
	//@e Class to use collision detection.
	public class CollisionCheck : GameActor
	{
		public CollisionCheck(GameFrameworkSample gs, string name) : base (gs, name){}

		public override void Update()
		{
			Player player =(Player)gs.Root.Search("Player");
			Actor bulletManager=gs.Root.Search("bulletManager");
			Actor enemyManager =gs.Root.Search("enemyManager");
			Actor bulletEnemyManager=gs.Root.Search("enemyBulletManager");
			Actor effectManager =gs.Root.Search("effectManager");
			
			//@j 弾と敵のあたり判定。
			//@e Collision detection for bullets and enemies
			foreach( Bullet bullet in  bulletManager.Children)
			{
				if(bullet.Status == Actor.ActorStatus.Action)
				{
					foreach( Enemy enemy in  enemyManager.Children)
					{
						if(enemy.Status ==  Actor.ActorStatus.Action &&  
						   Math.Abs(bullet.Sprite.Position.X -enemy.Sprite.Position.X ) < 30 && 
							Math.Abs(bullet.Sprite.Position.Y -enemy.Sprite.Position.Y ) < 30
								)
						{
							bullet.Status = Actor.ActorStatus.Dead;
							enemy.Status = Actor.ActorStatus.Dead;
							effectManager.AddChild(new Explosion(gs, "explosion", gs.textureExplosion, 
							    new Vector3(enemy.Sprite.Position.X, enemy.Sprite.Position.Y, 0.3f)));
							
							gs.Score += 100;
							gs.soundPlayerExplosion.Play();
						}
					}
				}
			}
			
			if(player.playerStatus== Player.PlayerStatus.Normal)
			{
				//@j 自機と敵のあたり判定。
				//@e Collision detection of player and enemy
				foreach( Enemy enemy in  enemyManager.Children)
				{
					float distanceX = (player.Sprite.Width + enemy.Sprite.Width) / 2;
					float distanceY = (player.Sprite.Height + enemy.Sprite.Height) / 2;
					
					// ぶつかる条件
					if(enemy.Status ==  Actor.ActorStatus.Action &&  
					   Math.Abs(player.Sprite.Position.X -enemy.Sprite.Position.X ) < distanceX * 0.7 && 
						Math.Abs(player.Sprite.Position.Y -enemy.Sprite.Position.Y ) < distanceY * 0.7
							)
					{
						
						//effectManager.AddChild(new Explosion(gs, "explosion", gs.textureExplosion, 
						//    new Vector3(player.Sprite.Position.X, player.Sprite.Position.Y, 0.3f)));
						
						//player.playerStatus = Player.PlayerStatus.Explosion;	
						
						//gs.soundPlayerExplosion.Play();
						
						//gs.NumShips--;
						enemy.Back();
					}
				}
				
				
				//@j 自機と敵弾のあたり判定。
				//@e Collision detection for player's and enemy's bullet
				foreach( EnemyBullet enemyBullet in  bulletEnemyManager.Children)
				{
					if(enemyBullet.Status ==  Actor.ActorStatus.Action &&  
					   Math.Abs(player.Sprite.Position.X -enemyBullet.Sprite.Position.X ) < 26 && 
						Math.Abs(player.Sprite.Position.Y -enemyBullet.Sprite.Position.Y ) < 26
							)
					{
						enemyBullet.Status = Actor.ActorStatus.Dead;
						effectManager.AddChild(new Explosion(gs, "explosion", gs.textureExplosion, 
						    new Vector3(player.Sprite.Position.X, player.Sprite.Position.Y, 0.3f)));
						
						player.playerStatus = Player.PlayerStatus.Explosion;	
						gs.soundPlayerExplosion.Play();
						gs.NumShips--;
					}
				}
			}
			
			base.Update();
		}
	}
}

