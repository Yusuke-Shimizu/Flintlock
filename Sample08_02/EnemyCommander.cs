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
	public class EnemyCommander : GameActor
	{
		public EnemyCommander(GameFrameworkSample gs, string name) : base (gs, name)	{}

		public override void Update()
		{
			//@j 敵を出現させる。
			//@e Make enemies appear. 
			if( gs.Step== GameFrameworkSample.StepType.GamePlay && gs.GameCounter >= 60 && gs.appCounter % 30 == 0) 
			{
				Vector3 position;
				
				position.X = (int)(gs.rectScreen.Width * gs.rand.NextDouble());
				position.Y = 0.0f;
				position.Z = 0.2f;
				
				gs.Root.Search("enemyManager").AddChild(new Enemy(gs, "enemy", gs.textureEnemy, position));
			}
			
			base.Update();
		}
	}
}

