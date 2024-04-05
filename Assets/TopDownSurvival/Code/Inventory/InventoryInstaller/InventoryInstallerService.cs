using UnityEngine;
using Zenject;

namespace _Project.Common.Inventory.InventoryInstaller
{
    public class InventoryInstallerService : MonoInstaller
    {
        [SerializeField] private InventoryManager _manager;
        [SerializeField] private Inventory _inventory;

        public override void InstallBindings()
        {
            Container.Bind<IInventoryManager>().To<InventoryManager>().FromInstance(_manager).AsSingle();
            Container.Bind<IProviderInventory>().To<Inventory>().FromInstance(_inventory).AsSingle();
        }
    }
}