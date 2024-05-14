using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] TankAbility tankAb;
    [Header("Player Stats")]
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float damage;
    [SerializeField] float health;
    [SerializeField] float currentHealth;
    [SerializeField] float luck;
    [SerializeField] int currency;
    [Header("Ability Stats")]
    [SerializeField] float summonRange;
    [SerializeField] float siphonRange;
    [SerializeField] float healRange;
    [SerializeField] float xp;

    public Vector3 PlayerLocation;
    public Quaternion PlayerRotation;

    private void Update()
    {
        if (currentHealth > health)
            currentHealth = health;
    }
    public Vector3 GetPlayerLocation()
    {
        PlayerLocation = GameObject.Find("Player").transform.position;
        return PlayerLocation;
    }

    public void SetPlayerLocation(Vector3 PlayerLocation)
    {
        if (PlayerLocation != null) GameObject.Find("Player").transform.position = PlayerLocation;
    }

    public Quaternion GetPlayerRotation()
    {
        PlayerRotation = GameObject.Find("Player").transform.rotation;
        return PlayerRotation;
    }

    public void SetPlayerRotation(Quaternion PlayerRotation)
    {
        if (PlayerRotation != null) GameObject.Find("Player").transform.rotation = PlayerRotation;
    }
    public int GetCurrency()
    { 
        return currency;
    }
    public void SetCurrency(int input)
    {
        currency += input;
    }
    public float GetStat(string stat)
    {
        if (stat == "health")
            return health;
        if (stat == "speed")
            return speed;
        if (stat == "damage")
            return damage;
        if (stat == "range")
            return range;
        if (stat == "luck")
            return luck;
        if (stat == "summon")
            return summonRange;
        if (stat == "siphon")
            return siphonRange;
        if (stat == "heal")
            return healRange;
        if (stat == "currentHealth")
            return currentHealth;
        else
            return 0;
    }
    public void SetStat(string stat, float num)
    {
        if (stat == "health")
            health = num;
        if (stat == "speed")
            speed = num;
        if (stat == "damage")
            damage = num;
        if (stat == "range")
            range = num;
        if (stat == "luck")
            luck = num;
        if (stat == "currentHealth")
            currentHealth = num;
    }
    public void IncreaseStat(string stat, float num)
    {
        if (stat == "health")
            health += num;
        if (stat == "speed")
            speed += num;
        if (stat == "damage")
            damage += num;
        if (stat == "range")
            range += num;
        if (stat == "luck")
            luck += num;
        if (stat == "currentHealth")
        {
            if (this.gameObject.name == "Tank Ally" && tankAb.GetShieldState())
                num *= 0.75f;
            currentHealth += num;
            //Debug.Log(num);
        }
    }
}
