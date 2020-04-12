using System;
using System.Diagnostics.CodeAnalysis;

namespace Generic.RepositoryTest.Int.Model
{
    public class FakeInt : IEquatable<FakeInt>
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool Equals([AllowNull] FakeInt other)
        {
            if (ReferenceEquals(other, null)) return false;

            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id && Value == other.Value;
        }
    }
}