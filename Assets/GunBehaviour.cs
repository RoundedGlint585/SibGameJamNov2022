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
    // Start is called before the first frame update
    void Start()
    {
        gunBase = transform.parent.AddComponent<Pistol>();
        gunType = GunType.Pistol;
    }


    private float shootedLastTime;
    void SetGun(GunType gunType)
    {
        this.gunType = gunType;
        switch (gunType)
        { 
            case GunType.Pistol: gunBase = transform.parent.AddComponent<Pistol>(); break;
            case GunType.MachineGun: gunBase = transform.parent.AddComponent<MachineGun>(); break;
            case GunType.Shotgun: gunBase = transform.parent.AddComponent<Shotgun>(); break;
            case GunType.SniperRifle: gunBase = transform.parent.AddComponent<SniperRifle>(); break;
            case GunType.Chainsaw: gunBase = transform.parent.AddComponent<Chainsaw>(); break;
            case GunType.Bazooka: gunBase = transform.parent.AddComponent<Bazooka>(); break;
            default: break;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        shootedLastTime += Time.deltaTime;

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
}

public abstract class GunBase : MonoBehaviour
{
    public int clipSize;
    public int roundsLeft;
    public bool canGoThroughEnemy;
    public int projectilesPerShot;
    public float shotCooldown;
    public Sprite sprite;
    public GameObject projectileObject;
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
}

public class Pistol : GunBase
{
    public Pistol() : base(7, false, 1, 0.1f)
    {

    }
    void Start()
    {
        projectileObject = Resources.Load("Prefabs/Projectile") as GameObject;
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
    }

}

public class MachineGun : GunBase
{
    void Start()
    {
        projectileObject = Resources.Load("Prefabs/Projectile") as GameObject;
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