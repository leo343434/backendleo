using Backend.Core.Exceptions;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/Materia")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly IMateriaRepository _materiaRepository;
        private readonly ILogger<MateriaController> _logger;

        public MateriaController(IMateriaRepository materiaRepository, ILogger<MateriaController> logger)
        {
            _materiaRepository = materiaRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetMaterias")]
        public async Task<IActionResult> GetMaterias()
        {
            try
            {
                _logger.LogInformation("Get Materias Controller");
                var materias = await _materiaRepository.GetMaterias();
                return Ok(materias);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new BusinessException(error.Message);
            }
        }
    }
}
