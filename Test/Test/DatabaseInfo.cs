using System;

namespace Test
{
    /// <summary>
    /// Database information with name and size in GB
    /// </summary>
    public class DatabaseInfo
    {
        public readonly string Name;

        public readonly double Size;

        public DatabaseInfo(string name, double size)
        {
            Name = name;
            Size = size;
        }

        public override string ToString()
        {
            return Name + ": " + Size;
        }
    }
}
