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
    class Flyer
    {
        public bool exists, landing, touchDown, moving, remove = false, girado = false, landed = false, girando = false, down = false;
        private int assignedPista = -1, punt = 100;
        private const int vAvion = 4, vCopter = 2, vOVNI = 10, vMilitar = 6;
        public float direccion, velocidad, scale = 1.0f, turningDir;
        private float vInicial;
        public Vector2 Position;
        public Tipo tipo;
        public myColor mycolor;
        public Color flyrColor;
        public Texture2D tFlyer, tFlyerT;
        public List<Part> partesFlyer = new List<Part>();

        //static
        private static int loopcount = 0;

        //Tipos
        public enum Tipo
        {
            Avion,
            Helicoptero,
            Militar,
            OVNI,
            nulo
        }
        //Colores
        public enum myColor
        {
            Azul,
            Rojo,
            Amarillo,
            Verde,
            Naranja,
            Violeta,
            Negro,
            Blanco,
            nulo
        }

        //Constructores
        public Flyer()
        {
        }

        public Flyer(int heightVP, int widthVP, Game1 game)
        {
            this.exists = true; //La aeronave esta activa
            this.moving = true; //La aeronave esta en movimiento
            this.tipo = GetNewFlyerTipo(OVNI(Global.landCount)); //Se le asigna un TIPO a la aeronave
            if (this.tipo == Tipo.OVNI)
            {
                this.mycolor = myColor.Amarillo;
            }
            else
            {
                try
                {
                    this.mycolor = GetNewFlyerColor(this.tipo); //Se le asigna un COLOR a la aeronave
                }
                catch (Exception e)
                {
                    this.mycolor = myColor.nulo;
                    while (this.mycolor == myColor.nulo) //Si todos los colores de ese tipo estan "tomado" se le asigna un nuevo tipo
                    {
                        this.tipo = GetNewFlyerTipo(OVNI(Global.landCount));
                        try
                        {
                            this.mycolor = GetNewFlyerColor(this.tipo); //Se le asigna un COLOR a la aeronave
                        }
                        catch (Exception ex)
                        {
                            this.mycolor = myColor.nulo;
                        }
                    }
                }
            }
            switch (this.tipo) //Dependiendo del tipo:
            {
                case Tipo.Avion:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)]; //Se le da un Color de Filtro
                    this.tipo = Tipo.Avion; //Se guarda el tipo
                    this.velocidad = vAvion; //Se le asigna una velocidad
                    this.vInicial = vAvion; //Y una velocidad inicial (para resetear la aeronave)
                    this.punt = 125; //Se le asigna un puntaje
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/AvionGeneral0"); //Una textura para la posicion 1
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/AvionGeneral1"); //Una textura para la posicion 2
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoCabAvion"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoColaAvion"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoAlaAvion"), this.flyrColor, this.Position));
                    break;
                case Tipo.Helicoptero:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)];
                    this.tipo = Tipo.Helicoptero;
                    this.velocidad = vCopter;
                    this.vInicial = vCopter;
                    this.punt = 75;
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/CopterGeneral0");
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/CopterGeneral1");
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoCabCopter"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoColaCopter"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoHelice"), this.flyrColor, this.Position));
                    break;
                case Tipo.Militar:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)];
                    this.tipo = Tipo.Militar;
                    this.velocidad = vMilitar;
                    this.vInicial = vMilitar;
                    this.punt = 150;
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/MilitarGeneral0");
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/MilitarGeneral1");
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoCabMilitar"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoAlaMilitar"), this.flyrColor, this.Position));
                    this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoAlaMilitar"), this.flyrColor, this.Position));
                    break;
                case Tipo.OVNI:
                    this.flyrColor = Color.White;
                    this.tipo = Tipo.OVNI;
                    this.velocidad = vOVNI;
                    this.vInicial = vOVNI;
                    this.punt = 250;
                    if (Global.landCountOVNI != 4)
                    {
                        this.tFlyer = game.Content.Load<Texture2D>("Imagenes/OVNIGeneral0");
                        this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/OVNIGeneral1");
                        this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoOVNI"), this.flyrColor, this.Position));
                        this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/pedazoOVNI"), this.flyrColor, this.Position));
                    }
                    else
                    {
                        this.tFlyer = game.Content.Load<Texture2D>("Imagenes/TARDISEasterEgg");
                        this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/TARDISEasterEgg");
                        this.partesFlyer.Add(new Part(game.Content.Load<Texture2D>("Imagenes/TARDISEasterEgg"), this.flyrColor, this.Position));
                    }
                    break;
            }
            float x, y;
            this.direccion = SetPositionDireccion(this, heightVP, widthVP, out x, out y); //Se asigna una Posicion y una Direccion
            int tries = 0;
            while (AlarmaChoque(this) && tries < 50) //Si la aeronave va a aparecer cerca de otra, se asigna una nueva posicion y direccion
            {
                tries++;
                this.direccion = SetPositionDireccion(this, heightVP, widthVP, out x, out y);
            }
            this.Position.X = x; //Se guarda la posicion X
            this.Position.Y = y; //Se guarda la posicion Y
            this.turningDir = this.direccion;
            switch (this.tipo)
            {
                case Tipo.Avion:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[1].Play();
                    }
                    else
                    {
                        Global.SEInstances[1].Stop();
                    }
                    break;
                case Tipo.Helicoptero:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[2].Play();
                    }
                    else
                    {
                        Global.SEInstances[2].Stop();
                    }
                    break;
                case Tipo.Militar:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[3].Play();
                    }
                    else
                    {
                        Global.SEInstances[3].Stop();
                    }
                    break;
                case Tipo.OVNI:
                    if (Global.landCountOVNI != 4)
                    {
                        if (!Global.soundmute)
                        {
                            Global.SEInstances[4].Play();
                        }
                        else
                        {
                            Global.SEInstances[4].Stop();
                        }
                    }
                    else
                    {
                        if (!Global.soundmute)
                        {
                            Global.SEInstances[7].Play();
                        }
                        else
                        {
                            Global.SEInstances[7].Stop();
                        }
                    }
                    break;
            }
            Global.flyers.Add(this); //Se agrega a la lista con todas las aeronaves
        }

        public Flyer(int heightVP, int widthVP, Game1 game, string position, Tipo typeIN, myColor colorIN) //Used in Tutorial
        {
            this.exists = true; //La aeronave esta activa
            this.moving = false; //La aeronave esta en movimiento
            this.tipo = typeIN; //Se le asigna un TIPO a la aeronave
            this.mycolor = colorIN;
            switch (this.tipo) //Dependiendo del tipo:
            {
                case Tipo.Avion:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)]; //Se le da un Color de Filtro
                    this.tipo = Tipo.Avion; //Se guarda el tipo
                    this.velocidad = 0.0f; //Se le asigna una velocidad
                    this.vInicial = 0.0f; //Y una velocidad inicial (para resetear la aeronave)
                    this.punt = 0; //Se le asigna un puntaje
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/AvionGeneral0"); //Una textura para la posicion 1
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/AvionGeneral1"); //Una textura para la posicion 2
                    break;
                case Tipo.Helicoptero:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)];
                    this.tipo = Tipo.Helicoptero;
                    this.velocidad = 0.0f;
                    this.vInicial = 0.0f;
                    this.punt = 0;
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/CopterGeneral0");
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/CopterGeneral1");
                    break;
                case Tipo.Militar:
                    this.flyrColor = Global.colorsFly[Global.ColorTypeToSubindex(this.mycolor)];
                    this.tipo = Tipo.Militar;
                    this.velocidad = 0.0f;
                    this.vInicial = 0.0f;
                    this.punt = 0;
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/MilitarGeneral0");
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/MilitarGeneral1");
                    break;
                case Tipo.OVNI:
                    this.flyrColor = Color.White;
                    this.tipo = Tipo.OVNI;
                    this.velocidad = 0.0f;
                    this.vInicial = 0.0f;
                    this.punt = 0;
                    this.tFlyer = game.Content.Load<Texture2D>("Imagenes/OVNIGeneral0");
                    this.tFlyerT = game.Content.Load<Texture2D>("Imagenes/OVNIGeneral1");
                    break;
            }
            this.direccion = 0.0f; //Se asigna una Posicion y una Direccion
            this.Position.X = heightVP / 2; //Se guarda la posicion X
            this.Position.Y = widthVP  / 2; //Se guarda la posicion Y
            this.turningDir = this.direccion;
            switch (this.tipo)
            {
                case Tipo.Avion:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[1].Play();
                    }
                    else
                    {
                        Global.SEInstances[1].Stop();
                    }
                    break;
                case Tipo.Helicoptero:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[2].Play();
                    }
                    else
                    {
                        Global.SEInstances[2].Stop();
                    }
                    break;
                case Tipo.Militar:
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[3].Play();
                    }
                    else
                    {
                        Global.SEInstances[3].Stop();
                    }
                    break;
                case Tipo.OVNI:
                    if (Global.landCountOVNI != 4)
                    {
                        if (!Global.soundmute)
                        {
                            Global.SEInstances[4].Play();
                        }
                        else
                        {
                            Global.SEInstances[4].Stop();
                        }
                    }
                    else
                    {
                        if (!Global.soundmute)
                        {
                            Global.SEInstances[7].Play();
                        }
                        else
                        {
                            Global.SEInstances[7].Stop();
                        }
                    }
                    break;
            }
            Global.flyers.Add(this); //Se agrega a la lista con todas las aeronaves
        }


        //Extras
        public myColor GetNewFlyerColor(Tipo tipo)
        {
            int i = Global.random.Next(0, 8);
            if (!Global.flyers.Exists(x => x.tipo == tipo && x.mycolor == Global.SubindexToMyColor(i)))
            {
                return Global.SubindexToMyColor(i);
            }
            else if (loopcount > 20)
            {
                loopcount++;
                return GetNewFlyerColor(tipo);
            }
            else
            {
                throw new Exception();
            }
            /*bool full = false;
            bool[] type = new bool[8];
            foreach (Flyer element in Global.flyers)
            {
                if (element != null && element.tipo == tipo)
                {
                    switch (element.mycolor)
                    {
                        //0: Amarillo
                        case myColor.Amarillo:
                            type[0] = true;
                            break;
                        //1: Azul
                        case myColor.Azul:
                            type[1] = true;
                            break;
                        //2: Blanco
                        case myColor.Blanco:
                            type[2] = true;
                            break;
                        //3: Naranja
                        case myColor.Naranja:
                            type[3] = true;
                            break;
                        //4: Negro
                        case myColor.Negro:
                            type[4] = true;
                            break;
                        //5: Rojo
                        case myColor.Rojo:
                            type[5] = true;
                            break;
                        //6: Verde
                        case myColor.Verde:
                            type[6] = true;
                            break;
                        //7: Violeta
                        case myColor.Violeta:
                            type[7] = true;
                            break;
                    }
                }
            }
            int i = 0;
            foreach (bool element in type)
            {
                if (!element)
                {
                    full = false;
                    break;
                }
                i++;
            }
            if (!full)
            {
                if (i >= 8)
                {
                    return myColor.nulo;
                }
                return Global.SubindexToMyColor(i);
            }
            else
            {
                throw new Exception("Error, all colors");
            }*/
        } //Se asigna el color en orden

        public Tipo GetNewFlyerTipo(bool ovniSINO)
        {
            int ran = Global.random.Next(0, 101);
            //Condicional para decidir si entra o no el OVNI
            if (ovniSINO == false)
            {
                if (ran <= 20)
                {
                    return Tipo.Militar;
                }
                else
                {
                    if (ran > 20 && ran <= 60)
                    {
                        return Tipo.Avion;
                    }
                    else
                    {
                        return Tipo.Helicoptero;
                    }
                }
            }
            else
            {
                return Tipo.OVNI;
            }
        } //Se asigna el tipo segun probabilidades

        public bool OVNI(int landed)
        {
            if (Global.ovni)
            {
                Global.ovni = false;
                Global.ufo += 5;
                Global.ovniC = 0;
                return true;
            }
            else
            {
                return false;
            }
        } //Se decide si el tipo va a ser un OVNI (aterrizados = multipos de 5, excepto 1 y 5 Y excepto que todavia haya un ovni en el juego)

        public float SetPositionDireccion(Flyer flyobj, int heightVP, int widthVP, out float x, out float y)
        {
            switch (Global.random.Next(0, 4))
            {
                //se suma 1 porque el random no incluye el fin del intervalo //se resta el tamaño del sprite para que tarde en entrar al viewport
                case 0:
                    //Borde Izquierda //Cualquier punto en Y
                    x = 0;
                    y = Global.random.Next(0, heightVP + 1);    
                    //Cualquier direccion que vaya hacia: derecha es decir no directo al borde. +30 -30 para que no avanze paralelo al borde
                    return Global.random.Next(30, 151);
                case 1:
                    //Borde Derecha //Cualquier punto en Y
                    x = widthVP;
                    y = Global.random.Next(0, heightVP + 1);     
                    //Cualquier direccion que vaya hacia: izquierda es decir no directo al borde. +30 -30 para que no avanze paralelo al borde
                    return Global.random.Next(210, 331);
                case 2:
                    //Borde Arriba //Cualquier punto en X
                    x = Global.random.Next(0, widthVP + 1);
                    y = 0;
                    //Cualquier direccion que vaya hacia: abajo es decir no directo al borde. +30 -30 para que no avanze paralelo al borde
                    return Global.random.Next(120, 241);
                case 3:
                    //Borde Abajo //Cualquier punto en X
                    x = Global.random.Next(0, widthVP + 1);
                    y = heightVP;
                    //Cualquier direccion que vaya hacia: arriba es decir no directo al borde. +30 -30 para que no avanze paralelo al borde
                    if (Global.random.Next(0, 2) == 0)
                    {
                        return Global.random.Next(0, 61);
                    }
                    else
                    {
                        return Global.random.Next(300, 361);
                    }
                default:
                    x = 0;
                    y = 0;
                    return 135;
            }
        }

        
        //Update
        public static bool Update(GraphicsDevice graphicsD, int index, Flyer element, out int indexToRemove)
        {
            bool gameover = false;
            if (element.exists) //Si el elemento esta activo
            {
                if (element.moving || element.landing) //Solo si se encuentra en movimiento o aterrizando
                {
                    element.Mover(graphicsD); //Se mueve, en la direccion que esta yendo * su velocidad
                    element.Landed(index); //Se le da una nueva direccion hacia la pista/Se pregunta si ya llego a la pista/Se pregunta si llego al final de la pista
                    if (element.down && element.touchDown && element.scale > 0.40f)
                    {
                        element.scale -= 0.0030f; //Se achica el tamaño de la aeronave
                    }
                }
                else
                {
                    element.Girar(graphicsD); //Estado de "ESPERA", las aeronaves esperan girando en circulo
                }
            }
            //ALARMA DE CHOQUE
            if (element.AlarmaChoque(element)) //La alarma de choque es un "beep" que suena cuando dos aeronaves se encuentran muy cercanas.
            {
                //Funciona de la misma forma que las coliciones, pero con un rectangulo mayor
                //Console.Beep(1000, 10);
                if (!Global.soundmute)
                {
                    Global.SEInstances[0].Play();
                }
                else
                {
                    Global.SEInstances[0].Stop();
                }
            }
            //Colisiones
            Rectangle cuadradoFlyer = new Rectangle((int)element.Position.X - element.tFlyer.Width / 2,  //Se debe restar la mitad del tamaño de la nave
                                                    (int)element.Position.Y - element.tFlyer.Height / 2, //para lograr la correcta ubicacion del rectangulo
                                                    (int)element.tFlyer.Width,
                                                    (int)element.tFlyer.Height); //Rectangulo de la aeronave A
            foreach (Flyer elementB in Global.flyers)
            {
                if ((element != elementB)
                    && (!element.landed && !elementB.landed)
                    && ((!element.touchDown && !elementB.touchDown)
                    && ((cuadradoFlyer.Intersects(new Rectangle((int)elementB.Position.X - elementB.tFlyer.Width / 2,  //Se debe restar la mitad del tamaño de la nave
                                                                (int)elementB.Position.Y - elementB.tFlyer.Height / 2, //para lograr la correcta ubicacion del rectangulo
                                                                (int)elementB.tFlyer.Width,
                                                                (int)elementB.tFlyer.Height)))))) //Se pregunta si se intersecta con el rectangulo de la aeronave B
                {
                    gameover = true;
                    Global.partes.Clear();
                    foreach (Part part in element.partesFlyer)
                    {
                        part.PlacePart(element.Position);
                        Global.partes.Add(part);
                    }
                    foreach (Part part in elementB.partesFlyer)
                    {
                        part.PlacePart(elementB.Position);
                        Global.partes.Add(part);
                    }
                    if (!Global.soundmute)
                    {
                        Global.SEInstances[6].Play();
                    }
                    else
                    {
                        Global.SEInstances[6].Stop();
                    }
                    break; //como se modifica la lista es necesario salir del foreach
                }
                else
                {
                    gameover = false;
                }
            }
            if (gameover)
            {
                if (element.remove)
                    indexToRemove = index; //Si una aeronave aterrizo de guarda su index en la lista para que sea eliminado de esta.
                else
                    indexToRemove = 100;
                return true;
            }
            else
            {
                if (element.remove)
                    indexToRemove = index;
                else
                    indexToRemove = 100;
                return false;
            }
        }

        public void Mover(GraphicsDevice graphicsD)
        {
            if ((this.tipo == Tipo.Helicoptero || this.tipo == Tipo.OVNI) && this.down == true)
            {
                this.direccion += 0.75f;
            }
            else
            {
                if (this.direccion < this.turningDir - 1)
                {
                    this.direccion += 0.5f;
                }
                else if (this.direccion > this.turningDir + 1)
                {
                    this.direccion -= 0.5f;
                }
                else
                {
                    this.turningDir = this.direccion;
                }
                float mX = (float)Math.Sin((double)Global.DegreesToRadians(this.direccion));
                float mY = -(float)Math.Cos((double)Global.DegreesToRadians(this.direccion));
                this.Position.X += this.velocidad * mX * Global.gameSpeed;
                this.Position.Y += this.velocidad * mY * Global.gameSpeed;
                //Bordes
                //Derecha
                if (this.Position.X > graphicsD.Viewport.Width)
                {
                    this.Position.X = graphicsD.Viewport.Width;
                    if (this.direccion <= 90)
                    {
                        this.direccion += 270;
                        this.turningDir = this.direccion;
                    }
                    else
                    {
                        this.direccion -= 90;
                        this.turningDir = this.direccion;
                    }
                }
                //Izquierda
                if (this.Position.X < 0)
                {
                    this.Position.X = 0;
                    if (this.direccion <= 270)
                    {
                        this.direccion -= 90;
                        this.turningDir = this.direccion;
                    }
                    else
                    {
                        this.direccion -= 270;
                        this.turningDir = this.direccion;
                    }
                }
                //Abajo
                if (this.Position.Y > graphicsD.Viewport.Height)
                {
                    this.Position.Y = graphicsD.Viewport.Height;
                    if (this.direccion <= 180)
                    {
                        this.direccion -= 90;
                        this.turningDir = this.direccion;
                    }
                    else
                    {
                        this.direccion += 90;
                        this.turningDir = this.direccion;
                    }
                }
                //Arriba
                if (this.Position.Y < 0)
                {
                    this.Position.Y = 0;
                    if (this.direccion <= 90)
                    {
                        this.direccion += 90;
                        this.turningDir = this.direccion;
                    }
                    else
                    {
                        this.direccion -= 90;
                    }
                }
            }
        }

        public void Landed(int index)
        {
            if (this.landing)
            {
                if (this.assignedPista < Global.pistas.Count<Pista>() && this.assignedPista != -1)
                {
                    if ((Math.Abs(this.Position.X - Global.pistas[this.assignedPista].position.X) <= 5
                        && Math.Abs(this.Position.Y - Global.pistas[this.assignedPista].position.Y) <= 5)
                        && this.landing
                        && !this.touchDown
                        && (!Global.pistas[this.assignedPista].blocked
                        && !Global.pistas[this.assignedPista].landing))
                    {
                        this.touchDown = true;
                        if (this.scale >= 0.40f)
                            this.down = true;
                        else
                            this.down = false;
                        this.moving = false;
                        if (this.tipo != Tipo.Helicoptero || this.tipo != Tipo.OVNI)
                        {
                            this.direccion = Global.pistas[this.assignedPista].direccion;
                            this.turningDir = this.direccion;
                        }
                        this.velocidad = this.velocidad * 0.25f;
                        Global.pistas[this.assignedPista].landing = true;
                    }
                    if (this.landing && this.touchDown && this.velocidad > 0.01f)
                    {
                        this.velocidad = this.velocidad - 0.0005f;
                    }
                    if ((Math.Abs(this.Position.X - Global.pistas[this.assignedPista].end.X) <= (this.tFlyer.Width / 2)
                        && Math.Abs(this.Position.Y - Global.pistas[this.assignedPista].end.Y) <= (this.tFlyer.Height / 2))
                        && (this.landing && this.touchDown && this.scale <= 0.40f))
                    {
                        this.exists = false;
                        Global.pistas[this.assignedPista].landing = false;
                        this.landing = false;
                        this.touchDown = false;
                        switch (this.tipo)
                        {
                            case Tipo.Avion:
                                Global.landCountAvion++;
                                break;
                            case Tipo.Helicoptero:
                                Global.landCountCopter++;
                                break;
                            case Tipo.Militar:
                                Global.landCountMilitar++;
                                break;
                            case Tipo.OVNI:
                                Global.landCountOVNI++;
                                Global.NASAtimer = 2.5f;
                                Global.gameState = Global.GameState.NASA;
                                break;
                            default:
                                Console.WriteLine("error, flyer sin tipo");
                                break;
                        }
                        Global.landCount++;
                        Global.ovniC++;
                        if (Global.ovniC % Global.ufo == 0)
                        {
                            if (Global.flyers.Find(x => x.tipo == Tipo.OVNI) == null)
                            {
                                Global.ovni = true;
                            }
                        }
                        Global.puntaje += this.punt;
                        this.landed = true;
                        this.remove = true;
                    }
                }
            }
        }

        public void Girar(GraphicsDevice graphicsD)
        {
            girando = true;
            direccion -= 1.0f;
            this.Mover(graphicsD);
        }

        public bool AlarmaChoque(Flyer element)
        {
            bool alarma = false;
            Rectangle cuadradoFlyer = new Rectangle((int)element.Position.X - (int)(element.tFlyer.Width * 1.5f),
                                                        (int)element.Position.Y - (int)(element.tFlyer.Height * 1.5f),
                                                        (int)element.tFlyer.Width * 3,
                                                        (int)element.tFlyer.Height * 3); //Rectangulo mas grande
            foreach (Flyer elementB in Global.flyers)
            {
                if ((element != elementB) 
                    && (!element.landed && !elementB.landed)
                    && ((!element.touchDown && !elementB.touchDown)
                    && ((cuadradoFlyer.Intersects(new Rectangle((int)elementB.Position.X - elementB.tFlyer.Width / 2,
                                                                (int)elementB.Position.Y - elementB.tFlyer.Height / 2,
                                                                (int)elementB.tFlyer.Width,
                                                                (int)elementB.tFlyer.Height)))))) //Si se intersectan se devuelve true
                {
                    alarma = true;
                    break;
                }
                else
                {
                    alarma = false;
                }
            }
            if (alarma)
            {
                return true;
            }
            return false;
        }

        public bool Colisiones(Flyer element)
        {
            Rectangle cuadradoFlyer = new Rectangle((int)element.Position.X - element.tFlyer.Width / 2,  //Se debe restar la mitad del tamaño de la nave
                                                       (int)element.Position.Y - element.tFlyer.Height / 2, //para lograr la correcta ubicacion del rectangulo
                                                       (int)element.tFlyer.Width,
                                                       (int)element.tFlyer.Height); //Rectangulo de la aeronave A
            foreach (Flyer elementB in Global.flyers)
            {
                if ((element != elementB)
                        && (!element.landed && !elementB.landed)
                        && ((!element.touchDown && !elementB.touchDown)
                        && ((cuadradoFlyer.Intersects(new Rectangle((int)elementB.Position.X - elementB.tFlyer.Width / 2,  //Se debe restar la mitad del tamaño de la nave
                                                                    (int)elementB.Position.Y - elementB.tFlyer.Height / 2, //para lograr la correcta ubicacion del rectangulo
                                                                    (int)elementB.tFlyer.Width,
                                                                    (int)elementB.tFlyer.Height))))))
                {
                    //If transparent
                    return true;
                }
            }
            return false;
        }

        //Tutorial - start
        public static bool UpdateOnTutorial(GraphicsDevice graphicsD, Flyer element)
        {
            if (element.direccion < element.turningDir - 1)
            {
                element.direccion += 0.5f;
            }
            else if (element.direccion > element.turningDir + 1)
            {
                element.direccion -= 0.5f;
            }
            else
            {
                element.turningDir = element.direccion;
            }
            return false;
        }
        //Tutorial - end

        //Draw
        public void render(SpriteBatch sb)
        {
            if (this.exists)
            {

                if (this.girado)
                {
                            /*textura*/  /*posicion*/   /*parte*/ /*filtro*/ /*Rotacion*/                              /*Origen (centro)*/                             /*tamaño*/  /*efectos*/ /*Orden en pantalla*/
                    sb.Draw(this.tFlyer, this.Position, null, this.flyrColor, Global.DegreesToRadians(this.direccion), new Vector2(this.tFlyer.Width / 2, this.tFlyer.Height / 2), this.scale, SpriteEffects.None, 1);
                    this.girado = false;
                }
                else
                {
                    sb.Draw(this.tFlyerT, this.Position, null, this.flyrColor, Global.DegreesToRadians(this.direccion), new Vector2(this.tFlyer.Width / 2, this.tFlyer.Height / 2), this.scale, SpriteEffects.None, 1);
                    this.girado = true;
                }
                //DRAW HITBOXES - TEMP
                /*Rectangle cuadradoFlyer = new Rectangle((int)this.Position.X - this.tFlyer.Width / 2,  //Se debe restar la mitad del tamaño de la nave
                                                       (int)this.Position.Y - this.tFlyer.Height / 2, //para lograr la correcta ubicacion del rectangulo
                                                       (int)this.tFlyer.Width,
                                                       (int)this.tFlyer.Height); //Rectangulo de la aeronave A
                sb.Draw(this.tFlyer, cuadradoFlyer, Color.Black);*/
            }
        }

        //Comandos
        public static int GetFlyerIndex(KeyboardState ks)
        {
            Tipo bTipo = Tipo.nulo;
            myColor bColor = myColor.nulo;
            if (ks.GetPressedKeys().Length == 3)
            {
                if (ks.IsKeyDown(Keys.A))
                    bTipo = Tipo.Avion;
                if (ks.IsKeyDown(Keys.H))
                    bTipo = Tipo.Helicoptero;
                if (ks.IsKeyDown(Keys.M))
                    bTipo = Tipo.Militar;
                if (ks.IsKeyDown(Keys.O))
                    bTipo = Tipo.OVNI;
                if (ks.IsKeyDown(Keys.D0))
                    bColor = myColor.Amarillo;
                if (ks.IsKeyDown(Keys.K))
                    bColor = myColor.Azul;
                if (ks.IsKeyDown(Keys.D2))
                    bColor = myColor.Blanco;
                if (ks.IsKeyDown(Keys.D3))
                    bColor = myColor.Naranja;
                if (ks.IsKeyDown(Keys.D4))
                    bColor = myColor.Negro;
                if (ks.IsKeyDown(Keys.D5))
                    bColor = myColor.Rojo;
                if (ks.IsKeyDown(Keys.D6))
                    bColor = myColor.Verde;
                if (ks.IsKeyDown(Keys.D7))
                    bColor = myColor.Violeta;
                try
                {
                    return Global.flyers.FindIndex(x => x.tipo == bTipo && x.mycolor == bColor);
                }
                catch (ArgumentNullException anex)
                {
                    return -1;
                }
            }
            return -1;
        }

        public static int GetFlyerIndex(String[] recognized, string[] palabras)
        {
            bool full = true;
            foreach (String element in recognized)
            {
                if (element == null)
                {
                    full = false;
                    break;
                }
            }
            if (full)
            {
                Tipo bTipo = Tipo.nulo;
                myColor bColor = myColor.nulo;
                if (recognized[0] == palabras[0]) //Avion/Plane
                    bTipo = Tipo.Avion;
                if (recognized[0] == palabras[1]) //Helicoptero/Chopper
                    bTipo = Tipo.Helicoptero;
                if (recognized[0] == palabras[2]) //Militar/Militar
                    bTipo = Tipo.Militar;
                if (recognized[0] == palabras[3]) //OVNI/UFO
                    bTipo = Tipo.OVNI;
                if (recognized[1] == palabras[4]) //Amarillo/Yellow
                    bColor = myColor.Amarillo;
                if (recognized[1] == palabras[5]) //Azul/Blue
                    bColor = myColor.Azul;
                if (recognized[1] == palabras[6]) //Blanco/White
                    bColor = myColor.Blanco;
                if (recognized[1] == palabras[7]) //Naranja/Orange
                    bColor = myColor.Naranja;
                if (recognized[1] == palabras[8]) //Negro/Black
                    bColor = myColor.Negro;
                if (recognized[1] == palabras[9]) //Rojo/Red
                    bColor = myColor.Rojo;
                if (recognized[1] == palabras[10]) //Verde/Green
                    bColor = myColor.Verde;
                if (recognized[1] == palabras[11]) //Violeta/Purple
                    bColor = myColor.Violeta;
                try
                {
                    return Global.flyers.FindIndex(x => x.tipo == bTipo && x.mycolor == bColor);
                }
                catch (ArgumentNullException anex)
                {
                    return -1;
                }
            }
            return -1;
        }

        public static void Comando(KeyboardState ks, int index)
        {
            
            if (ks.IsKeyDown(Keys.P))
            {
                cmdSeguir(index);
                cmdEsperar(index);
            }
            if (ks.IsKeyDown(Keys.S))
            {
                cmdSeguir(index);
            }
            if (ks.IsKeyDown(Keys.D))
            {
                cmdSeguir(index);
                cmdDerecha(index);
            }
            if (ks.IsKeyDown(Keys.I))
            {
                cmdSeguir(index);
                cmdIzquierda(index);
            }
            if (ks.IsKeyDown(Keys.L))
            {
                if (Global.flyers[index].tipo == Tipo.Avion || Global.flyers[index].tipo == Tipo.Militar)
                {
                    cmdSeguir(index);
                    cmdLand(index, Global.random.Next(0, 2));
                }
                else
                {
                    cmdSeguir(index);
                    cmdLand(index, Global.random.Next(2, 4));
                }
            }
        }

        public static void Comando(String[] recognized, int index, string[] palabras)
        {
            bool full = true;
            foreach (String element in recognized)
            {
                if (element == null) {
                    full = false;
                    break;
                }
            }
            if (full)
            {
                if (recognized[2] == palabras[15]) //Se pregunta si se debe ESPERAR
                {
                    cmdSeguir(index);
                    cmdEsperar(index);
                }
                if (recognized[2] == palabras[13]) //Se pregunta si se debe girar DERECHA
                {
                    cmdSeguir(index);
                    cmdDerecha(index);
                }
                if (recognized[2] == palabras[14]) //Se pregunta si se debe girar IZQUIERDA
                {
                    cmdSeguir(index);
                    cmdIzquierda(index);
                }
                if (recognized[2] == palabras[12]) //Se pregunta si se debe ATERRIZAR
                {
                    //Se asigna la pista elegida (si coincide on el tipo)
                    if (Global.flyers[index].tipo == Tipo.Avion || Global.flyers[index].tipo == Tipo.Militar)
                    {
                        if (recognized[3] == palabras[16])
                        {
                            cmdSeguir(index);
                            cmdLand(index, 0);
                        }
                        if (recognized[3] == palabras[17])
                        {
                            cmdSeguir(index);
                            cmdLand(index, 1);
                        }
                    }
                    else
                    {
                        if (recognized[3] == palabras[18])
                        {
                            cmdSeguir(index);
                            cmdLand(index, 2);
                        }
                        if (recognized[3] == palabras[19])
                        {
                            cmdSeguir(index);
                            cmdLand(index, 3);
                        }
                    }
                }
            }
            //Se Vacia par aun nuevo comando
            Global.recognizedCmd[0] = null;
            Global.recognizedCmd[1] = null;
            Global.recognizedCmd[2] = null;
            Global.recognizedCmd[3] = null;
        }

        public static void cmdDerecha(int index)
        {
            Global.flyers[index].girando = false;
            Global.flyers[index].turningDir = Global.flyers[index].direccion + 30;
            if (Global.flyers[index].turningDir >= 360)
            {
                Global.flyers[index].turningDir -= 360;
            }
            Global.flyers[index].landing = false;
            Global.flyers[index].touchDown = false;
            Global.flyers[index].scale = 1.0f;
            Global.flyers[index].velocidad = Global.flyers[index].vInicial;
            if (Global.flyers[index].assignedPista != -1)
            {
                Global.pistas[Global.flyers[index].assignedPista].blocked = false;
                Global.pistas[Global.flyers[index].assignedPista].landing = false; 
            }   
        }

        public static void cmdIzquierda(int index)
        {
            Global.flyers[index].girando = false;
            Global.flyers[index].turningDir = Global.flyers[index].direccion - 30;
            if (Global.flyers[index].turningDir < 0)
            {
                Global.flyers[index].turningDir += 360;
            }
            Global.flyers[index].landing = false;
            Global.flyers[index].touchDown = false;
            Global.flyers[index].scale = 1.0f;
            Global.flyers[index].velocidad = Global.flyers[index].vInicial;
            if (Global.flyers[index].assignedPista != -1)
            {
                Global.pistas[Global.flyers[index].assignedPista].blocked = false;
                Global.pistas[Global.flyers[index].assignedPista].landing = false;
            } 
        }

        public static void cmdLand(int index, int pista)
        {
            try
            { //Intenta setear para aterrizar
                Global.flyers[index].girando = false;
                Global.flyers[index].landing = true;
                Global.flyers[index].assignedPista = pista;
                Global.flyers[index].Landed(index); //Se le da una dirreccion hacia la pista
                float adj = (Global.flyers[index].Position.X - Global.pistas[Global.flyers[index].assignedPista].position.X);
                float opu = (Global.flyers[index].Position.Y - Global.pistas[Global.flyers[index].assignedPista].position.Y);
                float div = (adj / -opu);
                float arc = (float)Math.Atan((double)div);
                float degrees = Global.RadiansToDegrees(arc);
                if (opu < 0)
                    degrees += 180;
                Global.flyers[index].direccion = degrees;
                Global.flyers[index].turningDir = Global.flyers[index].direccion;
            }
            catch (IndexOutOfRangeException ioore)
            {
                Console.WriteLine("Index Out Of Range :(");
            }
        }

        public static void cmdEsperar(int index)
        {
            //Setea para el giro
            Global.flyers[index].girando = true;
            Global.flyers[index].moving = false;
            Global.flyers[index].landing = false;
            Global.flyers[index].touchDown = false;
            Global.flyers[index].scale = 1.0f;
            Global.flyers[index].velocidad = Global.flyers[index].vInicial;
            if (Global.flyers[index].assignedPista != -1)
            {
                Global.pistas[Global.flyers[index].assignedPista].blocked = false;
                Global.pistas[Global.flyers[index].assignedPista].landing = false;
            }  
        }

        public static void cmdSeguir(int index)
        {
            //Setea para ir derecho
            Global.flyers[index].girando = false;
            Global.flyers[index].moving = true;
            Global.flyers[index].landing = false;
            Global.flyers[index].touchDown = false;
            Global.flyers[index].scale = 1.0f;
            Global.flyers[index].velocidad = Global.flyers[index].vInicial;
        }
    }
}
