using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;//Assert用
using C3.XNA;
using Microsoft.Xna.Framework.Media;

namespace OBG.Device
{
    class Renderer
    {
        private ContentManager contentManager; //コンテンツ管理者
        private GraphicsDevice graphicsDevice; //グラフィック機器
        private SpriteBatch spriteBatch; //スプライト一括描画用オブジェクト

        //複数画像管理用変数の宣言と生成
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        //private Dictionary<string, Video> videos = new Dictionary<string, Video>();
        ////ビデオを再生するためのプレイヤ
        //VideoPlayer vPlayer;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1クラスのコンテンツ管理者</param>
        /// <param name="graphics">Game1クラスのグラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;

            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="assetName">アセット名（ファイルの名前）</param>
        /// <param name="filepath">画像へのファイルパス</param>
        public void LoadContent(string assetName, string filepath = "./")
        {
            //すでにキー（assetName：アセット名）が登録されているとき
            if (textures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
                Console.WriteLine(assetName + "はすでに読み込まれています。\n プログラムを確認してください。");
#endif

                //それ以上読み込まないのでここで終了
                return;
            }
            //画像の読み込みとDictionaryへアセット名と画像を登録
            textures.Add(assetName, contentManager.Load<Texture2D>(filepath + assetName));
        }

//        public void LoadVideo(string assetName, string filepath = "./")
//        {
//            //すでにキー（assetName：アセット名）が登録されているとき
//            if (videos.ContainsKey(assetName))
//            {
//#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
//                Console.WriteLine(assetName + "はすでに読み込まれています。\n プログラムを確認してください。");
//#endif

//                //それ以上読み込まないのでここで終了
//                return;
//            }
//            //画像の読み込みとDictionaryへアセット名と画像を登録
//            videos.Add(assetName, contentManager.Load<Video>(filepath + assetName));
//        }

        /// <summary>
        /// アンロード
        /// </summary>
        public void Unload()
        {
            textures.Clear();//Dictionaryの情報をクリア
        }

        /// <summary>
        /// 描画開始
        /// </summary>
        public void Begin()
        {
            spriteBatch.Begin();
        }

        /// <summary>
        /// 描画終了
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// 画像の描画（画像サイズはそのまま）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="alpha">透明値（1.0f：不透明 0.0f：透明）</param>
        public void DrawTexture(string assetName, Vector2 position, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            spriteBatch.Draw(textures[assetName], position, Color.White * alpha);
        }
        //public void DrawVideo(string assetName, Vector2 position, float alpha = 1.0f)
        //{
        //    //デバッグモードの時のみ、画像描画前のアセット名チェック
        //    Debug.Assert(
        //        videos.ContainsKey(assetName),
        //        "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");
        //    //プレイヤを作って
        //    vPlayer = new VideoPlayer();
        //    //ループを設定する
        //    vPlayer.IsLooped = true;
        //    //ビデオを再生
        //    vPlayer.Play(videos[assetName]);
        //}
        /// <summary>
        /// 画像の描画（画像を指定範囲内だけ描画）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">指定範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string assetName, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、画像の読み込み自体できていません");

            spriteBatch.Draw(
                textures[assetName], //テクスチャ
                position,            //位置
                rect,                //指定範囲（矩形で指定：左上の座標、幅、高さ）
                Color.White * alpha);//透明値
        }

        /// <summary>
        /// 数字の描画(整数のみ)
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="position"></param>
        /// <param name="number"></param>
        /// <param name="alpha"></param>
        public void DrawNumber(string assetName, Vector2 position, int number, float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体できていません");

            //マイナスの数は0
            if (number < 0)
            {
                number = 0;
            }

            int width = 32;//画面横幅

            //数字を文字列化し、1文字ずつ取り出す
            foreach (var n in number.ToString())
            {
                //数字のテクスチャが数時1つにつき幅32高さ64
                //文字と文字を引き算し、整数値を取得している
                spriteBatch.Draw(
                    textures[assetName],
                    position,
                    new Rectangle((n - '0') * width, 0, width, 64),
                    Color.White * alpha);

                //1文字描画したら1桁分右にずらす
                position.X += width;
            }
        }

