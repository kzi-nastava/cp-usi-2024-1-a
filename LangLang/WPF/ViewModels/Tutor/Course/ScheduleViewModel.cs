using LangLang.Domain.Model;
using System.Collections.Generic;
using System;

namespace LangLang.WPF.ViewModels.Tutor.Course
{
    public class ScheduleViewModel
    {
        public Dictionary<WorkDay, Tuple<TimeOnly, int>> Schedule { get; set; }

        public ScheduleViewModel(Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule)
        {
            Schedule = schedule;
        }

        public override string ToString()
        {
            string schedule = "";
            foreach(KeyValuePair<WorkDay, Tuple<TimeOnly, int>> pair in Schedule){
                schedule += pair.Key + " at " + pair.Value.Item1 + ", class: " + pair.Value.Item2 + "\n";
            }
            return schedule;
        }
    }
}
