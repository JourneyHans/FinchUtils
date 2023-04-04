using UnityEngine;

namespace HzFramework.Singleton {
    internal class SingletonDriver : MonoBehaviour {
        void Update() {
            SingletonSystem.Update();
        }
    }
}