using System;

namespace FogBugz.Api.Domain
{
    public class Milestone : ISupportPartialRetrieval
    {
        public MilestoneId Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartNote { get; set; }
        public bool InActive { get; set; }
        public MilestoneId[] Dependencies { get; set; }
        public Project Project { get; set; }
        public bool Deleted { get; set; }
    }

   public struct MilestoneId
    {
        readonly int _value;

        private MilestoneId(int val)
        {
             _value = val;
        }

        public static explicit operator MilestoneId(int value)
        {
            return new MilestoneId(value);
        }

        public static implicit operator int(MilestoneId me)
        {
            return me._value;
        }    
    }}
