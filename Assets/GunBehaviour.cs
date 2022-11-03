using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
        gunBase = new Pistol();
    }


    private float shootedLastTime;
    void SetGun(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.Pistol: gunBase = new Pistol(); break;
            case GunType.MachineGun: gunBase = new MachineGun(); break;
            case GunType.Shotgun: gunBase = new Shotgun(); break;
            case GunType.SniperRifle: gunBase = new SniperRifle(); break;
            case GunType.Chainsaw: gunBase = new Chainsaw(); break;
            case GunType.Bazooka: gunBase = new Bazooka(); break;
        default: break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        shootedLastTime += Time.deltaTime;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.0f;
        GameObject shootingPoint = GameObject.Find("ShootingPoint");
        Vector3 currentPosition = shootingPoint.transform.position;
        Vector3 direction = (mousePosition - currentPosition).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            if (gunBase.CanShoot(shootedLastTime))
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
    public bool canGoThroughEnemy;
    public int projectilesPerShot;
    public float shotCooldown;
    public Sprite sprite;
    public GameObject projectileObject;
    protected GunBase(int clipSize, bool canGoThroughEnemy, int projectilesPerShot, float shotCooldown)
    {
        this.clipSize = clipSize;
        this.canGoThroughEnemy = canGoThroughEnemy;
        this.projectilesPerShot = projectilesPerShot;
        this.shotCooldown = shotCooldown;
        projectileObject = Resources.Load("Prefabs/Projectile") as GameObject;
    }

    public abstract void Shot(Vector3 direction, Vector3 shootingPointPos);

    public virtual bool CanShoot(float timeElapsedFromLastShot)
    {
        return timeElapsedFromLastShot > shotCooldown;
    }
}

public class Pistol : GunBase
{
    public Pistol() : base(7, false, 1, 0.1f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {
        GameObject projectile;
        projectile = Instantiate(projectileObject);
        projectile.transform.position = shootingPointPos;
        direction.z = 0;
        projectile.GetComponent<ProjectileBehaviour>().SetDirection(direction.normalized);
    }

}

public class MachineGun : GunBase
{

    public MachineGun() : base(100, false, 1, 0.01f)
    {

    }
    public override void Shot(Vector3 direction, Vector3 shootingPointPos)
    {

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