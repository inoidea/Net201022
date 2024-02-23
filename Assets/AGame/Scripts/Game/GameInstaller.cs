using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<WeaponState>().FromInstance(new WeaponState()).AsCached();
    }
}
