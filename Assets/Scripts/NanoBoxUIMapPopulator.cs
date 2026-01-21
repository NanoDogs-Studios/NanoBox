using Nanodogs.Nanobox.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.UI
{
    public class NanoBoxUIMapPopulator : MonoBehaviour
    {
        public NbMapItem[] mapItems;

        public GameObject mapItemPrefab;
        public Transform grid;

        private void Start()
        {
            foreach (var mapItem in mapItems)
            {
                GameObject mapObj = GameObject.Instantiate(mapItemPrefab, grid);

                Image image = mapObj.GetComponent<Image>();
                image.sprite = mapItem.mapImage;

                TMP_Text text = mapObj.transform.Find("TextHolder/Name").GetComponent<TMP_Text>();
                text.text = mapItem.mapName;
            }
        }
    }
}
