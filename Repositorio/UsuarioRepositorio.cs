using Microsoft.EntityFrameworkCore;
using ProjetoWebAPI.Model;
using ProjetoWebAPI.ORM;

namespace ProjetoWebAPI.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly BdEmpresaContext _context;

        public UsuarioRepositorio(BdEmpresaContext context)
        {
            _context = context;
        }

        public TbUsuario GetByCredentials(string usuario, string senha)
        {
            // Aqui você deve usar a lógica de hash para comparar a senha
            return _context.TbUsuarios.FirstOrDefault(u => u.Usuário == usuario && u.Senha == senha);
        }

        // Você pode adicionar métodos adicionais para gerenciar usuários
    }
}
