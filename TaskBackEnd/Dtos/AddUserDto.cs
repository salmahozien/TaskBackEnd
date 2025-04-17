namespace TaskBackEnd.Dtos
{
    public class AddUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile Signature { get; set; }
        public List<IFormFile> Images { get; set; }  =new List<IFormFile>();
    }
}
