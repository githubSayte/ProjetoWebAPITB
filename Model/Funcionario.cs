using System.Text.Json.Serialization;

namespace ProjetoWebAPI.Model
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }

        [JsonIgnore] // Ignora a serialização deste campo
        public byte[]? Foto { get; set; }

        [JsonIgnore] // Ignora a serialização deste campo
        public string? FotoBase64 => Foto != null ? Convert.ToBase64String(Foto) : null;

        public string UrlFoto { get; set; } // Certifique-se de que esta propriedade esteja visível
    }



}


