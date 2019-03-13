using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Firebase.Database
{
    public static class StudentDatabase
    {
        public const string DB_NAME = "Students";

        public static Task RegisterStudent(UserInfo info)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string key = dbRef.Push().Key;

            var entry = new Student
            {
                ID = key,
                email = info.email,
                userInfo = info,
            };

            return dbRef.Child(key).SetRawJsonValueAsync(JsonUtility.ToJson(entry));
        }
    }
}
