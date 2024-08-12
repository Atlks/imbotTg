using System.Runtime.Serialization;

namespace mdsj.libBiz
{
    [Serializable]
    internal class jmp2endCurFunEx : Exception
    {
        public jmp2endCurFunEx()
        {
        }

        public jmp2endCurFunEx(string? message) : base(message)
        {
        }

        public jmp2endCurFunEx(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected jmp2endCurFunEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}