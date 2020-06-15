using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sim.Objects
{
    class Cursor
    {
        private Color color = Color.Green;
        private readonly Texture2D texture;
        private readonly Vector2[] positions;
        private readonly Vector2 origin;
        private float scale;

        // Properties
        public bool Visible; // TODO: when setting, could possibly add or remove object from renderlist
        public Rectangle Target;

        public Cursor(Texture2D texture)
        {
            this.texture = texture;
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Console.Out.WriteLine(origin.ToString());
            positions = new Vector2[4];
            scale = 2f;

            Visible = false;

            Target = new Rectangle(800, 420, 240, 100);
        }

        public void Update(MouseState state, double totalTime)
        {
            if (Target == null || state == null)
            {
                Visible = false;
                return;
            }

            // TODO: just for testing
            if (Target.X <= state.X && state.X <= Target.X + Target.Width
             && Target.Y <= state.Y && state.Y <= Target.Y + Target.Height)
            {
                Visible = true;
            }
            else
            {
                Visible = false;
            }

            if (Visible)
            {
                Console.Out.WriteLine("X = " + state.X + " Y = " + state.Y);

                // HACK: confinded by a set grid
                //int halfWidth = Target.Width / 2;
                //int halfHeight = Target.Height / 2;
                //int x = (state.X + halfWidth) / Target.Width * Target.Width;
                //int y = (state.Y + halfHeight) / Target.Height * Target.Height;
                //positions[0] = new Vector2(x - halfWidth, y + halfHeight);
                //positions[1] = new Vector2(x - halfWidth, y - halfHeight);
                //positions[2] = new Vector2(x + halfWidth, y - halfHeight);
                //positions[3] = new Vector2(x + halfWidth, y + halfHeight);

                positions[0] = new Vector2(Target.X, Target.Y + Target.Height);
                positions[1] = new Vector2(Target.X, Target.Y);
                positions[2] = new Vector2(Target.X + Target.Width, Target.Y);
                positions[3] = new Vector2(Target.X + Target.Width, Target.Y + Target.Height);

                scale = 3f + (float)Math.Sin(totalTime * 10.0) * 0.5f; // TODO: make a helper for "bouncy" things, we really should only need one SIN call
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible) // TODO: this shouldn't be done here
            {
                spriteBatch.Draw(texture, positions[0], null, color, 0f, origin, scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(texture, positions[1], null, color, 1.5708f, origin, scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(texture, positions[2], null, color, 3.14159f, origin, scale, SpriteEffects.None, 1f);
                spriteBatch.Draw(texture, positions[3], null, color, 4.71239f, origin, scale, SpriteEffects.None, 1f);
            }
        }
    }
}