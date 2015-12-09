#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio; //Directiva using para efectos de sonido
using Microsoft.Xna.Framework.Media;//Directiva using para música de fondo
using Microsoft.Xna.Framework.GamerServices;


#endregion

namespace Game_of_Drones
{/// <summary>
	/// This is the main type for your game

	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{   
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Animation animacionexplosion;
		Color color;
		bool vida, pausa, pantalla; 
		int puntos=0;
		int vidas=3;
		int nivel=1;
		Texture2D texturaAvion, avionIz, avionDer, explosion;
		Texture2D texturaEnemigo, fondo, texturaAstronauta;
		// Un vector para las posiciones X,Y del personaje
		Vector2 posicionAvion, posexplosion, pos4, cartel, cordes, posNivel, posPausa, menu, inicio; 
		int velocidadAvion = 5;
		//Texto en pantalla
		SpriteFont fuente1;
		SpriteFont fuente2;

		// Variable para cálculo de rectángulos de Sprites
		BoundingBox bboxPersona;
		BoundingBox bboxEnemigo;
		BoundingBox bboxAstronauta;

		Song musicadeFondo; // Variable para manipular música

		        public SoundEffect efectodeSonido, sonidoexplosion; // Vaiable para manipular efectos de sonido
		// Una lista de vector2 para las pocisiones X,Y del enemigo
		List <Vector2> posicionesEnemigo = new  List<Vector2> ();
		double probabilidadEnemigo = 0.02;    // 3% 
		int velocidadEnemigo = 3;
		Random aleatorio = new Random ();
		//lista de vstor para el astronauta
		List <Vector2> posicionesAstronauta = new  List<Vector2> ();
		double probabilidadAstronauta = 0.02;    // 3% 
		int velocidadAstronauta = 2;
	

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "../../Content";
		}

