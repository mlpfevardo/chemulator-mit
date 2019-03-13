using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Firebase.Database
{
    public class InstructorDatabase
    {
        private const string DB_NAME = "Instructors";

        public static Task RegisterInstructor(UserInfo info)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string key = dbRef.Push().Key;

            var entry = new Instructor
            {
                ID = key,
                email = info.email,
            };

            return dbRef.Child(key).SetRawJsonValueAsync(JsonUtility.ToJson(entry));
        }
    }
}
