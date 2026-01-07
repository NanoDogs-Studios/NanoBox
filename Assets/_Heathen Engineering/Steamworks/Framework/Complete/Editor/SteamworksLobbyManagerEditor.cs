//#if !UNITY_STANDALONE && !UNITY_EDITOR
//
//#endif

//#if !DISABLESTEAMWORKS
//using UnityEditor;
//using UnityEngine;

//namespace HeathenEngineering.Steamworks.Editors
//{
//    [CustomEditor(typeof(MatchmakingManager))]
//    public class SteamworksLobbyManagerEditor : Editor
//    {
//        private SerializedProperty OnLobbyMatchList;
//        private SerializedProperty OnLobbyCreated;
//        private SerializedProperty OnLobbyEnter;
//        private SerializedProperty OnLobbyExit;
//        private SerializedProperty OnGameServerSet;
//        private SerializedProperty OnLobbyChatUpdate;
//        private SerializedProperty QuickMatchFailed;
//        private SerializedProperty SearchStarted;
//        private SerializedProperty OnChatMessageReceived;
//        private SerializedProperty OnGameLobbyJoinRequest;

//        private int tabPage = 0;

//        private void BuildReferences()
//        {
//            OnLobbyMatchList = serializedObject.FindProperty("OnLobbyMatchList");
//            OnLobbyCreated = serializedObject.FindProperty("OnLobbyCreated");
//            OnLobbyEnter = serializedObject.FindProperty("OnLobbyEnter");
//            OnLobbyExit = serializedObject.FindProperty("OnLobbyExit");
//            OnGameServerSet = serializedObject.FindProperty("OnGameServerSet");
//            OnLobbyChatUpdate = serializedObject.FindProperty("OnLobbyChatUpdate");
//            QuickMatchFailed = serializedObject.FindProperty("QuickMatchFailed");
//            SearchStarted = serializedObject.FindProperty("SearchStarted");
//            OnChatMessageReceived = serializedObject.FindProperty("OnChatMessageReceived");
//            OnGameLobbyJoinRequest = serializedObject.FindProperty("OnGameLobbyJoinRequest");
//        }

//        public override void OnInspectorGUI()
//        {
//            BuildReferences();

//            Rect hRect = EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.LabelField("");

//            Rect bRect = new Rect(hRect);
//            bRect.width = hRect.width / 3f;
//            tabPage = GUI.Toggle(bRect, tabPage == 1, "Common Events", EditorStyles.toolbarButton) ? 1 : tabPage;
//            bRect.x += bRect.width;
//            tabPage = GUI.Toggle(bRect, tabPage == 2, "Search Events", EditorStyles.toolbarButton) ? 2 : tabPage;
//            bRect.x += bRect.width;
//            tabPage = GUI.Toggle(bRect, tabPage == 3, "Chat Events", EditorStyles.toolbarButton) ? 3 : tabPage;
//            EditorGUILayout.EndHorizontal();

//            switch (tabPage)
//            {
//                case 1: DrawCommonEventsTab(); break;
//                case 2: DrawSearchEventsTab(); break;
//                case 3: DrawChatEventsTab(); break;
//                default: DrawCommonEventsTab(); break;
//            }

//            serializedObject.ApplyModifiedProperties();
//        }

//        private void DrawCommonEventsTab()
//        {
//            EditorGUILayout.PropertyField(OnGameLobbyJoinRequest);
//            EditorGUILayout.PropertyField(OnLobbyCreated);
//            EditorGUILayout.PropertyField(OnLobbyEnter);
//            EditorGUILayout.PropertyField(OnLobbyExit);
//            EditorGUILayout.PropertyField(OnGameServerSet);
//        }

//        private void DrawSearchEventsTab()
//        {
//            EditorGUILayout.PropertyField(SearchStarted);
//            EditorGUILayout.PropertyField(OnLobbyMatchList);
//            EditorGUILayout.PropertyField(QuickMatchFailed);
//        }

//        private void DrawChatEventsTab()
//        {
//            EditorGUILayout.PropertyField(OnLobbyChatUpdate);
//            EditorGUILayout.PropertyField(OnChatMessageReceived);
//        }
//    }
//}
//#endif