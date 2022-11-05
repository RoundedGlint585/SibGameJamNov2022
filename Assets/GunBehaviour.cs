using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static GunBehaviour;

public class GunBehaviour : MonoBehaviour
{

    public enum GunType
    {
        Pistol = 1,
        MachineGun = 2,
        Shotgun = 3,
        SniperRifle = 4,
        Chainsaw = 5,
        Bazooka = 6,
    }

    [SerializeField]
    public GunType gunType;

    public GunBase gunBase;

    public AudioSource audioSource;

    public AudioClip changeWeapon;

    public GameObject scopePoint;
    public GameObject holdingPoint;
    // Start is called before the first frame update
    void Start()
    {
        gunBase = transform.AddComponent<Pistol>();
        gunType = GunType.Pistol;
        audioSource = GetComponent<AudioSource>();
        changeWeapon = Resources.Load("Sounds/Puff") as AudioClip;
        holdingPoint = transform.GetChild(0).gameObject;
        scopePoint = holdingPoint.transform.GetChild(0).gameObject;
    }


    private float shootedLastTime;
    void SetGun(GunType gunType)
    {
        audioSource.PlayOneShot(changeWeapon);
        this.gunType = gunType;
        Destroy(gunBase);
        switch (gunType)
        { 
            case GunType.Pistol: gunBase = transform.AddComponent<Pistol>(); break;
            case GunType.MachineGun: gunBase = transform.AddComponent<MachineGun>(); break;
            case GunType.Shotgun: gunBase = transform.AddComponent<Shotgun>(); break;
            case GunType.SniperRifle: gunBase = transform.AddComponent<SniperRifle>(); break;
            case GunType.Chainsaw: gunBase = transform.AddComponent<Chainsaw>(); break;
            case GunType.Bazooka: gunBase = transform.AddComponent<Bazooka>(); break;
            default: break;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        shootedLastTime += Time.deltaTime;
        GunMovement();
        if (gunBase.GetRoundsLeft() == 0)
        {

            int newGunType = Random.Range(1, (int)GunType.MachineGun + 1);
            SetGun((GunType)newGunType);
            shootedLastTime = 0.0f;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;
        GameObject shootingPoint = GameObject.Find("ShootingPoint");
        Vector3 currentPosition = shootingPoint.transform.position;
        Vector3 direction = (mousePosition - currentPosition).normalized;
        if (gunBase.CanShoot(shootedLastTime))
        {
            bool isMouseProperlyClicked = false;
            if(gunType == GunType.MachineGun || gunType == GunType.Chainsaw)
            {
                isMouseProperlyClicked = Input.GetMouseButton(0);
            }
            else
            {
                isMouseProperlyClicked = Input.GetMouseButtonDown(0);
            }
            if (isMouseProperlyClicked)
            {
                gunBase.Shot(direction, currentPosition);
                shootedLastTime = 0.0f;
            }
        }

    }

    private void GunMovement() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 currentPosition = holdingPoint.transform.position;
        Vector3 direction = -(mousePosition - currentPosition);
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        FlippingUpdate();
        VisibilityUpdate();
    }

    private void VisibilityUpdate() 
    {
        Vector3 lookingDirection = scopePoint.transform.position - holdingPoint.transform.position;
        SpriteRenderer renderer = holdingPoint.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(Vector3.Dot(lookingDirection, Vector3.up) < 0.0f)
        {
            renderer.sortingOrder = 2;
        }
        else
        {
            renderer.sortingOrder = 0;
        }
    }

    private void FlippingUpdate()
    {

        Vector3 position = holdingPoint.transform.localPosition;
        Vector3 lookingDirection = scopePoint.transform.position - holdingPoint.transform.position;
        if (Vector3.Dot(lookingDirection, Vector3.right) < -0.1f)
        {
            position.x = -Mathf.Abs(position.x);
        }
        else if(Vector3.Dot(lookingDirection, Vector3.right) > 0.1f)
        {
            position.x = Mathf.Abs(position.x);
        }
        holdingPoint.transform.localPosition = position;
    }
}

public abstract class GunBase : MonoBehaviour
{
    public int clipSize;
    public int roundsLeft;
    public bool canGoThroughEnemy;
    public int projectilesPerShot;
    public float shotCooldown;
    public GameObject projectileObject;

    public AudioClip shotSound;
    public AudioSource audioSource;
    protected GunBase(int clipSize, bool canGoThroughEnemy, int projectilesPerShot, float shotCooldown)
    {
        this.clipSize = clipSize; 
        this.roundsLeft = clipSize;
        this.canGoThroughEnemy = canGoThroughEnemy;
        this.projectilesPerShot = projectilesPerShot;
        this.shotCooldown = shotCooldown;

    }


    public abstract void Shot(Vector3 direction, Vector3 shootingPointPos);

    public virtual bool CanShoot(float timeElapsedFromLastShot)
    {
        return timeElapsedFromLastShot > shotCooldown && roundsLeft > 0;
    }

    public virtual int GetRoundsLeft()
    {
        return roundsLeft;
    }

    public virtual void MakeSound()
    {
        audioSource.PlayOneShot(shotSound);
    }
}

public class Pistol : GunBase
{
    public Pistol() : base(7, false, 1, 0.1f)
    {

    }
    void Start()
    {
        projectileObject = Resources.Load("Prefabs/Projectile") as GameObject;
        shotSound = Resources.Load("Sounds/Shot") as AudioClip;

        audioSource = GetComponent<AudioSource>();
    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

        roundsLeft--;
        GameObject projectile;
        projectile = Instantiate(projectileObject);
        projectile.transform.position = shootingPointPos;
        direction.z = 0;

        float angle = Random.Range(-0.1f, 0.1f);
        Quaternion quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 randomizedDirection = quaternion * direction;
        randomizedDirection.z = 0.0f;
        projectile.GetComponent<ProjectileBehaviour>().SetDirection(direction.normalized);
        MakeSound();
    }

}

public class MachineGun : GunBase
{
    void Start()
    {
        projectileObject = Resources.Load("Prefabs/Projectile") as GameObject;
        shotSound = Resources.Load("Sounds/Shot") as AudioClip;
        audioSource = transform.gameObject.GetComponent<AudioSource>();
    }
    public MachineGun() : base(21, false, 1, 0.2f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {
        roundsLeft--;
        GameObject projectile;
        projectile = Instantiate(projectileObject);
        projectile.transform.position = shootingPointPos;
        direction.z = 0;

        float angle = Random.Range(-3.0f, 3.0f);
        Quaternion quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 randomizedDirection = quaternion * direction;
        randomizedDirection.z = 0.0f;
        projectile.GetComponent<ProjectileBehaviour>().SetDirection(randomizedDirection.normalized);
        MakeSound();
    }
}

public class Shotgun : GunBase
{
    public Shotgun() : base(5, false, 3, 0.1f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

    }
}

public class SniperRifle : GunBase
{
    public SniperRifle() : base(10, true, 1, 0.5f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

    }
}

public class Chainsaw : GunBase
{
    public Chainsaw() : base(100, true, 1, 0.5f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

    }
}

public class Bazooka : GunBase
{
    public Bazooka() : base(5, true, 1, 0.5f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

    }
}