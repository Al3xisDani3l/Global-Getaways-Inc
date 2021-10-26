
namespace GG.Core
{
    public class CustomerDto : BaseEntity
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? IdAddress { get; set; }
        public string Phone { get; set; }
        public string ImagePath { get; set; }


    }
}
