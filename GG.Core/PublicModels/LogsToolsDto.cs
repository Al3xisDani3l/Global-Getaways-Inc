
using System;
namespace GG.Core
{
    public class LogsToolsDto : BaseEntity
    {
        public int Id { get; set; }
        public int IdTool { get; set; }
        public int IdAdminister { get; set; }
        public DateTime StartUse { get; set; }
        public DateTime EndUse { get; set; }


    }
}
