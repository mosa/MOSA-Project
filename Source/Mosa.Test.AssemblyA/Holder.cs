
namespace Mosa.Test.AssemblyA
{
    public class Holder<T>
    {
        public T value;

        public T GetValue() { return value; }
        public void SetValue(T value) { this.value = value; }

        public Holder(T value)
        {
            this.value = value;
        }
    }
}
