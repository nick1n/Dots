using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Objects
{
    class Dot
    {
        private Color color = Color.White;
        private Texture2D texture;
        private Vector2 position;
        private readonly int direction;
        //private double speed = 1.4; // average walking speed m/s
        private readonly int[] neighbours;

        public bool Dead { get; private set; }

        public Dot(Texture2D texture, int position) : this(texture, new Vector2(position % Main.screenWidth, position / Main.screenWidth)) { }

        public Dot(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;

            Random rand = new Random();

            direction = rand.Next(3);
            if (direction == 0)
            {
                color.R = (byte)rand.Next(256);
            }
            if (direction == 1)
            {
                color.G = (byte)rand.Next(256);
            }
            if (direction == 2)
            {
                color.B = (byte)rand.Next(256);
            }

            //direction = rand.Next(4);

            Dead = false;
            neighbours = GetNeighbours();
        }

        public void Update(Dictionary<int, Dot> state, ref HashSet<int> check)
        {
            //check = new List<Dot)();

            // number of neighbours
            int count = 0;

            foreach (int neighbour in neighbours)
            {
                if (!state.ContainsKey(neighbour))
                {
                    check.Add(neighbour);
                }
                else
                {
                    ++count;
                }
            }

            // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            // Any live cell with more than three live neighbours dies, as if by overpopulation.
            if (count < 2 || count > 3)
            {
                Dead = true;
            }


            //if (direction == 0)
            //{
            //    position.Y -= (float)(speed * deltaTime);
            //}

            //if (direction == 1)
            //{
            //    position.Y += (float)(speed * deltaTime);
            //}

            //if (direction == 2)
            //{
            //    position.X -= (float)(speed * deltaTime);
            //}

            //if (direction == 3)
            //{
            //    position.X += (float)(speed * deltaTime);
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }

        private int[] GetNeighbours()
        {
            return GetNeighbours(GetKey());
        }

        public static int[] GetNeighbours(int position)
        {
            return new int[] {
                position - Main.screenWidth - 1,
                position - Main.screenWidth,
                position - Main.screenWidth + 1,
                position - 1,
                //position,
                position + 1,
                position + Main.screenWidth - 1,
                position + Main.screenWidth,
                position + Main.screenWidth + 1
            };
        }

        public static bool CheckNeighbours(Dictionary<int, Dot> state, int position)
        {
            int count = 0;

            foreach (int neighbour in GetNeighbours(position))
            {
                if (state.ContainsKey(neighbour))
                {
                    ++count;
                }
            }

            // Any dead cell with exactly three live neighbours becomes a live cell
            return count == 3;
        }

        public int GetKey()
        {
            return GetKey((int)position.X, (int)position.Y);
        }

        public static int GetKey(int x, int y)
        {
            return x + y * Main.screenWidth;
        }
    }
}
