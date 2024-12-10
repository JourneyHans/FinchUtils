namespace FinchUtils.Component
{
    /// <summary>
    /// 默认基础组件基类
    /// 只做了一件事情，就是默认Name为类名
    /// 如果需要自定义Name，请直接继承并实现IComponent接口
    /// </summary>
    public abstract class DefaultBaseComponent : IComponent
    {
        public string Name => GetType().Name;
        public ComponentManager Manager { get; set; }
        public abstract void Initialize();
        public abstract void Destroy();
    }
}