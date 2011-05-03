namespace FillMe
{
	public interface IGenerateDummyData{
		object Generate(GenerationContext rootObject);
	}

    public class GenerationContext
    {
        private readonly object rootObject;
        private readonly object currentObject;

        public T RootAs<T>()
        {
            return (T) rootObject;
        }

        public GenerationContext(object rootObject)
            : this(rootObject, rootObject)
        {
        }

        public GenerationContext(object rootObject, object currentObject)
        {
            this.rootObject = rootObject;
            this.currentObject = currentObject;
        }

        public T CurrentAs<T>()
        {
            return (T) currentObject;
        }
    }
}
