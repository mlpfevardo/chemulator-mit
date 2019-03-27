using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Firebase.Database
{
    public static class GradeDatabase
    {
        public const string DB_NAME = "Grades";

        public static async Task<string> RegisterGradeInfoAsync(UserInfo user, Exercise exercise, double score)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            string key = dbRef.Push().Key;

            var data = new StudentGrade
            {
                ExerciseID = exercise.ID,
                Score = score,
                UserID = user.ID,
            };

            await dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(data));

            return key;
        }

        public static Task UpdateGradeAsync(StudentGrade studentGrade)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return dbRef.Child(studentGrade.ID).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(studentGrade));
        }

        public static async Task<StudentGrade> GetGradeInfoAsync(UserInfo user, Exercise exercise)
        {
            if (user == null || exercise == null)
            {
                return null;
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            DataSnapshot exerData = await dbRef.OrderByChild("userid").EqualTo(user.ID).GetValueAsync();

            if (exerData != null)
            {
                var data = exerData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (data != null)
                {
                    foreach (KeyValuePair<string, object> d in data)
                    {
                        var info = d.Value as IEnumerable<KeyValuePair<string, object>>;

                        if (info.Where(m => m.Key == "exerciseid" && m.Value.ToString() == exercise.ID).Count() > 0)
                        {
                            var result = JsonConvert.DeserializeObject<StudentGrade>(JsonConvert.SerializeObject(d.Value));
                            result.ID = d.Key;
                            return result;
                        }
                    }
                }
            }

            string key = await RegisterGradeInfoAsync(user, exercise, 0);
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return await GetGradeInfoAsync(user, exercise);
        }
    }
}
