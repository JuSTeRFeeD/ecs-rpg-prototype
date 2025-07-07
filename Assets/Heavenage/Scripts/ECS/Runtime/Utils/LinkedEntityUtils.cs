using System.Runtime.CompilerServices;
using Heavenage.Scripts.ECS.Runtime.Common.Components;
using Heavenage.Scripts.ECS.Runtime.Extensions;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace Heavenage.Scripts.ECS.Runtime.Utils
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class LinkedEntityUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Link(Entity owner, Entity child)
        {
            var ownerStash = StashRegistry.GetStash<OwnerComponent>();
            var linkedStash = StashRegistry.GetStash<LinkedEntitiesComponent>();
            
            ownerStash.Set(child, new OwnerComponent
            {
                Value = owner
            });

            if (!linkedStash.Has(owner))
            {
                linkedStash.Add(owner, new LinkedEntitiesComponent
                {
                    Values = new FastList<Entity>()
                });
            }
            linkedStash.Get(owner).Values.Add(child);
        }
    }
}