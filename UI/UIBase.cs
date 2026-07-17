using System;

namespace FinchUtils.MVC {
    public enum Group {
        Bottom = 1,
        Common = 2,
        Top = 999,
    }

    public class LayerOrder : IComparable<LayerOrder> {
        public Group Group;
        public int Order;

        public int CompareTo(LayerOrder other) {
            if (Group == other.Group) {
                return Order.CompareTo(other.Order);
            }

            return Group.CompareTo(other.Group);
        }
    }

    public interface IUIBase : IComparable<IUIBase> {
        public LayerOrder LayerOrder { get; }
        public void Show(object[] showParams);
        public void Init();
        public void Close();
    }
}