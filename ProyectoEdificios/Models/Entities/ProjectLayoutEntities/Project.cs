namespace ProyectoEdificios.Models.Entities.ProjectLayoutEntities
{
    public class Project
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public  string Address { get; set; }
        public  string Province { get; set; }
        public  string Municipality { get; set; }
        public  string PlanImageUrl { get; set; }

        public ProjectLayout? Layout { get; set; }

    }
}
