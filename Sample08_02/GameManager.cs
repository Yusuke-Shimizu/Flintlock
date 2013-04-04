#define KDEBUG
#undef KDEBUG
/* SCE CONFIDENTIAL
 * PlayStation(R)Suite SDK 0.98.2
 * Copyright (C) 2012 Sony Computer Entertainment Inc.
 * All Rights Reserved.
 */
/*
 * ～キャラ移動～
 * キャラの座標を変更
 * 変更後の座標によって以下を判定
 * 1.マップが動くか固定か（マップの端なら固定，それ以外なら動かす）
 * 描画領域とマップ座標の値を比較して判定
 * 2.画面遷移するかどうか
 * マップ固有の遷移ポイントにいるかどうかを判定
 * いた場合，
 * 
 * ～画面遷移～
 * ・ログインから広場
 * 
 * ・広場から会議室
 * 
 * 
 * 
 * */
using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.Framework;
using Tutorial.Utility;



namespace Sample
{
	public class GameManager : GameActor
	{
		/*
		 * オブジェクト
		 * */
		Int32 cnt=0;	
				
		// 現在のシーン
		MyScene currentScene;
		
		// 各シーンのインスタンス
		MyScene login;
		MyScene openSpace;
		MyScene conferenceRoom;
		
		//チャット画面用定義＠柏本
		SimpleSprite spritechbk;
		
		Vector3 abstmp,scenetmp;
		
		//チャットテキスト
		/*
		DebugString chat;
		Texture2D textureFont;
		int textWidth = 10;
		int textHeight = 20;
		*/
		static int fontsize = 22;
		static String[] text;
		static String[] draw_text;
		static int IndexEndText=0;
		static int IndexDrawStartText = 0;
		static int tmptext_cnt = 0;
		static int rown;
		static int intarvalcnt_tr = 0; //▲ボタン，スクロール速度を落とすためのタイマー用カウンタ
		static int intarvalcnt_ci = 0; //●ボタン，スクロール速度を落とすためのタイマー用カウンタ
		static int intarvalcnt_cr = 0; //☓ボタン，スクロール速度を落とすためのタイマー用カウンタ
		//
		
		//発言用ダイアログ
		private static SampleButton button;
    	private static TextInputDialog dialog;
		
		SimpleSprite spritetalk;
		
		int speed = 4;

		/*
		 * コンストラクタ
		 * */
		public GameManager(GameFrameworkSample gs, string name) : base (gs, name)
		{
			// 3つのシーンの初期化と現在のシーンの設定
			createScene(gs);
			initialMyScene(gs);
			//チャットテキスト
			ChatInit (gs);
			//発言ダイアログ設定
			InputInit();
		}

