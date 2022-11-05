using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    private GameObject player;
    private LifeController liveCtrl;
    private GunBehaviour gunBeh;
    public TextMeshProUGUI ammoCountText;

    [SerializeField] private int hpCount;
    [SerializeField] private List<Image> hpImageList;

    [SerializeField] private GameObject EnemyHPBar;

    // Start is called before the first frame update
    void Start()
    {
        ammoCountText.text = "0";
        player = GameObject.Find("MainCharacter");
        liveCtrl = player.GetComponentInChildren<LifeController>();
        gunBeh = player.GetComponentInChildren<GunBehaviour>();
        Debug.Log(hpImageList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoCount();
    }

    private void UpdateAmmoCount()
    {
        ammoCountText.text = gunBeh.gunBase.GetRoundsLeft().ToString();
    }

    public void UpdateHPCount(int currentHealthCount) 
    {
        Debug.Log("Current HP" + currentHealthCount.ToString());
        int hpImageIndex = currentHealthCount;

        if (hpImageIndex < 0)
            return;

        if (hpImageIndex >= hpCount)
            return;

        if (hpImageList[hpImageIndex].enabled)
            hpImageList [hpImageIndex].enabled = false;
        else
            hpImageList[hpImageIndex].enabled = false;
    }

}
