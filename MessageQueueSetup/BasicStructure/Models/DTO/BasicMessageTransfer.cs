using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTO
{
    [Serializable]
    public class BasicMessageTransfer
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return $"message: {Message}";
        }
    }
}