		public override void Update()
		{
			/*
			// 遷移の確認
			if((this.gs.PadData.Buttons & GamePadButtons.Left) != 0 && this.currentScene.Name == "Login")
			{
				this.nextMyScene();
			}else if((this.gs.PadData.Buttons & GamePadButtons.Up) != 0 )
			{
				this.nextMyScene();
			}
			// ここから　図の通り作る
			*/
			cnt++;
			//現在のシーン毎に処理分岐
			switch(this.currentScene.Name){
			//ログイン画面：ボタン押下したらジャンブ	
			case "Login":
				if((this.gs.PadData.Buttons/* & GamePadButtons.Start*/) != 0 ){
					//openSpace.MyPlayer.calMyScenePosition(openSpace);
					this.nextMyScene();
					}
				break;
			//広場画面：
			case "OpenSpace":
				if((gs.PadData.Buttons & GamePadButtons.Left) != 0){
					//openSpace.Sprite.Position.X +=4;
					//tmp.X = openSpace.Sprite.Position.X +4;
					abstmp.X = openSpace.MyPlayer.AbsolutePosition.X - speed;
					openSpace.MyPlayer.setAbsPosition(abstmp,openSpace);
					scenetmp.X = openSpace.MyPlayer.MyScenePosition.X + speed;
					openSpace.MyPlayer.setMyScenePosition(scenetmp,openSpace);
					openSpace.Sprite.Position.X = openSpace.MyPlayer.MyScenePosition.X;
					openSpace.MyPlayer.setRelatPosition(openSpace.MyPlayer.AbsolutePosition,openSpace.Sprite.Position);
					openSpace.MyPlayer.Sprite.Position.X = openSpace.MyPlayer.RelativePosition.X;
				}
				if((gs.PadData.Buttons & GamePadButtons.Right) != 0){
					//openSpace.Sprite.Position.X -=4;
					abstmp.X = openSpace.MyPlayer.AbsolutePosition.X + speed;
					openSpace.MyPlayer.setAbsPosition(abstmp,openSpace);
					scenetmp.X = openSpace.MyPlayer.MyScenePosition.X - speed;
					openSpace.MyPlayer.setMyScenePosition(scenetmp,openSpace);
					openSpace.Sprite.Position.X = openSpace.MyPlayer.MyScenePosition.X;
					openSpace.MyPlayer.setRelatPosition(openSpace.MyPlayer.AbsolutePosition,openSpace.Sprite.Position);
					openSpace.MyPlayer.Sprite.Position.X = openSpace.MyPlayer.RelativePosition.X;
				}
				if((gs.PadData.Buttons & GamePadButtons.Up) != 0){
					//openSpace.Sprite.Position.Y +=4;
					abstmp.Y = openSpace.MyPlayer.AbsolutePosition.Y - speed;
					openSpace.MyPlayer.setAbsPosition(abstmp,openSpace);
					scenetmp.Y = openSpace.MyPlayer.MyScenePosition.Y + speed;
					openSpace.MyPlayer.setMyScenePosition(scenetmp,openSpace);
					openSpace.Sprite.Position.Y = openSpace.MyPlayer.MyScenePosition.Y;
					openSpace.MyPlayer.setRelatPosition(openSpace.MyPlayer.AbsolutePosition,openSpace.Sprite.Position);
					openSpace.MyPlayer.Sprite.Position.Y = openSpace.MyPlayer.RelativePosition.Y;
				}
				if((gs.PadData.Buttons & GamePadButtons.Down) != 0){
					//openSpace.Sprite.Position.Y -=4;
					abstmp.Y = openSpace.MyPlayer.AbsolutePosition.Y + speed;
					openSpace.MyPlayer.setAbsPosition(abstmp,openSpace);
					scenetmp.Y = openSpace.MyPlayer.MyScenePosition.Y - speed;
					openSpace.MyPlayer.setMyScenePosition(scenetmp,openSpace);
					openSpace.Sprite.Position.Y = openSpace.MyPlayer.MyScenePosition.Y;
					openSpace.MyPlayer.setRelatPosition(openSpace.MyPlayer.AbsolutePosition,openSpace.Sprite.Position);
					openSpace.MyPlayer.Sprite.Position.Y = openSpace.MyPlayer.RelativePosition.Y;
				}
				//建物のドアに来たら会議室に飛ぶ
				if((openSpace.MyPlayer.AbsolutePosition.X > 1520 && openSpace.MyPlayer.AbsolutePosition.X < 1580) && (openSpace.MyPlayer.AbsolutePosition.Y > 300 && openSpace.MyPlayer.AbsolutePosition.Y < 340)){
					//if(openSpace.MyPlayer.checkChangePoint()) 
					this.nextMyScene();
				}
				break;
			//会議室画面：
			case "ConferenceRoom":
				/*
				if((gs.PadData.Buttons & GamePadButtons.Left) != 0){
					conferenceRoom.Sprite.Position.X +=4;
				}
				if((gs.PadData.Buttons & GamePadButtons.Right) != 0){
					conferenceRoom.Sprite.Position.X -=4;
				}
				if((gs.PadData.Buttons & GamePadButtons.Up) != 0){
					conferenceRoom.Sprite.Position.Y +=4;
				}
				if((gs.PadData.Buttons & GamePadButtons.Down) != 0){
					conferenceRoom.Sprite.Position.Y -=4;
				}				
				break;
				*/
				
				if((gs.PadData.Buttons & GamePadButtons.Left) != 0){
					//openSpace.Sprite.Position.X +=4;
					//tmp.X = openSpace.Sprite.Position.X +4;
					abstmp.X = conferenceRoom.MyPlayer.AbsolutePosition.X - speed;
					conferenceRoom.MyPlayer.setAbsPosition(abstmp,conferenceRoom);
					scenetmp.X = conferenceRoom.MyPlayer.MyScenePosition.X + speed;
					conferenceRoom.MyPlayer.setMyScenePosition(scenetmp,conferenceRoom);
					conferenceRoom.Sprite.Position.X = conferenceRoom.MyPlayer.MyScenePosition.X;
					conferenceRoom.MyPlayer.setRelatPosition(conferenceRoom.MyPlayer.AbsolutePosition,conferenceRoom.Sprite.Position);
					conferenceRoom.MyPlayer.Sprite.Position.X = conferenceRoom.MyPlayer.RelativePosition.X;
				}
				if((gs.PadData.Buttons & GamePadButtons.Right) != 0){
					//openSpace.Sprite.Position.X -=4;
					abstmp.X = conferenceRoom.MyPlayer.AbsolutePosition.X + speed;
					conferenceRoom.MyPlayer.setAbsPosition(abstmp,conferenceRoom);
					scenetmp.X = conferenceRoom.MyPlayer.MyScenePosition.X - speed;
					conferenceRoom.MyPlayer.setMyScenePosition(scenetmp,conferenceRoom);
					conferenceRoom.Sprite.Position.X = conferenceRoom.MyPlayer.MyScenePosition.X;
					conferenceRoom.MyPlayer.setRelatPosition(conferenceRoom.MyPlayer.AbsolutePosition,conferenceRoom.Sprite.Position);
					conferenceRoom.MyPlayer.Sprite.Position.X = conferenceRoom.MyPlayer.RelativePosition.X;
				}
				if((gs.PadData.Buttons & GamePadButtons.Up) != 0){
					//openSpace.Sprite.Position.Y +=4;
					abstmp.Y = conferenceRoom.MyPlayer.AbsolutePosition.Y - speed;
					conferenceRoom.MyPlayer.setAbsPosition(abstmp,conferenceRoom);
					scenetmp.Y = conferenceRoom.MyPlayer.MyScenePosition.Y + speed;
					conferenceRoom.MyPlayer.setMyScenePosition(scenetmp,conferenceRoom);
					conferenceRoom.Sprite.Position.Y = conferenceRoom.MyPlayer.MyScenePosition.Y;
					conferenceRoom.MyPlayer.setRelatPosition(conferenceRoom.MyPlayer.AbsolutePosition,conferenceRoom.Sprite.Position);
					conferenceRoom.MyPlayer.Sprite.Position.Y = conferenceRoom.MyPlayer.RelativePosition.Y;
				}
				if((gs.PadData.Buttons & GamePadButtons.Down) != 0){
					//openSpace.Sprite.Position.Y -=4;
					abstmp.Y = conferenceRoom.MyPlayer.AbsolutePosition.Y + speed;
					conferenceRoom.MyPlayer.setAbsPosition(abstmp,conferenceRoom);
					scenetmp.Y = conferenceRoom.MyPlayer.MyScenePosition.Y - speed;
					conferenceRoom.MyPlayer.setMyScenePosition(scenetmp,conferenceRoom);
					conferenceRoom.Sprite.Position.Y = conferenceRoom.MyPlayer.MyScenePosition.Y;
					conferenceRoom.MyPlayer.setRelatPosition(conferenceRoom.MyPlayer.AbsolutePosition,conferenceRoom.Sprite.Position);
					conferenceRoom.MyPlayer.Sprite.Position.Y = conferenceRoom.MyPlayer.RelativePosition.Y;
				}
				//神戸空港に来たら会議室に飛ぶ
				//if((openSpace.MyPlayer.AbsolutePosition.X > 820 && openSpace.MyPlayer.AbsolutePosition.X < 1100) && (openSpace.MyPlayer.AbsolutePosition.Y > 744 && openSpace.MyPlayer.AbsolutePosition.Y < 828))
				//if(openSpace.MyPlayer.checkChangePoint()) this.nextMyScene();				
				break;
				
			default:
				break;
			}
			
			// ここまで
#if KDEBUG	
			//柏本用デバッグ　現在のプレイヤーの位置表示
			if(currentScene.Name == "OpenSpace"){
			Console.WriteLine("Current Player Location X-axis:" + openSpace.Sprite.Position.X);
			Console.WriteLine("Current Player Location Y-axis:" + openSpace.Sprite.Position.Y);				
			}
			
			//國本用デバッグ　現在のプレイヤーの絶対位置表示
			
			if(currentScene.Name == "OpenSpace"){
			Console.WriteLine("X-abs:" + openSpace.MyPlayer.AbsolutePosition.X);
			Console.WriteLine("Y-abs:" + openSpace.MyPlayer.AbsolutePosition.Y);				
			}
			if(currentScene.Name == "OpenSpace"){
			Console.WriteLine("X-scene:" + openSpace.MyPlayer.MyScenePosition.X);
			Console.WriteLine("Y-scene:" + openSpace.MyPlayer.MyScenePosition.Y);				
			}
			if(currentScene.Name == "OpenSpace"){
			Console.WriteLine("X-player:" + openSpace.MyPlayer.RelativePosition.X);
			Console.WriteLine("Y-player:" + openSpace.MyPlayer.RelativePosition.Y);				
			}
			if(currentScene.Name == "ConferenceRoom"){
			Console.WriteLine("X-abs:" + conferenceRoom.MyPlayer.AbsolutePosition.X);
			Console.WriteLine("Y-abs:" + conferenceRoom.MyPlayer.AbsolutePosition.Y);				
			}
			if(currentScene.Name == "ConferenceRoom"){
			Console.WriteLine("X-scene:" + conferenceRoom.MyPlayer.MyScenePosition.X);
			Console.WriteLine("Y-scene:" + conferenceRoom.MyPlayer.MyScenePosition.Y);				
			}
			if(currentScene.Name == "ConferenceRoom"){
			Console.WriteLine("X-player:" + conferenceRoom.MyPlayer.RelativePosition.X);
			Console.WriteLine("Y-player:" + conferenceRoom.MyPlayer.RelativePosition.Y);				
			}
#endif			
			//ここから追加(國本)　テキスト表示
			//gs.debugString.WriteLine ("test");
			/*chat.Clear();
			chat.WriteLine("test");	
			if((this.gs.PadData.Buttons & GamePadButtons.Circle) != 0 ){
				//gs.debugString.AddFirstLine("test");
				chat.WriteLine("Puch circle");
				chat.WriteLine ("テスト");
				chat.AddFirstLine ("insert");
			}*/
			const int INTARVAL_TH = 6;	//スクロール、テキスト追加の速度
			//書き込み追加(ここをストーリーのテキストに置き換える)
			//if(((gs.PadData.Buttons & GamePadButtons.Triangle) != 0) && ++intarvalcnt_tr > INTARVAL_TH) {
			if( (cnt%300)==0 && (this.currentScene.Name != "Login")){
				if(tmptext_cnt < 42){
				IndexEndText++;
				//text[IndexEndText] = "追加テキスト"+IndexEndText;
				draw_text[IndexEndText] = text[tmptext_cnt];
				tmptext_cnt++;
				IndexDrawStartText = IndexEndText;
				}
				intarvalcnt_tr = 0;
			}
			//新しい書き込みに向かってスクロール
			if(((gs.PadData.Buttons & GamePadButtons.Cross) != 0) && ++intarvalcnt_cr > INTARVAL_TH) {
				if(IndexDrawStartText<IndexEndText)IndexDrawStartText++;
				intarvalcnt_cr = 0;
			}
			//古い書き込みに向かってスクロール
			if(((gs.PadData.Buttons & GamePadButtons.Circle) != 0) && ++intarvalcnt_ci > INTARVAL_TH) {
				if(IndexDrawStartText>=rown){
					IndexDrawStartText--;
				}
				intarvalcnt_ci = 0;
			}
			//Console.WriteLine("IndexEndText:" + IndexEndText);
			

			//ここまで(國本)
			dialog_update();	//2/11追加
			
			base.Update();
		}
		
