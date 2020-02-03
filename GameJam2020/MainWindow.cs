using GameJam2020.Model.World;
using GameJam2020.Model.World.Objects;
using GameJam2020.View;
using GameJam2020.View.Objects;
using GameJam2020.View.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJam2020
{
    public class MainWindow
    {
        private OfficeWorld officeWorld;

        private Object2DManager object2DManager;

        private AObject objectFocused;

        private Vector2f resolutionScreen;

        public MainWindow()
        {
            this.officeWorld = new OfficeWorld();
            this.object2DManager = new Object2DManager(this.officeWorld);

            this.objectFocused = null;
        }

        public void Run()
        {
            //var mode = new SFML.Window.VideoMode(800, 600);
            var window = new SFML.Graphics.RenderWindow(SFML.Window.VideoMode.FullscreenModes[0], "Repair Project", SFML.Window.Styles.Fullscreen);
            window.KeyPressed += Window_KeyPressed;

            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseMoved += OnMouseMoved;

            //this.object2DManager.SizeScreen = window.GetView().Size;


            SFML.Graphics.View view = window.GetView();
            this.resolutionScreen = new Vector2f(view.Size.X, view.Size.Y);
            view.Center = new Vector2f(0, 0);
            window.SetView(view);

            window.SetVerticalSyncEnabled(true);

            Clock clock = new Clock();

            this.officeWorld.StartLevel();

            // Start the game loop
            while (window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Game logic update
                this.officeWorld.UpdateLogic(deltaTime);

                window.Clear();

                this.object2DManager.DrawIn(window);

                // Process events
                window.DispatchEvents();


                AObject2D.UpdateZoomAnimationManager(deltaTime);         

                // Finally, display the rendered frame on screen
                window.Display();
            }

            this.object2DManager.Dispose(this.officeWorld);

            AObject2D.StopAnimationManager();
        }

        private void OnMouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            if(this.objectFocused != null)
            {
                this.officeWorld.OnMouseDragOnObject(this.objectFocused, new Vector2f(e.X - this.resolutionScreen.X / 2, e.Y - this.resolutionScreen.Y / 2));
            }
        }

        private void OnMouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            if(e.Button == SFML.Window.Mouse.Button.Left)
            {
                if (this.objectFocused != null)
                {
                    AObject lObject = this.object2DManager.getFieldTokenAt(new Vector2f(e.X - this.resolutionScreen.X / 2, e.Y - this.resolutionScreen.Y / 2));

                    this.officeWorld.OnMouseUpOnObject(this.objectFocused, lObject);

                    this.objectFocused = null;
                }
                else
                {
                    AObject lObject = this.object2DManager.getTimerTokenAt(new Vector2f(e.X - this.resolutionScreen.X / 2, e.Y - this.resolutionScreen.Y / 2));

                    this.officeWorld.OnMouseUpOnObject(null, lObject);
                }
            }
        }

        private void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                AObject lObject = this.object2DManager.getAnswerTokenAt(new Vector2f(e.X - this.resolutionScreen.X / 2, e.Y - this.resolutionScreen.Y / 2));

                if(lObject != null)
                {
                    this.officeWorld.OnMouseDownOnObject(lObject);

                    this.objectFocused = lObject;
                }
            }
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}
