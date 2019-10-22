// このファイルで必要なライブラリのnamespaceを指定
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using OBG.Device;
using OBG.Scene;
using OBG.Actor;
using OBG.Def;

/// <summary>
/// プロジェクト名がnamespaceとなります
/// </summary>
namespace OBG
{
    /// <summary>
    /// ゲームの基盤となるメインのクラス
    /// 親クラスはXNA.FrameworkのGameクラス
    /// </summary>
    public class Game1 : Game
    {
        // フィールド（このクラスの情報を記述）
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィックスデバイスを管理するオブジェクト
        private GameDevice gameDevice;
        private Renderer renderer;
        private SceneManager sceneManager;

        private VertexBuffer lineListVertexBuffer = null;


        Ball ball;
        Pin pin;

        bool flag = false;

        /// <summary>
        /// コンストラクタ
        /// （new で実体生成された際、一番最初に一回呼び出される）
        /// </summary>
        public Game1()
        {
            //グラフィックスデバイス管理者の実体生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            //コンテンツデータ（リソースデータ）のルートフォルダは"Contentに設定
            Content.RootDirectory = "Content";

            //スクリーンサイズ
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.Width;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.Height;

            Window.Title = "ワン・ボタン・ゲーム";
        }

        /// <summary>
        /// 初期化処理（起動時、コンストラクタの後に1度だけ呼ばれる）
        /// </summary>
        protected override void Initialize()
        {
            // この下にロジックを記述
            gameDevice = GameDevice.Instance(Content, GraphicsDevice);

            sceneManager = new SceneManager();
            IScene addScene = new GamePlay();
            sceneManager.Add(Scene.Scene.Title, new Title());
            sceneManager.Add(Scene.Scene.GamePlay, addScene);
            sceneManager.Add(Scene.Scene.Ending, new Ending(addScene));
            sceneManager.Change(Scene.Scene.Title);

            //sceneManager.Add(Scene.Scene.Ending, addScene);

            // この上にロジックを記述
            base.Initialize();// 親クラスの初期化処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// コンテンツデータ（リソースデータ）の読み込み処理
        /// （起動時、１度だけ呼ばれる）
        /// </summary>
        protected override void LoadContent()
        {
            // ゲームデバイスから持ってきて格納
            renderer = gameDevice.GetRenderer();
            Sound sound = gameDevice.GetSound();

            // この下にロジックを記述
            string filepathT = "./Texture/"; //画像フォルダのパス
            string filepathS = "./SoundSE/"; //サウンドフォルダのパス

            renderer.LoadContent("black", filepathT);
            renderer.LoadContent("pin", filepathT);
            renderer.LoadContent("stage", filepathT);
            renderer.LoadContent("white", filepathT);
            renderer.LoadContent("title", filepathT);
            renderer.LoadContent("ending", filepathT);
            renderer.LoadContent("particle", filepathT);
            renderer.LoadContent("kiiro", filepathT);
            renderer.LoadContent("back", filepathT);
            renderer.LoadContent("back2", filepathT);
            renderer.LoadContent("Player1", filepathT);
            renderer.LoadContent("Player2", filepathT);
            renderer.LoadContent("Player4", filepathT);
            renderer.LoadContent("pinmusic2", filepathT);
            renderer.LoadContent("pinmovie2", filepathT);
            Texture2D col = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colors = new Color[1 * 1];
            colors[0] = Color.Gold;
            col.SetData(colors);
            renderer.LoadContent("col", col);



            sound.LoadBGM("titlebgm", filepathS);
            sound.LoadBGM("gameplaybgm", filepathS);
            sound.LoadBGM("endingbgm", filepathS);
            sound.LoadBGM("congratulation", filepathS);

            sound.LoadSE("titlese", filepathS);
            sound.LoadSE("gameplayse", filepathS);
            sound.LoadSE("endingse", filepathS);

            // この上にロジックを記述
        }

        /// <summary>
        /// コンテンツの解放処理
        /// （コンテンツ管理者以外で読み込んだコンテンツデータを解放）
        /// </summary>
        protected override void UnloadContent()
        {
            // この下にロジックを記述


            // この上にロジックを記述
        }

        /// <summary>
        /// 更新処理
        /// （1/60秒の１フレーム分の更新内容を記述。音再生はここで行う）
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲーム終了処理（ゲームパッドのBackボタンかキーボードのエスケープボタンが押されたら終了）
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                 (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }

            // この下に更新ロジックを記述
            gameDevice.Update(gameTime);

            sceneManager.Update(gameTime);


            //if (Input.GetKeyTrigger(Keys.P))
            //{
            //    ball.moveFlag = true;
            //    ball.GetMoveDirection(pin.GetPinPosition());
            //}
            //if (CheckDistance(ball.GetBallPos(), pin.GetPinPosition()) < 20)
            //{
            //    ball.moveFlag = false;
            //    flag = true;
            //}

            //if (flag)
            //    ball.SetBallPos(pin.GetCatchPos());

            // この上にロジックを記述
            base.Update(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Draw(GameTime gameTime)
        {
            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // この下に描画ロジックを記述
            sceneManager.Draw(renderer);

            //renderer.DrawTexture("black", ball.GetBallPos(), Color.White);
            //renderer.DrawTexture("pin", pin.GetPinPosition(), Color.White);
            


            //この上にロジックを記述
            base.Draw(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }


    }
}
