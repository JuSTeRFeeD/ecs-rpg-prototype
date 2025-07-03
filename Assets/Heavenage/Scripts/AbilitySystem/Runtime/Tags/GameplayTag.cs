using System.Text;

namespace Heavenage.Scripts.AbilitySystem.Runtime.Tags
{
    public struct GameplayTag
    {
        public readonly string Name;
        public readonly int Id;

        public GameplayTag(string name)
        {
            Name = name;
            Id = GenerateHash(name);
        }

        public override bool Equals(object obj)
        {
            return obj is GameplayTag tag && Id.Equals(tag.Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(GameplayTag left, GameplayTag right)
        {
            return left.Id == right.Id;
        }

        public static bool operator !=(GameplayTag left, GameplayTag right)
        {
            return left.Id != right.Id;
        }

        private static readonly uint[] CRCTable = GenerateCrcTable();

        private static uint[] GenerateCrcTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                var crc = i;
                for (var j = 0; j < 8; j++)
                {
                    crc = (crc >> 1) ^ ((crc & 1) == 1 ? 0xEDB88320U : 0);
                }

                table[i] = crc;
            }

            return table;
        }

        public static int GenerateHash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return 0;

            var inputBytes = Encoding.UTF8.GetBytes(inputString);

            // CRC32
            var hash = 0xFFFFFFFFU;
            foreach (var b in inputBytes)
            {
                hash = (hash >> 8) ^ CRCTable[(hash & 0xFF) ^ b];
            }

            hash ^= 0xFFFFFFFFU;

            return (int)hash;
        }
    }
}
