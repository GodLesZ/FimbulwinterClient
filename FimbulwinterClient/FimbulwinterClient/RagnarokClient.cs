using System.IO;
using FimbulwinterClient.Audio;
using FimbulwinterClient.Core.Config;
using FimbulwinterClient.Core.Assets;
using FimbulwinterClient.Gui.Nuclex.Input;
using FimbulwinterClient.Gui.System;
using FimbulwinterClient.Lua;
using FimbulwinterClient.Network;
using FimbulwinterClient.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FimbulwinterClient.Core;

namespace FimbulwinterClient {

	public class RagnarokClient : Game {
		public static RagnarokClient Singleton {
			get;
			set;
		}

		public GraphicsDeviceManager Graphics {
			get;
			set;
		}

		public SpriteBatch SpriteBatch {
			get;
			set;
		}

		public BGMManager BgmManager {
			get;
			set;
		}

		public EffectManager EffectManager {
			get;
			set;
		}

		public LuaManager LuaManager {
			get;
			set;
		}

		public InputManager InputManager {
			get;
			set;
		}

		public GuiManager GuiManager {
			get;
			set;
		}

		public IGameScreen Screen {
			get;
			set;
		}

		public NetworkState NetworkState {
			get;
			set;
		}

		public Connection CurrentConnection {
			get;
			set;
		}

		public RagnarokClient() {
			Singleton = this;
			Graphics = new GraphicsDeviceManager(this) {
				SynchronizeWithVerticalRetrace = false
			};
			Window.Title = "Ragnarok Online";

			SharedInformation.Initialize(Services, GraphicsDevice);
			Content = SharedInformation.ContentManager;

			try {
				// @TODO: Move Configuration loading to content pipeline
				using (var s = Content.Load<Stream>(Configuration.DefaultPath)) {
					SharedInformation.Config = Configuration.FromStream(s);
				}
			} catch {
				SharedInformation.Config = new Configuration();
			}

			BgmManager = new BGMManager();
			EffectManager = new EffectManager();
			LuaManager = new LuaManager();

			InputManager = new InputManager(Services, Window.Handle);

			GuiManager = new GuiManager(this);
			GuiManager.DrawOrder = 1000;

			Components.Add(InputManager);
			Components.Add(GuiManager);
			Components.Add(new FPSCounter(this)); // REMOVE ME LATER

			IsFixedTimeStep = false; // REMOVE ME LATER

			Services.AddService(typeof(InputManager), InputManager);
			Services.AddService(typeof(GuiManager), GuiManager);
			Services.AddService(typeof(EffectManager), EffectManager);
			Services.AddService(typeof(BGMManager), BgmManager);
			Services.AddService(typeof(LuaManager), LuaManager);

			Graphics.PreferredBackBufferWidth = SharedInformation.Config.ScreenWidth;
			Graphics.PreferredBackBufferHeight = SharedInformation.Config.ScreenHeight;
			Graphics.ApplyChanges();

			NetworkState = new NetworkState();
		}

		protected override void Initialize() {
			SharedInformation.GraphicsDevice = GraphicsDevice;
			Gui.Utils.Init(GraphicsDevice);

			InputManager.GetKeyboard().KeyPressed += kb_KeyReleased;

			//ChangeScreen(new ServiceSelectScreen());
			//ChangeScreen(new LoadingScreen("prontera.gat"));
			StartMapChange("prontera");

			base.Initialize();
		}

		protected override void LoadContent() {
			SpriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void UnloadContent() {

		}

		protected override void Update(GameTime gameTime) {
			if (Screen != null)
				Screen.Update(SpriteBatch, gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			if (Screen != null)
				Screen.Draw(SpriteBatch, gameTime);

			base.Draw(gameTime);
		}

		public void ChangeScreen(IGameScreen screen) {
			if (this.Screen != null)
				this.Screen.Dispose();

			this.Screen = screen;
		}

		private static int mCounter;
		void MakeScreenshot() {
			int w = RagnarokClient.Singleton.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int h = RagnarokClient.Singleton.GraphicsDevice.PresentationParameters.BackBufferHeight;

			Draw(new GameTime());

			int[] backBuffer = new int[w * h];
			RagnarokClient.Singleton.GraphicsDevice.GetBackBufferData(backBuffer);

			//copy into a texture 
			var texture = new Texture2D(RagnarokClient.Singleton.GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
			texture.SetData(backBuffer);

			//save to disk 
			if (!System.IO.Directory.Exists("ScreenShot")) System.IO.Directory.CreateDirectory("ScreenShot");
			Stream stream = File.OpenWrite(System.IO.Path.Combine("ScreenShot", "screen" + mCounter + ".png"));

			texture.SaveAsPng(stream, w, h);
			stream.Dispose();

			texture.Dispose();
			mCounter++;
		}

		void kb_KeyReleased(Keys key) {
			if (key == Keys.PrintScreen)
				MakeScreenshot();
		}

		public void StartMapChange(string p) {
			var ls = new LoadingScreen(p);
			ls.Loaded += ls_Loaded;

			ChangeScreen(ls);
		}

		private void ls_Loaded(Map obj) {
			ChangeScreen(new IngameScreen(obj));
		}

	}

}
