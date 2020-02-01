using GameJam2020.Model.World;
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

        public void Run()
        {
            this.officeWorld = new OfficeWorld();
            this.object2DManager = new Object2DManager(this.officeWorld);

            var mode = new SFML.Window.VideoMode(800, 600);
            var window = new SFML.Graphics.RenderWindow(mode, "Repair Project");
            window.KeyPressed += Window_KeyPressed;

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

        private void UpdateGame(Time deltaTime)
        {

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
