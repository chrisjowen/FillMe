using System;

namespace FillMe.Tests.Functional
{
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Description { get; set; }
        public Foo Friend { get; set; }
        public Bar Enemy { get; set; }
        public Goo Maid { get; set; }
        public Sex Sex { get; set; }
        public DateTime Dob { get; set; }
        public AllowedPartner AllowedPartner { get; set; }
    }
}