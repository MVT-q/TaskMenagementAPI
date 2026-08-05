namespace TaskMenagementAPI.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; } = null!;

        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
