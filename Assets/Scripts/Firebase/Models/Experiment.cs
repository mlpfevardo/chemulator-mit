using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Experiment
{
    [JsonIgnore]
    public string ID { get; set; }
    public string Name { get; set; }
}
