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
using Tutorial.Utility;


namespace Sample
{
	public class EnemyBullet : GameActor
	{
		static int idNum=0;
		
		float speed=6;
		
		Vector3 direction;
		
		public EnemyBullet(GameFrameworkSample gs, string name, Texture2D textrue, Vector3 position, Vector2 direction) : base (gs, name)
		{
			Name = name + idNum.ToString();
			this.sprite = new SimpleSprite( gs.Graphics, textrue );
			this.sprite.Center.X = 0.5f;
			this.sprite.Center.Y = 0.5f;
			
			idNum++;
			
			this.direction.X=direction.X*speed;
			this.direction.Y=direction.Y*speed;
			this.direction.Z=0.0f;
			
			this.sprite.Position = position;
		}

		public override void Update()
		{
			sprite.Position += direction;
			
			//@j 画面から出たら、死亡させる。
			//@e Kill if it gets out of screen. 
			if (sprite.Position.X < 0 -sprite.Width ||
			    sprite.Position.Y < 0 -sprite.Height||
			    gs.rectScreen.Width + sprite.Width < sprite.Position.X ||
				gs.rectScreen.Height + sprite.Height < sprite.Position.Y  )
			{
				this.Status = Actor.ActorStatus.Dead;
			}
			
			base.Update();
		}
	}
}


