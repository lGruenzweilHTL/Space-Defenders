using UnityEngine;

public class Player_Shoot : MonoBehaviour {
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private PlayerUpgrades playerUpgrades;

    [SerializeField] private float bulletForce;

    public int damage;

    [SerializeField] private AudioSource source;

    private void Start() {
        if (source == null) {
            Debug.LogError("AudioSource component is missing.", this);
            return;
        }

        source.enabled = true;
        source.gameObject.SetActive(true);
        Debug.Log($"Audio source initialized: {source.gameObject.activeSelf && source.enabled}");
    }
    
    public void Shoot() {
        if (source == null || !source.enabled || !source.gameObject.activeSelf) {
            Debug.Log($"Source exists: {source != null}, enabled: {source.enabled}, active: {source.gameObject.activeSelf}", source);
            Debug.LogError("AudioSource is not properly configured.", source);
            return;
        }

        source.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damage = damage + (playerUpgrades.player_damageLevel * 2);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}