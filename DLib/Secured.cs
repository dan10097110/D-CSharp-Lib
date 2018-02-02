namespace DLib
{
    public class Secured<T>
    {
        T t;
        string password;

        public Secured(T t, string password)
        {
            this.t = t;
            this.password = password;
        }

        public T Get(string password) => this.password == password ? t : default(T);
    }
}
