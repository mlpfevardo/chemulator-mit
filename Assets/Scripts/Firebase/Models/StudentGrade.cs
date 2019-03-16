using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum GradeType
{
    Exercise,
    Experiment
}

public class StudentGrade
{
    [JsonIgnore]
    public string ID { get; set; }
    public string StudentID { get; set; }
    public string ExperimentID { get; set; }
    public GradeType GradeType { get; set; }
    public double Score { get; set; }
}
