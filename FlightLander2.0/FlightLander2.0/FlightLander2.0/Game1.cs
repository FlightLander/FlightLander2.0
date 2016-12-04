using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Flyer planes;
        Texture2D backJuego,
                  backGameOver,
                  gameOverCrashAvionCola,
                  gameOverCrashAvionCab,
                  gameOverCrashAvionAla,
                  gameOverCrashCopterCab,
                  gameOverCrashCopterHel,
                  gameOverCrashCopterCola,
                  gameOverCrashOVNI,
                  gameOverCrashMilitarCab,
                  gameOverCrashMilitarAla,
                  backNASA,
                  backInicio1,
                  backInicio2,
                  backInicio3,
                  backInicio4,
                  backInicio5,
                  backInicio6,
                  backInicio7,
                  HighscoreTrofeo,
                  HighscoreList,
                  HighscoreTitulo,
                  HighscoreBack;
        SoundEffectInstance instance;
        int indexToRemove = 100;
        bool gameover = false;

        //Recognizer
        SpeechRecognizer sr;
        string recognized;
        SpriteFont sf;
        SpriteFont sfBIG;

        //Tutorial
        string instText = "ERROR. PLEASE GO BACK.";
        public static int tutorialStage = 0;
        public static bool finished = false;
        float timerStartTime = -1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.vpwidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Global.vpheight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //Hacer fullscreen
            this.graphics.IsFullScreen = true; //TEMP
            graphics.PreferredBackBufferWidth = Global.vpwidth;
            graphics.PreferredBackBufferHeight = Global.vpheight;
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 30);
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.gameSpeed = 0.25f;
            Global.colorsFly[0] = Color.Yellow;
            Global.colorsFly[1] = Color.Blue;
            Global.colorsFly[2] = Color.White;
            Global.colorsFly[3] = Color.Orange;
            Global.colorsFly[4] = Color.DimGray;
            Global.colorsFly[5] = Color.Red;
            Global.colorsFly[6] = Color.Green;
            Global.colorsFly[7] = Color.Purple;
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Recognizer
            try
            {
                if (Global.idioma)
                {
                    LoadRecognizer(Global.esp);
                }
                else
                {
                    LoadRecognizer(Global.eng);
                }
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine("InvalidOperationException. SAPI failed to load.");
            }

            backInicio1 = this.Content.Load<Texture2D>("Imagenes/fondo1");
            backInicio2 = this.Content.Load<Texture2D>("Imagenes/fondo2");
            backInicio3 = this.Content.Load<Texture2D>("Imagenes/fondo3");
            backInicio4 = this.Content.Load<Texture2D>("Imagenes/fondo4");
            backInicio5 = this.Content.Load<Texture2D>("Imagenes/fondo5");
            backInicio6 = this.Content.Load<Texture2D>("Imagenes/fondo6");
            backInicio7 = this.Content.Load<Texture2D>("Imagenes/fondo7");
            backJuego = this.Content.Load<Texture2D>("Imagenes/fondo1");
            backGameOver = this.Content.Load<Texture2D>("Imagenes/backGameOver");
            gameOverCrashCopterCab = this.Content.Load<Texture2D>("Imagenes/pedazoCabCopter");
            gameOverCrashCopterCola = this.Content.Load<Texture2D>("Imagenes/pedazoColaCopter");
            gameOverCrashCopterHel = this.Content.Load<Texture2D>("Imagenes/pedazoHelice");
            gameOverCrashAvionCab = this.Content.Load<Texture2D>("Imagenes/pedazoCabAvion");
            gameOverCrashAvionCola = this.Content.Load<Texture2D>("Imagenes/pedazoColaAvion");
            gameOverCrashAvionAla = this.Content.Load<Texture2D>("Imagenes/pedazoAlaAvion");
            gameOverCrashOVNI = this.Content.Load<Texture2D>("Imagenes/pedazoOVNI");
            gameOverCrashMilitarCab = this.Content.Load<Texture2D>("Imagenes/pedazoCabMilitar");
            gameOverCrashMilitarAla = this.Content.Load<Texture2D>("Imagenes/pedazoAlaMilitar");
            backNASA = this.Content.Load<Texture2D>("Imagenes/backNASA");
            HighscoreList = this.Content.Load<Texture2D>("Imagenes/HSNewspaper");
            HighscoreTrofeo = this.Content.Load<Texture2D>("Imagenes/HSTrofeo");
            HighscoreTitulo = this.Content.Load<Texture2D>("Imagenes/HSTitulo");
            HighscoreBack = this.Content.Load<Texture2D>("Imagenes/BackHS");

            //Texto
            sf = this.Content.Load<SpriteFont>("Tipografias/font");
            sfBIG = this.Content.Load<SpriteFont>("Tipografias/fontBIG");

            //Sonidos
            Global.backmusic = this.Content.Load<SoundEffect>("Sonidos/background");
            Global.alarm = this.Content.Load<SoundEffect>("Sonidos/alarm");
            Global.newplane = this.Content.Load<SoundEffect>("Sonidos/newplane");
            Global.newchopper = this.Content.Load<SoundEffect>("Sonidos/newchopper");
            Global.newmilitar = this.Content.Load<SoundEffect>("Sonidos/newsonic");
            Global.newovni = this.Content.Load<SoundEffect>("Sonidos/newovni");
            Global.land = this.Content.Load<SoundEffect>("Sonidos/landed");
            Global.crash = this.Content.Load<SoundEffect>("Sonidos/crash");
            Global.tardis = this.Content.Load<SoundEffect>("Sonidos/tardis");
            Global.buttonpress = this.Content.Load<SoundEffect>("Sonidos/btnpress");
            Global.success = this.Content.Load<SoundEffect>("Sonidos/success");
            instance = Global.backmusic.CreateInstance();
            instance.IsLooped = true;
            Global.SEInstances[0] = Global.alarm.CreateInstance();
            Global.SEInstances[1] = Global.newplane.CreateInstance();
            Global.SEInstances[2] = Global.newchopper.CreateInstance();
            Global.SEInstances[3] = Global.newmilitar.CreateInstance();
            Global.SEInstances[4] = Global.newovni.CreateInstance();
            Global.SEInstances[5] = Global.land.CreateInstance();
            Global.SEInstances[6] = Global.crash.CreateInstance();
            Global.SEInstances[7] = Global.tardis.CreateInstance();
            Global.SEInstances[8] = Global.buttonpress.CreateInstance();
            Global.SEInstances[9] = Global.success.CreateInstance();
            foreach (SoundEffectInstance element in Global.SEInstances)
            {
                element.Volume = 0.2f;
            }

            //Load Botones
            Global.botones[0] = new Button(Button.btnTypes.Exit, this, Global.vpwidth, Global.vpheight); //Salir
            Global.botones[1] = new Button(Button.btnTypes.Highscores, this, Global.vpwidth, Global.vpheight); //Highscores
            Global.botones[2] = new Button(Button.btnTypes.Play, this, Global.vpwidth, Global.vpheight); //Jugar
            Global.botones[3] = new Button(Button.btnTypes.Pause, this, Global.vpwidth, Global.vpheight); //Pausa
            Global.botones[4] = new Button(Button.btnTypes.Idioma, this, Global.vpwidth, Global.vpheight); //Idioma
            Global.botones[5] = new Button(Button.btnTypes.Inst, this, Global.vpwidth, Global.vpheight); //Instrucciones
            Global.botones[6] = new Button(Button.btnTypes.Music, this, Global.vpwidth, Global.vpheight); //Musica
            Global.botones[7] = new Button(Button.btnTypes.Sounds, this, Global.vpwidth, Global.vpheight); //Sonidos
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            //Console.WriteLine("foo\n");
            if (!Global.musicmute)
            {
                instance.Volume = 0.05f;
                instance.Play();
            }
            else
            {
                instance.Stop();
            }
            //Volver a Inicio
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape)) {
                Global.botones[0].present = true ;  //Salir
                Global.botones[1].present = true ;  //Highscores
                Global.botones[2].present = true ;  //Jugar
                Global.botones[3].present = false;  //Pausa
                Global.botones[4].present = true ;  //Idioma
                Global.botones[5].present = true ;  //Instrucciones
                Global.gameState = Global.GameState.Menu;
            }
            //Recognizer
            if (Global.reload) {
                try
                {
                    sr.Dispose();
                }
                catch (NullReferenceException nre)
                {
                    Console.WriteLine("NullReferenceException. Cannot dispose of an none existing object.");
                }
                try
                {
                    if (Global.idioma)
                    {
                        LoadRecognizer(Global.esp);
                    }
                    else
                    {
                        LoadRecognizer(Global.eng);
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("InvalidOperationException. SAPI failed to load.");
                }
                Global.reload = false;
            }
            //Update
            switch (Global.gameState) { //Se decide que objetos se van a actualizar (cada GameState es una pantalla del juego)
                case Global.GameState.Menu:
                    UpdateMenu(gameTime);
                    break;
                case Global.GameState.Highscores:
                    UpdateHighscores(gameTime);
                    break;
                case Global.GameState.Jugando:
                    UpdateJugando(gameTime);
                    break;
                case Global.GameState.GameOver:
                    UpdateGameOver(gameTime);
                    break;
                case Global.GameState.Instrucciones:
                    UpdateInstrucciones(gameTime);
                    break;
                case Global.GameState.Pausa:
                    UpdatePausa(gameTime);
                    break;
                case Global.GameState.NASA:
                    UpdateNASA(gameTime);
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (Global.gameState) //Se decide que objetos mostrar (cada GameState es una pantalla del juego)
            {
                case Global.GameState.Menu:
                    DrawMenu(gameTime);
                    break;
                case Global.GameState.Highscores:
                    DrawHighscores(gameTime);
                    break;
                case Global.GameState.Jugando:
                    DrawJugando(gameTime);
                    break;
                case Global.GameState.GameOver:
                    DrawGameOver(gameTime);
                    break;
                case Global.GameState.Instrucciones:
                    DrawInstrucciones(gameTime);
                    break;
                case Global.GameState.Pausa:
                    DrawPausa(gameTime);
                    break;
                case Global.GameState.NASA:
                    DrawNASA(gameTime);
                    break;
            }
        }

        private void UpdateJugando(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Botones
            MouseState mouse = Mouse.GetState();
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Update(mouse, this, spriteBatch, this.GraphicsDevice);
                }
            }

            //Juego
            //Nuevos Flyers, crea nuevos flyers cada vez en menor tiempo, minimo 7 segundos
            if (Global.timerNewFlyer <= 0 && Global.flyers.Count() <= 20)
            {
                planes = new Flyer(GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, this);
                Global.timerNewFlyer = 15.0f - ((float)Global.landCount * 1.0f);
                if (Global.timerNewFlyer <= 7.0f)
                {
                    Global.timerNewFlyer = 7.0f;
                }
            }

            //Update Flyers
            int index = 0;
            foreach (Flyer element in Global.flyers)
            {
                gameover = Flyer.Update(GraphicsDevice, index, element, out indexToRemove);
                //Borrar Flyer aterrizado (se debe hacer fuera del foreach)
                if (indexToRemove != 100)
                {
                    try
                    {
                        Global.flyers.RemoveAll(x => x.tipo == Global.flyers[indexToRemove].tipo && x.mycolor == Global.flyers[indexToRemove].mycolor);
                        if (!Global.soundmute)
                        {
                            Global.SEInstances[5].Play();
                        }
                        else
                        {
                            Global.SEInstances[5].Stop();
                        }
                        indexToRemove = 100;
                        break;
                    }
                    catch (ArgumentOutOfRangeException aoore)
                    {
                        indexToRemove = 100;
                    }
                }
                if (gameover)
                {
                    break;
                }
                index++;
            }

            //Sacar todos los flyers
            if (gameover) //Si se pierde
            {
                //Clear contadores
                Global.gameState = Global.GameState.GameOver;
                Global.botones[0].present = true ;  //Salir
                Global.botones[1].present = true ;  //Highscores
                Global.botones[2].present = true ;  //Jugar
                Global.botones[3].present = false;  //Pausa
                Global.botones[4].present = true ;  //Idioma
                Global.botones[5].present = true ;  //Instrucciones
                //CONECTAR
                OleDbConnection cn;
                cn = new OleDbConnection();
                cn.ConnectionString = "Provider = 'Microsoft.ACE.OLEDB.12.0';Data Source = 'Highscores.accdb'";
                cn.Open();
                //GUARDAR EN DB (Global.puntaje y Global.Aterrizados)
                OleDbCommand CMD;
                string sql = "insert into Highscores (Puntajes, Aterrizados) values (" + (Global.puntaje * 10).ToString() + ", " + Global.landCount + ")";
                CMD = new OleDbCommand(sql, cn);
                CMD.ExecuteNonQuery();
                //DESCONECTAR
                cn.Close();
                //GUARDAR PUNTAJE EN VARIABLE (NUEVA);
                Global.hsAterrizados = Global.landCount;
                Global.hsPuntaje = Global.puntaje;
                Global.flyers.Clear();
                gameover = false;
            }

            //Comandos
            KeyboardState ks = Keyboard.GetState();
            if (Flyer.GetFlyerIndex(ks) != -1)
            {
                Flyer.Comando(ks, Flyer.GetFlyerIndex(ks));
            }
            if (Global.idioma)
            {
                if (Flyer.GetFlyerIndex(Global.recognizedCmd, Global.esp) != -1)
                {
                    Flyer.Comando(Global.recognizedCmd, Flyer.GetFlyerIndex(Global.recognizedCmd, Global.esp), Global.esp);
                }
            }
            else
            {
                if (Flyer.GetFlyerIndex(Global.recognizedCmd, Global.eng) != -1)
                {
                    Flyer.Comando(Global.recognizedCmd, Flyer.GetFlyerIndex(Global.recognizedCmd, Global.eng), Global.eng);
                }
            }

            //Close Game
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }
            Global.timerNewFlyer -= (float)gameTime.ElapsedGameTime.TotalSeconds; //Se cresta el tiempo pasado al restante para que aparesca una nueva aeronave
            base.Update(gameTime);
        }

        private void UpdateMenu(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();
            //Botones
            foreach (Button element in Global.botones) //Se actualizan los botones
            {
                if (element.present)
                {
                    element.Update(mouse, this, spriteBatch, this.GraphicsDevice);
                }
            }

            KeyboardState ks = Keyboard.GetState();
            //Close Game
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        private void UpdateGameOver(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Update(mouse, this, spriteBatch, this.GraphicsDevice);
                }
            }

            //Partes Flyers
            foreach (Part element in Global.partes)
            {
                element.Update();
            }

            KeyboardState ks = Keyboard.GetState();
            //Close Game
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        private void UpdateHighscores(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState ks = Keyboard.GetState();
            if (Flyer.GetFlyerIndex(ks) != -1)
            {
                Flyer.Comando(ks, Flyer.GetFlyerIndex(ks));
            }
            //Close Game
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }


            base.Update(gameTime);
        }

        private int timerForTutorial(float timerHit, int currentTutorialStage, GameTime gameTime)
        {
            if (timerStartTime == -1)
            {
                timerStartTime = (float)gameTime.TotalGameTime.TotalSeconds;
                Console.WriteLine("Start time: " + timerStartTime.ToString());
                Console.WriteLine("Hit time: " + timerHit.ToString());
            }
            else if ((float)gameTime.TotalGameTime.TotalSeconds - timerStartTime >= timerHit)
            {
                Console.WriteLine("Current time: " + ((float)gameTime.TotalGameTime.TotalSeconds).ToString());
                timerStartTime = -1;
                Console.WriteLine("Timer finished. ( " + timerStartTime.ToString() + " )");
                return currentTutorialStage + 1;
            }
            Console.WriteLine("Current time: " + ((float)gameTime.TotalGameTime.TotalSeconds).ToString());
            return currentTutorialStage;
        }

        private void playTutorailSuccessSound()
        {
            if (!Global.soundmute)
            {
                Global.SEInstances[9].Play();
            }
            else
            {
                Global.SEInstances[9].Stop();
            }
        }

        private void UpdateInstrucciones(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();
            //Botones
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Update(mouse, this, spriteBatch, this.GraphicsDevice);
                }
            }

            //Tutorial - START
            if (Global.flyers.Count() == 0 && Global.gameState == Global.GameState.Instrucciones)
                planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.Avion, Flyer.myColor.Blanco);
            else if (Global.gameState == Global.GameState.Instrucciones)
                Global.flyers[0].Position = new Vector2(Global.vpwidth / 2, Global.vpheight / 2);
            //Update Flyer
            Flyer.UpdateOnTutorial(GraphicsDevice, planes);
            if (Global.idioma)
            {
                instText = "Diga: ";
                switch (tutorialStage)
                {
                    case 0:
                        instText += "AVION";
                        if (recognized == Global.esp[00] || recognized == Global.eng[00])
                            tutorialStage = 01;
                        break;
                    case 1:
                        instText += "BLANCO";
                        if (recognized == Global.esp[06] || recognized == Global.eng[06])
                            tutorialStage = 02;
                        break;
                    case 2:
                        instText += "DERECHA";
                        if (recognized == Global.esp[13] || recognized == Global.eng[13])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 3:
                        tutorialStage = 04;
                        Global.flyers.Clear();
                        planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.Militar, Flyer.myColor.Verde);
                        break;
                    case 4:
                        instText += "MILITAR";
                        if (recognized == Global.esp[02] || recognized == Global.eng[02])
                            tutorialStage = 05;
                        break;
                    case 5:
                        instText += "VERDE";
                        if (recognized == Global.esp[10] || recognized == Global.eng[10])
                            tutorialStage = 06;
                        break;
                    case 6:
                        instText += "ATERRIZAR";
                        if (recognized == Global.esp[12] || recognized == Global.eng[12])
                            tutorialStage = 07;
                        break;
                    case 7:
                        instText += "PISTA UNO";
                        if (recognized == Global.esp[16] || recognized == Global.eng[16])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 8:
                            tutorialStage = 09;
                            Global.flyers.Clear();
                            planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.Helicoptero, Flyer.myColor.Naranja);
                        break;
                    case 9:
                        instText += "HELICOPTERO";
                        if (recognized == Global.esp[01] || recognized == Global.eng[01])
                            tutorialStage = 10;
                        break;
                    case 10:
                        instText += "NARANJA";
                        if (recognized == Global.esp[07] || recognized == Global.eng[07])
                            tutorialStage = 11;
                        break;
                    case 11:
                        instText += "ESPERAR";
                        if (recognized == Global.esp[15] || recognized == Global.eng[15])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 12:
                        tutorialStage = 13;
                        Global.flyers.Clear();
                        planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.OVNI, Flyer.myColor.Amarillo);
                        break;
                    case 13:
                        instText += "OVNI";
                        
                        if (recognized == Global.esp[03] || recognized == Global.eng[03])
                        {
                            tutorialStage = 14;
                            if (Global.idioma)
                                recognized = Global.esp[04];
                            else
                                recognized = Global.eng[04];
                        }
                        break;
                    case 14:
                        instText += "AMARRILLO";
                        if (recognized == Global.esp[04] || recognized == Global.eng[04])
                            tutorialStage = 15;
                        break;
                    case 15:
                        instText += "ATERRIZAR";
                        if (recognized == Global.esp[12] || recognized == Global.eng[12])
                            tutorialStage = 16;
                        break;
                    case 16:
                        instText += "HELIPUERTO DOS";
                        if (recognized == Global.esp[19] || recognized == Global.eng[19])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = 17;
                        }
                        break;
                    case 17:
                        instText = "FELICIDADES!!!!";
                        finished = true;
                        tutorialStage = timerForTutorial(5.0f, tutorialStage, gameTime);
                        break;
                    case 18:
                        if (finished)
                        {
                            Global.ClearForPlay(spriteBatch, this, GraphicsDevice, true);
                            Global.gameState = Global.GameState.Jugando;
                            Global.botones[0].present = false; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = false; //Jugar
                            Global.botones[3].present = true;  //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            Global.flyers.Clear();
                        }
                        break;
                    default:
                        playTutorailSuccessSound();
                        instText = "error";
                        break;
                }
                if (!finished)
                    instText += ", y luego espere que aparesca la palabra en pantalla.";
            }
            else
            {
                instText = "Say: ";
                switch (tutorialStage)
                {
                    case 0:
                        instText += "PLANE";
                        if (recognized == Global.esp[00] || recognized == Global.eng[00])
                            tutorialStage = 01;
                        break;
                    case 1:
                        instText += "WHITE";
                        if (recognized == Global.esp[06] || recognized == Global.eng[06])
                            tutorialStage = 02;
                        break;
                    case 2:
                        instText += "RIGHT";
                        if (recognized == Global.esp[13] || recognized == Global.eng[13])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 3:
                        tutorialStage = 04;
                        Global.flyers.Clear();
                        planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.Militar, Flyer.myColor.Verde);
                        break;
                    case 4:
                        instText += "MILITAR";
                        if (recognized == Global.esp[02] || recognized == Global.eng[02])
                            tutorialStage = 05;
                        break;
                    case 5:
                        instText += "GREEN";
                        if (recognized == Global.esp[10] || recognized == Global.eng[10])
                            tutorialStage = 06;
                        break;
                    case 6:
                        instText += "LAND";
                        if (recognized == Global.esp[12] || recognized == Global.eng[12])
                            tutorialStage = 07;
                        break;
                    case 7:
                        instText += "AIRSTRIP ONE";
                        if (recognized == Global.esp[16] || recognized == Global.eng[16])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 8:
                        tutorialStage = 09;
                        Global.flyers.Clear();
                        planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.Helicoptero, Flyer.myColor.Naranja);
                        break;
                    case 9:
                        instText += "CHOPPER";
                        if (recognized == Global.esp[01] || recognized == Global.eng[01])
                            tutorialStage = 10;
                        break;
                    case 10:
                        instText += "ORANGE";
                        if (recognized == Global.esp[07] || recognized == Global.eng[07])
                            tutorialStage = 11;
                        break;
                    case 11:
                        instText += "WAIT";
                        if (recognized == Global.esp[15] || recognized == Global.eng[15])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = timerForTutorial(3.5f, tutorialStage, gameTime);
                        }
                        break;
                    case 12:
                        tutorialStage = 13;
                        Global.flyers.Clear();
                        planes = new Flyer(Global.vpheight, Global.vpwidth, this, "center", Flyer.Tipo.OVNI, Flyer.myColor.Amarillo);
                        break;
                    case 13:
                        instText += "UFO";

                        if (recognized == Global.esp[03] || recognized == Global.eng[03])
                        {
                            tutorialStage = 14;
                            if (Global.idioma)
                                recognized = Global.esp[04];
                            else
                                recognized = Global.eng[04];
                        }
                        break;
                    case 14:
                        instText += "YELLOW";
                        if (recognized == Global.esp[04] || recognized == Global.eng[04])
                            tutorialStage = 15;
                        break;
                    case 15:
                        instText += "LAND";
                        if (recognized == Global.esp[12] || recognized == Global.eng[12])
                            tutorialStage = 16;
                        break;
                    case 16:
                        instText += "PORT TWO";
                        if (recognized == Global.esp[19] || recognized == Global.eng[19])
                        {
                            playTutorailSuccessSound();
                            tutorialStage = 17;
                        }
                        break;
                    case 17:
                        instText = "CONGRATULATIONS!!!!";
                        finished = true;
                        tutorialStage = timerForTutorial(5.0f, tutorialStage, gameTime);
                        break;
                    case 18:
                        if (finished)
                        {
                            Global.ClearForPlay(spriteBatch, this, GraphicsDevice, true);
                            Global.gameState = Global.GameState.Jugando;
                            Global.botones[0].present = false; //Salir
                            Global.botones[1].present = false; //Highscores
                            Global.botones[2].present = false; //Jugar
                            Global.botones[3].present = true;  //Pausa
                            Global.botones[4].present = false; //Idioma
                            Global.botones[5].present = false; //Instrucciones
                            Global.flyers.Clear();
                        }
                        break;
                    default:
                        instText = "error";
                        break;
                }
                instText += ", and then wait for the word to appear on the screen.";
            }
            //Comandos
            KeyboardState ks = Keyboard.GetState();
            if (Flyer.GetFlyerIndex(ks) != -1)
            {
                Flyer.Comando(ks, Flyer.GetFlyerIndex(ks));
            }
            if (Global.idioma)
            {
                if (Flyer.GetFlyerIndex(Global.recognizedCmd, Global.esp) != -1)
                {
                    Flyer.Comando(Global.recognizedCmd, Flyer.GetFlyerIndex(Global.recognizedCmd, Global.esp), Global.esp);
                }
            }
            else
            {
                if (Flyer.GetFlyerIndex(Global.recognizedCmd, Global.eng) != -1)
                {
                    Flyer.Comando(Global.recognizedCmd, Flyer.GetFlyerIndex(Global.recognizedCmd, Global.eng), Global.eng);
                }
            }
            //Tutorial -  END 
            
            //Close Game
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        private void UpdatePausa(GameTime gameTime)
        {
            //Botones
            MouseState mouse = Mouse.GetState();
            foreach (Button element in Global.botones) //SOLO SE ACTUALIZA EL BOTON DE PAUSA
            {
                if (element.present)
                {
                    element.Update(mouse, this, spriteBatch, this.GraphicsDevice);
                }
            }
        }

        private void UpdateNASA(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            //NASA
            Global.NASAtimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; //Se muestra un logo de la NASA por 5 segundos
            if (Global.NASAtimer <= 0)
            {
                Global.gameState = Global.GameState.Jugando;
            }

            KeyboardState ks = Keyboard.GetState();
            //Close Game
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape) && ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
            }
            base.Update(gameTime);
        }

        private void DrawJugando(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backInicio1, //Textura //Back
                             new Vector2((float)Global.vpwidth, 0), //Ubicacion
                             null, //Parte de la textura a mostrar
                             Color.White, //Filtro de color
                             0, //Rotacion
                             new Vector2((float)backInicio1.Width, 0), //Origen de la Imagen
                             new Vector2(((Global.vpwidth * 100.0f) / backInicio1.Width) / 100.0f,   //Escalar en X
                                        ((Global.vpheight * 100.0f) / backInicio1.Height) / 100.0f), //Escalar en Y
                             SpriteEffects.None, //Efectos
                             0); //Orden de los Objetos en pantalla (0 atras, 1 adelante)
            //Se sigue el mismo patron para todos los objetos
            spriteBatch.Draw(backInicio3, new Vector2((float)Global.vpwidth / 2, 20), null, Color.White, 0, new Vector2((float)backInicio2.Width / 2, 0), 0.125f, SpriteEffects.None, 0); //Insigna
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 60), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0); //Hangar 1
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 75 + backInicio6.Height), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0); //Hangar 2
            //btnPausa
            foreach (Button element in Global.botones) //Se renderiza el boton de pausa
            {
                if (element.present)
                {
                    element.Draw(spriteBatch, Color.Orange);
                }
            }
            //Pistas
            foreach (Pista element in Global.pistas) //Se renderizan las pistas de aterrizaje
            {
                if (element != null)
                    element.render();
            }
            //Number pistas
            spriteBatch.DrawString(sf, "1", Global.pistas[0].position, Color.White, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[0].position.X - 1, Global.pistas[0].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[1].position, Color.White, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[1].position.X - 1, Global.pistas[1].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", Global.pistas[2].position, Color.White, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[2].position.X - 1, Global.pistas[2].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[3].position, Color.White, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[3].position.X - 1, Global.pistas[3].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            //Flyers
            foreach (Flyer element in Global.flyers) //Se renderizan las aeronaves
            {
                element.render(spriteBatch);
            }
            //Recognizer
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(0, 40), Color.White); //Se renderiza el comando reconocido
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(-1, 41), Color.Orange); //Color
            //PUNTOS
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), Vector2.Zero, Color.White); //Se renderiza el puntaje hasta el momento
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), new Vector2(-1, 1), Color.Orange); //Color
            //ATERRIZADOS
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(0, 60), Color.White); //Se renderiza el total de aeronaves aterrizadas
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(-1, 61), Color.Orange); //Color
            //FPS
            if (gameTime.ElapsedGameTime.TotalSeconds != 0)
            {
                spriteBatch.DrawString(sf, "FPS: " + ((int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds)).ToString(), new Vector2(0, 20), Color.White); //Se renderiza la cantidad de FPS
                spriteBatch.DrawString(sf, "FPS: " + ((int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds)).ToString(), new Vector2(-1, 21), Color.Orange); //Color
            }
            else
            {
                //Si los segundos son 0, la division tiene a infinito
                spriteBatch.DrawString(sf, "FPS: Inf.", new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(sf, "FPS: Inf.", new Vector2(99, 101), Color.Orange);
            }
            //COMANDO ACTUAL
            string cmd = "Comando: ";
            for (int i = 0; i < Global.recognizedCmd.Length; i++) {
                if (isNotInOptions(Global.recognizedCmd[i]))
                    cmd += "_____";
                else
                    cmd += Global.recognizedCmd[i];
                if (i < Global.recognizedCmd.Length - 1)
                    cmd += " + ";
            }
            spriteBatch.DrawString(sf, cmd, new Vector2(0, 80), Color.White);
            spriteBatch.DrawString(sf, cmd, new Vector2(-1, 81), Color.Orange);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool isNotInOptions(string query)
        {
            bool found = false;
            for (int i = 0; i < Global.esp.Length; i++)
                if (query == Global.esp[i] || query == Global.eng[i])
                {
                    found = true;
                    break;
                }
            return !found;
        }

        private void DrawMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backInicio1, new Vector2((float)Global.vpwidth, 0), null, Color.White, 0, new Vector2((float)backInicio1.Width, 0), new Vector2(((Global.vpwidth * 100.0f) / backInicio1.Width) / 100.0f, ((Global.vpheight * 100.0f) / backInicio1.Height) / 100.0f), SpriteEffects.None, 0);
            spriteBatch.Draw(backInicio2, new Vector2((float)Global.vpwidth / 2, backInicio3.Height + 40), null, Color.White, 0, new Vector2((float)backInicio2.Width / 2, 0), 1.0f, SpriteEffects.None, 1); //Titulo
            spriteBatch.Draw(backInicio3, new Vector2((float)Global.vpwidth / 2, 20), null, Color.White, 0, new Vector2((float)backInicio2.Width / 2, 0), 1.0f, SpriteEffects.None, 1); //Insigna
            spriteBatch.Draw(backInicio4, new Vector2(((float)Global.vpwidth / 2) + (backInicio3.Width / 2) + 20, 100), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1); //Avion
            spriteBatch.Draw(backInicio5, new Vector2(((float)Global.vpwidth / 2) - (backInicio3.Width / 2) - 20, 100), null, Color.White, 0, new Vector2(backInicio5.Width, 0), 1.0f, SpriteEffects.None, 1); //Copter
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 60), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1); //Hangar 1
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 75 + backInicio6.Height), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1); //Hangar 2
            spriteBatch.Draw(backInicio7, new Vector2(0, Global.vpheight), null, Color.White, 0, new Vector2(0, backInicio7.Height), 1.0f, SpriteEffects.None, 1); //Version (1.0)
            //Botones
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Draw(spriteBatch, Color.Orange);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawGameOver(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backGameOver, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            //Partes de todos las aeronaves moviendose a gran velocidad por la pantalla
            //Se le asigna una posicion y una rotacion aleatoria (nueva) a cada parte de las aeronaves en todos los frames.
            /*spriteBatch.Draw(gameOverCrashCopterCab, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);             //Cabina Helicoptero
            spriteBatch.Draw(gameOverCrashCopterCola, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);            //Cola   Helicoptero
            spriteBatch.Draw(gameOverCrashCopterHel, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);             //Helice Helicoptero
            spriteBatch.Draw(gameOverCrashAvionCab, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);              //Cabina Avion
            spriteBatch.Draw(gameOverCrashAvionCola, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);             //Cola   Avion
            spriteBatch.Draw(gameOverCrashAvionAla, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);              //Ala    Avion
            spriteBatch.Draw(gameOverCrashOVNI, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);                  //Izq    OVNI
            spriteBatch.Draw(gameOverCrashMilitarAla, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.None, 1);            //Ala Militar
            spriteBatch.Draw(gameOverCrashMilitarCab, new Vector2(Global.random.Next(0, Global.vpwidth), Global.random.Next(0, Global.vpheight)), null, Color.White, Global.DegreesToRadians(Global.random.Next(0, 360)), Vector2.Zero, 0.75f, SpriteEffects.FlipVertically, 1);  //Cab Militar
            //Chocados
            spriteBatch.Draw(Global.fly1text, Global.fly1pos, null, Global.fly1colour, Global.DegreesToRadians(Global.fly1dir), new Vector2(Global.fly1text.Width / 2, Global.fly1text.Height / 2), 1.0f, SpriteEffects.None, 1); //Chocado 1
            spriteBatch.Draw(Global.fly2text, Global.fly2pos, null, Global.fly2colour, Global.DegreesToRadians(Global.fly2dir), new Vector2(Global.fly2text.Width / 2, Global.fly2text.Height / 2), 1.0f, SpriteEffects.None, 1); //Chocado 2
            */
            foreach (Part element in Global.partes)
            {
                element.render(spriteBatch);
            }
            //Puntajes
            spriteBatch.DrawString(sf, (Global.hsPuntaje * 10).ToString() + " Puntos!", new Vector2(Global.vpwidth / 2, Global.vpheight / 2), Color.White, 0, sf.MeasureString(Global.hsPuntaje.ToString() + " Puntos!") / 2, 4.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, (Global.hsPuntaje * 10).ToString() + " Puntos!", new Vector2(Global.vpwidth / 2 - 1, Global.vpheight / 2 + 1), Color.Black, 0, sf.MeasureString(Global.hsPuntaje.ToString() + " Puntos!") / 2, 4.0f, SpriteEffects.None, 1);
            //Botones
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Draw(spriteBatch, Color.Orange);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawHighscores(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //FONDO HIGHSCORES
            spriteBatch.Draw(HighscoreBack, new Vector2((float)Global.vpwidth + 50, 0), null, Color.White, 0, new Vector2((float)HighscoreBack.Width, 0), new Vector2((((Global.vpwidth * 100.0f) / HighscoreBack.Width) / 100.0f) * 2.0f, (((Global.vpheight * 100.0f) / HighscoreBack.Height) / 100.0f) * 2.0f), SpriteEffects.None, 0);
            spriteBatch.Draw(HighscoreTrofeo, new Vector2(40, 40), null, Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(HighscoreTitulo, new Vector2(Global.vpwidth - 40, 50), null, Color.White, 0, new Vector2(HighscoreTitulo.Width, 0), 2.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(HighscoreList, new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2, Global.vpheight / 2 - HighscoreList.Height / 2), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
            //TEXTO Puntaje 1, aterrizados 1; Puntaje 2, aterrizados 2; Puntaje 3, aterrizados 3;
            spriteBatch.DrawString(sf, //Tipografia
                                   Global.hs[1], //PUNTAJE
                                   new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 100, Global.vpheight / 2 - HighscoreList.Height / 2 + 130), //Ubicacion (relativa a la ubicacion de la lista)
                                   Color.Black, //Color
                                   Global.DegreesToRadians(-2), //Rotacion
                                   Vector2.Zero, //Origen
                                   2.0f, //Tamanio
                                   SpriteEffects.None, //Efectos
                                   1); //Orden de los objetos
            spriteBatch.DrawString(sf, Global.hs[0], new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 200, Global.vpheight / 2 - HighscoreList.Height / 2 + 130), Color.Black, Global.DegreesToRadians(-2), Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, Global.hs[3], new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 100, Global.vpheight / 2 - HighscoreList.Height / 2 + 180), Color.Black, Global.DegreesToRadians(-2), Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, Global.hs[2], new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 200, Global.vpheight / 2 - HighscoreList.Height / 2 + 180), Color.Black, Global.DegreesToRadians(-2), Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, Global.hs[5], new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 100, Global.vpheight / 2 - HighscoreList.Height / 2 + 230), Color.Black, Global.DegreesToRadians(-2), Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, Global.hs[4], new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 200, Global.vpheight / 2 - HighscoreList.Height / 2 + 230), Color.Black, Global.DegreesToRadians(-2), Vector2.Zero, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "Puntos"    , new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 100, Global.vpheight / 2 - HighscoreList.Height / 2 + 280), Color.Black, Global.DegreesToRadians(2), Vector2.Zero, 1.25f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "Aeronaves" , new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 200, Global.vpheight / 2 - HighscoreList.Height / 2 + 280), Color.Black, Global.DegreesToRadians(2), Vector2.Zero, 1.25f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "Puntos", new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 100 - 1, Global.vpheight / 2 - HighscoreList.Height / 2 + 280 + 1), Color.White, Global.DegreesToRadians(2), Vector2.Zero, 1.25f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "Aeronaves", new Vector2(Global.vpwidth / 2 - HighscoreList.Width / 2 + 200 - 1, Global.vpheight / 2 - HighscoreList.Height / 2 + 280 + 1), Color.White, Global.DegreesToRadians(2), Vector2.Zero, 1.25f, SpriteEffects.None, 1);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawInstrucciones(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backInicio1, new Vector2((float)Global.vpwidth, 0), null, Color.White, 0, new Vector2((float)backInicio1.Width, 0), new Vector2(((Global.vpwidth * 100.0f) / backInicio1.Width) / 100.0f, ((Global.vpheight * 100.0f) / backInicio1.Height) / 100.0f), SpriteEffects.None, 0);
            spriteBatch.Draw(backInicio3, new Vector2((float)Global.vpwidth / 2, 20), null, Color.White, 0, new Vector2((float)backInicio2.Width / 2, 0), 0.125f, SpriteEffects.None, 0); //Insigna
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 60), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1); //Hangar 1
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 75 + backInicio6.Height), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1); //Hangar 2
            //Botones
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Draw(spriteBatch, Color.Orange);
                }
            }

            //Tutorial - start
            //Pistas
            foreach (Pista element in Global.pistas) //Se renderizan las pistas de aterrizaje
            {
                if (element != null)
                    element.render();
            }
            //Number pistas
            spriteBatch.DrawString(sf, "1", Global.pistas[0].position, Color.White, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[0].position.X - 1, Global.pistas[0].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[1].position, Color.White, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[1].position.X - 1, Global.pistas[1].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", Global.pistas[2].position, Color.White, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[2].position.X - 1, Global.pistas[2].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[3].position, Color.White, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[3].position.X - 1, Global.pistas[3].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 1.0f, SpriteEffects.None, 1);
            //Flyers
            foreach (Flyer element in Global.flyers) //Se renderizan las aeronaves
            {
                element.render(spriteBatch);
            }
            //Recognizer
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(0, 40), Color.White); //Se renderiza el comando reconocido
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(-1, 41), Color.Orange); //Color
            //Instruction Text
            spriteBatch.DrawString(sfBIG, instText, new Vector2(Global.vpwidth / 2 - sfBIG.MeasureString(instText).X / 2, 45), Color.White);
            spriteBatch.DrawString(sfBIG, instText, new Vector2(Global.vpwidth / 2 - sfBIG.MeasureString(instText).X / 2 - 1, 46), Color.White);
            //Tutorial - end

            /*Texture2D inst; -OLD
            if (Global.idioma) //Mostar en idioma correcto
            {
                inst = this.Content.Load<Texture2D>("Imagenes/InstESP");
            }
            else
            {
                inst = this.Content.Load<Texture2D>("Imagenes/InstENG");
            }
            spriteBatch.Draw(inst, new Vector2(Global.vpwidth / 2, Global.vpheight / 2), null, Color.White, 0, new Vector2(inst.Width / 2, inst.Height / 2), 0.4f, SpriteEffects.None, 1);*/
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawPausa(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backInicio1, new Vector2((float)Global.vpwidth, 0), null, Color.White, 0, new Vector2((float)backInicio1.Width, 0), new Vector2(((Global.vpwidth * 100.0f) / backInicio1.Width) / 100.0f, ((Global.vpheight * 100.0f) / backInicio1.Height) / 100.0f), SpriteEffects.None, 0);
            spriteBatch.Draw(backInicio3, new Vector2((float)Global.vpwidth / 2, 20), null, Color.White, 0, new Vector2((float)backInicio2.Width / 2, 0), 0.125f, SpriteEffects.None, 0); //Insigna
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 60), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0); //Hangar 1
            spriteBatch.Draw(backInicio6, new Vector2((((float)Global.vpwidth / 2) + backInicio3.Width / 2 + backInicio4.Width + 50), 75 + backInicio6.Height), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0); //Hangar 2
            //btnPausa
            foreach (Button element in Global.botones)
            {
                if (element.present)
                {
                    element.Draw(spriteBatch, Color.Green);
                }
            }
            //Pistas
            foreach (Pista element in Global.pistas) //AUNQUE SOLO SE ACTUALIZE EL BOTON DE PAUSA, TAMBIEN SE RENDERIZAN LAS PISTAS
            {
                if (element != null)
                    element.render();
            }
            //Number pistas
            spriteBatch.DrawString(sf, "1", Global.pistas[0].position, Color.White, 0, sf.MeasureString("1") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[0].position.X - 1, Global.pistas[0].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[1].position, Color.White, 0, sf.MeasureString("2") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[1].position.X - 1, Global.pistas[1].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", Global.pistas[2].position, Color.White, 0, sf.MeasureString("1") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "1", new Vector2(Global.pistas[2].position.X - 1, Global.pistas[2].position.Y - 1), Color.Red, 0, sf.MeasureString("1") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", Global.pistas[3].position, Color.White, 0, sf.MeasureString("2") / 2, 2.0f, SpriteEffects.None, 1);
            spriteBatch.DrawString(sf, "2", new Vector2(Global.pistas[3].position.X - 1, Global.pistas[3].position.Y - 1), Color.Red, 0, sf.MeasureString("2") / 2, 2.0f, SpriteEffects.None, 1);
            //Flyers
            foreach (Flyer element in Global.flyers) //AUNQUE SOLO SE ACTUALIZE EL BOTON DE PAUSA, TAMBIEN SE RENDERIZAN LAS AERONAVES
            {
                element.render(spriteBatch);
            }
            //Recognizer
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(-1, 41), Color.Orange);
            //PUNTOS
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), Vector2.Zero, Color.White);
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), new Vector2(-1, 1), Color.Orange);
            //ATERRIZADOS
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(0, 60), Color.White);
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(-1, 61), Color.Orange);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawNASA(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            //Fondo
            spriteBatch.Draw(backInicio1, new Vector2((float)Global.vpwidth, 0), null, Color.White, 0, new Vector2((float)backInicio1.Width, 0), new Vector2(((Global.vpwidth * 100.0f) / backInicio1.Width) / 100.0f, ((Global.vpheight * 100.0f) / backInicio1.Height) / 100.0f), SpriteEffects.None, 0);
            spriteBatch.Draw(backNASA, new Vector2(Global.vpwidth / 2, Global.vpheight / 2), null, Color.White, 0, new Vector2(backNASA.Width / 2, backNASA.Height / 2), 1.0f, SpriteEffects.None, 0); //LogoNASA
            
            //Recognizer
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(sf, "Comando: " + recognized, new Vector2(-1, 41), Color.Orange);
            //PUNTOS
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), Vector2.Zero, Color.White);
            spriteBatch.DrawString(sf, "Puntos: " + (Global.puntaje * 10).ToString(), new Vector2(-1, 0), Color.Orange);
            //ATERRIZADOS
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(0, 60), Color.White);
            spriteBatch.DrawString(sf, "Aterrizados: " + Global.landCount.ToString(), new Vector2(-1, 61), Color.Orange);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadRecognizer(string[] palabras)
        {
            sr = new SpeechRecognizer();
            //Se elije el nombre de la lista de palabras
            Choices colores = new Choices();
            //Se agregan las palabras a la lista
            colores.Add(new string[] { palabras[4], palabras[5], palabras[6], palabras[7], palabras[8], palabras[9], palabras[10], palabras[11] });
            //Se crea un nuevo GrammarBuilder
            GrammarBuilder gb = new GrammarBuilder();
            //Se le agrega la lista
            gb.Append(colores);
            Grammar g = new Grammar(gb);
            //Se coloca el GrammarBuilder dentro de un objeto de gramatica (grammar)
            sr.LoadGrammar(g);

            Choices acciones = new Choices();
            acciones.Add(new string[] { palabras[12], palabras[13], palabras[14], palabras[15], palabras[20] });
            GrammarBuilder gb1 = new GrammarBuilder();
            gb1.Append(acciones);
            Grammar g1 = new Grammar(gb1);
            sr.LoadGrammar(g1);

            Choices objetos = new Choices();
            objetos.Add(new string[] { palabras[0], palabras[1], palabras[2], palabras[3], palabras[16], palabras[17], palabras[18], palabras[19] });
            GrammarBuilder gb3 = new GrammarBuilder();
            gb3.Append(objetos);
            Grammar g3 = new Grammar(gb3);
            sr.LoadGrammar(g3);
            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
        }

        void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            recognized = e.Result.Text;
            if (Global.idioma)
            {
                IdentificarComando(recognized, Global.esp); //Se llama al interpretador de comandos en español
            }
            else
            {
                IdentificarComando(recognized, Global.eng); //Se llama al interpretador de comandos en ingles
            }
        }

        private void IdentificarComando(string recognized, string[] palabras)
        {
            if (recognized == palabras[20]) /*Cancel/Cancelar*/
            {
                Global.recognizedCmd[0] = null;
                Global.recognizedCmd[1] = null;
                Global.recognizedCmd[2] = null;
                Global.recognizedCmd[3] = null;
            }
            else
            {
                string[] flyrsS = new string[4] { palabras[00] /*PlaneAvion*/, palabras[01] /*ChopperHelicoptero*/, palabras[02] /*MilitarMilitar*/, palabras[03] /*UFOOVNI*/ };
                string[] colorS = new string[8] { palabras[04] /*YellowAmarillo*/, palabras[05] /*BlueAzul*/, palabras[06] /*WhiteBlanco*/, palabras[07] /*OrangeNaranja*/, 
                                                  palabras[08] /*BlackNegro*/,     palabras[09] /*RedRojo*/,  palabras[10] /*GreenVerde*/,  palabras[11] /*Purple/Violeta*/ };
                string[] comandS = new string[4] { palabras[12] /*LandAterrizar*/, palabras[13] /*RightDerecha*/, palabras[14] /*LeftIzquierda*/, palabras[15] /*WaitEsperar*/ };
                string[] pistasS = new string[4] { palabras[16] /*Airstrip OnePista Uno*/, palabras[17] /*Airstrip Two/Pista Dos*/, palabras[18] /*Port One/Helipuerto Uno*/, palabras[19] /*Port Two/Helipuerto Dos*/ };
                bool found = false;
                int i = 0;
                while (!found && i < flyrsS.Length) //Se busca si la palabra es un Objeto Volador
                {
                    if (recognized == flyrsS[i])
                    {
                        found = true;
                    }
                    else
                    {
                        i++;
                    }
                }
                if (found)
                {
                    //Se agrega aun array que guarda los comandos reconocidos para ser ejecutados luego
                    Global.recognizedCmd[0] = flyrsS[i];
                    Global.recognizedCmd[1] = null;
                    Global.recognizedCmd[2] = null;
                    Global.recognizedCmd[3] = null;
                    if (flyrsS[i] == palabras[3]) //OVNI, en este caso el espacio del color no es necesario. Por esto se le asigna automaticamente el Amarillo (default)
                    {
                        Global.recognizedCmd[1] = palabras[4];
                    }
                }
                else
                {
                    i = 0;
                    while (!found && i < colorS.Length) //Se busca si la palabra es un Color
                    {
                        if (recognized == colorS[i])
                        {
                            found = true;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    if (found)
                    {
                        //Se agrega aun array que guarda los comandos reconocidos para ser ejecutados luego
                        Global.recognizedCmd[1] = colorS[i];
                    }
                    else
                    {
                        i = 0;
                        while (!found && i < comandS.Length) //Se busca si la palabra es uno de los comandos
                        {
                            if (recognized == comandS[i])
                            {
                                found = true;
                                if (recognized != palabras[12])
                                {
                                    Global.recognizedCmd[3] = "";
                                }
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (found)
                        {
                            //Se agrega aun array que guarda los comandos reconocidos para ser ejecutados luego
                            Global.recognizedCmd[2] = comandS[i];
                        }
                        else
                        {
                            i = 0;
                            while (!found && i < pistasS.Length) //Se busca si la palabra es una de las pista de aterrizaje
                            {
                                if (recognized == pistasS[i])
                                {
                                    found = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                            if (found)
                            {
                                //Se agrega aun array que guarda los comandos reconocidos para ser ejecutados luego
                                Global.recognizedCmd[3] = pistasS[i];
                            }
                        }
                    }
                }
            }
            Console.WriteLine(recognized);
            Console.WriteLine(Global.recognizedCmd[0] + " " + Global.recognizedCmd[1] + " " + Global.recognizedCmd[2] + " " + Global.recognizedCmd[3] + ".");
        }
    }
}
