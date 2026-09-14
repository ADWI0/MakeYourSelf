using System;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Goal Instance { get; private set; }

    TMP_Text tmp;
    ParticleSystem particle;
    GameObject[] walls;

    float bestTime;
    float timer;
    bool fin;
    int deadCnt = 0;
    int brakeCnt = 0;

    public void AddDeadCnt()
    {
        deadCnt++;
    }

    public void AddBrakeCnt()
    {
        brakeCnt++;
    }
    public bool Fin { get { return fin; } }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tmp = GameObject.Find("Count").GetComponent<TMP_Text>();
        particle = GetComponentInChildren<ParticleSystem>();
        fin = false;
        walls = GameObject.FindGameObjectsWithTag("Wall");
        bestTime = 0f;
    }

    void Update()
    {
        if (!fin)
        {
            Counter();
        }
    }

    public void Restart()
    {
        fin = false;
        timer = 0f;
        foreach (GameObject wall in walls) wall.SetActive(true);
        deadCnt = 0;
        brakeCnt = 0;
    }

    private void Counter()
    {
        timer += Time.deltaTime;
        tmp.text =
            "Best : " + TimeSpan.FromSeconds(bestTime).ToString(@"hh\:mm\:ss") +
            "\nTime : " + TimeSpan.FromSeconds(timer).ToString(@"hh\:mm\:ss") +
            $"\nBrake : {brakeCnt}" +
            $"\nDead : {deadCnt}";
        //tmp.text = $"Time : {(int)timer / 3600}:{(int)timer % 3600 / 60}:{(int)timer % 60}\n" +
        //    $"Brake : {brakeCnt}\n" +
        //    $"Dead : {deadCnt}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (bestTime > timer || bestTime == 0f) bestTime = timer;
            Counter();
            foreach (GameObject wall in walls) wall.SetActive(false);
            particle.Play();
            fin = true;
        }
    }
}