        ///<summary>
        ///数字の描画(
        ///</summary>
        ///<param name="assetName">数字画像の名前</param>
        ///<param name="position">位置</param>
        ///<param name="number">表示したい実数値</param>
        ///<param name="alpha">透明値</param>
        public void DrawNumber(
            string assetName,
            Vector2 position,
            float number,
            float alpha = 1.0f)
        {
            Debug.Assert(textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体できていません");
            //マイナスは0へ
            if (number < 0.0f)
            {
                number = 0.0f;
            }

            int width = 32;//数字画像1つ分の横幅
            //少数部は2桁まで、整数部が1桁の時は0で埋める
            foreach (var n in number.ToString("00.00"))
            {
                //少数の「.」か？
                if (n == '.')
                {
                    spriteBatch.Draw(
                        textures[assetName],
                        position,
                        new Rectangle(10 * width, 0, width,
                        64),//
                        Color.White * alpha);
                }
                else
                {
                    //数字の描画
                    spriteBatch.Draw(
                        textures[assetName],
                        position,
                        new Rectangle((n - '0') * width, 0, width, 64),
                        Color.White * alpha);
                }

                //一文字描画したら1桁分右にずらす
                position.X += width;
            }
        }

        /// <summary>
        /// 画像の読み込み(画像オブジェクト版)
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="texture">2D画像オブジェクト</param>
        public void LoadContent(string assetName, Texture2D texture)
        {
            //すでにキー(assetName:アセット名)が登録されているとき
            if (textures.ContainsKey(assetName))
            {
#if DEBUG //DEBUGモードの時のみ下記エラー分をコンソールへ表示
                Console.WriteLine(
                    assetName +
                    "はすでに読み込まれています。\n" +
                    "プログラムを確認してください。");
#endif

                //それ以上読み込まないのでここで終了
                return;
            }

            textures.Add(assetName, texture);
        }

        /// <summary>
        /// 画像の描画（拡大縮小回転対応版）
        /// </summary>
        /// <param name="assetName">アセット名</param>
        /// <param name="positioin">位置</param>
        /// <param name="rect">切り出し範囲</param>
        /// <param name="rotate">回転角度</param>
        /// <param name="rotatePosition">回転軸位置</param>
        /// <param name="scale">拡大縮小</param>
        /// <param name="effects">表示反転効果</param>
        /// <param name="depth">スプライト深度</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(
            string assetName,
            Vector2 positioin,
            Rectangle? rect,//nullを受け入れられるよう「？」で
            float rotate,
            Vector2 rotatePosition,
            Vector2 scale,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 0.0f,
            float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[assetName],//テクスチャ
                positioin,          //位置
                rect,               //切り取り範囲
                Color.White * alpha,//透明値
                rotate,             //回転角度
                rotatePosition,     //回転軸
                scale,              //拡大縮小
                effects,            //表示反転効果
                depth               //スプライト深度
                );
        }


        public void DrawTexture(string assetName, Vector2 position, Color color,
            float alpha = 1.0f)
        {
            //デバッグモードの時のみ、画像描画前のアセット名チェック
            Debug.Assert(
                textures.ContainsKey(assetName),
                "描画時にアセット名の指定を間違えたか、" +
                "画像の読み込み自体出来ていません");

            spriteBatch.Draw(textures[assetName], position, color * alpha);
        }

        /// <summary>
        /// 線の描画（第3引数が線の色、第4引数が線の太さ）
        /// </summary>
        /// <param name="pos1">線の始まり</param>
        /// <param name="pos2">線の終わり</param>
        public void DrawLine(Vector2 pos1, Vector2 pos2 , Color color)
        {
            Primitives2D.DrawLine(spriteBatch, pos1, pos2, color, 10);

        }

        public void DrawCircle(Vector2 pos, float radius)
        {
            Primitives2D.DrawCircle(spriteBatch, pos, radius, 100, Color.Black, radius * 100);
        }

        public void DrawTexture(
            string assetName,
            Vector2 positioin,
            Rectangle? rect,//nullを受け入れられるよう「？」で
            Color color,
            float rotate,
            Vector2 rotatePosition,
            Vector2 scale,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 0.0f,
            float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[assetName],//テクスチャ
                positioin,          //位置
                rect,               //切り取り範囲
                color * alpha,      //透明値
                rotate,             //回転角度
                rotatePosition,     //回転軸
                scale,              //拡大縮小
                effects,            //表示反転効果
                depth               //スプライト深度
                );
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="assetName">画像の名前</param>
        /// <param name="positioin">位置</param>
        /// <param name="destRect">貼り付けサイズ</param>
        /// <param name="rect">切り抜き範囲</param>
        /// <param name="origin">原点</param>
        /// <param name="color">色</param>
        /// <param name="rotate">回転</param>
        /// <param name="scale">大きさ</param>
        /// <param name="effects">反転用</param>
        /// <param name="depth">レイヤ順</param>
        /// <param name="alpha">透明度</param>
        public void DrawTexture(
            string assetName,
            Vector2 positioin,
            Rectangle? destRect,
            Rectangle? rect,//nullを受け入れられるよう「？」で
            Vector2 origin,
            Color color,
            float rotate,
            Vector2 scale,
            SpriteEffects effects = SpriteEffects.None,
            float depth = 0.0f,
            float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[assetName],//テクスチャ
                positioin,          //位置
                destRect,           //渡したサイズに引き伸ばし
                rect,               //切り取り範囲
                origin,             //画像の原点
                rotate,             //回転角度
                scale,              //拡大縮小
                color * alpha,      //透明値
                effects,            //表示反転効果
                depth              //スプライト深度
                );
        }
    }
}
