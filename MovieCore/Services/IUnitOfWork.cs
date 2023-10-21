using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesCore.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Movie> Movies { get; }
        IBaseRepository<Genre> Genres { get; }
    }
}
