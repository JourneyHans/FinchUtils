using System;

namespace HzFramework.MVC {

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

    public abstract class UIBase : IComparable<UIBase> {
        private object[] _showParams;

        public LayerOrder LayerOrder { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsDestroyed { get; private set; }



        public int CompareTo(UIBase other) {
            return LayerOrder.CompareTo(other.LayerOrder);
        }
    }
}