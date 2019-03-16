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

        public static async Task<ExerciseAnswer> GetExerciseAnswer(Student student, Exercise exercise)
        {
            if (student == null || exercise == null)
            {
                return null;
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            DataSnapshot exerData = await dbRef.OrderByChild("studentid").EqualTo(student.ID).GetValueAsync();

            if (exerData != null)
            {
                var data = exerData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (data != null)
                {
                    foreach(KeyValuePair<string, object> d in data)
                    {
                        var info = d.Value as IEnumerable<KeyValuePair<string, object>>;

                        if (info.Where(m => m.Key == "exerciseid" && m.Value.ToString() == exercise.ID) != null)
                        {
                            return JsonConvert.DeserializeObject<ExerciseAnswer>(JsonConvert.SerializeObject(d.Value));
                        }
                    }
                }
            }

            return null;
        }
    }
}