		//2/11追加　input
		bool dialog_update(){			
			List<TouchData> touchDataList = Touch.GetData(0);
			if (button.TouchDown(touchDataList) ) {
            if (dialog == null) {
                dialog = new TextInputDialog();
                dialog.Text = button.Label;
                dialog.Open();
            }
            return true;
        }

        if (dialog != null) {
            if (dialog.State == CommonDialogState.Finished) {
                if (dialog.Result == CommonDialogResult.OK) {
                    //button.Label = dialog.Text;
					IndexEndText++;
					//text[IndexEndText] = "追加テキスト"+IndexEndText;
					draw_text[IndexEndText] = "自/" + dialog.Text;
					//IndexDrawStartText++;
					IndexDrawStartText = IndexEndText;
					openSpace.MyPlayer.GetExe(6);
                }
                dialog.Dispose();
                dialog = null;
            }
        }
			return true;
		}
		//2/11追加ここまで		
		
		public override void Render ()
		{
			// ここから
			//ログイン画面以外でチャット画面を描画する＠柏本
			if(this.currentScene != login){
				this.spritechbk.Render();
				//テキスト表示(國本)
				DrawExe ();
				DrawText();
				this.spritetalk.Render();
			}			
			//通常のシーン描画
			this.currentScene.Render();
			//this.currentScene.MyPlayer.Render();
			// ここまで　図の通り作る
			
			//inputbutton　2/11追加
			if(this.currentScene != login)button.Draw();
			
			base.Render ();
		}
		
