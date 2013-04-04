/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.Framework;
using Tutorial.Utility;

namespace Sample
{
	public class Player : GameActor
	{
	
		/*
		 * オブジェクト
		 * */
		//Int32 cnt=0;
		//int speed = 4;	// キャラが動くスピード
		
		// 各座標
		private Vector3 absolutePosition;	// 絶対キャラ座標
		private Vector3 relativePosition;	// 相対キャラ座標
		private Vector3 myScenePosition;	// シーン座標
		
		private int exe;	//プレイヤーの経験値
		
		public Vector3 AbsolutePosition 
		{
			get{ return this.absolutePosition; }
		}
		public Vector3 RelativePosition 
		{
			get{ return this.relativePosition; }
		}
		public Vector3 MyScenePosition 
		{
			get{ return this.myScenePosition; }
		}
		
		public int Exe
		{
			get{ return this.exe; }
		}
		
		private MyScene myScene;
	
		// ここから　使わない
		public enum PlayerStatus
		{
			Normal,		// 通常
			Explosion,	// 爆発
			Invincible,	// 透明
			GameOver,	// ゲームオーバー
		};
		
		public PlayerStatus playerStatus;
		// ここまで　使わない
		
		
		public Player(GameFrameworkSample gs, string name, Texture2D textrue, MyScene _myScene) : base(gs, name)
		{

			sprite = new SimpleSprite(gs.Graphics, textrue);
			
			sprite.Center.X = 0.0f;
			sprite.Center.Y = 0.0f;
			
			// シーンの設定
			this.myScene = _myScene;
			
			this.Init();

		}
		
		public void Init()
		{

			this.playerStatus = PlayerStatus.Normal;
			
			//sprite.Position.X=gs.rectScreen.Width/2 - sprite.Width/2;
			//sprite.Position.Y=gs.rectScreen.Height/2 - sprite.Height/2;
			sprite.Position.Z=0.1f;
			//cnt=0;
			/*
			absolutePosition.X = gs.rectScreen.Width/2 - sprite.Width/2;
			absolutePosition.Y = gs.rectScreen.Height/2 - sprite.Height/2;
			myScenePosition.X = 0;
			myScenePosition.Y = 0;
			
			relativePosition.X = gs.rectScreen.Width/2 - sprite.Width/2;
			relativePosition.Y = gs.rectScreen.Height/2 - sprite.Height/2;
			*/
			//absolutePosition = this.myScene.ChangeInPoint;
			setAbsPosition(this.myScene.ChangeInPoint,this.myScene);
			setMyScenePosition(absolutePosition,this.myScene);
			//calMyScenePosition(this.myScene);
			setRelatPosition(absolutePosition,myScenePosition);
			
			sprite.Position=relativePosition;
				
			//Console.WriteLine("my name is " + this.Name);
			exe=0;
		}
		
		/*
		 * 絶対キャラ座標の設定
		 * Vector3 inputPos	: 設定する座標（不変）
		 * MyScene myScene	: 範囲外かどうか確認するシーン(不変)
		 * */
		public void setAbsPosition(Vector3 inputPos, MyScene myScene)
		{
			// この中を作成
			
			if( (inputPos.X < 0) || ((myScene.Sprite.Width-(inputPos.X+Sprite.Width)) < 0 )){
				//キャラクタがマップ外なら座標更新を行わない
			}
			else absolutePosition.X = inputPos.X;
			
			if( (inputPos.Y < 0) || ((myScene.Sprite.Height-(inputPos.Y+Sprite.Height)) < 0 )){
				//キャラクタがマップ外なら座標更新を行わない
			}
			else absolutePosition.Y = inputPos.Y;
			
			//absolutePosition = inputPos;
		}
		
