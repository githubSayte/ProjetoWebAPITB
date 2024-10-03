namespace ProjetoWebAPI.Model
{
    public class FuncionarioDto
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public IFormFile Foto { get; set; } // Campo para receber a foto
    }
}
