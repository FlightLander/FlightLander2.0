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
    class Button
    {
        public bool present;
        public Texture2D tButton;
        public Vector2 position, origin;
        public float scale = 1.0f;
        public btnStates state = btnStates.None;
        public btnTypes type;

        public enum btnStates
        {
            None,
            Hover,
            Pressed
        }

        public enum btnTypes
        {
            Exit,
            Play,
            Pause,
            Highscores,
            Idioma,
            Inst,
            Music,
            Sounds
        }

        public Button(btnTypes type, Game1 game, int x, int y)
        {
            switch (type)
            {
                case btnTypes.Exit:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnExit");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2((x / 4) * 3, y - 50);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Highscores:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnHighscores");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2((x / 4) * 2,  y - 50);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Play:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnPlay");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2((x / 4), y - 50);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Pause:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnPause");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2(Global.vpwidth - this.tButton.Width / 2, this.tButton.Height);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = false;
                    break;
                case btnTypes.Idioma:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnES");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2(Global.vpwidth - this.tButton.Width / 2, Global.vpheight);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Inst:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnInst");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Sounds:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnSoundsPlay");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2(Global.vpwidth - this.tButton.Width / 2 - Global.botones[3].tButton.Width - 5, this.tButton.Height + 5);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
                case btnTypes.Music:
                    this.type = type;
                    this.tButton = game.Content.Load<Texture2D>("Imagenes/btnMusicPlay");
                    this.scale = 1.0f;
                    this.state = btnStates.None;
                    this.position = new Vector2(Global.vpwidth - (this.tButton.Width * 1.5f) - Global.botones[3].tButton.Width - 15, this.tButton.Height + 5);
                    this.origin = new Vector2(this.tButton.Width / 2, this.tButton.Height);
                    this.present = true;
                    break;
            }
        }

        public void Update(MouseState ms, Game1 game, SpriteBatch sb, GraphicsDevice gd)
        {
            Rectangle btnRec = new Rectangle((int)this.position.X - ((int)this.tButton.Width / 2), 
                                             (int)this.position.Y - (int)this.tButton.Height, 
                                             (int)this.tButton.Width, 
                                             (int)this.tButton.Height);
            Rectangle msRec = new Rectangle(ms.X, ms.Y, 1, 1);
            if (btnRec.Intersects(msRec))
            {
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[8].Play();
                    }
                    else
                    {
                        Global.SEInstances[8].Stop();
                    }
                    this.state = btnStates.Pressed;
                    switch (this.type) //Ejecuta el codigo adecaudo del boton que se apreto
                    {
                        case btnTypes.Play:
                            Global.ClearForPlay(sb, game, gd, true);
                            Global.gameState = Global.GameState.Jugando;
                            Global.botones[0].present = false; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = false; //Jugar
                            Global.botones[3].present = true;  //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            Global.flyers.Clear();
                            break;
                        case btnTypes.Exit:
                            Global.botones[0].present = false; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = false; //Jugar
                            Global.botones[3].present = false; //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            game.Exit();
                            break;
                        case btnTypes.Highscores:
                            Global.botones[0].present = true; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = true; //Jugar
                            Global.botones[3].present = false; //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = true; //Instrucciones
                            Global.gameState = Global.GameState.Highscores;
                            //CONECTAR DB
                            OleDbConnection cn;
                            OleDbDataAdapter DA;
                            DataSet DS;
                            OleDbCommand cmd;
                            cn = new OleDbConnection();
                            cn.ConnectionString = "Provider = 'Microsoft.ACE.OLEDB.12.0';Data Source = 'Highscores.accdb'";
                            cn.Open();
                            //GUARDAR 3 mayores en array de strings
                            cmd = new OleDbCommand("SELECT TOP 3 * FROM Highscores ORDER by Puntajes DESC", cn);
                            DA = new OleDbDataAdapter(cmd);
                            DS = new DataSet();
                            DA.Fill(DS, "Highscores");
                            int i = 0;
                            foreach (DataRow row in DS.Tables["Highscores"].Rows)
                            {
                                try
                                {
                                    // Posible MEGA ERROR
                                    // Original: Global.hs[i] = row[0].ToString(); (este era en ves del row [2])
                                    Global.hs[i] = row[2].ToString();
                                    i++;
                                    Global.hs[i] = row[1].ToString();
                                    i++;
                                }
                                catch (IndexOutOfRangeException nre)
                                {
                                    Console.WriteLine("Error IndexOutOfRangeException... Microsoft must (but won't) fix this");
                                }
                            }
                            //DESCONECTAR DB
                            cn.Close();
                            break;
                        case btnTypes.Pause:
                            Global.botones[0].present = false; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = false; //Jugar
                            Global.botones[3].present = true; //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            if (Global.gameState == Global.GameState.Pausa) //Se mueve para que no sea apretado multiples veces
                            {
                                Global.botones[3].position = new Vector2(Global.vpwidth - this.tButton.Width / 2, this.tButton.Height);
                                Global.gameState = Global.GameState.Jugando;
                            }
                            else
                            {
                                Global.botones[3].position = new Vector2(Global.vpwidth / 2, Global.vpheight / 2);
                                Global.gameState = Global.GameState.Pausa;
                            }
                            break;
                        case btnTypes.Idioma:
                            if (Global.idioma)
                            {
                                this.position = new Vector2(Global.vpwidth - this.tButton.Width / 2, Global.vpheight);
                                Global.botones[4].tButton = game.Content.Load<Texture2D>("Imagenes/btnEN");
                                Global.idioma = false;
                                Global.reload = true;
                            }
                            else
                            {
                                this.position = new Vector2(Global.vpwidth - (this.tButton.Width / 2) * 3, Global.vpheight);
                                Global.botones[4].tButton = game.Content.Load<Texture2D>("Imagenes/btnES");
                                Global.idioma = true;
                                Global.reload = true;
                            }
                            break;
                        case btnTypes.Inst:
                            Global.ClearForPlay(sb, game, gd, false);
                            Game1.finished = false;
                            Game1.tutorialStage = 0;
                            Global.botones[0].present = true; //Salir
                            Global.botones[1].present = true; //Highscores
                            Global.botones[2].present = true; //Jugar
                            Global.botones[3].present = false; //Pausa
                            Global.botones[4].present = true; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            Global.gameState = Global.GameState.Instrucciones;
                            Global.flyers.Clear();
                            break;
                        case btnTypes.Music:
                            if (Global.musicmute)
                            {
                                Global.musicmute = false;
                                this.tButton = game.Content.Load<Texture2D>("Imagenes/btnMusicPlay");
                            }
                            else
                            {
                                Global.musicmute = true;
                                this.tButton = game.Content.Load<Texture2D>("Imagenes/btnMusicMute");
                            }
                            break;
                        case btnTypes.Sounds:
                            if (!Global.soundmute)
                            {
                                Global.soundmute = false;
                                this.tButton = game.Content.Load<Texture2D>("Imagenes/btnSoundsPlay");
                            }
                            else
                            {
                                Global.soundmute = true;
                                this.tButton = game.Content.Load<Texture2D>("Imagenes/btnSoundsMute");
                            }
                            break;
                    }
                }
                else
                {
                    this.state = btnStates.Hover;
                }
            }
            else
            {
                this.state = btnStates.None;
            }
            switch (this.state)
            {
                case btnStates.None:
                    this.scale = 1.0f;
                    break;
                case btnStates.Hover:
                    this.scale = 1.10f;
                    break;
                case btnStates.Pressed:
                    this.scale = 0.90f;
                    break;
            }
        }

        public void Draw(SpriteBatch sb, Color colour) //Se renderiza
        {
            switch (this.type)
            {
                case btnTypes.Exit:
                case btnTypes.Highscores:
                case btnTypes.Idioma:
                case btnTypes.Inst:
                case btnTypes.Play:
                    sb.Draw(this.tButton, this.position, null, colour, 0, this.origin, this.scale, SpriteEffects.None, 1.0f);
                    break;
                case btnTypes.Pause:
                    if ((Global.GameState.Jugando == Global.gameState || Global.GameState.Pausa == Global.gameState))
                    {
                        sb.Draw(this.tButton, this.position, null, colour, 0, this.origin, this.scale, SpriteEffects.None, 1.0f); 
                    }
                    break;
                case btnTypes.Music:
                    if (Global.musicmute)
                    {
                        sb.Draw(this.tButton, this.position, null, Color.Black, 0, this.origin, this.scale, SpriteEffects.None, 1.0f);
                    }
                    else
                    {
                        sb.Draw(this.tButton, this.position, null, Color.Orange, 0, this.origin, this.scale, SpriteEffects.None, 1.0f);
                    }
                    break;
                case btnTypes.Sounds:
                    if (!Global.soundmute)
                    {
                        sb.Draw(this.tButton, this.position, null, Color.Black, 0, this.origin, this.scale, SpriteEffects.None, 1.0f);
                    }
                    else
                    {
                        sb.Draw(this.tButton, this.position, null, Color.Orange, 0, this.origin, this.scale, SpriteEffects.None, 1.0f);
                    }
                    break;

            }

        }
    }
}
