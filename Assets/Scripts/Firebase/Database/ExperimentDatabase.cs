using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Firebase.Database
{
    public static class ExperimentDatabase
    {
        public static async Task<IEnumerable<Experiment>> GetExperimentsAsync()
        {
            var result = new List<Experiment>();

            result.Add(new Experiment
            {
                ID = "1",
                Name = "Matter: Classification and Seperation Techniques",
            });
            result.Add(new Experiment
            {
                ID = "2",
                Name = "Measuring the Density of a Solid and a Liquid",
            });
            result.Add(new Experiment
            {
                ID = "3",
                Name = "Stoichiometry",
            });
            result.Add(new Experiment
            {
                ID = "4",
                Name = "Graham's Law of Effusion",
            });
            result.Add(new Experiment
            {
                ID = "5",
                Name = "Effects of Nature of Solute and Solvent on Solubility",
            });

            return result;
        }
    }
}
