namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities
{
    public interface IAbilityTaskWithSetup : IAbilityTask
    {
        void SetupFromSubAbility(SubAbility ability);
    }
}