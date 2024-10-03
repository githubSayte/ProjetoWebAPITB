using ProjetoWebAPI.Model;

namespace ProjetoWebAPI.Repositorio
{
    public interface IFuncionarioRepositorio
    {
        public void Add(Funcionario funcionario, IFormFile foto);
        public List<Funcionario> GetAll();
        public Funcionario GetById(int id);
        void Update(Funcionario funcionario, IFormFile foto);
        void Delete(int id);

    }
}
