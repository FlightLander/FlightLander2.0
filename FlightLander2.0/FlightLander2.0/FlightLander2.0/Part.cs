using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace FlightLander2._0
{
    class Part
    {
        public Vector2 pos, origin;
        public Texture2D tParte;
        public Color colour;
        public float scale, direccion, velocidad, rotacion;

        public Part(Texture2D tex, Color colour, Vector2 pos)
        {
            this.pos = pos;
            this.tParte = tex;
            this.colour = colour;
            this.direccion = Global.random.Next(0, 360);
            this.rotacion = Global.random.Next(0, 360);
            this.origin = new Vector2(this.tParte.Width / 2, this.tParte.Height / 2);
            this.scale = 1.0f;
            this.velocidad = 30;
        }

        public void PlacePart(Vector2 pos)
        {
            this.pos = pos;
        }

        public void Update()
        {
            this.pos = Global.NewPos(this.pos, this.velocidad, this.direccion);
            this.rotacion += 0.5f;
            if (this.scale <= 0.1f)
            {
                this.scale -= 0.000001f;
            }
            else
            {
                this.scale = 1.0f;
            }
            //Bordes
            //Derecha
            if (this.pos.X > Global.vpwidth)
            {
                this.pos.X = Global.vpwidth;
                if (this.direccion <= 90)
                {
                    this.direccion += 270;
                }
                else
                {
                    this.direccion -= 90;
                }
            }
            //Izquierda
            if (this.pos.X < 0)
            {
                this.pos.X = 0;
                if (this.direccion <= 270)
                {
                    this.direccion -= 90;
                }
                else
                {
                    this.direccion -= 270;
                }
            }
            //Abajo
            if (this.pos.Y > Global.vpheight)
            {
                this.pos.Y = Global.vpheight;
                if (this.direccion <= 180)
                {
                    this.direccion -= 90;
                }
                else
                {
                    this.direccion += 90;
                }
            }
            //Arriba
            if (this.pos.Y < 0)
            {
                this.pos.Y = 0;
                if (this.direccion <= 90)
                {
                    this.direccion += 90;
                }
                else
                {
                    this.direccion -= 90;
                }
            }
        }

        public void render(SpriteBatch sb)
        {
            sb.Draw(this.tParte, this.pos, null, this.colour, this.rotacion, this.origin, this.scale, SpriteEffects.None, 1);
        }
    }
}
