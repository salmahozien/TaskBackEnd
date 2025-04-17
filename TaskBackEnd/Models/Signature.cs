namespace TaskBackEnd.Models
{
    public class Signature
    {
            public int Id { get; set; }
            public string SignaturePath { get; set; }
            public User User { get; set; }
            public int UserId { get; set; }
        
    }
}
