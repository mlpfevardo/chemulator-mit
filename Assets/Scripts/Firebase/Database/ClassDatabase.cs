using Firebase.Database;
using Newtonsoft.Json;
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

        public static async Task<LabClass> GetLabClassAsync(string classkey)
        {
            DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            DataSnapshot labData = await dbref.OrderByKey().EqualTo(classkey).LimitToFirst(1).GetValueAsync();

            if (labData != null)
            {
                var lab = labData.Value as Dictionary<string, object>;

                if (lab != null)
                {
                    var data = lab.First();
                    var info = JsonConvert.DeserializeObject<LabClass>(JsonConvert.SerializeObject(data.Value));
                    info.ID = data.Key;

                    return info;
                }
            }

            return null;
        }

        public static async Task<IEnumerable<Student>> GetLabClassStudentsAsync(string key)
        {
            Debug.Log("Start GetStudentAsync, key=" + key);
            if (string.IsNullOrEmpty(key))
            {
                Debug.Log("No key passed. End GetStudents");
                return Enumerable.Empty<Student>();
            }

            DatabaseReference studentRef = FirebaseDatabase.DefaultInstance.GetReference(StudentDatabase.DB_NAME);
            DataSnapshot studentData = await studentRef.OrderByChild("classkey").EqualTo(key).GetValueAsync();

            if (studentData != null)
            {
                var students = studentData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (students != null)
                {
                    var result = new List<Student>();

                    foreach(KeyValuePair<string, object> student in students)
                    {
                        var info = JsonConvert.DeserializeObject<Student>(JsonConvert.SerializeObject(student.Value));

                        if (info != null)
                        {
                            info.ID = student.Key;
                            info.UserInfo = await UserDatabase.GetUserInfoByEmailAsync(info.Email);

                            result.Add(info);
                        }
                    }

                    return result;
                }
            }

            return Enumerable.Empty<Student>();
        }
    }
}
