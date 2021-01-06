using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

namespace ExtensionMethods
{
    public static class MonoBehaviourExtensions
    {
        public static GameManager GetGameManager(this GameObject go)
        {
            List<GameObject> allGameObjects;
            GameObject gameManagerGameObject;
            GameManager gm;
            Scene s = SceneManager.GetSceneByName("SampleScene");
            
            allGameObjects = s.GetRootGameObjects().ToList();
            gameManagerGameObject = allGameObjects.First(g => g.name == "GameManager");
            gm = gameManagerGameObject.GetComponent<GameManager>();

            return gm;
        }

        public static List<GameObject> GetAllChildren(this Transform t) {
            List<GameObject> children = new List<GameObject>();

            for (int i =0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i).gameObject);
            }

            return children;
        }
    }
}
