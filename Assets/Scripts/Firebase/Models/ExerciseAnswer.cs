using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExerciseAnswer
{
    [JsonIgnore]
    public string ID { get; set; }
    public string ExerciseID { get; set; }
    public string StudentID { get; set; }
    public List<string> Answers { get; set; } = new List<string>();
    public DateTime DateTime { get; set; } = DateTime.Now;
}
