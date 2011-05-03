using System;

namespace FillMe.Exceptions
{
    public class CantAssignGeneratorProblem : Problem
    {
        public CantAssignGeneratorProblem() { }
        public CantAssignGeneratorProblem(string message)
            : base(message)
        { }
        public CantAssignGeneratorProblem(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}