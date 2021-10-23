namespace AdventOfCode._2016;

public class Day10 : Solution
{
    public Day10() : base(10, 2016) { }
    protected override void Solve()
    {
        var instructions = ReadLines()
            .ArrayMap(ParseInstruction);
        var bots = new Dictionary<int, (int Low, int High)>();
        var outputs = new Dictionary<int, List<int>>();
        var giveAways = instructions
            .OfType<BotGiveAway>()
            .ToDictionary(ga => ga.SourceBot);
        var gains = instructions
            .OfType<BotGain>()
            .ToArray();
        gains.ForEach(AddValue);

        while (giveAways.Count > 0)
        {
            var executeOn = bots.Where(kvp => kvp.Value.Low != 0 && kvp.Value.High != 0)
                .Where(kvp => giveAways.ContainsKey(kvp.Key))
                .ToArray();
                
            executeOn
                .ForEach(kvp =>
                {
                    Process(giveAways[kvp.Key]);
                    giveAways.Remove(kvp.Key);
                });
        }
                
        void AddValue(BotGain gain) => GiveToBot(gain.DestinationBot, gain.Value);

        void GiveToBot(int bot, int value)
        {
            bots.AddOrUpdate(bot, (value, 0),
                (_, current) => current.Item1 < value ? (current.Item1, value) : (value, current.Item1));
            if (bots[bot].Low == 17 && bots[bot].High == 61)
            {
                Console.WriteLine(bot);
            }
        }

        void Process(BotGiveAway ga)
        {
            var source = bots[ga.SourceBot];
            void ProcessSingle(DestinationType type, int index, int value)
            {
                if (type == DestinationType.Output)
                {
                    outputs.TryAdd(index, new List<int>());
                    outputs[index].Add(value);
                }
                else
                    GiveToBot(index, value);
            }

            ProcessSingle(ga.Low.Type, ga.Low.Index, source.Low);
            ProcessSingle(ga.High.Type, ga.High.Index, source.High);
            source.Low = 0;
            source.High = 0;
        }
            
    }
    enum DestinationType { Bot, Output };

    record Destination(DestinationType Type, int Index);

    interface IInstruction
    {
    };

    record BotGiveAway(int SourceBot, Destination Low, Destination High) : IInstruction;

    record BotGain(int Value, int DestinationBot) : IInstruction;

    IInstruction ParseInstruction(string instruction)
        => instruction.StartsWith("bot") ? ParseBotGiveAway(instruction) : ParseBotGain(instruction);

    private static BotGain ParseBotGain(string instruction)
    {
        var split = instruction.Split2();
        return new(split[1].ToInt(), split[5].ToInt());
    }

    private static BotGiveAway ParseBotGiveAway(string instruction)
    {
        var split = instruction.Split2();
        var sourceBot = split[1].ToInt();
        DestinationType getType(string typeDescription)
            => typeDescription == "bot" ? DestinationType.Bot : DestinationType.Output;

        var lowType = getType(split[5]);
        var highType = getType(split[10]);
        var lowIndex = split[6].ToInt();
        var highIndex = split[11].ToInt();
        var transaction = new BotGiveAway(sourceBot, new(lowType, lowIndex), new(highType, highIndex));
        return transaction;
    }
}