		/*
		 * シーン座標の設定
		 * Vector3 inputPos	: 設定する座標（不変）
		 * MyScene myScene	: 範囲外かどうか確認するシーン(不変)
		 * */
		public void setMyScenePosition(Vector3 inputPos, MyScene myScene)
		{
			// この中を作成
			//シーンがマップ外もしくはキャラクタがマップ端近辺なら座標更新を行わない
				//シーン座標がマップ外かどうか？
			if( (-inputPos.X < 0) || ((myScene.Sprite.Width-(-inputPos.X+gs.rectScreen.Width)) < 0 )
			   //キャラクタ絶対座標がマップ端近辺かどうか？
			   || (absolutePosition.X < (gs.rectScreen.Width/2-sprite.Width/2)) || (absolutePosition.X > (myScene.Sprite.Width-(gs.rectScreen.Width/2+sprite.Width/2)))){
				
			}
			else myScenePosition.X = inputPos.X;
			
			//シーンがマップ外もしくはキャラクタがマップ端近辺なら座標更新を行わない
				//シーン座標がマップ外かどうか？
			if( (-inputPos.Y < 0) || ((myScene.Sprite.Height-(-inputPos.Y+gs.rectScreen.Height)) < 0 )
			   //キャラクタ絶対座標がマップ端近辺かどうか？
			   || (absolutePosition.Y < (gs.rectScreen.Height/2-sprite.Height/2)) || (absolutePosition.Y > (myScene.Sprite.Height-(gs.rectScreen.Height/2+sprite.Height/2)))){
			
			}
			else myScenePosition.Y = inputPos.Y;
			
		}
		
		/*
		 * 相対キャラ座標の設定
		 * Vector3 absPos	: 設定に用いる絶対キャラ座標（不変）
		 * Vector3 scenePos	: 設定に用いるシーン座標(不変)
		 * */
		public void setRelatPosition(Vector3 absPos, Vector3 scenePos)
		{
			// この中を作成
			//絶対座標が、シーンが左端の時の画面中央座標より小さいとき、(相対座標=絶対座標)
			if(absPos.X <= gs.rectScreen.Width/2-sprite.Width/2)
					relativePosition.X = absPos.X;
			//絶対座標が、シーンが右端の時の画面中央座標より大きいとき、
			else if(absPos.X >= (myScene.Sprite.Width-(gs.rectScreen.Width/2+sprite.Width/2)))
			        relativePosition.X = absPos.X + gs.rectScreen.Width - myScene.Sprite.Width;
			else
					relativePosition.X = gs.rectScreen.Width/2 - sprite.Width/2;
			
			if(absPos.Y <= gs.rectScreen.Height/2-sprite.Height/2)
					relativePosition.Y = absPos.Y;
			else if(absPos.Y > (myScene.Sprite.Height-(gs.rectScreen.Height/2+sprite.Height/2)))
			        relativePosition.Y = absPos.Y + gs.rectScreen.Height - myScene.Sprite.Height;
			else
					relativePosition.Y = gs.rectScreen.Height/2 - sprite.Height/2;
				
		}
		
		/*
		 * 今の場所が遷移ポイントかどうか判定
		 * */
		public bool checkChangePoint()
		{
			// この中を作成
			if( (this.myScene.ChangeOutPoint.X == absolutePosition.X) && (this.myScene.ChangeOutPoint.Y == absolutePosition.Y))
				return true;
			else return false;
		}
		
		//2013_01_17追加　現在のプレイヤーの絶対座標とシーンから、シーン座標を計算し、セットする(画面遷移後に必要っぽい？)
		public void calMyScenePosition(MyScene myScene){
			if(absolutePosition.X < gs.rectScreen.Width/2) myScenePosition.X = 0;
			else if(absolutePosition.X > (myScene.Sprite.Width - gs.rectScreen.Width/2)) myScenePosition.X = -(myScene.Sprite.Width - gs.rectScreen.Width);
			else myScenePosition.X = -(absolutePosition.X - gs.rectScreen.Width/2);
			
			if(absolutePosition.Y < gs.rectScreen.Height/2) myScenePosition.Y = 0;
			else if(absolutePosition.Y > (myScene.Sprite.Height - gs.rectScreen.Height/2)) myScenePosition.Y = -(myScene.Sprite.Height - gs.rectScreen.Height);
			else myScenePosition.Y = -(absolutePosition.Y - gs.rectScreen.Height/2);
			
		}
		
		public void GetExe(int e){
			exe+=e;	
		}
	}
}

