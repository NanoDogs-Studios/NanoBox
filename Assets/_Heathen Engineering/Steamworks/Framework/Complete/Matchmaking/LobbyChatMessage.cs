#if !DISABLESTEAMWORKS && HE_STEAMPLAYERSERVICES && HE_STEAMCOMPLETE && !UNITY_SERVER
using HeathenEngineering.SteamAPI.UI;
using HeathenEngineering.Tools;
using System;
using UnityEngine.EventSystems;

namespace HeathenEngineering.SteamAPI
{
    public class LobbyChatMessage : HeathenUIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public SteamUserIconButton PersonaButton;
        public UnityEngine.UI.Text Message;
        public DateTime timeStamp;
        public UnityEngine.UI.Text timeRecieved;
        public bool ShowStamp = true;
        public bool AllwaysShowStamp = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ShowStamp && !timeRecieved.gameObject.activeSelf)
                timeRecieved.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!AllwaysShowStamp && timeRecieved.gameObject.activeSelf)
                timeRecieved.gameObject.SetActive(false);
        }
    }
}
#endif
