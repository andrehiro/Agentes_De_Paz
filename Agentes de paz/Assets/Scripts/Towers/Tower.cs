using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Basic Stats")]
    public float damage = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public float range = 10f;
    public int pierce = 1;
    public int cost = 100;
    public float sellValueReturn = 0.7f;

    [Header("Tower Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject rangeIndicator;

    protected TowerTargeting targetingSystem;
    protected TowerTargeting towerTargeting;
    protected TowerUpgrades towerUpgrades;
    protected float fireCooldown = 0f;

    protected virtual void Start()
    {
        targetingSystem = GetComponent<TowerTargeting>();
        towerTargeting = GetComponent<TowerTargeting>();
        towerUpgrades = GetComponent<TowerUpgrades>();
        TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
        towerUpgrades.UpdateSellValueText(Mathf.RoundToInt(cost * sellValueReturn));
    }

    protected virtual void Update()
    {
        fireCooldown = Mathf.Max(0, fireCooldown - Time.deltaTime);

        if (towerTargeting != null)
        {
            GameObject targetEnemy = targetingSystem.GetTarget(towerTargeting.targetingButtonText.text);

            if (targetEnemy != null && fireCooldown <= 0f)
            {
                ShootAtEnemy(targetEnemy);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    protected abstract void ShootAtEnemy(GameObject enemy);
}