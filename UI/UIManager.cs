using System;
using System.Collections.Generic;

namespace FinchUtils.MVC {
    public static class UIManager {
        private readonly static Dictionary<string, IUIBase> _uiDic = new();

        public static T Show<T>(params object[] args) where T : IUIBase {
            T ui = FindUI<T>();
            if (ui == null) {
                // 创建UI
                ui = Activator.CreateInstance<T>();
                ui.Init();
                _uiDic.Add(typeof(T).Name, ui);
            }

            // TODO: 已存在，重新显示（可能需要调整层级），如果不需要调整层级，就不调用Show了
            ui.Show(args);

            return ui;
        }

        public static T FindUI<T>() where T : IUIBase {
            if (_uiDic.TryGetValue(typeof(T).Name, out var ui)) {
                return (T)ui;
            }

            return default;
        }

        public static void Close<T>() where T : IUIBase {
            T ui = FindUI<T>();
            if (ui == null) {
                // 已经关闭
                return;
            }

            // 从数据结构中移除
            _uiDic.Remove(typeof(T).Name);

            ui.Close();
        }
    }
}