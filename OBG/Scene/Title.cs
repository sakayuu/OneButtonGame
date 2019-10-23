using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OBG.Device;

namespace OBG.Scene
{
    class Title : IScene //シーンインターフェイスを継承
    {
        //再生するビデオ
        Video video;
        //ビデオを再生するためのプレイヤ
        VideoPlayer vPlayer;
        //終了フラグ
        private bool isEndFlag;//終了フラグ
        private Sound sound;//サウンドオブジェクト

        private int ypos;
        private bool yflag;
        ///<summary>
        ///コンストラクタ
        /// </summary>
        public Title()
        {
             isEndFlag = false;
            var gamedevice = GameDevice.Instance();
            sound = gamedevice.GetSound();
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("back2", Vector2.Zero);
            //renderer.DrawTexture("Player2", new Vector2(250, 50+ypos/2), null, Color.White * 0.5f, 0.0f, new Vector2(1f, 1f),new Vector2(10.0f,10.0f));
            //renderer.DrawTexture("ウイルスバスター1", new Vector2(50,350+ypos/4), null, Color.White * 1.0f, -0.3f, Vector2.Zero, new Vector2(0.5f, 0.5f));
            //renderer.DrawTexture("プッシュ", new Vector2(150, 500 + ypos / 4), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.3f, 0.3f));
            renderer.DrawTexture("Player2", new Vector2(80, 50), null, Color.White * 0.1f, 0.0f, new Vector2(1f, 1f), new Vector2(12.0f, 12.0f));
            renderer.DrawTexture("ウイルスバスター1", new Vector2(100, 350 + ypos / 4), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.6f, 0.6f));
            renderer.DrawTexture("プッシュ", new Vector2(150, 500 + ypos / 4), null, Color.White * 1.0f, 0, Vector2.Zero, new Vector2(0.3f, 0.3f));
            //renderer.DrawVideo("PickKillerPV3_2", Vector2.Zero);
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;

        }

        /// <summary>
        /// 終了か？
        /// </summary>
        /// <returns>シーンが終わってたらtrue</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>次のシーン</returns>
        public Scene Next()
        {
            return Scene.GamePlay; //ゲームプレイまだ作ってない
            //return Scene.Ending;
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Shutdown()
        {
            sound.StopBGM();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            if (yflag)
                ypos++;
            if (!yflag)
                ypos--;
            if (ypos < 0)
                yflag = true;
            if (ypos > 30)
                yflag = false;
            sound.PlayBGM("titlebgm");

            //スペースキーが押されたか？
            if (Input.GetKeyRelease(Keys.Enter))
            {
                isEndFlag = true;
                sound.PlaySE("titlese");
            }
        }
    }
}
