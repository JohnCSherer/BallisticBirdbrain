using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerActivation : MonoBehaviour
{
    public MovementScript movementScript;
    public SpecialItemScript faceScript;

    public Sprite[] CountdownSprites;

    public Image Countdown;

    public Image NextPower;
    public Image SecondPower;


    /**
     * 1: Dash
     * 1: Grenade
     * 2: Earthquake
     */

    private int nextPower;
    private int secondPower;

    private ArrayList powerDeck;
    private int numPowers;

    public Sprite[] PowerSprites;


    private float timer;
    public int intervalBeats = 4;
    private float intervalSize;
    private int intTime;
    private float beatLength = 1 / 3.05f * 2;

    void Awake()
    {
        intervalSize = beatLength * intervalBeats;
        numPowers = PowerSprites.Length;
        powerDeck = new ArrayList();
        ShuffleDeck();
        nextPower = GetPower();
        NextPower.sprite = PowerSprites[nextPower - 1];
        secondPower = GetPower();
        SecondPower.sprite = PowerSprites[secondPower - 1];
        timer = intervalSize;
        intTime = (int)(timer/beatLength);
        DontDestroyOnLoad(gameObject);
    }

    public void Refresh()
    {
        powerDeck = new ArrayList();
        ShuffleDeck();
        nextPower = GetPower();
        NextPower.sprite = PowerSprites[nextPower - 1];
        secondPower = GetPower();
        SecondPower.sprite = PowerSprites[secondPower - 1];
        timer = intervalSize;
        intTime = (int)(timer / beatLength);
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        if(timer <= 0.0f)
        {
            timer = intervalSize;
            ActivatePower();
            intTime = (int) (timer/beatLength);
            Countdown.sprite = CountdownSprites[0];
        }
        if( (int) (timer/beatLength) < intTime)
        {
            intTime = (int) (timer/beatLength);
            if (intTime + 1 < CountdownSprites.Length)
            {
                Countdown.sprite = CountdownSprites[intTime + 1];
            }
        }
    }

    void ActivatePower()
    {
        switch (nextPower)
        {
            case 1:
                movementScript.SpeedBoost();
                break;
            case 2:
                movementScript.Pound();
                break;
            case 3:
                faceScript.ThrowGrenade();
                break;
            default:
                Debug.LogError("Error, tried to execute invalid power.");
                break;
        }
        nextPower = secondPower;
        NextPower.sprite = SecondPower.sprite;
        secondPower = GetPower();
        SecondPower.sprite = PowerSprites[secondPower - 1];
    }

    private int GetPower()
    {
        if (powerDeck.Count == 0)
        {
            ShuffleDeck();
        }
        int index = UnityEngine.Random.Range(0, powerDeck.Count);
        int result = (int)powerDeck[index];
        powerDeck.RemoveAt(index);
        return result;
    }

    private void ShuffleDeck()
    {
        powerDeck.Clear();
        for (int i = 1; i <= numPowers; i++)
        {
            powerDeck.Add(i);
        }
        
    }
}
