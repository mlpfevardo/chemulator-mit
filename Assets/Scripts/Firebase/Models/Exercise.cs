﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Exercise
{
    [JsonIgnore]
    public string ID { get; set; }
    public string ClassID { get; set; }
    //public int LabActivity { get; set; } = 1;
    public string Name { get; set; }
    public string Instructions { get; set; }
    public int TimeLimit { get; set; }
    public int MaxAttempts { get; set; } = 1;
}