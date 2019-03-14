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
    public static class UserDatabase
    {
        public const string DB_NAME = "MemberRecord";

        public static Task RegisterUserAsync(UserInfo user)
        {
            Debug.Log("Start RegisterUserAsync, userId=" + user.ID);
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);
            string id = userRef.Push().Key;

            user.Email = user.Email.ToLower();

            return userRef.Child(id).SetRawJsonValueAsync(FirebaseJsonSerializer.SerializeObject(user))
                .ContinueWith<Task>(task =>
                {
                    if (task.IsCompleted)
                    {
                        switch (user.UserType)
                        {
                            case UserType.Student:
                                return StudentDatabase.RegisterStudent(user);
                            case UserType.Instructor:
                                return InstructorDatabase.RegisterInstructor(user);
                        }
                    }

                    return task;
                });
        }

        //public static Task<UserInfo> GetUserInfoAsync(string userId)
        //{
        //    Debug.Log("Start GetUserInfo, userId=" + userId);
        //    DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

        //    return userRef.Child(userId)
        //        .GetValueAsync()
        //        .ContinueWith<UserInfo>(task =>
        //        {
        //            if (task.IsFaulted)
        //            {
        //                Debug.Log("Failed to get user info: " + task.Exception.Message);
        //            }
        //            else if (task.IsCompleted)
        //            {
        //                var result = JsonConvert.DeserializeObject<UserInfo>(task.Result.GetRawJsonValue());
        //                result.ID = task.Result.Key;

        //                return result;
        //            }

        //            return null;
        //        });
        //}

        public static Task<UserInfo> GetUserInfoByEmailAsync(string email)
        {
            Debug.Log("Start GetUserInfoByEmail, email=" + email);
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(DB_NAME);

            return userRef.OrderByChild("email")
                .EqualTo(email.ToLower())
                .LimitToFirst(1)
                .GetValueAsync()
                .ContinueWith<UserInfo>(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Failed to get user info: " + task.Exception.Message);
                    }
                    else if (task.IsCompleted)
                    {
                        var result = task.Result.Value as IDictionary<string, object>;

                        if (result != null && result.Count > 0)
                        {
                            var data = result.First();
                            var info = JsonConvert.DeserializeObject<UserInfo>(JsonConvert.SerializeObject(data.Value));

                            info.ID = data.Key;

                            return info;
                        }
                    }

                    return null;
                });
        }

        public static async Task<IEnumerable<LabClass>> GetLabClasses(UserInfo user)
        {
            Debug.Log("Start GetLabClasses, user=" + user.ID);

            if (user.UserType == UserType.Student)
            {
                IEnumerable<Student> studentInfos = await StudentDatabase.GetStudentInfosAsync(user);
                var result = new List<LabClass>();

                foreach(var info in studentInfos)
                {
                    if (result.Exists(l => l.ID == info.ClassKey))
                    {
                        continue;
                    }

                    var lab = await ClassDatabase.GetLabClassAsync(info.ClassKey);
                    if (lab != null)
                    {
                        result.Add(lab);
                    }
                }

                return result;
            }
            else if (user.UserType == UserType.Instructor)
            {
                Instructor instructor = await InstructorDatabase.GetInstructorInfoAsync(user);
                return await InstructorDatabase.GetInstructorLabsAsync(instructor);
            }
            else
            {
                Debug.LogError("Unknown user type detected");
            }

            return Enumerable.Empty<LabClass>();
        }
    }
}
