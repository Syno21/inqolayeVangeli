using InqolaYevangeli.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static InqolaYevangeli.Models.Entities.Members;

namespace InqolaYevangeli.data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }  


        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipStatus> MembershipStatuses { get; set; }
        public DbSet<MemberStatusHistory> MemberStatusHistory { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Demographics> Demographics { get; set; }
        public DbSet<ActivityHistory> ActivityHistory { get; set; }

        public DbSet<Activity> Activity { get; set; }
        public DbSet<Members.Activity> Activities { get; set; }


    }
}
