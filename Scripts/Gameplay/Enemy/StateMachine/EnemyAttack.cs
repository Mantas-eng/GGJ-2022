public class EnemyAttack : State
{
    public EnemyAttack(EnemyController controller) : base(controller)
    {
    }

    public override void Update()
    {

        if (!controller.IsTargerVisible())
        {
            controller.SetState(new EnemyFollow(controller));
        }
        else if (controller.IsTargetTooClose())
        {
            controller.SetState(new Retreat(controller));
        }
        else
        {
            controller.RotationTarget = controller.GetTargetDirection();
            controller.AttackAction.PerformAction();
        }
    }
}