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

        public static async Task BuildDefaultExercise(LabClass labClass)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            var exps = await ExperimentDatabase.GetExperimentsAsync();

            foreach(var exp in exps)
            {
                var exer = new Exercise
                {
                    ClassID = labClass.ID,
                    MaxAttempts = 1,
                    TimeLimit = 60,
                    Name = exp.Name,
                };

                await RegisterExercise(exer);
            }
        }
    }
}
