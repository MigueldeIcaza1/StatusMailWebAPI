namespace AutoStatus.WebAPI.Models
{
    public class StatusRecord
    {
        public int SerialNumber { get; set; }
        public IdWithLink ParentIdWithLink { get; set; }
        public IdWithLink TaskIdWithLink { get; set; }
        public string TaskTitle { get; set; }
        public CurrentStatus TaskStatus { get; set; }
        public string ParentTitle { get; set; }
        public string AssignedTo { get; set; }
        public string CompletedWork { get; set; }
    }
}
