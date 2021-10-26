
namespace GG.Core
{
    public class AddressDto : BaseEntity
    {



        public string Street { get; set; }
        public string InteriorNumber { get; set; }
        public string ExteriorNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }
        public string Latitude { get; set; }
        public string Logitude { get; set; }
    }
}
