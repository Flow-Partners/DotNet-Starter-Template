using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBridge.Models.Entities
{
    /// <summary>
    /// Join table: which procedures each dentist can perform.
    /// </summary>
    public class DentistProcedure
    {
        public int DentistId { get; set; }
        public int ProcedureId { get; set; }

        [ForeignKey("DentistId")]
        public virtual Dentist Dentist { get; set; } = null!;

        [ForeignKey("ProcedureId")]
        public virtual Procedure Procedure { get; set; } = null!;
    }
}
