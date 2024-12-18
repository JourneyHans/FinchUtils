using System;
using FinchUtils.Collections;

namespace FinchUtils.MVC;

public static class UIManager {
    private static readonly PriorityQueue<UIBase> AllUIs = new();

    public static T Show<T>(params object[] args) where T : UIBase {
        T ui = FindUI<T>();
        if (ui == null) {
            // 创建UI
            ui = Activator.CreateInstance<T>();
            ui.Init();
            AllUIs.Add(ui);
        }

        // TODO: 已存在，重新显示（可能需要调整层级），如果不需要调整层级，就不调用Show了
        ui.Show(args);

        return ui;
    }

    public static T FindUI<T>() where T : UIBase {
        foreach (UIBase uiBase in AllUIs) {
            if (uiBase is T ui) {
                return ui;
            }
        }

        return null;
    }

    public static void Close<T>() where T : UIBase {
        T ui = FindUI<T>();
        if (ui == null) {
            // 已经关闭
            return;
        }

        // 从数据结构中移除
        AllUIs.Remove(ui);

        ui.Close();
    }
}