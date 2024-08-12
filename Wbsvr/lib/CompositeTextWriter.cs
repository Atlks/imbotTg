using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjx.libx
{
    class CompositeTextWriter : TextWriter
    {
        private readonly TextWriter _writer1;
        private readonly TextWriter _writer2;

        public CompositeTextWriter(TextWriter writer1, TextWriter writer2)
        {
            _writer1 = writer1 ?? throw new ArgumentNullException(nameof(writer1));
            _writer2 = writer2 ?? throw new ArgumentNullException(nameof(writer2));
        }

        public override Encoding Encoding => _writer1.Encoding;

        public override void Write(char value)
        {
            _writer1.Write(value);
            _writer2.Write(value);
        }

        public override void Write(string value)
        {
            try
            {
                _writer1.Write(value);
                _writer2.Write(value);
            }
            catch (Exception e)
            {
                PrintExcept("CompositeTextWriter.write()", e);
            }

        }

        public override void WriteLine(string value)
        {
            try
            {
                _writer1.WriteLine(value);
                _writer2.WriteLine(value);

            }
            catch (Exception e)
            {
                PrintExcept("CompositeTextWriter.write()", e);
            }
        }

        public override void WriteLine()
        {
            _writer1.WriteLine();
            _writer2.WriteLine();
        }

        // Override other Write methods as needed
    }
}
