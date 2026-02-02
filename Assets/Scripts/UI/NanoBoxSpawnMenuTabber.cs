using UnityEngine;
using UnityEngine.InputSystem;

namespace Nanodogs.Nanobox.UI
{
    public class NanoBoxSpawnMenuTabber : MonoBehaviour
    {
        [SerializeField] private InputAction tabAction;
        [SerializeField] private GameObject spawnMenu;
        private bool _isOpen = false;

        private void Awake()
        {
            // this sucks but has to be done.
            if (spawnMenu == null)
                spawnMenu = GameObject.Find("Canvas").transform.Find("Spawn Menu").gameObject;
        }

        private void OnEnable()
        {
            tabAction.Enable();
            tabAction.performed += OnTabPressed;

            _isOpen = false;
            if (spawnMenu != null) spawnMenu.SetActive(false);
        }

        private void OnDisable()
        {
            tabAction.performed -= OnTabPressed;
            tabAction.Disable();
        }

        private void OnTabPressed(InputAction.CallbackContext context)
        {
            _isOpen = !_isOpen;
            if (spawnMenu != null)
            {
                spawnMenu.SetActive(_isOpen);
                Cursor.visible = _isOpen;
                
                if(_isOpen)
                { Cursor.lockState = CursorLockMode.None; }
                else
                { Cursor.lockState = CursorLockMode.Locked; }
            }
        }
    }
}
