using System.Collections.Generic;
using UnityEngine;

namespace Nanodogs.Nanobox.Core.Inventory
{
    public class NanoBoxInventoryManager : MonoBehaviour
    {
        public List<NbInventoryItem> inventory = new();

        public int currentInventoryIndex = 0;
        public NbInventoryItem equipped;

        public GameObject arms;

        public void Equip(NbInventoryItem item)
        {
            inventory.Add(item); 
            equipped = item;
        }

        private void Start()
        {
            // slot 0 is always hands.
        }

        private void Update()
        {
            equipped = inventory[currentInventoryIndex];
        }
    }
}
