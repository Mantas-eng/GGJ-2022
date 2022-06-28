public class EnemyFollow : State
{
    public EnemyFollow(EnemyController controller) : base(controller)
    {
    }

    public override void Start()
    {
        controller.Agent.enabled = true;
    }

    public override void Update()
    {
        controller.Agent.SetDestination(controller.Target.position);
        controller.RotationTarget = controller.Target.position;

        if (controller.IsTargerVisible())
        {
            controller.SetState(new EnemyAttack(controller));
            controller.Agent.enabled = false;
        }
    }
}