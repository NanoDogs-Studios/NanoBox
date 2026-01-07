using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.Core
{
    public class NbMapItemHolder : MonoBehaviour
    {
        public NbMapItem nbMapItem;
        public TMP_Text mapNameText;
        public Image mapImageImage;

        private void UpdateUI()
        {
            mapNameText.text = nbMapItem.mapName;
            mapImageImage.sprite = nbMapItem.mapImage;
        }
    }
}
