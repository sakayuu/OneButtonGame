using Microsoft.Xna.Framework;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Enemy enemy;
        private List<Pin> pins;
        private List<Character> addNewCharacters;
        private List<Collider> cols;

        private List<RayLine> rayLines;

        public Pin pin = null;

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
            if (enemy != null)
                enemy.Initialize();
            if (pins != null)
                pins.Clear();
            else
                pins = new List<Pin>();

            if (cols != null)
                cols.Clear();
            else
                cols = new List<Collider>();

            if (rayLines != null)
                rayLines.Clear();
            else
                rayLines = new List<RayLine>();

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
            else if (character is Enemy)
                enemy = (Enemy)character;
            else
                //追加リストにキャラを追加
                addNewCharacters.Add(character);
        }


        public void AddCollider(Collider collider, int pinNum)
        {
            cols.Remove(cols[pinNum]);
            cols.Insert(pinNum, collider);
        }

        /// <summary>
        /// プレイヤーが当たっているか
        /// </summary>
        private void HitToCharacters()
        {
            if (enemy.IsCollision(ball))
            {
                enemy.Hit(ball);
                ball.Hit(enemy);
            }
            foreach (var pin in pins) //ピンで繰り返し
            {
                if (ball.IsCollision(pin))
                {
                    ball.Hit(pin);
                }
                //if (rayLines.Count != 0)
                //    foreach (var rl in rayLines)
                //    {
                //        if (enemy.IsCollision(rl))
                //            ball.Hit(pin);
                //        if (rl.IsCollision(pin))
                //        {
                //            rl.Hit(pin);
                //            //ball.Hit(enemy);
                //        }
                //    }

            }

            if (cols.Count > 0)
                foreach (var col in cols)
                {
                    if (ball.IsCollision(col))
                        ball.Hit(col);
                }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            GetBPosition();
            //全キャラ更新
            ball.Update(gameTime);

            enemy.Update(gameTime);
            foreach (var p in pins)
            {
                p.Update(gameTime);
            }

            if (cols.Count() > 0)
                foreach (var col in cols)
                {
                    col.Update(gameTime);
                }

            if (rayLines.Count != 0)
            {
                foreach (var rl in rayLines)
                {
                    rl.Update(gameTime);
                }
            }

            //キャラ判別
            foreach (var newChara in addNewCharacters)
            {
                if (newChara is Pin)
                {
                    newChara.Initialize();
                    pins.Add((Pin)newChara);
                    cols.Add(new Collider(Vector2.Zero, 0));
                }
                else if (newChara is RayLine)
                {
                    newChara.Initialize();
                    rayLines.Add((RayLine)newChara);
                }
            }
            //追加処理後、追加リストはクリア
            addNewCharacters.Clear();

            //当たり判定
            HitToCharacters();

            //死亡時に削除
            RemoveDeadCharacters();

            if (Ball.ballState == BallState.Link)
            {
                //Debug.WriteLine(ball.nowPinNum);
                if (MathCollision.Circle_Segment(enemy.GetPosition() + new Vector2(32, 32),
                            32, ball.GetPosition(), ball.pPosition))
                {
                    ball.Hit(pin);
                }

                for (int i = 0; i < pins.Count; i++)
                {
                    if (ball.nowPinNum != i &&
                        MathCollision.Circle_Segment(pins[i].GetPosition() +
                        new Vector2(32, 32), 32,
                        ball.GetPosition(), ball.pPosition))
                    {
                        ball.Hit(pin);
                    }
                }

                //switch (ball.nowPinNum)
                //{
                //    case 0:
                //        if ()
                //        {


                //        }
                //        else if (MathCollision.Circle_Segment(pins[2].GetPosition() + new Vector2(32, 32),
                //            32, ball.GetPosition(), ball.pPosition))
                //        {
                //            ball.Hit(pin);

                //        }
                //        break;
                //    case 1:
                //        if (MathCollision.Circle_Segment(pins[0].GetPosition() + new Vector2(32, 32),
                //            32, ball.GetPosition(), ball.pPosition))
                //        {
                //            ball.Hit(pin);

                //        }
                //        else if (MathCollision.Circle_Segment(pins[2].GetPosition() + new Vector2(32, 32),
                //            32, ball.GetPosition(), ball.pPosition))
                //        {
                //            ball.Hit(pin);

                //        }
                //        break;
                //    case 2:
                //        if (MathCollision.Circle_Segment(pins[0].GetPosition() + new Vector2(32, 32),
                //            32, ball.GetPosition(), ball.pPosition))
                //        {
                //            ball.Hit(pin);

                //        }
                //        else if (MathCollision.Circle_Segment(pins[1].GetPosition() + new Vector2(32, 32),
                //            32, ball.GetPosition(), ball.pPosition))
                //        {
                //            ball.Hit(pin);

                //        }
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        /// <summary>
        /// 死亡キャラの削除
        /// </summary>
        private void RemoveDeadCharacters()
        {
            //死んでいたら、リストから削除
            rayLines.RemoveAll(rl => rl.IsDead());
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画クラス</param>
        public void Draw(Renderer renderer)
        {
            //全キャラ描画
            ball.Draw(renderer);
            enemy.Draw(renderer);
            foreach (var p in pins)
            {
                p.Draw(renderer);
            }
            if (cols.Count > 0)
                foreach (var col in cols)
                {
                    col.Draw(renderer);
                }

            if (rayLines.Count != 0)
                foreach (var rl in rayLines)
                {
                    rl.Draw(renderer);
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

        /// <summary>
        /// 2点の距離を返す
        /// </summary>
        /// <param name="bPos">始め</param>
        /// <param name="pPos">終わり</param>
        /// <returns></returns>
        public double CheckDistance(Vector2 bPos, Vector2 pPos)
        {
            double dist = Math.Sqrt((pPos.X - bPos.X) * (pPos.X - bPos.X) + (pPos.Y - bPos.Y) * (pPos.Y - bPos.Y));
            return dist;
        }

        public void GetAngleForBallToPins()
        {
            if (pin != null)
                return;
            else
                pin = GetShortestCheck(ball, pins);
            //Debug.WriteLine(pin.GetPosition());
            ball.vector = ball.GetPosition() - pin.GetPosition();
            ball.vector.Normalize();


            pin.radius = (float)CheckDistance(ball.GetPosition(), pin.GetPosition());

            var ballRad = Math.Atan2(ball.vector.Y, ball.vector.X);

            if (MathHelper.ToDegrees((float)ballRad) < 0)
                ballRad = (MathHelper.ToRadians(361) + ballRad);
            //ball.ang = MathHelper.ToRadians(degree);
            ball.ang = (float)ballRad;
            //Debug.WriteLine(MathHelper.ToDegrees((float)ballRad));

            ball.nowPinNum = pin.GetPinNum();
        }
        /// <summary>
        /// プレイヤーを渡す
        /// </summary>
        /// <returns></returns>
        public Ball GetBall()
        {
            return ball;
        }
        public void GetBPosition()
        {

            enemy.Pposition = ball.GetPosition();
        }
        /// <summary>
        /// ピンのリストを渡す
        /// </summary>
        /// <returns></returns>
        public List<Pin> GetList()
        {
            return pins;
        }

        public List<Collider> GetColliders()
        {
            return cols;
        }
    }
}
