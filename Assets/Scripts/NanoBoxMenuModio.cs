using UnityEngine;
using Modio;
using Modio.Authentication;
using UnityEngine.UI;
using System.Threading.Tasks;
using Modio.Users;
using TMPro;

namespace Nanodogs.Nanobox.UI
{
    public class NanoBoxMenuModio : MonoBehaviour
    {
        [SerializeField] TMP_InputField authInput;
        [SerializeField] Button authRequest;

        [SerializeField] TMP_InputField codeInput;
        [SerializeField] Button codeSubmit;

        void Awake()
        {
            ModioServices.Bind<IModioAuthService>().FromInstance(new ModioEmailAuthService(GetAuthCode));
        }

        async Task OnInit()
        {
            if (User.Current.IsAuthenticated)
            {
                OnAuth();
                return;
            }
    
            // You can assign these using the Inspector if you prefer
            authRequest.onClick.AddListener(async () => await Authenticate());
        }
   
        async Task Authenticate()
        {
            Error error = await ModioClient.AuthService.Authenticate(true, authInput.text);
    
            if (error)
            {
                Debug.LogError($"Error authenticating with email: {error}");
                return;
            }
    
            OnAuth();
        }

        // This will be called by the ModioEmailAuthService object we constructed earlier
        async Task<string> GetAuthCode()
        {
            bool codeEntered = false;
    
            codeSubmit.onClick.AddListener(() => codeEntered = true);
    
            while (!codeEntered)
                await Task.Yield();
    
            return codeInput.text;
        }
   
        void OnAuth()
        {
            Debug.Log($"Authenticated user: {User.Current.Profile.Username}");
        }
    }
}
