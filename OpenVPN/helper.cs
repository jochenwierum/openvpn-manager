namespace OpenVPN
{
    internal class helper
    {
        private helper() { }

        public delegate void Action();
        public delegate void Action<T1>(T1 a);
        public delegate void Action<T1, T2>(T1 a, T2 b);

        public delegate T0 Function<T0>();
        public delegate T0 Function<T0, T1>(T1 a);
        public delegate T0 Function<T0, T1, T2>(T1 a, T2 b);
    }
}
