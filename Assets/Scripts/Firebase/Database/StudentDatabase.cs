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
    public static class StudentDatabase
    {
        public const string DB_NAME = "Students";

        //public static Task RegisterStudent(UserInfo info)
        //{
        //    Debug.Log("Start RegisterStudent, info=" + info.ID);
        //    var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
        //    string key = dbRef.Push().Key;

        //    var entry = new Student
        //    {
        //        ID = key,
        //        Email = info.Email,
        //        UserInfo = info,
        //    };

        //    return dbRef.Child(key).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(entry));
        //}

        public static async Task<IEnumerable<Student>> GetStudentInfosAsync(UserInfo user)
        {
            Debug.Log("Start GetStudentInfosAsync, user=" + user?.ID);
            if (user.UserType != UserType.Student)
            {
                return Enumerable.Empty<Student>();
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            DataSnapshot studentData = await dbRef.OrderByChild("email").EqualTo(user.Email).GetValueAsync();

            if (studentData != null)
            {
                var students = studentData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (students != null)
                {
                    var result = new List<Student>();

                    foreach (var student in students)
                    {
                        var info = JsonConvert.DeserializeObject<Student>(JsonConvert.SerializeObject(student.Value));

                        if (info != null)
                        {
                            info.ID = student.Key;
                            info.UserInfo = user;

                            result.Add(info);
                        }
                    }

                    return result;
                }
            }

            return Enumerable.Empty<Student>();
        }

        public static async Task<IEnumerable<StudentGrade>> GetStudentGradesAsync(Student student, LabClass labClass)
        {
            Debug.Log($"GetStudentGradesAsync, student={student?.ID} labClass={labClass?.ID}");

            if (student == null || labClass == null)
            {
                return Enumerable.Empty<StudentGrade>();
            }

            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(GradeDatabase.DB_NAME);

            DataSnapshot gradeData = await dbRef.OrderByChild("studentid").EqualTo(student.ID).GetValueAsync();

            if (gradeData != null)
            {
                var grades = gradeData.Value as IEnumerable<KeyValuePair<string, object>>;

                if (grades != null)
                {
                    var result = new List<StudentGrade>();

                    foreach (var grade in grades)
                    {
                        var info = JsonConvert.DeserializeObject<StudentGrade>(JsonConvert.SerializeObject(grade.Value));

                        if (info != null)
                        {
                            info.ID = grade.Key;

                            result.Add(info);
                        }
                    }

                    return result;
                }
            }

            return Enumerable.Empty<StudentGrade>();
        }
    }
}
