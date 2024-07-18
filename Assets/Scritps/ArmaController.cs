using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ShotType
{
    Manual,
    Automatic
}

public class ArmaController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip disparar;
    [SerializeField] private AudioClip recargar;
    public Weapon weapon;

    [Header("References")]
    public Transform weaponMuzzle;
    //public Animator animator;

    public int currentAmmo { get; private set; }
    public GameObject owner { set; get; }

    private float lastTimeShoot = Mathf.NegativeInfinity;
    private Transform cameraPlayerTransform;
    private bool isReloading;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Asegúrate de que el arma esté bien posicionada al iniciar
        cameraPlayerTransform = Camera.main.transform;
        
    }

    private void Awake()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        currentAmmo = weapon.maxAmmo;
        /*EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);*/
    }

    public void SetAmmo(int newAmmo)
    {
        currentAmmo = newAmmo;
        EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
    }

    private void OnEnable()
    {
        isReloading = true;
        StartCoroutine(Draw(weapon.drawTime));
    }

    private void Update()
    {
        // Mantener el arma en una posición fija
        transform.localPosition = Vector3.zero;
    }

    public void Fire()
    {
        if (!isReloading)
        {
            if (weapon.shotType == ShotType.Manual || weapon.shotType == ShotType.Automatic)
            {
                //animator.SetTrigger("Disparar");
                TryShoot();
            }
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator Draw(float time)
    {
        yield return new WaitForSeconds(time - 0.15f);
        isReloading = false;
    }

    private bool TryShoot()
    {
        if (lastTimeShoot + weapon.fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandleShoot();
                currentAmmo -= 1;
                EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
                return true;
            }
        }
        return false;
    }

    private void HandleShoot()
    {
        // efecto flash
        GameObject flashClone = Instantiate(weapon.flashEffectPrefab, weaponMuzzle.position, Quaternion.Euler(transform.forward.x, transform.forward.y, transform.forward.z), transform);
        Destroy(flashClone, 1f);

        audioSource.PlayOneShot(disparar);
        AddRecoil();

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, weapon.fireRange, weapon.hittableLayers) && hit.collider.gameObject != owner)
        {
            GameObject bulletHoleClone = Instantiate(weapon.bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal), hit.collider.gameObject.transform);
            Destroy(bulletHoleClone, 2.3f);

            if (hit.collider.gameObject.GetComponent<Damageable>())
            {
                hit.collider.gameObject.GetComponent<Damageable>().InflictDamage(weapon.damage, false, owner);
            }
        }

        AddBulletTrial();

        lastTimeShoot = Time.time;
    }

    private void AddRecoil()
    {
        transform.Rotate(-weapon.recoilForce, 0f, 0f);
        transform.position = transform.position - transform.forward * (weapon.recoilForce / 50f);
    }

    private void AddBulletTrial()
    {
        if (weapon.bulletTrialPrefab == null) return;

        //GameObject bulletTrialEffect = Instantiate
    }

    private IEnumerator ReloadCoroutine()
    {
        if (isReloading || currentAmmo == weapon.maxAmmo)
            yield break;

        isReloading = true;
        audioSource.PlayOneShot(recargar);    

        yield return new WaitForSeconds(weapon.reloadTime - 0.15f);
        currentAmmo = weapon.maxAmmo;
        EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);

        Debug.Log("Recargada");
        isReloading = false;
    }

    public void Hide()
    {
       /* if (animator)
        {
            isReloading = false;
            animator.SetTrigger("Hiding");
        }*/
    }
}
