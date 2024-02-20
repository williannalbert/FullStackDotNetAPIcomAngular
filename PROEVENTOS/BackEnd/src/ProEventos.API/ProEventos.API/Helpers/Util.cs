using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ProEventos.API.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public void DeleteImage(string imageName, string destino)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                var caminhoImagem = Path.Combine(_hostEnvironment.ContentRootPath, @$"Recursos/{destino}", imageName);
                if (System.IO.File.Exists(caminhoImagem))
                    System.IO.File.Delete(caminhoImagem);

            }
        }
        public async Task<string> SaveImage(IFormFile arquivoImagem, string destino)
        {
            string nomeImagem = new String(
                Path.GetFileNameWithoutExtension(arquivoImagem.FileName)
                .Take(10)
                .ToArray()
                ).Replace(' ', '-');

            nomeImagem = $"{nomeImagem}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(arquivoImagem.FileName)}";

            string caminhoImagem = Path.Combine(_hostEnvironment.ContentRootPath, @$"Recursos/{destino}", nomeImagem);

            using (var fileStream = new FileStream(caminhoImagem, FileMode.Create))
            {
                await arquivoImagem.CopyToAsync(fileStream);
            }
            return nomeImagem;
        }
    }
}
