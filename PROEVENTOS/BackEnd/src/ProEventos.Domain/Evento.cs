﻿using ProEventos.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain
{
    //[Table("Eventos")]
    public class Evento
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime? DataEvento { get; set; }
        /*[Required]
        [MaxLength(100)]*/
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImgUrl { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Lote>? Lotes { get; set; }
        public IEnumerable<RedeSocial>? RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento>? PalestrantesEventos { get; set; }
        /*[NotMapped]
         * para campo não ir para o banco de dados
        public int ContagemDias { get; set; }*/
    }
}
