﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public enum UserType
{
    Student,
    Instructor
}

[System.Serializable]
public class UserInfo
{
    [System.NonSerialized]
    public string ID;
    public string firstName;
    public string lastName;
    public string email;
    public string address = string.Empty;
    public UserBirthday birthday;
    public UserType userType = UserType.Student;

    public override string ToString()
    {
        return firstName + " " + lastName;
    }
}