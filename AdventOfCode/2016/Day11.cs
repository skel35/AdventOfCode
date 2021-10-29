namespace AdventOfCode._2016;

public class Day11 : Solution
{
    public Day11() : base(11, 2016) { }
    protected override void Solve()
    {
        var state = new State();
        state.Set(1, 1);
        state.Set(5, 1);


    }

    class State
    {
        private const int Size = 10;
        private readonly BitArray _bitArray = new BitArray(Size * 4);

        public State()
        {
            
        }

        public State(State stateToCopy)
        {
            _bitArray = (BitArray) stateToCopy._bitArray.Clone();
        }
        
        public void Set(int index, int floor)
        {
            _bitArray[index * 2] = floor / 2 == 1;
            _bitArray[index * 2 + 1] = floor % 2 == 1;
        }

        public int Get(int index)
            => (_bitArray[index * 2] ? 1 : 0) * 2 + (_bitArray[index * 2 + 1] ? 1 : 0);

        public bool IsValid()
        {
            for (var i = 0; i < Size / 2; i++)
            {
                var microchip = Get(i * 2 + 1);
                if (microchip == Get(i * 2))
                    continue;
                for (var j = 0; j < Size / 2; j++)
                {
                    if (j == i) continue;
                    if (microchip == Get(j * 2))
                        return false;
                }
            }

            return true;
        }

        private IEnumerable<State> GenerateAllPossibleMoves()
        {
            for (var i = 0; i < Size; i++)
            {
                var floor = Get(i);
                if (floor != 0)
                {
                    var newState = new State(this);
                    newState.Set(i, floor - 1);
                    if (newState.IsValid())
                        yield return newState;

                    foreach (var anotherState in GenerateFrom(newState, i))
                        yield return anotherState;
                }

                if (floor != 3)
                {
                    var newState = new State(this);
                    newState.Set(i, floor + 1);
                    if (newState.IsValid())
                        yield return newState;
                    
                    foreach (var anotherState in GenerateFrom(newState, i))
                        yield return anotherState;
                }

                IEnumerable<State> GenerateFrom(State state, int moved)
                {
                    for (var j = moved + 1; j < Size; j++)
                    {
                        var floor2 = Get(j);
                        if (floor2 != 0)
                        {
                            var newState = new State(state);
                            newState.Set(j, floor2 - 1);
                            if (newState.IsValid())
                                yield return newState;
                        }
                        
                        if (floor2 != 3)
                        {
                            var newState = new State(state);
                            newState.Set(j, floor2 + 1);
                            if (newState.IsValid())
                                yield return newState;
                        }
                    }    
                }
                
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is State otherState && Equals(otherState);
        }

        protected bool Equals(State other)
        {
            return _bitArray.Equals(other._bitArray);
        }

        public override int GetHashCode()
        {
            return _bitArray.GetHashCode();
        }
    }
}