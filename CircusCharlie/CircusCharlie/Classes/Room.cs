using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CircusCharlie.Classes
{
    class Room
    {
        private const int mapWidth = 29;
        private const int mapHeight = 22;

        private Dictionary<IntVector2D, Tile> tiles;
        private List<Col> cols;
        private Dictionary<IntVector2D, Actor> actors;

        private string description = "";

        public Room()
        {
            tiles = new Dictionary<IntVector2D, Tile>();
            actors = new Dictionary<IntVector2D, Actor>();

            cols = new List<Col>();
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= mapWidth || y >= mapHeight || x < 0 || y < 0) return null;

            return null;
        }

        public void SetActor(IntVector2D v, Actor actor)
        {
            if (actor != null)
            {
                actors[v] = actor;
            }
            else
            {
                actors.Remove(v);
            }
        }

        public void SetTile(IntVector2D v, Tile tile)
        {
            if (tile != null)
            {
                tiles[v] = tile;
            }
            else
            {
                tiles.Remove(v);
            }
        }

        public Vector2 CheckCol(Actor other)
        {
            Vector2 output = Vector2.Zero;

            foreach (Col e in cols)
            {
                output -= other.CheckCol(e);
            }

            
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                Vector2 col = other.CheckCol(e.Value);

                if (col != Vector2.Zero)
                {
                    output += col;

                    if (col.Y != 0f)
                    {
                        e.Value.Destroy();
                    }
                }
            }

            return output;
        }

        public bool IsLevelComplete()
        {
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                if (e.Value.IsAlive()) return false;
            }

            return true;
        }

        public void InitRoom()
        {
            // Maybe have a parent class for ALL ACTORS which has a common draw and 
            // check collision function which returns a vector.
            // This way the ACTORS can check the type of thing they're colliding with (you pass the other actor to them).

            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                cols.Add(new ColSquare(new Vector2(e.Key.X * 24, e.Key.Y * 24),
                                       Vector2.Zero,
                                       new Vector2(24, 24)));
            }
        }

        public void StopRoom()
        {
            cols.Clear();
            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                e.Value.UnDestroy();
            }
        }

        public void SaveRoom(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                // Write the amount of tiles in the whole room
                writer.Write(tiles.Count);

                // Then write each tile in one by one
                foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
                {
                    writer.Write(e.Key.X);
                    writer.Write(e.Key.Y);
                    writer.Write(e.Value.GetOff().X);
                    writer.Write(e.Value.GetOff().Y);
                    writer.Write(e.Value.GetRotation());
                    writer.Write(e.Value.GetColor());
                }

                // Write the amount of actors in the room
                writer.Write(actors.Count);

                foreach (KeyValuePair<IntVector2D, Actor> e in actors)
                {
                    writer.Write(e.Key.X);
                    writer.Write(e.Key.Y);
                }
            }
        }
        public void LoadRoom(string filename, ref Sprite spr, ref Sprite _block)
        {
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    tiles.Clear();

                    // Read how many tiles are in this room
                    int count = reader.ReadInt32();

                    // Read the tiles into the room
                    for (int i = 0; i < count; i++)
                    {
                        int posX = reader.ReadInt32();
                        int posY = reader.ReadInt32();
                        int offX = reader.ReadInt32();
                        int offY = reader.ReadInt32();
                        int rotation = reader.ReadInt32();
                        int color = reader.ReadInt32();

                        Tile tile = new Tile(spr,
                                             new IntVector2D(offX, offY),
                                             new IntVector2D(posX, posY));
                        tile.SetRotation(rotation);
                        tile.SetColor(color);

                        tiles.Add(new IntVector2D(posX, posY), tile);
                    }

                    count = reader.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        int posX = reader.ReadInt32();
                        int posY = reader.ReadInt32();

                        Actor block = new Block(new IntVector2D(posX, posY), _block);

                        actors.Add(new IntVector2D(posX, posY), block);
                    }
                }
            }
        }

        public void DrawShadow(IntVector2D _pos)
        {
            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                e.Value.DrawShadow(new IntVector2D(0, 0));
            }
        }

        public void Draw(IntVector2D _pos)
        {

            foreach (KeyValuePair<IntVector2D, Tile> e in tiles)
            {
                e.Value.Draw(_pos);
            }

            foreach (Col e in cols)
            {
                e.DrawDebug();
            }

            foreach (KeyValuePair<IntVector2D, Actor> e in actors)
            {
                e.Value.Draw();
            }
        }

    }
}
