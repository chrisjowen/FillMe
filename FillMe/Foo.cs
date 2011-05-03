using System.Collections.Generic;

namespace FillMe
{
    public class Foo
    {
        public string Info { get; set; }
        public int Age { get; set; }
        public int CalculatedAge { get; set; }
        public string Name { get; set; }
        public Bar Bar { get; set; }
        public Goo Goo { get; set; }
        public string Description { get; set; }
        public List<Bar> Bars { get; set; }
    }
    public class Bar
    {
        public string Name { get; set; }
    }   
    
    public class Goo
    {
        public string Name { get; set; }
    }
}