		protected override void Initialize ()
		{   vida = true;
			pantalla = true;
			// Titulo de la ventana
			Window.Title = "MonoGame sobre Linux - El Cyberalphabetizador";
			// Resolición de la Pantalla
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 560;
			posicionAvion.X = 320;
			posicionAvion.Y = 400;
			pos4 = Vector2.Zero;
			cartel = new Vector2 (650, 0);
			posNivel = new Vector2 (350, 0);
			posPausa = new Vector2 (350, 350);
			menu = new Vector2 (350, 350);
			inicio = new Vector2 (350, 370);

			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			// Cargar las texturas 

			texturaAvion = Content.Load<Texture2D> ("Graficos/avion1");
			animacionexplosion = new Animation();
			            spriteBatch = new SpriteBatch(GraphicsDevice);
			            fondo = Content.Load< Texture2D>("fondos/fondo4");
			            fuente1 = Content.Load<SpriteFont>("fuente/fuente1");
			            fuente2 = Content.Load<SpriteFont>("fuente/fuente2");
			            texturaAvion = Content.Load< Texture2D> ("Graficos/avion1");
			            avionIz = Content.Load< Texture2D> ("Graficos/avion0");
			            avionDer = Content.Load< Texture2D> ("Graficos/avion2");
			            texturaEnemigo = Content.Load<Texture2D> ("Graficos/drone7_chico");
			            texturaAstronauta = Content.Load<Texture2D> ("Graficos/astronauta");
			            explosion= Content.Load<Texture2D> ("Graficos/explosions");


			            posexplosion = new Vector2(
				                GraphicsDevice.Viewport.TitleSafeArea.X +
				                GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
				                GraphicsDevice.Viewport.TitleSafeArea.Y +
				                GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			            animacionexplosion.Initialize(explosion, posexplosion, 64, 71, 16, 5000, Color.White, 1.0f, true);
			            //playerAnimation.Initialize(playerTexture, spritePos, 32, 32, 9, 120, Color.White, 1.0f, true);
			            //explosions.Add(explosion);
			            /*
             * Donde
             * 1) playerTexture: es el grupo de sprite a animar.
             * 2) spritePos: Coordenadas de ubicación del sprite.
             * 3) 106: Ancho del sprite.
             * 4) 110: Alto del sprite.
             * 5) 6: Cantidad de sprites
             * 6) 80: Tiempo de refresco o muestreo de una imagen a otra.
             * 7) Color.White: Despliega la imagen en su color real White = transparente.
             * 8) 1.0f: escala del sprite.
             * 9) true: repite indefinidamente, false: solo una.
             */

			            //Sonido de Fondo: Tema musical
			            musicadeFondo = Content.Load<Song>("Sonidos/Angeles.wav");
			            //musicadeFondo = Content.Load<Song>("Sonido/gameMusic.mp3");
			            MediaPlayer.Play(musicadeFondo);
			            MediaPlayer.IsRepeating = true;
			            MediaPlayer.Volume = 0.3f;
			            //Efectos
			            efectodeSonido = Content.Load<SoundEffect> ("Sonidos/efectoLaser");
			            sonidoexplosion = Content.Load<SoundEffect> ("Sonidos/efectoExplosion");
		}

		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update (GameTime gameTime)
		{   
			if (pausa == false) {
				// Asignamos el color del cielo
				color = Color.White; 
				// Tomamos el evento de teclado 
				KeyboardState keyboard = Keyboard.GetState (); 
				if (keyboard.IsKeyDown (Keys.P)) {
					spriteBatch.DrawString (fuente1, "Pausa", posPausa, Color.GhostWhite);
					pausa = true;

				}

				// Dar la posibilidad de salir del juego 
				if (keyboard.IsKeyDown (Keys.Escape)) { 
					// si se presiona la tecla escape se termina el juego 
					this.Exit (); 
				} 
				//Modifico Volumen
				if (keyboard.IsKeyDown (Keys.D1))
					MediaPlayer.Volume = 0.1f;
				if (keyboard.IsKeyDown (Keys.D2))
					MediaPlayer.Volume = 0.2f;
				if (keyboard.IsKeyDown (Keys.D3))
					MediaPlayer.Volume = 0.3f;
				if (keyboard.IsKeyDown (Keys.D4))
					MediaPlayer.Volume = 0.4f;
				if (keyboard.IsKeyDown (Keys.D5))
					MediaPlayer.Volume = 0.5f;
				if (keyboard.IsKeyDown (Keys.D6))
					MediaPlayer.Volume = 0.6f;
				if (keyboard.IsKeyDown (Keys.D7))
					MediaPlayer.Volume = 0.7f;
				if (keyboard.IsKeyDown (Keys.D8))
					MediaPlayer.Volume = 0.8f;
				if (keyboard.IsKeyDown (Keys.D9))
					MediaPlayer.Volume = 1.0f;

				// Mover el personaje con las flechas derecha e izqeuierda  
				if (keyboard.IsKeyDown (Keys.Down)) {
					posicionAvion.Y += velocidadAvion;
				}
				if (keyboard.IsKeyDown (Keys.Up)) {
					posicionAvion.Y -= velocidadAvion;
				} 
				if (keyboard.IsKeyUp (Keys.Up)) {
					texturaAvion = Content.Load<Texture2D> ("Graficos/avion1");
				}

				if (keyboard.IsKeyDown (Keys.Left)) { 
					//texturaAvion = avionIz;
					texturaAvion = Content.Load< Texture2D> ("Graficos/avion0");
					posicionAvion.X -= velocidadAvion; 
				} 
				if (keyboard.IsKeyDown (Keys.Right)) { 
					//texturaAvion = avionDer;
					texturaAvion = Content.Load< Texture2D> ("Graficos/avion2");
					posicionAvion.X += velocidadAvion; 
				}





				// El método Clamp evita que el personaje salga de la pantalla  
				posicionAvion.X = MathHelper.Clamp (posicionAvion.X, 
				                                   0, Window.ClientBounds.Width - texturaAvion.Width);
				posicionAvion.Y = MathHelper.Clamp (posicionAvion.Y, 
				                                   0, Window.ClientBounds.Height - texturaAvion.Height); 

				// aparecen nuevos enemigos según la probabilidad 
				if (aleatorio.NextDouble () < probabilidadEnemigo) { 
					float x = (float)aleatorio.NextDouble () * 
						Window.ClientBounds.Width; 
					posicionesEnemigo.Add (new Vector2 (x, 0)); 

				}
				// aparecen nuevos astronautas según la probabilidad 
				if (aleatorio.NextDouble () < probabilidadAstronauta) { 
					float x = (float)aleatorio.NextDouble () * 
						Window.ClientBounds.Width; 
					posicionesAstronauta.Add (new Vector2 (x, 0)); 
				}

				// obtener el rectángulo de la persona 
				bboxPersona =
				new BoundingBox (new Vector3 (posicionAvion, 0), 
				                 new Vector3 (texturaAvion.Width + posicionAvion.X,
				                              texturaAvion.Height + posicionAvion.Y, 0));

				// actualizar cada enemigo
				for (int i = 0; i < posicionesEnemigo.Count; i++) { 
					// actualizo las posiciones de las nave enemiga 
					posicionesEnemigo [i] = new Vector2 (posicionesEnemigo [i].X, 
					                                    posicionesEnemigo [i].Y + velocidadEnemigo);

					// obtener el rectangulo del enemigo
					bboxEnemigo = new BoundingBox (new Vector3 (posicionesEnemigo [i], 0),
					                              new Vector3 (texturaEnemigo.Width + posicionesEnemigo [i].X,
					                                           texturaEnemigo.Height + posicionesEnemigo [i].Y, 0));

					// eliminar los enemigos cuando salen de la pantalla 
					if (posicionesEnemigo [i].Y > Window.ClientBounds.Height) { 
						posicionesEnemigo.RemoveAt (i);
						// decrecemos i, por que hay un enemigo menos 
						i--;
					}
					// evaluar colisión con el personaje y cambiamos el color
					if (bboxPersona.Intersects (bboxEnemigo)) {
						color = Color.Red;
						vidas = vidas - 1;
						sonidoexplosion.Play ();
						animacionexplosion.Update (gameTime, posicionAvion);
						posicionesEnemigo.RemoveAt (i);
						//posicionAvion.RemoveAt (i);
					} 


				}

				// actualizar cada astronauta
				for (int i = 0; i < posicionesAstronauta.Count; i++) { 
					// actualizo las posiciones de las nave enemiga 
					posicionesAstronauta [i] = new Vector2 (posicionesAstronauta [i].X, 
					                                       posicionesAstronauta [i].Y + velocidadAstronauta);

					// obtener el rectangulo del astronauta
					bboxAstronauta = new BoundingBox (new Vector3 (posicionesAstronauta [i], 0),
					                                 new Vector3 (texturaAstronauta.Width + posicionesAstronauta [i].X,
					                                           texturaAstronauta.Height + posicionesAstronauta [i].Y, 0));

					// eliminar los astronautas cuando salen de la pantalla 
					if (posicionesAstronauta [i].Y > Window.ClientBounds.Height) { 
						posicionesAstronauta.RemoveAt (i);
						// decrecemos i, por que hay un enemigo menos 
						i--;
					}
					// evaluar colisión con el astronauta y cambiamos el color
					if (bboxPersona.Intersects (bboxAstronauta)) {
						color = Color.Yellow;
						puntos = puntos + 50;
						//sonidoexplosion.Play ();
						posicionesAstronauta.RemoveAt (i);
					} 


				}

				if ((puntos == 300) && (vida == true)) {
					vidas ++;
					vida = false; 
				}

			
				if ((puntos == 600) && (pantalla == true)) {
					fondo = Content.Load< Texture2D> ("fondos/fondo6");
					nivel = nivel + 1;

					pantalla = false; 
				}
			
				if ((puntos == 800) && (pantalla == false)) {
					fondo = Content.Load< Texture2D> ("fondos/fondo3");
					texturaEnemigo = Content.Load<Texture2D> ("Graficos/enemigo3");
					nivel = nivel + 1;
					velocidadEnemigo = 4;
					pantalla = true;
				}
				if ((puntos == 1000) && (pantalla == true)){
					fondo = Content.Load< Texture2D> ("fondos/fondo5");
					texturaEnemigo = Content.Load<Texture2D> ("Graficos/enemigo3");
					nivel = nivel + 1;
					velocidadEnemigo = 6;
					pantalla = false;
				}
				if (vidas == 0) {
				pausa = true;
				}
			}

			base.Update (gameTime); 
		}

		protected override void Draw (GameTime gameTime)
		{
			// borrar la pantalla
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin (); 

			// Dibujar personaje
			spriteBatch.Draw (fondo, cordes, Color.White);
			spriteBatch.Draw (texturaAvion, posicionAvion, color); 
			spriteBatch.DrawString(fuente1, "Vidas"+ vidas, pos4, Color.Red);
			            spriteBatch.DrawString(fuente2, "Puntaje: " + puntos, cartel, Color.Yellow);
			spriteBatch.DrawString(fuente1, "Nivel "+ nivel, posNivel, Color.GhostWhite);
			//spriteBatch.DrawString(fuente1, "Pausa", posPausa, Color.GhostWhite);
			if (pausa == true) {
				spriteBatch.DrawString (fuente1, "Pausa", posPausa, Color.GhostWhite);
					}

			if (vidas == 0) {
				spriteBatch.DrawString(fuente1, "GAME OVER", posPausa, Color.GhostWhite);
			}
			// Dibujar enemigos 
			foreach (Vector2 posicionEnemigo in posicionesEnemigo) { 
				spriteBatch.Draw (texturaEnemigo, posicionEnemigo, Color.White);
			}
			foreach (Vector2 posicionAstronauta in posicionesAstronauta) { 
				spriteBatch.Draw (texturaAstronauta, posicionAstronauta, Color.White);
			}
			if (color == Color.Red)
				            animacionexplosion.Draw(spriteBatch);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}

