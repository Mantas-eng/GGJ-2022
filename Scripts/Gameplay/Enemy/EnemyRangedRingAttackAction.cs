using UnityEngine;

public class EnemyRangedRingAttackAction : EnemyAttackAction
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask canCollideWith;

    protected override void Attack()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                CreateProjectile(new Vector2(i, j));
            }
        }


    }

    private void CreateProjectile(Vector2 direction)
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);

        EnemyController controller = transform.parent.gameObject.GetComponent<EnemyController>();
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.SpawnProjectile(direction, AttackDamage, controller.ProjectileSpeed, canCollideWith);
    }
}