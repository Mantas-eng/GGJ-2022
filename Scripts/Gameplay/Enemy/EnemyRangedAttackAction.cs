using UnityEngine;

public class EnemyRangedAttackAction : EnemyAttackAction
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask canCollideWith;

    protected override void Attack()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);

        EnemyController controller = transform.parent.gameObject.GetComponent<EnemyController>();
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        Vector2 projectileDirection = controller.GetTargetDirection();
        projectile.SpawnProjectile(projectileDirection, AttackDamage, controller.ProjectileSpeed, canCollideWith);
    }
}