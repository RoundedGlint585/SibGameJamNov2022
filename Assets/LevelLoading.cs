using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{

    [SerializeField]
    private int level = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isDead = GetComponent<LifeController>().IsDead();
        bool isAnimationPlayed = transform.GetChild(0).gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FinishState");
        if(isDead && isAnimationPlayed && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(level);
        }
    }
}
