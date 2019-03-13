using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Firebase.Database
{
    public static class ClassDatabase
    {
        public const string DB_NAME = "Classes";

        public static Task<IEnumerable<Student>> GetStudents(string key)
        {
            DatabaseReference studentRef = FirebaseDatabase.DefaultInstance.GetReference(StudentDatabase.DB_NAME);

            return studentRef.OrderByChild("classKey").EqualTo(key).GetValueAsync()
                .ContinueWith<IEnumerable<Student>>(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Failed to GetStudents of class " + key);
                        Debug.LogError(task.Exception.Message);
                    }
                    else if (task.IsCompleted)
                    {
                        var x = task.Result;
                    }

                    return Enumerable.Empty<Student>();
                });
        }
    }
}
