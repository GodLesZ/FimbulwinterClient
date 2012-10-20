using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FimbulwinterClient.Screens
{
    public interface IGameScreen : IDisposable
    {
        void Draw(SpriteBatch sb, GameTime gameTime);
        void Update(SpriteBatch sb, GameTime gameTime);
    }
}
