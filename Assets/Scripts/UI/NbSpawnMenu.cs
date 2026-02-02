using Nanodogs.Nanobox.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nanodogs.Nanobox.UI
{
    /// <summary>
    /// Spawn menu that uses a single content container and rebuilds items when switching tabs.
    /// </summary>
    public class NbSpawnMenu : MonoBehaviour
    {
        // Single parent for all spawned UI items
        [Header("UI References")]
        [SerializeField] private Transform content;
        [SerializeField] private GameObject itemPrefab;

        [Header("Data")]
        public List<NbObj> ObjPool = new();

        private int currentTab = 0;

        private void OnEnable()
        {
            SwitchTab(currentTab);
        }

        /// <summary>
        /// Switches the active tab and rebuilds the UI for that category.
        /// </summary>
        /// <param name="tabIndex">Index matching NbScriptableObj.NBCategory</param>
        public void SwitchTab(int tabIndex)
        {
            currentTab = tabIndex;
            ClearContent();

            foreach (var obj in ObjPool)
            {
                if ((int)obj.NanoBoxScriptableObject.category != tabIndex)
                    continue;

                CreateItemUI(obj);
            }
        }

        /// <summary>
        /// Clears all spawned UI items.
        /// </summary>
        private void ClearContent()
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Creates a single UI item entry.
        /// </summary>
        private void CreateItemUI(NbObj obj)
        {
            var so = obj.NanoBoxScriptableObject;
            Sprite render = obj.GenerateRender();

            GameObject itemUi = Instantiate(itemPrefab, content);
            
            Button button = itemUi.GetOrAddComponent<Button>();
            button.onClick.AddListener(() => {
                obj.Spawn(Camera.main.transform.forward * 5f, Quaternion.identity);
            });

            TMP_Text text = itemUi.GetComponentInChildren<TMP_Text>();
            Image image = itemUi.GetComponent<Image>();

            if (text != null)
                text.text = so.ObjName;

            if (image != null)
                image.sprite = render;
        }
    }
}
