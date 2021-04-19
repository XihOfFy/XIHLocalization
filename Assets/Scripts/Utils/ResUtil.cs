using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XIHLocalization
{
    public static class ResUtil
    {
        private static Dictionary<string, ScriptableObject> cfgs = new Dictionary<string, ScriptableObject>(10);
        public static T LoadScriptableObject<T>(string path) where T : ScriptableObject
        {

            T cfg;
            if (cfgs.ContainsKey(path))
                cfg = cfgs[path] as T;
            else
                cfgs.Add(path, cfg = Resources.Load<T>(path));
            return cfg;
        }
        private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>(10);
        public static Sprite LoadSprite(string ph)
        {
            if (!sprites.ContainsKey(ph))
                sprites.Add(ph, Resources.Load<Sprite>(ph));
            return sprites[ph];
        }
        private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(10);
        public static GameObject GetObjFromPrefab(string path)
        {
            if (!prefabs.ContainsKey(path))
                prefabs.Add(path, Resources.Load<GameObject>(path));
            return GameObject.Instantiate(prefabs[path], Vector3.zero, new Quaternion());
        }
    }
}
