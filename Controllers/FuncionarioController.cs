using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoWebAPI.Model;
using ProjetoWebAPI.Repositorio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Adicionado para proteger todos os métodos com autenticação
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioRepositorio _funcionarioRepo; // O repositório que contém GetAll()

        public FuncionarioController(FuncionarioRepositorio funcionarioRepo)
        {
            _funcionarioRepo = funcionarioRepo;
        }

        // GET: api/Funcionario/{id}/foto
        [HttpGet("{id}/foto")]
        public IActionResult GetFoto(int id)
        {
            // Busca o funcionário pelo ID
            var funcionario = _funcionarioRepo.GetById(id);

            // Verifica se o funcionário foi encontrado
            if (funcionario == null || funcionario.Foto == null)
            {
                return NotFound(new { Mensagem = "Foto não encontrada." });
            }

            // Retorna a foto como um arquivo de imagem
            return File(funcionario.Foto, "image/jpeg"); // Ou "image/png" dependendo do formato
        }

        // GET: api/Funcionario
        [HttpGet]
        public ActionResult<List<Funcionario>> GetAll()
        {
            // Chama o repositório para obter todos os funcionários
            var funcionarios = _funcionarioRepo.GetAll();

            // Verifica se a lista de funcionários está vazia
            if (funcionarios == null || !funcionarios.Any())
            {
                return NotFound(new { Mensagem = "Nenhum funcionário encontrado." });
            }

            // Mapeia a lista de funcionários para incluir a URL da foto
            var listaComUrl = funcionarios.Select(funcionario => new Funcionario
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Idade = funcionario.Idade,
                UrlFoto = $"{Request.Scheme}://{Request.Host}/api/Funcionario/{funcionario.Id}/foto" // Define a URL completa para a imagem
            }).ToList();

            // Retorna a lista de funcionários com status 200 OK
            return Ok(listaComUrl);
        }

        // GET: api/Funcionario/{id}
        [HttpGet("{id}")]
        public ActionResult<Funcionario> GetById(int id)
        {
            // Chama o repositório para obter o funcionário pelo ID
            var funcionario = _funcionarioRepo.GetById(id);

            // Se o funcionário não for encontrado, retorna uma resposta 404
            if (funcionario == null)
            {
                return NotFound(new { Mensagem = "Funcionário não encontrado." }); // Retorna 404 com mensagem
            }

            // Mapeia o funcionário encontrado para incluir a URL da foto
            var funcionarioComUrl = new Funcionario
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Idade = funcionario.Idade,
                UrlFoto = $"{Request.Scheme}://{Request.Host}/api/Funcionario/{funcionario.Id}/foto" // Define a URL completa para a imagem
            };

            // Retorna o funcionário com status 200 OK
            return Ok(funcionarioComUrl);
        }


        // POST api/<FuncionarioController>        
        [HttpPost]
        public ActionResult<object> Post([FromForm] FuncionarioDto novoFuncionario)
        {
            // Cria uma nova instância do modelo Funcionario a partir do DTO recebido
            var funcionario = new Funcionario
            {
                Nome = novoFuncionario.Nome,
                Idade = novoFuncionario.Idade
            };

            // Chama o método de adicionar do repositório, passando a foto como parâmetro
            _funcionarioRepo.Add(funcionario, novoFuncionario.Foto);

            // Cria um objeto anônimo para retornar
            var resultado = new
            {
                Mensagem = "Usuário cadastrado com sucesso!",
                Nome = funcionario.Nome,
                Idade = funcionario.Idade
            };

            // Retorna o objeto com status 200 OK
            return Ok(resultado);
        }
               
        // PUT api/<FuncionarioController>        
        [HttpPut("{id}")]
        public ActionResult<object> Put(int id, [FromForm] FuncionarioDto funcionarioAtualizado)
        {
            // Busca o funcionário existente pelo Id
            var funcionarioExistente = _funcionarioRepo.GetById(id);

            // Verifica se o funcionário foi encontrado
            if (funcionarioExistente == null)
            {
                return NotFound(new { Mensagem = "Funcionário não encontrado." });
            }

            // Atualiza os dados do funcionário existente com os valores do objeto recebido
            funcionarioExistente.Nome = funcionarioAtualizado.Nome;
            funcionarioExistente.Idade = funcionarioAtualizado.Idade;

            // Chama o método de atualização do repositório, passando a nova foto
            _funcionarioRepo.Update(funcionarioExistente, funcionarioAtualizado.Foto);

            // Cria a URL da foto
            var urlFoto = $"{Request.Scheme}://{Request.Host}/api/Funcionario/{funcionarioExistente.Id}/foto";

            // Cria um objeto anônimo para retornar
            var resultado = new
            {
                Mensagem = "Usuário atualizado com sucesso!",
                Nome = funcionarioExistente.Nome,
                Idade = funcionarioExistente.Idade,
                UrlFoto = urlFoto // Inclui a URL da foto na resposta
            };

            // Retorna o objeto com status 200 OK
            return Ok(resultado);
        }


        // DELETE api/<FuncionarioController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            // Busca o funcionário existente pelo Id
            var funcionarioExistente = _funcionarioRepo.GetById(id);

            // Verifica se o funcionário foi encontrado
            if (funcionarioExistente == null)
            {
                return NotFound(new { Mensagem = "Funcionário não encontrado." });
            }

            // Chama o método de exclusão do repositório
            _funcionarioRepo.Delete(id);

            // Cria um objeto anônimo para retornar
            var resultado = new
            {
                Mensagem = "Usuário excluído com sucesso!",
                Nome = funcionarioExistente.Nome,
                Idade = funcionarioExistente.Idade
            };

            // Retorna o objeto com status 200 OK
            return Ok(resultado);
        }

    }
}
