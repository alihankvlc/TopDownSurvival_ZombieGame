using DeadNation;
using UnityEngine;
using Zenject;

namespace Efekan.Installers
{
    public class DependencyInjectionService : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputHandler>().FromInstance(InputManager.Instance).AsSingle();
            Container.Bind<IStatHandler>().FromInstance(StatManager.Instance).AsSingle();
            Container.Bind<IWeaponHandler>().FromInstance(PlayerWeaponManager.Instance).AsSingle();
            Container.Bind<IEquippableWeapon>().FromInstance(PlayerCombat.Instance).AsSingle();
        }
    }
}