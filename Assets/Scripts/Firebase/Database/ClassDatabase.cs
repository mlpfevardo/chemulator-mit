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

        public static Task RegisterLabClassAsync(LabClass lab)
        {
            DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return dbRef.Child(lab.ID).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(lab));
        }

        public static Task RegisterLabStudent(LabClass lab, UserInfo user)
        {
            DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference(StudentDatabase.DB_NAME);

            string key = dbRef.Push().Key;

            var entry = new Student
            {
                ID = key,
                Email = user.Email,
                ClassKey = lab.ID,
                UserInfo = user,
            };

            return dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(entry));
        }

        public static async Task<Instructor> GetLabInstructorAsync(LabClass lab)
        {
            DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference(InstructorDatabase.DB_NAME);

            DataSnapshot instructorData = await dbref.OrderByKey().EqualTo(lab.InstructorID).LimitToFirst(1).GetValueAsync();

            if (instructorData != null)
            {
                var instructor = instructorData.Value as Dictionary<string, object>;

                if (instructor != null)
                {
                    var data = instructor.First();
                    var info = JsonConvert.DeserializeObject<Instructor>(JsonConvert.SerializeObject(data.Value));
                    info.ID = data.Key;

                    info.UserInfo = await UserDatabase.GetUserInfoByEmailAsync(info.Email);

                    return info;
                }
            }

            return null;
        }

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

        public static async Task<bool> IsLabHasStudent(LabClass lab, UserInfo user)
        {
            DatabaseReference dbref = FirebaseDatabase.DefaultInstance.GetReference(StudentDatabase.DB_NAME);

            DataSnapshot studData = await dbref.OrderByChild("email").EqualTo(user.Email).GetValueAsync();

            if (studData != null)
            {
                var data = studData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (data != null)
                {
                    foreach (KeyValuePair<string, object> d in data)
                    {
                        var info = d.Value as IEnumerable<KeyValuePair<string, object>>;
                        if (info.Where(m => m.Key == "classkey" && m.Value.ToString() == lab.ID) != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
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

                    foreach (KeyValuePair<string, object> student in students)
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
        
        public static async Task<IEnumerable<Exercise>> GetLabClassExercisesAsync(LabClass lab)
        {
            Debug.Log("Start GetLabClassExercises, lab=" + lab.ID);
            if (lab == null)
            {
                return Enumerable.Empty<Exercise>();
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(ExerciseDatabase.DB_NAME);

            DataSnapshot exerciseData = await dbRef.OrderByChild("classid").EqualTo(lab.ID).GetValueAsync();

            if (exerciseData != null)
            {
                var exercises = exerciseData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (exercises != null)
                {
                    var result = new List<Exercise>();

                    foreach(var exercise in exercises)
                    {
                        var info = JsonConvert.DeserializeObject<Exercise>(JsonConvert.SerializeObject(exercise.Value));

                        if (info != null)
                        {
                            info.ID = exercise.Key;

                            result.Add(info);
                        }
                    }

                    return result;
                }
            }

            return Enumerable.Empty<Exercise>();
        }
    }
}
