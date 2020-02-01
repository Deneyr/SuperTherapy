using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Textures
{
    public class FontManager
    {
        private Dictionary<string, Font> fontsDictionary;

        public FontManager()
        {
            this.fontsDictionary = new Dictionary<string, Font>();
        }

        public Font GetFont(string path)
        {
            return this.fontsDictionary[path];
        }

        public void LoadFonts(HashSet<string> fontsToLoad)
        {
            HashSet<string> fontsNotToLoad = new HashSet<string>();
            List<string> fontsToRemove = new List<string>();

            foreach (KeyValuePair<string, Font> keyValuePair in this.fontsDictionary)
            {
                if (fontsToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    fontsToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    fontsNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach (string keyToRemove in fontsToRemove)
            {
                this.fontsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in fontsToLoad)
            {
                if (fontsNotToLoad.Contains(pathToLoad) == false)
                {
                    this.fontsDictionary.Add(pathToLoad, new Font(pathToLoad));
                }
            }
        }
    }
}
