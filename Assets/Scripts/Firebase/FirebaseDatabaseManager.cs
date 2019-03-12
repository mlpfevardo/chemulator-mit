using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabaseManager : MonoBehaviour {
    private static FirebaseDatabaseManager instance = null;

	public static FirebaseDatabaseManager Instance { get { return instance; } }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Task RegisterUser(UserInfo user)
    {
        DatabaseReference userRef = GetUserReference();

        Debug.Log("Running transaction");

        string json = JsonUtility.ToJson(user);

        return userRef.Child(user.ID).SetRawJsonValueAsync(json);
    }

    public Task<UserInfo> GetUserInfo(string userId)
    {
        Debug.Log("Start GetUserInfo, userId=" + userId);
        DatabaseReference userRef = GetUserReference();

        return userRef.Child(userId).GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Failed to get user info: " + task.Exception.Message);
                }
                else if (task.IsCompleted)
                {
                    return JsonUtility.FromJson<UserInfo>(task.Result.GetRawJsonValue());
                }

                return null;
            });
    }

    private DatabaseReference GetUserReference()
    {
        return FirebaseDatabase.DefaultInstance.GetReference("Users");
    }
}
