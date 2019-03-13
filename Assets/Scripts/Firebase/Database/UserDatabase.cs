using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Firebase.Database
{
    public static class UserDatabase
    {
        public const string DB_NAME = "MemberRecord";

        public static Task RegisterUser(UserInfo user)
        {
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string json = JsonUtility.ToJson(user);

            return userRef.Child(user.ID).SetRawJsonValueAsync(json)
                .ContinueWith<Task>(task =>
                {
                    if (task.IsCompleted)
                    {
                        switch (user.userType)
                        {
                            case UserType.Student:
                                return StudentDatabase.RegisterStudent(user);
                            case UserType.Instructor:
                                return InstructorDatabase.RegisterInstructor(user);
                        }
                    }

                    return task;
                });
        }

        public static Task<UserInfo> GetUserInfo(string userId)
        {
            Debug.Log("Start GetUserInfo, userId=" + userId);
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return userRef.Child(userId)
                .GetValueAsync()
                .ContinueWith<UserInfo>(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Failed to get user info: " + task.Exception.Message);
                    }
                    else if (task.IsCompleted)
                    {
                        var result = JsonUtility.FromJson<UserInfo>(task.Result.GetRawJsonValue());
                        result.ID = userId;

                        return result;
                    }

                    return null;
                });
        }
    }
}
