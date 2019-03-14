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
    public class InstructorDatabase
    {
        public const string DB_NAME = "Instructors";

        public static Task RegisterInstructor(UserInfo info)
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string key = dbRef.Push().Key;

            var entry = new Instructor
            {
                ID = key,
                Email = info.Email,
            };

            return dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(entry));
        }

        public static async Task<Instructor> GetInstructorInfoAsync(UserInfo user)
        {
            if (user.UserType != UserType.Instructor)
            {
                return null;
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            DataSnapshot instructorData = await dbRef.OrderByChild("email").EqualTo(user.Email).LimitToFirst(1).GetValueAsync();

            if (instructorData != null)
            {
                var instructor = instructorData.Value as Dictionary<string, object>;

                if (instructor != null)
                {
                    var data = instructor.First();
                    var info = JsonConvert.DeserializeObject<Instructor>(JsonConvert.SerializeObject(data.Value));
                    info.ID = data.Key;

                    return info;
                }
            }

            return null;
        }

        public static async Task<IEnumerable<LabClass>> GetInstructorLabsAsync(Instructor instructor)
        {
            if (instructor == null)
            {
                return Enumerable.Empty<LabClass>();
            }
            Debug.Log("Start GetInstructorLabsAsync, instructor=" + instructor.ID);

            DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference(ClassDatabase.DB_NAME);
            DataSnapshot labData = await dbRef.OrderByChild("instructorid").EqualTo(instructor.ID).GetValueAsync();

            if (labData != null)
            {
                var labs = labData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (labs != null)
                {
                    var result = new List<LabClass>();

                    foreach (KeyValuePair<string, object> lab in labs)
                    {
                        var info = JsonConvert.DeserializeObject<LabClass>(JsonConvert.SerializeObject(lab.Value));

                        if (info != null)
                        {
                            info.ID = lab.Key;

                            result.Add(info);
                        }
                    }

                    return result;
                }
            }

            return Enumerable.Empty<LabClass>();
        }
    }
}
