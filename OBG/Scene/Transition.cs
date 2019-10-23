using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OBG.Def;
using OBG.Device;
using OBG.Util;

namespace OBG.Scene
{
    class Transition : IScene
    {
        /// <summary>
        /// フェードシーン状態
        /// </summary>
        private enum IrisState
        { In, Out, None };

        private IrisState irisState; //現在の状態
        private Timer timer; //フェード時間

        //フェード切り替えの時間
        private readonly float FADE_TIME = 2.0f;
        private IScene scene; //現在のシーン
        private bool isEndFlag = false; //終了フラグ 
        private bool? ballDeadFlag;
        float a;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン名</param>
        public Transition(IScene scene, bool? ballDeadFlag)
        {
            this.scene = scene;
            this.ballDeadFlag = ballDeadFlag;
        }

        public void Draw(Renderer renderer)
        {
            switch (irisState)
            {
                case IrisState.In:
                    DrawFadeIn(renderer);
                    break;
                case IrisState.Out:
                    DrawFadeOut(renderer);
                    break;
                case IrisState.None:
                    DrawFadeNone(renderer);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            scene.Initialize();
            irisState = IrisState.In;
            timer = new CountDownTimer(FADE_TIME);
            isEndFlag = false;
        }

        /// <summary>
        /// 終了かを返す
        /// </summary>
        /// <returns>終了ならtrue</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーン名を取得
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            return scene.Next();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            scene.Shutdown();
        }

        public void Update(GameTime gameTime)
        {
            switch (irisState)
            {
                case IrisState.In:
                    FadeInUpdate(gameTime);
                    break;
                case IrisState.Out:
                    FadeOutUpdate(gameTime);
                    break;
                case IrisState.None:
                    FadeNoneUpdate(gameTime);
                    break;
                default:
                    break;
            }
        }

        #region フェードイン
        /// <summary>
        /// フェードイン状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void FadeInUpdate(GameTime gameTime)
        {
            //シーンの更新
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                irisState = IrisState.Out;
            }
            //時間の更新
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                irisState = IrisState.None;
            }
        }

        /// <summary>
        /// フェードイン状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeIn(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, 1 - timer.Rate());
        }
        #endregion

        #region フェードアウト
        /// <summary>
        /// フェードアウト状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void FadeOutUpdate(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                irisState = IrisState.Out;
            }
            timer.Update(gameTime);
            if (timer.IsTime())
            {
                isEndFlag = true;
            }
        }

        /// <summary>
        /// フェードアウト状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeOut(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, timer.Rate());
        }
        #endregion

        #region フェード無し
        /// <summary>
        /// フェードなし状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void FadeNoneUpdate(GameTime gameTime)
        {
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                irisState = IrisState.Out;
                timer.Initialize();
            }
        }

        /// <summary>
        /// フェードなし状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeNone(Renderer renderer)
        {
            scene.Draw(renderer);
        }
        #endregion

        private void DrawEffect(Renderer renderer, float alpha)
        {
            renderer.Begin();
            //renderer.DrawTexture(
            //    "irisT",
            //    new Vector2(Screen.Width / 2, Screen.Height / 2),
            //    null,
            //    null,
            //    new Vector2(2000, 2000),
            //    Color.White,
            //    0.0f,
            //    new Vector2(2f+(alpha-1)),
            //    SpriteEffects.None,
            //    0.0f,
            //    1);
            renderer.DrawCircle(
                new Vector2(Screen.Width / 2, Screen.Height / 2),
                1000 * alpha);
            renderer.End();
        }
    }
}
