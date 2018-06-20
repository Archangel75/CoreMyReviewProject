using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MyReviewProject.NewFolder
{
    public class DebugTextWriter : System.IO.TextWriter
    {
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.Write(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            System.Diagnostics.Debug.Write(value);
        }

        public static void Write(string component, string message)
        {
            System.Diagnostics.Debug.Write("Component: " + component + " Message: " + message);
        }
    
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.Default; }
        }
    }
}