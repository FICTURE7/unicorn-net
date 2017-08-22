namespace Unicorn.x86
{
    public static class x86Instructions
    {
        public static readonly Instruction AAA = new Instruction(1);

        public static readonly Instruction IN = new Instruction(218);
        public static readonly Instruction OUT = new Instruction(500);
    }
}
