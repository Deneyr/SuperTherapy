using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Sounds
{
    public class SoundManager
    {
        private Dictionary<string, SoundBuffer> soundsDictionary;

        private Dictionary<string, Music> musicsDictionary;

        public SoundManager()
        {
            this.soundsDictionary = new Dictionary<string, SoundBuffer>();

            this.musicsDictionary = new Dictionary<string, Music>();
        }

        public SoundBuffer GetSound(string path)
        {
            return this.soundsDictionary[path];
        }

        public Music GetMusic(string path)
        {
            return this.musicsDictionary[path];
        }

        public void LoadSounds(HashSet<string> soundsToLoad)
        {
            HashSet<string> soundNotToLoad = new HashSet<string>();
            List<string> soundToRemove = new List<string>();

            foreach (KeyValuePair<string, SoundBuffer> keyValuePair in this.soundsDictionary)
            {
                if (soundsToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    soundToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    soundNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach (string keyToRemove in soundToRemove)
            {
                this.soundsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in soundsToLoad)
            {
                if (soundNotToLoad.Contains(pathToLoad) == false)
                {
                    //this.soundsDictionary.Add(pathToLoad, new SoundBuffer(pathToLoad));
                }
            }
        }

        public void LoadMusics(HashSet<string> musicsToLoad)
        {
            HashSet<string> musicNotToLoad = new HashSet<string>();
            List<string> musicToRemove = new List<string>();

            foreach (KeyValuePair<string, Music> keyValuePair in this.musicsDictionary)
            {
                if (musicsToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    musicToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    musicNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach (string keyToRemove in musicToRemove)
            {
                this.musicsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in musicsToLoad)
            {
                if (musicNotToLoad.Contains(pathToLoad) == false)
                {
                    this.musicsDictionary.Add(pathToLoad, new Music(pathToLoad));
                }
            }
        }
    }
}
