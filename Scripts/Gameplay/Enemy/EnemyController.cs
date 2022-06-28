using UnityEngine;
using UnityEngine.AI;

public class EnemyController : StateMashine
{
	[Header("References")]
	[SerializeField] private EnemyAttackAction attackAction;

	[Header("Parameters")]
	[SerializeField] private float health;
	[SerializeField] private float speed;
	[SerializeField] private float attackDistance;
	[SerializeField] private float attackDamage;
	[SerializeField] private float attackSpeed;
	[SerializeField] private float minimalDistanceToTarget;
	[SerializeField] private float retreatRecalculationPeriod;
	[SerializeField] private float projectileSpeed;
	[SerializeField] private LayerMask visionCollisionLayers;
	[SerializeField] private int killScore = 10;
	private Vector3 previousLocation;
	private Vector3 rotationTarget;

	public float Health { get => health; set => health = value; }
	public float Speed { get => Agent.speed; set => Agent.speed = value; }
	public float AttackDistance { get => attackDistance; set => attackDistance = value; }
	public float AttackDamage { get => attackAction.AttackDamage; set => attackAction.AttackDamage = value; }
	public float AttackSpeed { get => attackAction.AttackSpeed; set => attackAction.AttackSpeed = value; }
	public float MinimalDistanceToTarget { get => minimalDistanceToTarget; set => minimalDistanceToTarget = value; }

	public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
	public EnemyAttackAction AttackAction { get => attackAction; }
	public float RetreatRecalculationPeriod { get => retreatRecalculationPeriod; set => retreatRecalculationPeriod = value; }
	public Vector3 RotationTarget { get => rotationTarget; set => rotationTarget = value; }
	public Transform Target { get; private set; }
	public Animator EnemyAnimator { get; private set; }
	public NavMeshAgent Agent { get; private set; }

	private void Start()
	{
		Target = GameManager.Instance.Player.transform;
		EnemyAnimator = GetComponent<Animator>();
		Agent = GetComponent<NavMeshAgent>();
		Agent.updateRotation = false;
		Agent.updateUpAxis = false;

		Agent.speed = speed;
		AttackDamage = attackDamage;
		AttackSpeed = attackSpeed;

		SetState(new EnemyFollow(this));

		previousLocation = transform.position;
	}

	protected override void Update()
	{
		base.Update();

		EnemyAnimator.SetFloat("speed", previousLocation != transform.position ? 1 : 0);
		previousLocation = transform.position;

		Vector3 vectorToTarget = rotationTarget - transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
	}

	public void Damage(float damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			Die();
		} 
	}

	public void Die()
	{
		EnemyAnimator.SetBool("isDead", true);
		currentState = null;
		Agent.enabled = false;

		PlayerPrefs.SetInt("CurrentScore", PlayerPrefs.GetInt("CurrentScore", 0) + killScore);
		PlayerPrefs.Save();
		GameManager.Instance.UpdateScore();

		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<AudioSource>().Play();
	}

	public bool IsTargerVisible()
	{
		return IsTargerVisible(AttackDistance);
	}

	public bool IsTargerVisible(float distance)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, GetTargetDirection(), distance, visionCollisionLayers);

		return hit.transform == Target;
	}

	public float GetDistanceToTarget()
	{
		return Vector2.Distance(transform.position, Target.position);
	}

	public Vector2 GetTargetDirection()
	{
		return Target.position - transform.position;
	}

	public bool IsTargetTooClose()
	{
		return MinimalDistanceToTarget > GetDistanceToTarget();
	}

	public void DestroyEnemy()
	{
		Destroy(gameObject);
	}
}