using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StudentGrade
{
    [JsonIgnore]
    public string ID { get; set; }
    public string StudentID { get; set; }
    public string ExerciseID { get; set; }
    public double Score { get; set; }
}
