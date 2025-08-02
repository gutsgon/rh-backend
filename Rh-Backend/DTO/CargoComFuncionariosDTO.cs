using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rh_Backend.DTO
{
    public class CargoComFuncionariosDTO
    {
        public CargoCreateDTO Cargo { get; set; } = new CargoCreateDTO();
        public ICollection<FuncionarioReadDTO> Funcionarios { get; set; } = new List<FuncionarioReadDTO>();
    }
}