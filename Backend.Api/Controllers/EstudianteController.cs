using Backend.Core.Entities;
using Backend.Core.Exceptions;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/Estudiante")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly ILogger<EstudianteController> _logger;

        public EstudianteController(IEstudianteRepository estudianteRepository, ILogger<EstudianteController> logger)
        {
            _estudianteRepository = estudianteRepository;
            _logger = logger;
        }
        [HttpGet]
        [Route("GetEstudiantes")]
        public async Task<IActionResult> GetEstudiantes()
        {
            try
            {
                _logger.LogInformation("Get Estudiantes Controller");
                var estudiantes = await _estudianteRepository.GetEstudiantes();
                return Ok(estudiantes);

            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new BusinessException(error.Message);
            }
        }
        [HttpPost]
        [Route("AddEstudiante")]
        public async Task<IActionResult> AddEstudiante(Estudiante estudiante)
        {
            try
            {
                _logger.LogInformation("Add Estudiante Controller");
                var est = await _estudianteRepository.AddEstudiante(estudiante);
                return Ok(est);

            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new BusinessException(error.Message);
            }
        }
        [HttpGet]
        [Route("GenerateData")]
        public async Task<IActionResult> GenerateData()
        {
            try
            {
                _logger.LogInformation("Generate Data Controller");
                var res = await _estudianteRepository.GenerarDatos();
                return Ok(res);

            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new BusinessException(error.Message);
            }
        }
    }
}
