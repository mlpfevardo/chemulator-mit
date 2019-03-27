using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Firebase.Database
{
    public static class ExerciseAnswerDatabase
    {
        public const string DB_NAME = "ExerciseAnswers";

        public static async Task<string> UpdateExerciseAnswer(ExerciseAnswer answer)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string key = string.IsNullOrEmpty(answer.ID) ? dbRef.Push().Key : answer.ID;

            await dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(answer));

            return key;
        }

        public static async Task<ExerciseAnswer> GetExerciseAnswer(UserInfo user, Exercise exercise)
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
                    foreach(KeyValuePair<string, object> d in data)
                    {
                        var info = d.Value as IEnumerable<KeyValuePair<string, object>>;

                        if (info.Where(m => m.Key == "exerciseid" && m.Value.ToString() == exercise.ID).Count() > 0)
                        {
                            var result = JsonConvert.DeserializeObject<ExerciseAnswer>(JsonConvert.SerializeObject(d.Value));
                            result.ID = d.Key;
                            return result;
                        }
                    }
                }
            }

            return null;
        }
    }
}
