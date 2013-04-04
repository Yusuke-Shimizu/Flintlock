/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Framework;
using Tutorial.Utility;


namespace Sample
{
	public class GameActor : Actor
	{
		protected GameFrameworkSample gs;
		protected SimpleSprite sprite;
		
		public SimpleSprite Sprite
		{
			get { return sprite;}	
		}
		
		public GameActor(GameFrameworkSample gs, string name) : base(name) 
		{	
			this.gs = gs;	
		}
		
		public override void Render ()
		{
			if(sprite!=null && this.Status == ActorStatus.Action)
				sprite.Render();
			
			base.Render ();
		}
	}
}
