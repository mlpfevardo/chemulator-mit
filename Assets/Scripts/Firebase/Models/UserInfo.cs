using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public enum UserType
{
    Student,
    Instructor
}

public class UserInfo
{
    [JsonIgnore]
    public string ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; } = string.Empty;
    public UserBirthday Birthday { get; set; }
    public UserType UserType { get; set; } = UserType.Student;

    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
}
