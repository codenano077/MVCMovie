using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Models;
using MvcMovie.Data;
using MVCMovie.Services;

namespace MVCMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly IMovieService _movieService;

        public MoviesController(MvcMovieContext context, IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;   
        }
        [HttpGet]
    public async Task<IActionResult> SearchMovie(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Json(new List<object>());

        var result = await _movieService.SearchMoviesAsync(query);

        if (result == null)
            return Json(new List<object>());

        var movies = result.Results
            .Where(m => !string.IsNullOrEmpty(m.Title))
            .Take(10)
            .Select(m => new
            {
                id = m.Id,
                title = m.Title,
                year = string.IsNullOrWhiteSpace(m.ReleaseDate)
                    ? ""
                    : m.ReleaseDate.Substring(0, 4),
                poster = string.IsNullOrWhiteSpace(m.PosterPath)
                    ? null
                    : $"https://image.tmdb.org/t/p/w200{m.PosterPath}"
            });
         return Json(movies);
    }

    [HttpGet]
    public async Task<IActionResult> GetMovieDetails(int id)
    {
        var movie = await _movieService.GetMovieDetailsAsync(id);

        if (movie == null)
            return NotFound();

        return Json(new
        {
            title = movie.Title,
            releaseDate = movie.ReleaseDate,
            rating = movie.VoteAverage,
            genre = string.Join(", ", movie.Genres.Select(g => g.Name)),
            poster = string.IsNullOrWhiteSpace(movie.PosterPath)
                ? ""
                : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}"
        });
    }

        // GET: Movies
        public async Task<IActionResult> Index(string MovieGenre, string searchString)
        {
            if(_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);
        }
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    
        public IActionResult Import()
        {
            return View(new ImportMovieViewModel());
        }

        [HttpPost]
[ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(ImportMovieViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var movie = new Movie
        {
            Title = model.Title,
            Genre = model.Genre,
            ReleaseDate = model.ReleaseDate,
            Rating = model.Rating,
            Price = model.Price,
            PosterUrl = model.PosterUrl
        };

        _context.Movie.Add(movie);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    }
}
