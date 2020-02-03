using GameJam2020.Model.Events;
using GameJam2020.Model.World;
using GameJam2020.Model.World.Objects;
using GameJam2020.View.Objects;
using GameJam2020.View.Sounds;
using GameJam2020.View.Textures;
using SFML.Audio;
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
        private SoundManager soundManager;

        Dictionary<string, List<string>> mappingIdObjectToTextures;
        Dictionary<string, List<string>> mappingIdObjectToFonts;
        Dictionary<string, List<string>> mappingIdObjectToSounds;
        Dictionary<string, List<string>> mappingIdObjectToMusics;

        private List<LayerObject2D> layersList;
        private Dictionary<ALayer, LayerObject2D> mappingLayerToLayerObject2D;

        private Dictionary<AObject, AObject2D> mappingObjectToObject2D;


        public Object2DManager(OfficeWorld world)
        {
            //this.SizeScreen = new Vector2f(800, 600);

            this.textureManager = new TextureManager();
            this.fontManager = new FontManager();
            this.soundManager = new SoundManager();

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
            world.ObjectTextStateChanged += OnObjectTextStateChanged;
            world.TextUpdated += OnTextUpdated;
            world.GameStateChanged += OnGameStateChanged;
        }

        private void initializeResources()
        {
            this.mappingIdObjectToTextures = new Dictionary<string, List<string>>();

            this.mappingIdObjectToTextures.Add("office", new List<string> { @"Resources\background\Fond_Cabinet.png",
                @"Resources\background\Lampe.png" });
            this.mappingIdObjectToTextures.Add("darkOffice", new List<string> { @"Resources\background\Fond_Cabinet_Dark.png" });

            this.mappingIdObjectToTextures.Add("arrow", new List<string> { @"Resources\foreground\arrow.png" });
            this.mappingIdObjectToTextures.Add("bubbleHeader", new List<string> { @"Resources\foreground\bulleTitreAnim.png" });
            this.mappingIdObjectToTextures.Add("bubbleTuto", new List<string> { @"Resources\foreground\Bulle_Tuto.png" });
            this.mappingIdObjectToTextures.Add("bubble", new List<string> { @"Resources\foreground\bubble.png" });

            this.mappingIdObjectToTextures.Add("patient", new List<string> { @"Resources\middleground\Spritemap_Patient_1_574_226.png" });
            this.mappingIdObjectToTextures.Add("toubib", new List<string> { @"Resources\middleground\Spritemap_Psy_417_419.png" });

            this.mappingIdObjectToTextures.Add("queueDream", new List<string> { @"Resources\foreground\queueDream.png" });
            this.mappingIdObjectToTextures.Add("queueTalk", new List<string> { @"Resources\foreground\queueTalk2.png" });

            this.mappingIdObjectToTextures.Add("notebook", new List<string> { @"Resources\foreground\Carnet_Psy.png" });

            this.mappingIdObjectToTextures.Add("test", new List<string> { @"Resources\testSprite.jpg" });

            this.mappingIdObjectToTextures.Add("answerToken", new List<string> { @"Resources\foreground\RectangleMot.png" });
            this.mappingIdObjectToTextures.Add("fieldToken", new List<string> { @"Resources\foreground\RectangleMot.png" });

            this.mappingIdObjectToFonts = new Dictionary<string, List<string>>();

            this.mappingIdObjectToFonts.Add("normalToken", new List<string> { @"Resources\lemon.otf" });
            this.mappingIdObjectToFonts.Add("answerToken", new List<string> { @"Resources\lemon.otf" });
            this.mappingIdObjectToFonts.Add("fieldToken", new List<string> { @"Resources\lemon.otf" });
            this.mappingIdObjectToFonts.Add("sanctuaryToken", new List<string> { @"Resources\Quentin.otf"});
            this.mappingIdObjectToFonts.Add("headerToken", new List<string> { @"Resources\lemon.otf" });

            // Sounds & Musics
            this.mappingIdObjectToSounds = new Dictionary<string, List<string>>();

            /*this.mappingIdObjectToSounds.Add("patient", new List<string> { @"Resources\middleground\Spritemap_Patient_1_574_226.png" });
            this.mappingIdObjectToSounds.Add("toubib", new List<string> { @"Resources\sounds\Bruitages" });*/
            this.mappingIdObjectToSounds.Add("lampClipped", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Clic_Lampe_Mixed.mp3" });
            this.mappingIdObjectToSounds.Add("wordPicked", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Deplacement_Mot_Mixed.mp3" });
            this.mappingIdObjectToSounds.Add("bubbleClosed", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Fermeture_Bulle_Mixed.mp3" });
            this.mappingIdObjectToSounds.Add("bubbleOpened", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Ouverture_Bulle_Mixed.mp3" });
            this.mappingIdObjectToSounds.Add("wordInserted", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Placement_Mot_Trou_Mixed.mp3" });
            this.mappingIdObjectToSounds.Add("wordDroped", new List<string> { @"Resources\sounds\Bruitages\Mixed\SFX_Relacher_Mot_Mixed.mp3" });

            this.mappingIdObjectToMusics = new Dictionary<string, List<string>>();
            this.mappingIdObjectToMusics.Add("level", new List<string> { @"Resources\sounds\Musiques\Night in Venice.mp3" });
        }

        /*public Vector2f SizeScreen
        {
            get;
            set;
        }*/

        public AObject getAnswerTokenAt(Vector2f position)
        {
            foreach(KeyValuePair<AObject, AObject2D> pair in this.mappingObjectToObject2D)
            {
                if(pair.Value is AnswerTokenObject2D)
                {
                    ATokenObject2D token = pair.Value as ATokenObject2D;
                    FloatRect bounds = token.TextGlobalBounds;

                    if(position.X > bounds.Left && position.X < bounds.Left + bounds.Width
                        && position.Y > bounds.Top && position.Y < bounds.Top + bounds.Height)
                    {
                        return pair.Key;
                    }
                }
            }
            return null;
        }

        public AObject getFieldTokenAt(Vector2f position)
        {
            foreach (KeyValuePair<AObject, AObject2D> pair in this.mappingObjectToObject2D)
            {
                if (pair.Value is FieldTokenObject2D)
                {
                    ATokenObject2D token = pair.Value as ATokenObject2D;
                    FloatRect bounds = token.TextGlobalBounds;

                    float BoundTop = bounds.Top;
                    float BoundHeight = bounds.Height;
                    if ( bounds.Height < 40)
                    {
                        BoundTop -= 40 - bounds.Height;
                        BoundHeight = 40;
                    }

                    if (position.X > bounds.Left && position.X < bounds.Left + bounds.Width
                        && position.Y > BoundTop && position.Y < BoundTop + BoundHeight)
                    {
                        return pair.Key;
                    }
                }
            }
            return null;
        }

        public void DrawIn(RenderWindow window)
        {
            foreach (LayerObject2D layer in this.layersList)
            {
                layer.DrawIn(window);
            }
        }

        private void OnGameStateChanged(string levelName, GameEvent obj)
        {
            string pathSound = null;
            switch (obj.Type)
            {
                case EventType.START:
                    if (this.mappingIdObjectToMusics.ContainsKey(levelName))
                    {
                        string pathMusic = this.mappingIdObjectToMusics[levelName][0];
                        this.soundManager.PlayMusic(pathMusic);

                        pathSound = this.mappingIdObjectToSounds["lampClipped"][0];
                        this.soundManager.PlaySound(pathSound);
                    }
                    break;
                case EventType.ENDING:
                    this.soundManager.StopAllSounds();
                    break;
                case EventType.PICK_WORD:
                    pathSound = this.mappingIdObjectToSounds["wordPicked"][0];
                    this.soundManager.PlaySound(pathSound);
                    break;
                case EventType.DROP_WORD:
                    pathSound = this.mappingIdObjectToSounds["wordDroped"][0];
                    this.soundManager.PlaySound(pathSound);
                    break;
                case EventType.INSERT_WORD:
                    pathSound = this.mappingIdObjectToSounds["wordInserted"][0];
                    this.soundManager.PlaySound(pathSound);
                    break;
                case EventType.OPEN_BUBBLE:
                    pathSound = this.mappingIdObjectToSounds["bubbleOpened"][0];
                    this.soundManager.PlaySound(pathSound);
                    break;
            }
        }

        private void OnResourcesToLoad(List<string> obj)
        {
            HashSet<string> texturesToLoad = new HashSet<string>();
            HashSet<string> fontsToLoad = new HashSet<string>();

            HashSet<string> soundsToLoad = new HashSet<string>();
            HashSet<string> musicsToLoad = new HashSet<string>();

            foreach (string idObject in obj)
            {
                //string idObject = aliasObject.Split(' ')[0];

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

                if (this.mappingIdObjectToSounds.ContainsKey(idObject))
                {
                    List<string> resourcesPerObject = this.mappingIdObjectToSounds[idObject];
                    foreach (string resource in resourcesPerObject)
                    {
                        soundsToLoad.Add(resource);
                    }
                }

                if (this.mappingIdObjectToMusics.ContainsKey(idObject))
                {
                    List<string> resourcesPerObject = this.mappingIdObjectToMusics[idObject];
                    foreach (string resource in resourcesPerObject)
                    {
                        musicsToLoad.Add(resource);
                    }
                }
            }

            this.textureManager.LoadTextures(texturesToLoad);
            this.fontManager.LoadFonts(fontsToLoad);

            this.soundManager.LoadSounds(soundsToLoad);
            this.soundManager.LoadMusics(musicsToLoad);
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

        private void OnTextUpdated(AObject arg1, AObject arg2, AObject arg3, Vector2f arg4)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            if (arg2 == null)
            {
                object2D.SetPosition(arg4);
            }
            else
            {
                AObject2D previousObject2D = this.mappingObjectToObject2D[arg2];
                AObject2D associtedObject2D = null;
                if (arg3 != null)
                {
                    associtedObject2D = this.mappingObjectToObject2D[arg3];
                }

                FloatRect boundsPrevious = previousObject2D.TextGlobalBounds;

                float width = previousObject2D.TextGlobalBounds.Width;
                if (associtedObject2D != null)
                {
                    width = associtedObject2D.TextGlobalBounds.Width;
                }

                float newPositionX = width + previousObject2D.Position.X;
                float newPositionY = arg4.Y;

                Vector2f newPosition = new Vector2f(newPositionX, newPositionY);

                object2D.SetPosition(newPosition);
            }
        }


        private void OnObjectTextStateChanged(AObject arg1, bool arg2)
        {
            AObject2D object2D = this.mappingObjectToObject2D[arg1];

            if(object2D != null && object2D is ATokenObject2D)
            {
                object2D.SetTextState(arg2);
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
                case "arrow":
                    object2D = new ArrowObject2D();
                    break;
                case "bubbleHeader":
                    object2D = new BubbleHeaderObject2D();
                    break;
                case "bubbleTuto":
                    object2D = new BubbleTutoObject2D();
                    break;
                case "bubble":
                    object2D = new BubbleObject2D();
                    break;
                case "office":
                    object2D = new OfficeObject2D();
                    break;
                case "darkOffice":
                    object2D = new DarkOfficeObject2D();
                    break;
                case "patient":
                    object2D = new PatientObject2D();
                    break;
                case "toubib":
                    object2D = new ToubibObject2D();
                    break;
                case "queueDream":
                    object2D = new QueueDreamObject2D();
                    break;
                case "queueTalk":
                    object2D = new QueueTalkObject2D();
                    break;
                case "notebook":
                    object2D = new NotebookObject2D();
                    break;
                case "test":
                    object2D = new TestObject2D();
                    break;
                case "normalToken":
                    object2D = new NormalTokenObject2D();
                    break;
                case "answerToken":
                    object2D = new AnswerTokenObject2D();
                    break;
                case "fieldToken":
                    object2D = new FieldTokenObject2D();
                    break;
                case "sanctuaryToken":
                    object2D = new SanctuaryTokenObject2D();
                    break;
                case "headerToken":
                    object2D = new HeaderTokenObject2D();
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
            world.ObjectTextStateChanged -= OnObjectTextStateChanged;
            world.TextUpdated -= OnTextUpdated;
            world.GameStateChanged -= OnGameStateChanged;
        }
    }
}
