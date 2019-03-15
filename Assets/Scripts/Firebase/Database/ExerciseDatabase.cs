using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

namespace Assets.Scripts.Firebase.Database
{
    public static class ExerciseDatabase
    {
        public const string DB_NAME = "Exercises";

        public static Task RegisterExercise(Exercise exercise)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string key = dbRef.Push().Key;

            return dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(exercise));
        }

        public static Task UpdateExercise(Exercise exercise)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return dbRef.Child(exercise.ID).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(exercise));
        }

        public static Task RemoveExercise(Exercise exercise)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return dbRef.Child(exercise.ID).RemoveValueAsync();
        }
    }
}
