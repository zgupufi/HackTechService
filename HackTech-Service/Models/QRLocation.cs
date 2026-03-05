using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackTech_Service.Models
{
    public class QRLocation
    {
        public int Id { get; set; }

        public string QRCodeValue { get; set; } = string.Empty;

        public int NodeId { get; set; }

    }
}
