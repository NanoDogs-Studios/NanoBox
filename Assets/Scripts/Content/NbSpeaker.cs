using UnityEngine;
using Photon;
using Photon.Pun;
using Nanodogs.API.NanoMusic;

namespace Nanodogs.Nanobox.Content
{
    /// <summary>
    /// Wraps the NanoMusic in a public interface
    /// </summary>
    public class NbSpeaker : MonoBehaviourPunCallbacks
    {
        NanoMusic music;

        private void Start()
        {
            music = GetComponent<NanoMusic>();
        }

        #region Public API

        /// <summary>
        /// Plays a given <see cref="NanoMusicAsset"/>.
        /// </summary>
        /// <param name="asset">The <see cref="NanoMusicAsset"/>, create with Nanodogs/NanoMusic/</param>
        public void PlayMusic(NanoMusicAsset asset)
        {
            music.PlayMusic(asset);
        }

        /// <summary>
        /// Gets the current playing <see cref="NanoMusicAsset"/>.
        /// </summary>
        /// <returns>The <see cref="NanoMusicAsset"/> that is playing right now</returns>
        public NanoMusicAsset GetCurrentPlaying()
        {
            return music.CurrentPlaying;
        }

        /// <summary>
        /// Stops the currently playing NanoMusic.
        /// </summary>
        public void StopCurrentMusic()
        {
            music.StopCurrentMusic();
        }

        /// <summary>
        /// Resumes the currently playing NanoMusic.
        /// </summary>
        public void ResumeMusic()
        {
            music.ResumeCurrentMusic();
        }

        /// <summary>
        /// Pauses the currently playing NanoMusic.
        /// </summary>
        public void PauseMusic()
        {
            music.PauseCurrentMusic();
        }

        #endregion

        
    }
}
