using UnityEngine;

public class EnemyAttackAction : MonoBehaviour
{
    public float AttackDamage { get; set; } = 1;
    public float AttackSpeed { get; set; } = 1;

    private float attackTimer = 0;

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void PerformAction()
    {
        if (attackTimer < AttackSpeed) return;
        
        Attack();
        attackTimer = 0;
    }

    protected virtual void Attack()
    {
    }
}