		/*
		 * ログイン，広場，会議室のシーンを生成
		 * */
		void createScene(GameFrameworkSample gs)
		{
			Texture2D conferenceRoomScene = new Texture2D("/Application/resources/conferenceRoom.png", false);
			conferenceRoom = new MyScene(gs, "ConferenceRoom", conferenceRoomScene, 
			                             openSpace, new Vector3(40, 250, 1), new Vector3(12, 12, 1));
			Texture2D openSpaceScene = new Texture2D("/Application/resources/map.png", false);
			openSpace = new MyScene(gs, "OpenSpace", openSpaceScene, 
			                        //conferenceRoom, new Vector3(1000, 840, 1), new Vector3(1560, 320, 1));
									conferenceRoom, new Vector3(20, 20, 1), new Vector3(1560, 320, 1));
			Texture2D loginScene = new Texture2D("/Application/resources/Login.png", false);
			login = new MyScene(gs, "Login", loginScene, 
			                    openSpace, new Vector3(0, 0, 1), new Vector3(12, 12, 1));
		}
		
		/*
		 * 現在のシーンの初期化
		 * シーンをログインに設定
		 * */
		void initialMyScene(GameFrameworkSample _gs)
		{
			this.currentScene = login;
			Console.WriteLine("current scene is " + this.currentScene.Name);
			
			//チャット画面用データロード@柏本
			Texture2D chbk = new Texture2D("/Application/resources/chatbk.png", false);
			spritechbk = new SimpleSprite(gs.Graphics, chbk);
        	spritechbk.Position.X = 0;
        	spritechbk.Position.Y = gs.rectScreen.Height-spritechbk.Height;
        	spritechbk.Position.Z = 0.0f;	
		}
		
