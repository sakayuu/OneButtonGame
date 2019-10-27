using Microsoft.Xna.Framework;
using OBG.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Scene
{
    ///<summary>
    ///シーンの列挙型
    /// </summary>
    enum Scene
    {
        Title,
        GamePlay,
        Ending,
        Clear,
        GoodEnding
    }
    
    /// <summary>
    /// シーンインターフェイス
    /// </summary>
    interface IScene
    {
        void Initialize();//初期化
        void Update(GameTime gameTime);//更新
        void Draw(Renderer renderer);//描画
        void Shutdown();//終了

        //シーン管理用
        bool IsEnd();//終了チェック
        Scene Next();//次のシーンへ
    }
}
