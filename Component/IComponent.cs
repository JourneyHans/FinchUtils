namespace FinchUtils.Component
{
    /// <summary>
    /// 组件接口，被ComponentManager所管理
    /// 一般情况下，选择继承DefaultBaseComponent，默认Name为类名
    /// 如果对Name有特殊要求，请直接继承实现IComponent接口
    /// </summary>
    public interface IComponent
    {
        // 组件名称，在Manager中当做Key使用
        string Name { get; }
        // 所属的ComponentManager
        ComponentManager Manager { get; set; }
        // 生命周期：初始化，被ComponentManager添加时会调用
        void Initialize();
        // 生命周期：销毁，被ComponentManager移除时会调用
        void Destroy();
    }
}