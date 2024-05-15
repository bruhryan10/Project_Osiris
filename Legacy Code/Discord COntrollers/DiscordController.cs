using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;
    public LevelDataFetcher levelDataFetcher;

    private DateTime startTime;


    // Use this for initialization
    void Start()
    {

        discord = new Discord.Discord(1212572180748177468, (ulong)CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        var activity = new Activity
        {
            Details = levelDataFetcher.PlayerStatus,
            Timestamps =
            {
                Start = GetUnixTimestamp(startTime)
            }
    };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Result.Ok)
            {
                Debug.LogError("Everything is fine!");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }

    public TimeSpan GetElapsedTime()
    {
        return DateTime.UtcNow - startTime;
    }
    public long GetUnixTimestamp(DateTime dateTime)
    {
        return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}