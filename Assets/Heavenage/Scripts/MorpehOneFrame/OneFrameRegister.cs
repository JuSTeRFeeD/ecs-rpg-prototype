using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Heavenage.Scripts.MorpehOneFrame
{
    internal class OneFrameRegister : IDisposable
    {
        private static readonly IntHashMap<OneFrameRegister> Registers = new IntHashMap<OneFrameRegister>();

        private readonly World _world;
        private ICanClean[] _oneFrameFilters;
        private int _registeredFilters;

        private OneFrameRegister(World world)
        {
            _world = world;
            _oneFrameFilters = new ICanClean[1];
            _registeredFilters = 0;
        }

        public void Dispose()
        {
            CleanOneFrameEvents();
            Registers.Remove(_world.identifier, out _);
        }

        public static OneFrameRegister GetFor(World world)
        {
            if (Registers.TryGetValue(world.identifier, out var register))
                return register;

            register = new OneFrameRegister(world);
            Registers.Add(world.identifier, register, out _);
            return register;
        }

        private void RegisterOneFrame<T>()
            where T : struct, IComponent
        {
            for (var i = 0; i < _registeredFilters; i++)
            {
                if (_oneFrameFilters[i].GetInnerType() == typeof(T))
                {
                    return;
                }
            }

            if (_registeredFilters >= _oneFrameFilters.Length)
            {
                Array.Resize(ref _oneFrameFilters, _oneFrameFilters.Length << 1);
            }

            _oneFrameFilters[_registeredFilters++] = new OnFrameFilter<T>(_world);
        }

        public void CleanOneFrameEvents()
        {
            for (var i = 0; i < _registeredFilters; i++)
            {
                _oneFrameFilters[i].Clean();
            }
        }

        private sealed class OnFrameFilter<T> : ICanClean
            where T : struct, IComponent
        {
            private readonly Filter _filter;

            public OnFrameFilter(World world)
            {
                _filter = world.Filter.With<T>().Build();
            }

            public void Clean()
            {
                var stash = _filter.world.GetStash<T>();
                stash.RemoveAll();
            }

            public Type GetInnerType()
            {
                return typeof(T);
            }
        }

        private interface ICanClean
        {
            void Clean();
            Type GetInnerType();
        }
    }
}