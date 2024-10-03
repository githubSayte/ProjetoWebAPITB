
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ProjetoWebAPI.Model;
using ProjetoWebAPI.ORM;

namespace ProjetoWebAPI.Repositorio
{
    public class FuncionarioRepositorio : IFuncionarioRepositorio
    {
        private BdEmpresaContext _context;
        public FuncionarioRepositorio(BdEmpresaContext context)
        {
            _context = context;
        }
        public void Add(Funcionario funcionario, IFormFile foto)
        {
            // Verifica se uma foto foi enviada
            byte[] fotoBytes = null;
            if (foto != null && foto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    foto.CopyTo(memoryStream);
                    fotoBytes = memoryStream.ToArray();
                }
            }

            // Cria uma nova entidade do tipo TbFuncionario a partir do objeto Funcionario recebido
            var tbFuncionario = new TbFuncionario()
            {
                Nome = funcionario.Nome,
                Idade = funcionario.Idade,
                Foto = fotoBytes // Armazena a foto na entidade
            };

            // Adiciona a entidade ao contexto
            _context.TbFuncionarios.Add(tbFuncionario);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbFuncionario = _context.TbFuncionarios.FirstOrDefault(f => f.Id == id);

            // Verifica se a entidade foi encontrada
            if (tbFuncionario != null)
            {
                // Remove a entidade do contexto
                _context.TbFuncionarios.Remove(tbFuncionario);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Funcionário não encontrado.");
            }
        }                
        public List<Funcionario> GetAll()
        {
            List<Funcionario> listFun = new List<Funcionario>();

            var listTb = _context.TbFuncionarios.ToList();

            foreach (var item in listTb)
            {
                var funcionario = new Funcionario
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Idade = item.Idade                    
                };

                listFun.Add(funcionario);
            }

            return listFun;
        }
        public Funcionario GetById(int id)
        {
            // Busca o funcionário pelo ID no banco de dados
            var item = _context.TbFuncionarios.FirstOrDefault(f => f.Id == id);

            // Verifica se o funcionário foi encontrado
            if (item == null)
            {
                return null; // Retorna null se não encontrar
            }

            // Mapeia o objeto encontrado para a classe Funcionario
            var funcionario = new Funcionario
            {
                Id = item.Id,
                Nome = item.Nome,
                Idade = item.Idade,
                Foto = item.Foto // Mantém o campo Foto como byte[]
            };

            return funcionario; // Retorna o funcionário encontrado
        }
        public void Update(Funcionario funcionario, IFormFile foto)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbFuncionario = _context.TbFuncionarios.FirstOrDefault(f => f.Id == funcionario.Id);

            // Verifica se a entidade foi encontrada
            if (tbFuncionario != null)
            {
                // Atualiza os campos da entidade com os valores do objeto Funcionario recebido
                tbFuncionario.Nome = funcionario.Nome;
                tbFuncionario.Idade = funcionario.Idade;

                // Verifica se uma nova foto foi enviada
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        foto.CopyTo(memoryStream);
                        tbFuncionario.Foto = memoryStream.ToArray(); // Atualiza a foto na entidade
                    }
                }

                // Atualiza as informações no contexto
                _context.TbFuncionarios.Update(tbFuncionario);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Funcionário não encontrado.");
            }
        }

    }
}
