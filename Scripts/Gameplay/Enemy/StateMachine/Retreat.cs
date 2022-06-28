using UnityEngine;

public class Retreat : State
{
    private float repeatTimer;

    public Retreat(EnemyController controller) : base(controller)
    {
    }

    public override void Start()
    {
        controller.Agent.enabled = true;
        repeatTimer = controller.RetreatRecalculationPeriod;
    }

    public override void Update()
    {
        if (!controller.IsTargetTooClose())
        {
            controller.SetState(new EnemyAttack(controller));
            controller.Agent.enabled = false;
            return;
        }

        repeatTimer += Time.deltaTime;

        if (repeatTimer >= controller.RetreatRecalculationPeriod)
        {
            Vector2 targetPosition = controller.Target.position;
            Vector2 controllerPosition = controller.transform.position;

            Vector2 retreatPosition = new Vector2((controllerPosition.x - targetPosition.x) + controllerPosition.x, (controllerPosition.y - targetPosition.y) + controllerPosition.y);

            controller.Agent.SetDestination(retreatPosition);
            controller.RotationTarget = retreatPosition;

            repeatTimer = 0;
        }
    }
}