using System.Runtime.Serialization;

namespace prjx.libx
{
    [Serializable]
    internal class jmp2endEx : Exception
    {
        public jmp2endEx()
        {
         //   runtimeexc
        }

        public jmp2endEx(string? message) : base(message)
        {
        }

        public jmp2endEx(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected jmp2endEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}