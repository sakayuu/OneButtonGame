using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;//Assert用

namespace OBG.Device
{
    class Renderer
    {
        private ContentManager contentManager; //コンテンツ管理者
        private GraphicsDevice graphicsDevice; //グラフィック機器
        private SpriteBatch spriteBatch; //スプライト一括描画用オブジェクト

        private BasicEffect basicEffect = null;

        //複数画像管理用変数の宣言と生成
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        private VertexBuffer lineListVertexBuffer;

        VertexPositionColor[] lineListVertices = new VertexPositionColor[2];

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

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;

            int vertexCount = 2;

            lineListVertexBuffer = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionColor), vertexCount, BufferUsage.None);


        }



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


        public void DrawLine(Vector2 pos1, Vector2 pos2)
        {
            graphicsDevice.SetVertexBuffer(lineListVertexBuffer);

            lineListVertices[0] = new VertexPositionColor(new Vector3(pos1.X, pos1.Y, 0.0f),
                                                          Color.Red);
            lineListVertices[1] = new VertexPositionColor(new Vector3(pos2.X, pos2.Y, 0.0f),
                                                          Color.Blue);
            lineListVertexBuffer.SetData(lineListVertices);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 1);
            }
        }


    }
}
