#define KDEBUG
//#undef DEBUG
#undef KDEBUG
/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.Framework;
using Tutorial.Utility;


namespace Sample
{
	public class GameFrameworkSample: GameFramework
	{
		public ImageRect rectScreen;
		public Random rand = new Random(123);
		
		public Int32 appCounter=0;
		
		public Actor Root{ get; set;}
		
		
		public Texture2D texturePlayer;
#if KDEBUG
		public Texture2D kplayer;
#endif
		
		public Int32 GameCounter=0;
		public Int32 Score=0;
		public Int32 NumShips;
		
		public DebugString debugStringScore, debugStringShip;
		
		public override void Initialize()
		{
			base.Initialize();
			rectScreen = graphics.GetViewport();
			/*
			debugStringScore=new DebugString(graphics, textureFont, 10,20);
			debugStringScore.SetColor(new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
			
			debugStringShip=new DebugString(graphics, textureFont, 10,20);
			debugStringShip.SetColor(new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
			*/
			//@j アクターツリーの初期化。
			//@e Initialization of actor tree  
			Root = new Actor("root");
			
			//@j ゲームマネージャーの初期化。
			//@e Initialization of game manager 
			Actor gameManager = new GameManager(this, "gameManager");
			Root.AddChild(gameManager);

#if KDEBUG
			Texture2D kplayer = new Texture2D("/Application/resources/Player.png", false);
#endif

		}
		
		
		public override void Update()
		{
			base.Update();
			
			Root.Update();
			
			Root.CleanUpDeadActor();
			
			++appCounter;
		}
		
		
		public override void Render()
		{
			graphics.Clear();
			
			graphics.Enable(EnableMode.DepthTest);
			
			Root.Render();
			
			base.Render();
		}
		
		public override void Terminate()
		{				
			base.Terminate ();
		}
	}
}
