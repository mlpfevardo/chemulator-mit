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
    public string UserID { get; set; }
    public List<string> Answers { get; set; } = new List<string>();
    public bool IsSubmitted { get; set; } = false;
    public bool IsStarted { get; set; } = false;
    public DateTime SubmitTime { get; set; } = DateTime.Now;
    public DateTime StartTime { get; set; } = DateTime.Now;
}
