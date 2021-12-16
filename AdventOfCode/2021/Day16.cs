using System.Globalization;
using System.Numerics;

namespace AdventOfCode._2021;

public class Day16 : Solution
{
    public Day16() : base(16, 2021) { }
    protected override void Solve()
    {
        var textInput = ReadText();
        var bits = textInput.Select(ch => int.Parse(ch.ToString(), NumberStyles.HexNumber))
            .SelectMany(b => b.ToBoolArray(4)).ToArray();
        // var numInput = BigInteger.Parse(textInput, NumberStyles.HexNumber);
        // var bits = new BitArray(numInput.ToByteArray());
        (bits.Length == textInput.Length * 4).Assert();
        var packet = Packet.Parse(bits, out _);
        int sumVersions(Packet packet) => packet.Version + (packet is Operator op ? op.SubPackets.Sum(sumVersions) : 0);
        BigInteger getValue(Packet packet)
        {
            return packet switch
            {
                LiteralValue litVal => litVal.Value,
                Operator op => op.TypeId switch
                {
                    0 => op.SubPackets.Select(getValue).Sum(),
                    1 => op.SubPackets.Select(getValue).Product(),
                    2 => op.SubPackets.Select(getValue).Min(),
                    3 => op.SubPackets.Select(getValue).Max(),
                    5 => getValue(op.SubPackets[0]) > getValue(op.SubPackets[1]) ? 1 : 0,
                    6 => getValue(op.SubPackets[0]) < getValue(op.SubPackets[1]) ? 1 : 0,
                    7 => getValue(op.SubPackets[0]) == getValue(op.SubPackets[1]) ? 1 : 0,
                    _ => throw new InvalidOperationException(),
                },
                _ => throw new InvalidOperationException()
            };
        }

        // var versionSum = sumVersions(packet);
        // versionSum.ToString().Print();

        var val = getValue(packet);
        val.ToString().Print();
    }

    class Packet
    {
        protected Packet(int version, int typeId)
        {
            Version = version;
            TypeId = typeId;
        }
        public int Version { get; }
        public int TypeId { get; }

        public static Packet Parse(bool[] input, out int bitsUsed)
        {
            var version = input.Take(3).ToInt();
            var typeId = input.Skip(3).Take(3).ToInt();
            bitsUsed = 6;
            if (typeId == 4)
            {
                var bitsForValue = new List<bool>();
                while (true)
                {
                    var stop = !input[bitsUsed];
                    bitsForValue.AddRange(input.Skip(bitsUsed + 1).Take(4));
                    bitsUsed += 5;
                    if (stop)
                    {
                        break;
                    }
                }
                return new LiteralValue(version, bitsForValue.ToBigInt());
            }

            var lengthTypeId = input.Skip(6).Take(1).ToInt();
            if (lengthTypeId == 0)
            {
                var totalLength = input.Skip(7).Take(15).ToInt();
                bitsUsed = 22;
                var nextInput = input.Skip(bitsUsed).ToArray();
                var used = 0;
                var subPackets = new List<Packet>();
                while (used < totalLength)
                {
                    var subPacket = Parse(nextInput, out var usedBySubPacket);
                    subPackets.Add(subPacket);
                    used += usedBySubPacket;
                    nextInput = nextInput.Skip(usedBySubPacket).ToArray();
                }

                bitsUsed += totalLength;
                return new Operator(version, typeId, lengthTypeId, subPackets);
            }
            else if (lengthTypeId == 1)
            {
                var numberOfSubPackets = input.Skip(7).Take(11).ToInt();
                bitsUsed = 18;
                var nextInput = input.Skip(bitsUsed).ToArray();
                var subPackets = new List<Packet>();
                var used = 0;
                for (var i = 0; i < numberOfSubPackets; i++)
                {
                    var subPacket = Parse(nextInput, out var usedBySubPacket);
                    subPackets.Add(subPacket);
                    nextInput = nextInput.Skip(usedBySubPacket).ToArray();
                    used += usedBySubPacket;
                }

                bitsUsed += used;
                return new Operator(version, typeId, lengthTypeId, subPackets);
            }
            else throw new InvalidOperationException();
        }
    }

    class LiteralValue : Packet
    {
        public LiteralValue(int version, BigInteger value)
            : base(version, 4)
        {
            Value = value;
        }
        public BigInteger Value { get; }
    }

    class Operator : Packet
    {
        public Operator(int version, int typeId, int lengthTypeId, List<Packet> subPackets)
            : base(version, typeId)
        {
            LengthTypeId = lengthTypeId;
            SubPackets = subPackets;
        }
        public int LengthTypeId { get; }
        public List<Packet> SubPackets { get; }
    }
}