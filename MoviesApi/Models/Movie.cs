﻿using MoviesApi.DTOS;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie: BaseMovie
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; } = default!;
        public Genre Genre { get; set; } = default!;


    }
}
