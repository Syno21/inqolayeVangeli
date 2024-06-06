namespace InqolaYevangeli.Models
{
    public class ManagerDashboardViewModel
    {
        public string BranchName { get; set; }
        public string Location { get; set; }
        public List<MemberCountViewModel> MemberCounts { get; set; }
    }

    public class MemberCountViewModel
    {
        public string AgeGroup { get; set; }
        public int Count { get; set; }
    }
}
