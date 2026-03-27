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
    public float rangeAdjustment = 8f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject rangeIndicator;

    [Header("Audio")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 1f;

    protected TowerTargeting targetingSystem;
    protected TowerTargeting towerTargeting;
    protected TowerUpgrades towerUpgrades;
    protected float fireCooldown = 0f;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        targetingSystem = GetComponent<TowerTargeting>();
        towerTargeting = GetComponent<TowerTargeting>();
        towerUpgrades = GetComponent<TowerUpgrades>();
        towerUpgrades.UpdateSellValueText();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        TowerData towerData = GetComponent<TowerData>();
        fireCooldown = Mathf.Max(0, fireCooldown - Time.deltaTime);

        if (towerTargeting != null)
        {
            GameObject targetEnemy = targetingSystem.GetTarget(towerData.targetingMode);

            if (targetEnemy != null)
            {
                EnemyMovement enemyMovement = targetEnemy.GetComponent<EnemyMovement>();

                if (enemyMovement != null && enemyMovement.IsInvulnerable())
                    return;

                if (fireCooldown <= 0f)
                {
                    ShootAtEnemy(targetEnemy);
                    PlayShootSound();
                    fireCooldown = 1f / fireRate;
                }
            }
        }
    }

    protected void PlayShootSound()
    {
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
    }

    protected virtual void OnMouseDown()
    {
        TowerSelectionManager.instance.SelectTower(this);
    }

    protected abstract void ShootAtEnemy(GameObject enemy);
}