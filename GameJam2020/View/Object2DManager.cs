using GameJam2020.Model.World;
using GameJam2020.Model.World.Objects;
using GameJam2020.View.Objects;
using GameJam2020.View.Textures;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View
{
    public class Object2DManager
    {
        private TextureManager textureManager;
        private FontManager fontManager;

        Dictionary<string, List<string>> mappingIdObjectToTextures;
        Dictionary<string, List<string>> mappingIdObjectToFonts;

        private List<LayerObject2D> layersList;
        private Dictionary<ALayer, LayerObject2D> mappingLayerToLayerObject2D;

        private Dictionary<AObject, AObject2D> mappingObjectToObject2D;


        public Object2DManager(OfficeWorld world)
        {
            //this.SizeScreen = new Vector2f(800, 600);

            this.textureManager = new TextureManager();
            this.fontManager = new FontManager();

            this.layersList = new List<LayerObject2D>();
            this.initializeResources();

            this.mappingLayerToLayerObject2D = new Dictionary<ALayer, LayerObject2D>();

            this.mappingObjectToObject2D = new Dictionary<AObject, AObject2D>();

            world.ResourcesToLoad += OnResourcesToLoad;

            world.LayerCreated += OnLayerCreated;
            world.LayerDestroyed += OnLayerDestroyed;

            world.ObjectCreated += OnObjectCreated;
            world.ObjectDestroyed += OnObjectDestroyed;

            world.ObjectPositionChanged += OnObjectPositionChanged;
            world.TextPositionChanged += OnTextPositionChanged;
            world.ObjectFocusChanged += OnObjectFocusChanged;
            world.ObjectAnimationChanged += OnObjectAnimationChanged;
            world.ObjectTextChanged += OnObjectTextChanged;    
        }

        private void initializeResources()
        {
            this.mappingIdObjectToTextures = new Dictionary<string, List<string>>();

            this.mappingIdObjectToTextures.Add("patient", new List<string> { @"Resources\testSprite.jpg" });
            this.mappingIdObjectToTextures.Add("toubib", new List<string> { @"Resources\testSprite.jpg" });

            this.mappingIdObjectToTextures.Add("test", new List<string> { @"Resources\testSprite.jpg" });


            this.mappingIdObjectToFonts = new Dictionary<string, List<string>>();

            this.mappingIdObjectToFonts.Add("normalToken", new List<string> { @"Resources\lemon.otf" });
            this.mappingIdObjectToFonts.Add("sanctuaryToken", new List<string> { @"Resources\Quentin.otf"});
        }

        /*public Vector2f SizeScreen
        {
            get;
            set;
        }*/

        public void DrawIn(RenderWindow window)
        {
            foreach (LayerObject2D layer in this.layersList)
            {
                layer.DrawIn(window);
            }
        }

        private void OnResourcesToLoad(List<string> obj)
        {
            HashSet<string> texturesToLoad = new HashSet<string>();
            HashSet<string> fontsToLoad = new HashSet<string>();

            foreach (string idObject in obj)
            {
                if (this.mappingIdObjectToTextures.ContainsKey(idObject))
                {
                    List<string> resourcesPerObject = this.mappingIdObjectToTextures[idObject];
                    foreach (string resource in resourcesPerObject)
                    {
                        texturesToLoad.Add(resource);
                    }
                }

                if (this.mappingIdObjectToFonts.ContainsKey(idObject))
                {
                    List<string> resourcesPerObject = this.mappingIdObjectToFonts[idObject];
                    foreach (string resource in resourcesPerObject)
                    {
                        fontsToLoad.Add(resource);
                    }
                }
            }

            this.textureManager.LoadTextures(texturesToLoad);
            this.fontManager.LoadFonts(fontsToLoad);
        }

        private void OnObjectAnimationChanged(AObject arg1, int arg2)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            object2D.PlayAnimation(arg2);
        }

        private void OnObjectFocusChanged(AObject arg1, bool arg2)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            object2D.SetFocus(arg2);
        }

        private void OnObjectPositionChanged(AObject arg1, Vector2f arg2)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            object2D.SetPosition(arg2);
        }

        private void OnTextPositionChanged(AObject arg1, AObject arg2, Vector2f arg3)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            if(arg2 == null)
            {
                object2D.SetPosition(arg3);
            }
            else
            {
                AObject2D previousObject2D = this.mappingObjectToObject2D[arg2];

                FloatRect bounds = previousObject2D.TextGlobalBounds;

                float newPositionX = bounds.Width + bounds.Left;
                float newPositionY = arg3.Y;

                Vector2f newPosition = new Vector2f(newPositionX, newPositionY);

                object2D.SetPosition(newPosition);
            }
        }

        private void OnObjectTextChanged(AObject arg1, string arg2)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            object2D.SetText(arg2);
        }

        private void OnObjectDestroyed(ALayer arg1, AObject arg2)
        {
            if (this.mappingObjectToObject2D.ContainsKey(arg2))
            {
                LayerObject2D layer = this.mappingLayerToLayerObject2D[arg1];
                AObject2D object2D = this.mappingObjectToObject2D[arg2];

                layer.RemoveObject2D(object2D);
                this.mappingObjectToObject2D.Remove(arg2);
            }
        }

        private void OnObjectCreated(ALayer arg1, AObject arg2)
        {
            LayerObject2D layer = this.mappingLayerToLayerObject2D[arg1];

            List<Texture> listTextures = new List<Texture>();
            if (this.mappingIdObjectToTextures.ContainsKey(arg2.Id))
            {
                List<string> listPathTextures = this.mappingIdObjectToTextures[arg2.Id];
                foreach (string path in listPathTextures)
                {
                    listTextures.Add(this.textureManager.GetTexture(path));
                }
            }

            List<Font> listFonts = new List<Font>();
            if (this.mappingIdObjectToFonts.ContainsKey(arg2.Id))
            {
                List<string> listPathFonts = this.mappingIdObjectToFonts[arg2.Id];
                foreach (string path in listPathFonts)
                {
                    listFonts.Add(this.fontManager.GetFont(path));
                }
            }

            AObject2D object2D = null;
            switch (arg2.Id)
            {
                case "patient":
                    object2D = new PatientObject2D();
                    break;
                case "toubib":
                    object2D = new ToubibObject2D();
                    break;
                case "test":
                    object2D = new TestObject2D();
                    break;
                case "normalToken":
                    object2D = new NormalTokenObject2D();
                    break;
                case "sanctuaryToken":
                    object2D = new SanctuaryTokenObject2D();
                    break;
            }

            if (object2D != null)
            {
                object2D.AssignTextures(listTextures);
                object2D.AssignFonts(listFonts);

                this.mappingObjectToObject2D.Add(arg2, object2D);
                layer.AddObject2D(object2D);
            }
        }

        private void OnLayerDestroyed(ALayer obj)
        {
            LayerObject2D layer = this.mappingLayerToLayerObject2D[obj];
            this.layersList.Remove(layer);
            this.mappingLayerToLayerObject2D.Remove(obj);
        }

        private void OnLayerCreated(ALayer obj)
        {
            LayerObject2D layer = new LayerObject2D();
            this.layersList.Add(layer);
            this.mappingLayerToLayerObject2D.Add(obj, layer);
        }

        public void Dispose(OfficeWorld world)
        {
            world.ResourcesToLoad -= OnResourcesToLoad;

            world.LayerCreated -= OnLayerCreated;
            world.LayerDestroyed -= OnLayerDestroyed;

            world.ObjectCreated -= OnObjectCreated;
            world.ObjectDestroyed -= OnObjectDestroyed;

            world.ObjectPositionChanged -= OnObjectPositionChanged;
            world.ObjectFocusChanged -= OnObjectFocusChanged;
            world.TextPositionChanged -= OnTextPositionChanged;
            world.ObjectAnimationChanged -= OnObjectAnimationChanged;
            world.ObjectTextChanged -= OnObjectTextChanged;
        }
    }
}
