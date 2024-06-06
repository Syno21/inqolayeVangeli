using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.Models
{
    public class AttendanceViewModel
    {
        public int ActivityID { get; set; }

        public string MemberID { get; set; }
        public IEnumerable<Activity> Activities { get; set; }

    }
}
