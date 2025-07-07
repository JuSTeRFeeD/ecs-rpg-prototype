using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Heavenage.Scripts.MorpehOneFrame
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal sealed class OneFrameCleanupSystem : ISystem
    {
        public World World { get; set; }

        private OneFrameRegister _register;

        public void OnAwake()
        {
            _register = OneFrameRegister.GetFor(World);
        }

        public void OnUpdate(float deltaTime)
        {
            _register.CleanOneFrameEvents();
        }

        public void Dispose()
        {
            _register.Dispose();
        }
    }
}