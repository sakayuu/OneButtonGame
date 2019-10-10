using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OBG.Device;

namespace OBG.Scene
{
    class Title : IScene //シーンインターフェイスを継承
    {
        //終了フラグ
        private bool isEndFlag;//終了フラグ
        private Sound sound;//サウンドオブジェクト

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
            renderer.DrawTexture("title", Vector2.Zero);
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
            //return Scene.GamePlay; //ゲームプレイまだ作ってない
            return Scene.Ending;
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
            sound.PlayBGM("titlebgm");

            //スペースキーが押されたか？
            if (Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
                sound.PlaySE("titlese");
            }
        }
    }
}
