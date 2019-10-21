using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OBG.Scene;

namespace OBG.Actor
{
    class Enemy : Character,IGameMediator
    {
        IGameMediator mediator;
        CharacterManager characterManager;
        public Vector2 Pposition;
        public Enemy(string name, Vector2 position, IGameMediator mediator)
        {
            this.position = position;
            this.name = name;
            this.mediator = mediator;
            pixelSize = 64;
        }
        public void AddActor(Character character)
        {
            if (character is Ball)
                mediator.AddActor((Ball)character);
        }
        public override void Hit(Character other)
        {
            
        }

        public override void Initialize()
        {
            
        }

        public override void Shutdown()
        {
            
        }
        public Vector2 GetPposition()
        {
            return Pposition;
        }
        public override void Update(GameTime gameTime)
        {

            Vector2 direction = Pposition - position;
            direction.Normalize();
            position += direction * 1.0f;
        }
    }
}
