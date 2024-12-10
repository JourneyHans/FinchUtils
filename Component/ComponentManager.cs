using FinchUtils.Debugger;

namespace FinchUtils.Component
{
    /// <summary>
    /// 组件管理器，可释放
    /// </summary>
    public class ComponentManager : IDisposable
    {
        private object _owner;

        /// <summary>
        /// 被管理的组件
        /// </summary>
        public Dictionary<string, IComponent> NameToComponent { get; } = new();

        /// <summary>
        /// 组件管理器的构造函数
        /// </summary>
        /// <param name="owner">谁持有这个管理器，默认可以不传</param>
        public ComponentManager(object owner = null)
        {
            _owner = owner;
        }

        #region 添加组件

        /// <summary>
        /// 添加组件，模板形式，IComponent的构造不需要参数时可调用这个, 会调用component的Initialize方法
        /// 底层调用另一个重载方法，会判断是否重复添加
        /// </summary>
        public T AddComponent<T>() where T : class, IComponent
        {
            var component = Activator.CreateInstance<T>();
            AddComponent(component);
            return component;
        }

        /// <summary>
        /// 添加组件，重复添加会报错，会调用component的Initialize方法
        /// </summary>
        /// <param name="component">添加的组件</param>
        public void AddComponent(IComponent component)
        {
            if (NameToComponent.ContainsKey(component.Name))
            {
                Log.Instance?.Error($"重复添加组件 {component.Name}");
                return;
            }

            NameToComponent.Add(component.Name, component);
            component.Manager = this;
            component.Initialize();
        }
        
        #endregion
        
        #region 获取组件
        
        /// <summary>
        /// 按类型获取component
        /// T被约束为DefaultBaseComponent，保证key为类型名
        /// </summary>
        public bool TryGetComponent<T>(out T result) where T : DefaultBaseComponent
        {
            if (NameToComponent.TryGetValue(typeof(T).Name, out var component) && component is T componentT)
            {
                result = componentT;
                return true;
            }

            result = null;
            return false;
        }
        
        /// <summary>
        /// 按名称获取component
        /// </summary>
        public bool TryGetComponent(string name, out IComponent result)
        {
            return NameToComponent.TryGetValue(name, out result);
        }
        
        #endregion
        
        #region 移除组件
        
        /// <summary>
        /// 按类型移除component，会调用component的Destroy方法
        /// T被约束为DefaultBaseComponent，保证key为类型名
        /// </summary>
        public bool RemoveComponent<T>() where T : DefaultBaseComponent
        {
            return RemoveComponentByName(typeof(T).Name);
        }
        
        /// <summary>
        /// 移除一个组件, 会调用component的Destroy方法
        /// </summary>
        public bool RemoveComponent(IComponent component)
        {
            return RemoveComponentByName(component.Name);
        }

        /// <summary>
        /// 按名称移除component, 会调用component的Destroy方法
        /// </summary>
        public bool RemoveComponentByName(string name)
        {
            if (NameToComponent.TryGetValue(name, out var component))
            {
                component.Destroy();
            }

            return NameToComponent.Remove(name);
        }

        #endregion

        /// <summary>
        /// 按类型获取owner
        /// </summary>
        public bool TryGetOwner<T>(out T result) where T : class
        {
            if (_owner is T owner)
            {
                result = owner;
                return true;
            }

            result = null;
            return false;
        }

        public void Dispose()
        {
            _owner = null;
            foreach (var component in NameToComponent.Values)
            {
                component.Destroy();
            }
            NameToComponent.Clear();
        }
    }
}