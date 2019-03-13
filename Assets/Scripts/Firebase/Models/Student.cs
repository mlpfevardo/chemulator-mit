using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class Student
{
    [System.NonSerialized]
    public string ID;
    [System.NonSerialized]
    public UserInfo userInfo;
    public string email;
    public string classKey = string.Empty;
}
