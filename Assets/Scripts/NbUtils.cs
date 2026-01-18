using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nanodogs.Nanobox
{
    public static class NbUtils
    {
        public static GameObject IfNullFind(GameObject obj, string objPath)
        {
            return obj != null ? obj : GameObject.Find(objPath);
        }

        /// <summary>
        /// For cases where we can only have one of a component on an object and we 
        /// want to get the component, or add it if it doesn't exist.
        /// Credit to u/CiberX15
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="toAdd"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            //Attempt to find the component on the object
            T newComponent = go.GetComponent<T>();

            //If it doesn't exist, create a new one
            if (newComponent == null)
            {
                newComponent = go.AddComponent<T>();
            }

            //Return the component
            return newComponent;
        }
    }
}
