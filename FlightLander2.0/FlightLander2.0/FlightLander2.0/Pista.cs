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
    class Pista
    {
        public Texture2D tPista;
        public Vector2 position;
        public Vector2 origin;
        public Vector2 end;
        public float direccion;
        public float length;
        public bool landing;
        public TimeSpan blockLeft = new TimeSpan(0, 0, 0, 0, 0);
        public bool plane, blocked;
        public SpriteBatch sb;

        public static Vector2[] posiblePos = new Vector2[4]
        {
            new Vector2(Global.vpwidth / 3, Global.vpheight / 3), //Izq Arriba
            new Vector2((Global.vpwidth / 3) * 2, Global.vpheight / 3), //Der Arriba
            new Vector2((Global.vpwidth / 3) * 2, (Global.vpheight / 3) * 2), //Der Abajo
            new Vector2(Global.vpwidth / 3, (Global.vpheight / 3) * 2), //Izq Abajo
        };

        public Pista(Game1 game, GraphicsDevice graphicsD, bool plane, SpriteBatch sb)
        {
            if (plane) //Si la pista es para aviones:
            {
                this.tPista = game.Content.Load<Texture2D>("Imagenes/PistaAvion"); //Textura de PistaAviones
                this.direccion = Global.random.Next(0, 361); //Direccion aleatoria
                this.length = this.tPista.Height; //Largo
                this.position = GetPosition(graphicsD, this); //Posicion, una de las 4 posibles
                this.origin = new Vector2(this.tPista.Width / 2, this.tPista.Height); //Origen, principio de la pista, al medio
                switch ((int)this.direccion)
                {
                    //Casos simples:
                    case 0:
                    case 360:
                        this.end = new Vector2(this.position.X, this.position.Y - this.length);
                        break;
                    case 90:
                        this.end = new Vector2(this.position.X + this.length, this.position.Y);
                        break;
                    case 180:
                        this.end = new Vector2(this.position.X, this.position.Y + this.length);
                        break;
                    case 270:
                        this.end = new Vector2(this.position.X - this.length, this.position.Y);
                        break;
                    default:
                        //Casos complejos
                        if (this.direccion < 90)
                        {
                            //Se crea un triangulo imaginario para calcular la posicion del final de la pista
                            float angle = Global.DegreesToRadians(this.direccion); //La clase Math toma los angulos en radianes
                            float x = Math.Abs(((float)Math.Sin(angle) * this.length)); //Seno del angulo da la diferencia en X
                            float y = Math.Abs(((float)Math.Cos(angle) * this.length)); //Coseno del angulo da la diferencia en Y
                            this.end = new Vector2(this.position.X + x, this.position.Y - y); //Se agrega la diferencia a la posicion de la pista
                        }
                        else if (this.direccion < 180)
                        {
                            //Se crea un triangulo imaginario para calcular la posicion del final de la pista
                            float angle = Global.DegreesToRadians(this.direccion - 90); //La clase Math toma los angulos en radianes
                            float x = Math.Abs(((float)Math.Cos(angle) * this.length)); //Coseno del angulo da la diferencia en X
                            float y = Math.Abs(((float)Math.Sin(angle) * this.length)); //Seno del angulo da la diferencia en Y
                            this.end = new Vector2(this.position.X + x, this.position.Y + y); //Se agrega la diferencia a la posicion de la pista
                        }
                        else if (this.direccion < 270)
                        {
                            float angle = Global.DegreesToRadians(this.direccion - 180);
                            float x = Math.Abs(((float)Math.Sin(angle) * this.length));
                            float y = Math.Abs(((float)Math.Cos(angle) * this.length));
                            this.end = new Vector2(this.position.X - x, this.position.Y + y);
                            
                        }
                        else if (this.direccion < 360)
                        {
                            float angle = Global.DegreesToRadians(this.direccion - 270);
                            float x = Math.Abs(((float)Math.Cos(angle) * this.length));
                            float y = Math.Abs(((float)Math.Sin(angle) * this.length));
                            this.end = new Vector2(this.position.X - x, this.position.Y - y);
                        }
                        break;
                }
                this.landing = false;
                this.blocked = false;
                this.plane = true;
            }
            else
            {
                this.tPista = game.Content.Load<Texture2D>("Imagenes/PistaCopter");
                this.direccion = 0;
                this.origin = new Vector2(this.tPista.Width / 2, this.tPista.Height / 2);
                this.position = GetPosition(graphicsD, this);
                this.end = this.position;
                this.length = 0;
                this.landing = false;
                this.blocked = false;
                this.plane = false;
            }
            this.sb = sb;
        }

        private Vector2 GetPosition(GraphicsDevice graphicsD, Pista pista) //Devuelve una de las 4 posiciones posibles, de no estar ocupadas
        {
            int selPosIndex = Global.random.Next(0, 4);
            while (posiblePos[selPosIndex] == Vector2.Zero)
            {
                selPosIndex = Global.random.Next(0, 4);
            }
            Vector2 selectedPos = posiblePos[selPosIndex];
            posiblePos[selPosIndex] = Vector2.Zero;
            return selectedPos;
        }

        public static int AssignPista(Flyer.Tipo tipo)
        {
            Global.pistaassignloopcount++;
            int index = Global.random.Next(0, Global.pistas.Count<Pista>());
            if (tipo == Flyer.Tipo.Avion || tipo == Flyer.Tipo.Militar)
            {
                if (Global.pistas[index].plane)
                {
                    return index;
                }
                else
                {
                    if (Global.pistaassignloopcount >= 30)
                    {
                        return -1;
                    }
                    else
                    {
                        return AssignPista(tipo);
                    }
                }
            }
            else
            {
                if (!Global.pistas[index].plane)
                {
                    return index;
                }
                else
                {
                    if (Global.pistaassignloopcount > 20)
                    {
                        return -1;
                    }
                    else
                    {
                        return AssignPista(tipo);
                    }
                }
            }
        } //Asigna una pista aleatoriamente, del tipo correcto

        public void render()
        {
            sb.Draw(tPista, position, null, Color.White, Global.DegreesToRadians(direccion), origin, 1.0f, SpriteEffects.None, 0);
            //sb.Draw(tPista, end, null, Color.Green, Global.DegreesToRadians(0), new Vector2(0, 0), 0.1f, SpriteEffects.None, 0);
            //Final de la pista, dibujado par comprobar que funciona el algoritmo
        }
    }
}
