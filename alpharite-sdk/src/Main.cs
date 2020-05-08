using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AlphaRite
{
    public class Main: MonoBehaviour {
        private static GameObject alphaRiteInstance;

        public void Start() {

        }

        public void Update() {

        }

        public void OnGUI() {

        }

        public static void AlphaRiteEntryPoint() {
            Main.alphaRiteInstance = new GameObject();
            Main.alphaRiteInstance.AddComponent<Main>();

            GameObject.DontDestroyOnLoad(Main.alphaRiteInstance);
        }

        public static void AlphaRiteDestroy() {
            GameObject.Destroy(Main.alphaRiteInstance);
        }
    }
}