		//ここから追加　チャットテキスト関係(國本)
		public static void ChatInit(GameFrameworkSample g)
    	{
			GraphicsContext graphics = g.Graphics;
        	SampleDraw.Init(graphics);
			//Font font = new Font(FontAlias.System, fontsize, FontStyle.Regular);
			Font font = new Font("/Application/resources/azukiL.ttf",fontsize,FontStyle.Regular);
			SampleDraw.SetFont(font);
			text = new string[64];
			draw_text = new string[128];
			/*
			text[0] = "test0";
			text[1] = "test1";
			text[2] = "test2";
			text[3] = "test3";	
			text[4] = "test4";
			text[5] = "test5";
			text[6] = "test6";
			text[7] = "test7";
			text[8] = "test8";
			text[9] = "test9";
			*/
			text[0] = "A/そういえば今度小学校の子達がフィールドワークで上山にくることになったんですが、";
			text[1] = "A/子供に出すお菓子はどれくらいいるかな";
			text[2] = "B/人数にもよるけど、子供は結構たべるんじゃないかな";
			text[3] = "C/フィールドワークって何をするの？";
			text[4] = "A/来る人数は●人で、（　　　）と（　　　）をする予定です";
			text[5] = "C/それじゃあ冷たい飲み物も用意しとかないとね～。出すお菓子とかはきまってます？";
			text[6] = "A/まだです。用意する量によってあとで考えようかなって思ってたんだけど…だめかな？";
			text[7] = "C/量があるものだとポテチとかだろうけど…べたべたしたものだと、後で困りそうかな？";
			text[8] = "D/じゃあクッキーとか？";
			text[9] = "E/ポッキーとかもよさそうかも";
			text[10] = "F/でもポッキーだけじゃさみしくありません？？";
			text[11] = "B/何種類かは用意したほうが子供喜ぶんじゃないかなｧ";
			text[12] = "C/だねー。何種類か2、3袋ずつ買えば十分じゃない？";
			text[13] = "E/あ、エリーゼとかブルボンのお菓子とかもどう？";
			text[14] = "D/あー定番。袋入りだから手もよごれないね。関係ないけど、ブルボンのお菓子、";
			text[15] = "D/おばあちゃんに凄くおすすめされたこと思い出したわ…";
			text[16] = "C/バームロールとかあったね＾＾";
			text[17] = "F/すずかすてらとか、カントリーマームとかもよくすすめられましたね～";
			text[18] = "D/ココナッツサブレとかもあるね。うーん。でも甘いのばっかだな";
			text[19] = "F/チョコ系、クッキー系、あとは…塩辛ものもちょっとほしい？";
			text[20] = "E/ティッシュ用意すればポテチとかでも問題ないと思うけど";
			text[21] = "A/おお・・・かなり出ましたね。助かります";
			text[22] = "E/チョコと言えば何があるかな";
			text[23] = "C/チョコで美味しいお菓子…チョコパイとか…あと有名どころならきのこ山とか";
			text[24] = "D/あーたけのこときのこは美味しいよね～子供の頃好きだったわー";
			text[25] = "F/やっぱりたけのこがおいしいですよね";
			text[26] = "C/きのこたけのこなら、やっぱきのこだよねー";
			text[27] = "D/おや";
			text[28] = "F/えー。僕はたけのこのほうがおいしいと思いますよ";
			text[29] = "C/ええっ！きのこの方がおいしいって！みなさんはどっちですー？";
			text[30] = "D/うーん。俺はたけのこかなー";
			text[31] = "B/私もたけのこですねー";
			text[32] = "A/私はきのこをよくたべてたので…きのこかなぁ";
			text[33] = "E/私は…うーんどっちだろう。でもどちらかといえばたけのこかな…";
			text[34] = "F/ほら～たけのこの方が美味しいですって！";
			text[35] = "C/えー！たまたまですよ！きのこのほうがおいしい！チョコとビスケット生地別々に堪能できるし";
			text[36] = "A/かたちもかわいらしいですもんね";
			text[37] = "F/いやいや、味ならたけのこですよー！";
			text[38] = "F/サクサク感とチョコの味が合わさって美味しいのはたけのこだとおもいますよ！";
			text[39] = "C/やっぱきのこだってば！！！";
			text[40] = "F/たけのこですってば！！！";
			text[41] = "E/どっちもおいしいですけどもねぇ…";
			
			//IndexEndText = 38;
			IndexEndText = -1;
			//rown = (int)(spritechbk.Height/fontsize);	//エラーになる…
			rown = (int) (128+48)/fontsize;
			IndexDrawStartText = IndexEndText;
    	}
	    
