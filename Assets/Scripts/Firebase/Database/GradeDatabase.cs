﻿using Firebase.Database;
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

        public static async Task<StudentGrade> GetGradeInfoAsync(Student student, string id)
        {
            if (student == null || string.IsNullOrEmpty(id))
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
                    foreach (KeyValuePair<string, object> d in data)
                    {
                        var info = d.Value as IEnumerable<KeyValuePair<string, object>>;

                        if (info.Where(m => m.Key == "experimentid" && m.Value.ToString() == id) != null)
                        {
                            return JsonConvert.DeserializeObject<StudentGrade>(JsonConvert.SerializeObject(d.Value));
                        }
                    }
                }
            }

            return null;
        }
    }
}