using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Student
{
    [JsonIgnore]
    public string ID { get; set; }
    [JsonIgnore]
    public UserInfo UserInfo { get; set; }
    public string Email { get; set; }
    public string ClassKey { get; set; } = string.Empty;

    public override string ToString()
    {
        return UserInfo != null ? UserInfo.ToString() : Email;
    }
}
