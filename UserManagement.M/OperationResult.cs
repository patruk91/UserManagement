using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.M
{
    public class OperationResult
    {
        public bool Succes { get; set; }
        public List<string> Messages { get; private set; }

        public OperationResult()
        {
            Messages = new List<string>();
        }
    }
}
