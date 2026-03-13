using Zenject;

public class UntitledInstaller : MonoInstaller
{
    public Map map;
    public PinPrefab pinPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        Container.Bind<Map>().FromInstance(map).AsSingle();
        Container.Bind<PinPrefab>().FromInstance(pinPrefab).AsSingle();
    }
}