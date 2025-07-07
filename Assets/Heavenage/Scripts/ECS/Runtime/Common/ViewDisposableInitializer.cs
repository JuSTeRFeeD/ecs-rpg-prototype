using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Heavenage.Scripts.ECS.Runtime.Common
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ViewDisposableInitializer : IInitializer
    {
        public World World { get; set; }

        public void OnAwake()
        {
            World.GetStash<EntityViewComponent>().AsDisposable();
        }

        public void Dispose()
        {
        }
    }
}