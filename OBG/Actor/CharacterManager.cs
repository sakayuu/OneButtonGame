using Microsoft.Xna.Framework;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Actor
{
    /// <summary>
    /// キャラクター管理クラス
    /// </summary>
    class CharacterManager
    {
        private Ball ball;
        private List<Pin> pins;
        private List<Character> addNewCharacters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterManager()
        {
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            if (ball != null)
                ball.Initialize();

            if (pins != null)
                pins.Clear();
            else
                pins = new List<Pin>();

            if (addNewCharacters != null)
                addNewCharacters.Clear();
            else
                addNewCharacters = new List<Character>();
        }

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="character">追加するキャラクター</param>
        public void Add(Character character)
        {
            //早期リターン : 登録するものがなければ何もしない
            if (character == null)
            {
                return;
            }
            else if (character is Ball)
                ball = (Ball)character;
            else
                //追加リストにキャラを追加
                addNewCharacters.Add(character);
        }

        /// <summary>
        /// プレイヤーが当たっているか
        /// </summary>
        private void HitToCharacters()
        {
            foreach (var pin in pins) //ピンで繰り返し
            {
                if (ball.IsCollision(pin))
                {
                    ball.Hit(pin);
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //全キャラ更新
            ball.Update(gameTime);
            foreach (var p in pins)
            {
                p.Update(gameTime);
            }

            //キャラ判別
            foreach (var newChara in addNewCharacters)
            {
                if (newChara is Pin)
                {
                    newChara.Initialize();
                    pins.Add((Pin)newChara);
                }
            }
            //追加処理後、追加リストはクリア
            addNewCharacters.Clear();

            //当たり判定
            HitToCharacters();

        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画クラス</param>
        public void Draw(Renderer renderer)
        {
            //全キャラ描画
            ball.Draw(renderer);
            foreach (var p in pins)
            {
                p.Draw(renderer);
            }
        }

        /// <summary>
        /// プレイヤーに一番近いピンの位置を返す
        /// </summary>
        /// <param name="ball">プレイヤー</param>
        /// <param name="charas">ピンのリスト</param>
        /// <returns></returns>
        public Pin GetShortestCheck(Ball ball, List<Pin> charas)
        {
            List<double> minDists = new List<double>();

            for (int i = 0; i < charas.Count; i++)
            {
                minDists.Add(CheckDistance(ball.GetPosition(), charas[i].GetPosition()));
            }

            double a = minDists.Min();

            return charas[minDists.FindIndex(n => n == a)];
        }

        public double CheckDistance(Vector2 bPos, Vector2 pPos)
        {
            double dist = Math.Sqrt((pPos.X - bPos.X) * (pPos.X - bPos.X) + (pPos.Y - bPos.Y) * (pPos.Y - bPos.Y));
            return dist;
        }


        public Ball GetBall()
        {
            return ball;
        }

        public List<Pin> GetList()
        {
            return pins;
        }
    }
}
