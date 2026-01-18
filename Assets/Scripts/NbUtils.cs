using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nanodogs.Nanobox
{
    public class NbUtils
    {
        public static GameObject IfNullFind(GameObject obj, string objPath)
        {
            return obj != null ? obj : GameObject.Find(objPath);
        }
    }
}
