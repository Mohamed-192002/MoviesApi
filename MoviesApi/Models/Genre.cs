﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
    }
}