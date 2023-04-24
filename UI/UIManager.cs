using HzFramework.Collections;

namespace HzFramework.MVC {
    public static class UIManager {
        private static readonly PriorityQueue<UIBase> _stack = new();
    }
}