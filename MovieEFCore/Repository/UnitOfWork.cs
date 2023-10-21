using Movies;
using MoviesApi.Models;
using MoviesCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesEF.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IBaseRepository<Movie> Movies { get; private set; }

        public IBaseRepository<Genre> Genres { get; private set; }

        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
            Movies = new BaseRepository<Movie>(context);
            Genres = new BaseRepository<Genre>(context);
        }

        public void Dispose()
        => context.Dispose();


    }
}
