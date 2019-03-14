using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Instructor
{
    [JsonIgnore]
    public string ID { get; set; }
    [JsonIgnore]
    public UserInfo UserInfo { get; set; }
    public string Email { get; set; }
}
