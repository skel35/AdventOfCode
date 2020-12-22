using System;

namespace AdventOfCode
{
    public class Either<T1, T2>
    {
        private readonly T1? left;
        private readonly T2? right;
        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;
        public T1 Left => IsLeft ? left! : throw new InvalidOperationException();
        public T2 Right => !IsLeft ? right! : throw new InvalidOperationException();

        public Either(T1 left)
        {
            this.left = left;
            this.IsLeft = true;
        }

        public Either(T2 right)
        {
            this.right = right;
            this.IsLeft = false;
        }
    }
}