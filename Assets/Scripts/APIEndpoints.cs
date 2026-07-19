using UnityEngine;

public static class APIEndpoints
{
    public const string baseUrl =
        "https://catharsisgame-api-production.up.railway.app/api";

    public const string createNewAccount =
        baseUrl + "/newAcc";
    public const string createNewAccountTemplate =
        "{\"username\":\"INPUT_USERNAME\"}";

    public const string save =
        baseUrl + "/saveUserData";


    public const string updateEnemiesCounts =
        baseUrl + "/updateEnemiesCounts";
    public const string updateEnemiesCountsTemplate =
        "{\"username\":\"INPUT_USERNAME\",\"enemiesKilled\":{\"INPUT_ENEMY_NAME\":1}}";

    public const string completedLevel =
        baseUrl + "/completedLevel";

    public const string healthCheck =
        baseUrl + "/healthCheck";
}
