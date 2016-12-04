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
    class Global : Game
    {
        //Aviones
        public static List<Flyer> flyers = new List<Flyer>(20);
        //Pistas
        public static List<Pista> pistas = new List<Pista>(4);
        //Botones
        public static Button[] botones = new Button[8];

        //Cuenta aterrizajes
        public static int landCount, //Todos
                          landCountOVNI, //OVNIS
                          landCountAvion, //Aviones
                          landCountCopter, //Helicopteros
                          landCountMilitar; //Aviones Militares
        
        //Puntaje
        public static float puntaje = 0; //Puntaje
        
        //Highscores
        public static string[] hs = new string[6];
        public static float hsPuntaje, hsAterrizados;
        
        //NASA
        public static float NASAtimer = 5.0f;
        public static bool ovni = false;
        public static int ufo = 10, ovniC = 0;
        
        //Control del Juego
        public static float gameSpeed, timerNewFlyer = 3.0f;
        public static uint pistaassignloopcount = 0;
        
        //Aleatorio
        public static Random random = new Random(); //Random OBJECT, es necesario usar el mismo en todo el proyecto. Si existen 2 que se crearon muy cerca devuelven el mismo numero.
        
        //Colores
        public static Color[] colorsFly = new Color[8];
        
        //Control de Pantallas
        public static GameState gameState = GameState.Menu;
        public enum GameState
        {
            Menu,
            Highscores,
            Jugando,
            Pausa,
            GameOver,
            Instrucciones,
            NASA
        }

        //Gameover
        public static Vector2 fly1pos, fly2pos;
        public static Color fly1colour, fly2colour;
        public static float fly1dir, fly2dir;
        public static Texture2D fly1text, fly2text;
        public static List<Part> partes = new List<Part>();

        //Audio
        public static bool musicmute = false, soundmute = false;
        public static SoundEffect newplane, newchopper, newmilitar, newovni, land, crash, alarm, tardis, buttonpress, success, backmusic;
        public static SoundEffectInstance[] SEInstances = new SoundEffectInstance[10];

        //Multi-idioma
        public static bool idioma = true;
        public static string[] eng = new string[27]
        {
            "Plane",           //00
            "Chopper",         //01
            "Militar",         //02
            "UFO",             //03
            "Yellow",          //04
            "Blue",            //05
            "White",           //06
            "Orange",          //07
            "Black",           //08
            "Red",             //09
            "Green",           //10
            "Purple",          //11
            "Land",            //12
            "Right",           //13
            "Left",            //14
            "Wait",            //15
            "Airstrip One",    //16
            "Airstrip Two",    //17
            "Port One",        //18
            "Port Two",        //19
            "Cancel",          //20
            "Back",            //21
            "Play",            //22
            "Exit",            //23
            "Instructions",    //24
            "Highscores",      //25
            "Settings"         //26
        };
        public static string[] esp = new string[27]
        {
            "Avion",           //00
            "Helicoptero",     //01
            "Militar",         //02
            "OVNI",            //03
            "Amarillo",        //04
            "Azul",            //05
            "Blanco",          //06
            "Naranja",         //07
            "Negro",           //08
            "Rojo",            //09
            "Verde",           //10
            "Violeta",         //11
            "Aterrizar",       //12
            "Derecha",         //13
            "Izquierda",       //14
            "Esperar",         //15
            "Pista Uno",       //16
            "Pista Dos",       //17
            "Helipuerto Uno",  //18
            "Helipuerto Dos",  //19
            "Cancelar",        //20
            "Atras",           //21
            "Jugar",           //22
            "Salir",           //23
            "Instrucciones",   //24
            "Puntuaciones",    //25
            "Configuraciones"  //26

        };

        public static bool reload = false;
        public static string[] recognizedCmd = new string[4];


        //Pantalla
        public static int vpwidth = 800;
        public static int vpheight = 600;

        //Funciones Generales
        public static float DegreesToRadians(float degrees)
        {
            if (degrees < 0)
            {
                degrees += 360;
            }
            else
            {
                if (degrees >= 360)
                {
                    degrees -= 360;
                }
            }
            float radians = degrees * 0.0174532925f;
            return radians;
            //return (float)Math.PI * degrees / 180.0f;
        }

        public static float RadiansToDegrees(float radians)
        {
            float degrees = radians / 0.0174532925f;
            if (degrees < 0)
            {
                degrees += 360;
            }
            else
            {
                if (degrees > 360)
                {
                    degrees -= 360;
                }
            }
            return degrees;
            //return radians * (float)(180.0f / Math.PI);
        }


        //Colores Flyers
        //0: Amarillo
        //1: Azul
        //2: Blanco
        //3: Naranja
        //4: Negro
        //5: Rojo
        //6: Verde
        //7: Violeta
        public static int ColorTypeToSubindex(Flyer.myColor color)
        {
            switch (color)
            {
                case Flyer.myColor.Amarillo:
                    return 0;
                case Flyer.myColor.Azul:
                    return 1;
                case Flyer.myColor.Blanco:
                    return 2;
                case Flyer.myColor.Naranja:
                    return 3;
                case Flyer.myColor.Negro:
                    return 4;
                case Flyer.myColor.Rojo:
                    return 5;
                case Flyer.myColor.Verde:
                    return 6;
                case Flyer.myColor.Violeta:
                    return 7;
                default:
                    throw new Exception("Error 14751, el color no existe en el vector/array");
            }
        }

        public static Color SubindexToColor(int index)
        {
            return colorsFly[index];
        }

        public static Flyer.myColor SubindexToMyColor(int index)
        {
            switch (index)
            {
                case 0:
                    return Flyer.myColor.Amarillo;
                case 1:
                    return Flyer.myColor.Azul;
                case 2:
                    return Flyer.myColor.Blanco;
                case 3:
                    return Flyer.myColor.Naranja;
                case 4:
                    return Flyer.myColor.Negro;
                case 5:
                    return Flyer.myColor.Rojo;
                case 6:
                    return Flyer.myColor.Verde;
                case 7:
                    return Flyer.myColor.Violeta;
                default:
                    throw new Exception("Error 14751, el color no existe en el vector/array");
            }
        }

        public bool checkExitKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            // Check to see whether ESC was pressed on the keyboard 
            // or BACK was pressed on the controller.
            if (keyboardState.IsKeyDown(Keys.Escape) && keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.LeftAlt))
            {
                Exit();
                return true;
            }
            return false;
        }

        public static void ClearForPlay(SpriteBatch sb, Game1 game, GraphicsDevice gd, bool goToMenu)
        {
            Console.WriteLine("Cleared for Play");
            Global.puntaje = 0;
            Global.gameSpeed = 0.25f;
            Global.pistas.Clear();
            Pista.posiblePos = new Vector2[4]
            {
                new Vector2(Global.vpwidth / 3, Global.vpheight / 3), //Izq Arriba
                new Vector2((Global.vpwidth / 3) * 2, Global.vpheight / 3), //Der Arriba
                new Vector2((Global.vpwidth / 3) * 2, (Global.vpheight / 3) * 2), //Der Abajo
                new Vector2(Global.vpwidth / 3, (Global.vpheight / 3) * 2), //Izq Abajo
                //new Vector2(Global.vpwidth / 2, Global.vpheight / 2) //Centro
            };
            Global.pistas.Add(new Pista(game, gd, true, sb));
            Global.pistas.Add(new Pista(game, gd, true, sb));
            Global.pistas.Add(new Pista(game, gd, false, sb));
            Global.pistas.Add(new Pista(game, gd, false, sb));
            Global.flyers.Clear();
            if (goToMenu)
                Global.gameState = Global.GameState.Menu;
            Global.botones[0].present = true; //Salir
            Global.botones[1].present = true; //Highscores
            Global.botones[2].present = true; //Jugar
            Global.botones[3].present = false;  //Pausa
            Global.botones[4].present = true; //Idioma
            Global.botones[5].present = true; //Instrucciones
            Global.landCount = 0;
            Global.landCountAvion = 0;
            Global.landCountCopter = 0;
            Global.landCountMilitar = 0;
            Global.landCountOVNI = 0;
            Global.ufo = 10;
            Global.hs = new string[6];
            Global.hsAterrizados = 0;
            Global.hsPuntaje = 0;
            Global.NASAtimer = 2.5f;
            Global.ovni = false;
            Global.ovniC = 0;
            Global.pistaassignloopcount = 0;
            Global.recognizedCmd[0] = null;
            Global.recognizedCmd[1] = null;
            Global.recognizedCmd[2] = null;
            Global.recognizedCmd[3] = null;
            Global.timerNewFlyer = 3.0f;
        }

        public static Vector2 NewPos(Vector2 pos, float vel, float dir)
        {
            float mX = (float)Math.Sin((double)Global.DegreesToRadians(dir));
            float mY = -(float)Math.Cos((double)Global.DegreesToRadians(dir));
            pos.X += vel * mX * Global.gameSpeed;
            pos.Y += vel * mY * Global.gameSpeed;
            return pos;
        }
    }
}
