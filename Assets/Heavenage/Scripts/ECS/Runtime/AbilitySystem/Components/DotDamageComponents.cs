using Scellecs.Morpeh;

namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Components
{
    public enum DamageType
    {
        Physical,
        Magical,
        Fire
    }

    public enum DotStackPolicy
    {
        Overwrite,
        Stack,
        Refresh,
        MaxDuration
    }
    
    public struct DotEffectComponent : IComponent
    {
        public int EffectId; // TODO[!!!]: use to distinguish between different effects (fire, poison, bleed, etc.)
        
        public Entity Source;
        public Entity Target;
        public DamageType DamageType;
        
        public float Duration;
        public float Elapsed;
        public float TickInterval;
        public float DamagePerTick;
        
        public DotStackPolicy StackPolicy;
    }
}