using Safe.Inteface;

namespace Safe.Model {
    public class Notebook : HasId {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
