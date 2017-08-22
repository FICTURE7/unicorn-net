namespace Unicorn
{
    /// <summary>
    /// Represents an instruction.
    /// </summary>
    public struct Instruction
    {
        internal Instruction(int id)
        {
            _id = id;
        }

        internal readonly int _id;
    }
}
