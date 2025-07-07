namespace Heavenage.Scripts.ECS.Runtime.AbilitySystem.Abilities
{
    public class SubAbilityTask<TTask> : SubAbility where TTask : IAbilityTaskWithSetup, new()
    {
        public override IAbilityTask CreateTask()
        {
            var task = new TTask();
            task.SetupFromSubAbility(this);
            return task;
        }
    }
}