		void InputInit(){
			/*
			int rectW = SampleDraw.Width / 2;
	        int rectH = 32;
    	    int rectX = (SampleDraw.Width - rectW) / 2;
        	int rectY = (SampleDraw.Height - 24) / 2;
			*/
			
			int rectW = 48;
	        int rectH = 32;
    	    int rectX = 0;
        	float rectY = gs.rectScreen.Height-spritechbk.Height-rectH;
			
	        button = new SampleButton(rectX, (int)rectY, rectW, rectH);
			button.ButtonColor = 0x00ffffff;
			button.TextColor = 0xffff0000;
    	    button.SetText("発言", SampleButton.TextAlign.Left, SampleButton.VerticalAlign.Middle);
			//button.Label="Input";
	        dialog = null;
			Texture2D talk = new Texture2D("/Application/resources/talk.png", false);
			spritetalk = new SimpleSprite(gs.Graphics, talk);
        	spritetalk.Position.X = 0;
        	spritetalk.Position.Y = rectY;
        	spritetalk.Position.Z = 0.0f;
			
		}
		
		public void DrawText(){
			int j=0;
			for(int i=IndexDrawStartText; j<rown; i--){
				if(i>=0) SampleDraw.DrawText("(" + j + ")" + draw_text[i] , 0xffffffff,-28,gs.rectScreen.Height-(int)spritechbk.Height +16 + j*fontsize);
					//何故か正常に表示されない	
					//SampleDraw.DrawText(text[i] , 0xffffff00,0,gs.rectScreen.Height-128 + j*fontsize);
				j++;
			}
		}
		
		public void DrawExe(){
			SampleDraw.DrawText ( "LEVEL:" + openSpace.MyPlayer.Exe/10,0xffffffff,0,0);
			SampleDraw.DrawText ( "EXE  :" + openSpace.MyPlayer.Exe%10 + "/10",0xffffffff,0,fontsize);
		}
		
		/// Terminate
	    /*public static void Term()
    	{
	        SampleDraw.Term();
    	    graphics.Dispose();
   		}
   		*/
		//ここまで
		
		/*
		 * 現在のシーンをsceneに変更する
		 * */
		public void changeMyScene(MyScene scene)
		{
			this.currentScene = scene;
			Console.WriteLine("current scene is " + this.currentScene.Name);
		}
		
		/*
		 * 次のシーンをsceneに変更する
		 * */
		public void nextMyScene()
		{
			if(this.currentScene.ChangeScene != null)
			{
				this.currentScene = this.currentScene.ChangeScene;				
			}
			Console.WriteLine("current scene is " + this.currentScene.Name);
		}
		
	}
}

