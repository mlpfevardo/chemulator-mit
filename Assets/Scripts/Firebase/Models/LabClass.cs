using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class LabClass
{
    [JsonIgnore]
    public string ID { get; set; }
    public string Name { get; set; }
    public string InstructorID { get; set; }
}