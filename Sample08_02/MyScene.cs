using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.Framework;
using Tutorial.Utility;

namespace Sample
{
	public class MyScene : GameActor
	{
		/*
		 * オブジェクト
		 * */
		private Player myPlayer;
		private MyScene changeScene;
		private Vector3 changeInPoint;
		private Vector3 changeOutPoint;
		public Player MyPlayer
		{
			get{ return this.myPlayer; }
			set{ this.myPlayer = value; }	// 出来るか不安
		}
		public MyScene ChangeScene
		{
			get{ return this.changeScene; }
			set{ this.changeScene = value; }	// 出来るか不安
		}
		public Vector3 ChangeInPoint
		{
			get{ return this.changeInPoint; }
			set{ this.changeInPoint = value; }	// 出来るか不安
		}
		public Vector3 ChangeOutPoint
		{
			get{ return this.changeOutPoint; }
			set{ this.changeOutPoint = value; }	// 出来るか不安
		}
		
		/*
		 * コンストラクタ
		 * */
		// これは使わないコンストラクタなので最終的には消します

		
		// こっちが使うコンストラクタ
		public MyScene(GameFrameworkSample gs, string name, Texture2D textrue, 
		               MyScene _changeScene, Vector3 _changeInPoint, Vector3 _changeOutPoint) : base(gs, name)
		{
			sprite = new SimpleSprite(gs.Graphics, textrue);
			
			sprite.Center.X = 0.0f;
			sprite.Center.Y = 0.0f;
			
			// 遷移シーンとその座標の設定
			this.ChangeScene = _changeScene;
			this.ChangeInPoint = _changeInPoint;
			this.ChangeOutPoint = _changeOutPoint;
			sprite.Position.X = this.ChangeInPoint.X;
			sprite.Position.Y = this.ChangeInPoint.Y;
			
			this.Initilize();
		}
		
		/*
		 * 初期化
		 * */
		public void Initilize()
		{

			sprite.Position.Z=0.1f;
			createMyPlayer();
			//---------------1/25追加　國本
			this.myPlayer.calMyScenePosition(this);
			sprite.Position=this.myPlayer.MyScenePosition;
			this.myPlayer.Sprite.Position=this.myPlayer.RelativePosition;
			//
		}
		
		
		/*
		 * 遷移先のシーンを生成する
		 * */
		// ログイン→広場→会議室って流れが決まっているので今は作らなくておｋ
		// 他の流れも考える場合は作る必要あり（クラス内の変数も変更する必要あり）
		public void createChangeScene(MyScene _changeScene, Vector3 _changePoint)
		{
			// この中を作成
			
		}
		
		/*
		 * キャラを生成
		 * */
		public void createMyPlayer(){
			// プレイヤーの初期化
			Texture2D texturePlayer = new Texture2D("/Application/resources/Player.png", false);
			this.myPlayer = new Player(gs, "scenePlayer", texturePlayer, this);
			this.AddChild (this.myPlayer);
		}

		/*
		 * キャラを削除
		 * */
		public void deleteMyPlayer(){
			this.MyPlayer = null;
		}
		
		/*
		 * 描画関数
		 * プレイヤーがいないなら描画しない
		 * プレイヤーがいるならプレイヤーのシーン座標を見て
		 * シーンの部分を表示させる
		 * */
		public override void Render()
		{
			// この中身を書く
			if(this.Name != "Login") this.myPlayer.Render();
			
			base.Render();
		}
	}
}
