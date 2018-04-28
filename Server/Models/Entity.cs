using System;
using System.Text;

namespace Server.Models
{
    public class Entity
    {
        public static readonly Entity Constant = new Entity();

        public int EntityId { get; set; } = 1_000_000;

        public Entity ForeignKeyOne { get; set; } // Assume that reference objects are not loaded
        public int ForeignKeyOneId { get; set; } = 1_000_001;

        public Entity ForeignKeyTwo { get; set; } // Assume that reference objects are not loaded
        public int ForeignKeyTwoId { get; set; } = 1_000_002;

        public override string ToString()
            => new StringBuilder().Append("{\"entityId\":").Append(EntityId).Append(",\"foreignKeyOneId\":").Append(ForeignKeyOneId).Append(",\"foreignKeyTwoId\":").Append(ForeignKeyTwoId).Append(',').ToString();

        /// <summary>
        /// Assumes:
        ///     Requesting client knows how to construct object from the resulting string
        ///     Requesting client already knows the primary-key value
        /// </summary>
        public string ToStringCsv()
            => new StringBuilder().Append(ForeignKeyOneId).Append(',').Append(ForeignKeyTwoId).ToString();

        public static Entity FromBytes(byte[] bytes, int entityId = 0)
        {
            var foreignKeyOneIdBytes = new byte[4];
            Buffer.BlockCopy(bytes, 0, foreignKeyOneIdBytes, 0, 4);
            var foreignKeyTwoIdBytes = new byte[4];
            Buffer.BlockCopy(bytes, 4, foreignKeyTwoIdBytes, 0, 4);
            return new Entity
            {
                EntityId = entityId,
                ForeignKeyOneId = BitConverter.ToInt32(foreignKeyOneIdBytes, 0),
                ForeignKeyTwoId = BitConverter.ToInt32(foreignKeyTwoIdBytes, 0)
            };
        }

        public byte[] ToBytes()
        {
            var result = new byte[8];
            Buffer.BlockCopy(new[] { ForeignKeyOneId, ForeignKeyTwoId }, 0, result, 0, 8);
            return result;
        }
    }
}
