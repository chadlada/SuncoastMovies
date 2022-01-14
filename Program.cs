using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SuncoastMovies
{

    class Program
    {
        static void Main(string[] args)
        {
            var context = new SuncoastMoviesContext();

            var movieCount = context.Movies.Count();
            Console.WriteLine($"There are {movieCount} movies!");

            var moviesWithRatings = context.Movies.Include(movie => movie.Rating);

            foreach (var movie in moviesWithRatings)
            {
                if (movie.Rating == null)
                {
                    Console.WriteLine($"There is a movie named {movie.Title} - with no rating");
                }
                else
                {
                    Console.WriteLine($"{movie.Title} - {movie.Rating.Description}");
                }
            }

            // Makes a new collection of movies but each movie knows the associated Rating object
            var moviesWithRatingsRolesAndActors = context.Movies.
                                                    // from our movie, please include the associated Rating object
                                                    Include(movie => movie.Rating).
                                                    // ... and from our movie, please include the associated Roles LIST
                                                    Include(movie => movie.Roles).
                                                    // THEN for each of roles, please include the associated Actor object
                                                    ThenInclude(role => role.Actor);
            foreach (var movie in moviesWithRatingsRolesAndActors)
            {
                if (movie.Rating == null)
                {
                    Console.WriteLine($"There is a movie named {movie.Title} and has not been rated yet");
                }
                else
                {
                    Console.WriteLine($"There is a movie named {movie.Title} and a rating of {movie.Rating.Description}");
                }
                foreach (var role in movie.Roles)
                {
                    Console.WriteLine($" - Has a character named {role.CharacterName} played by {role.Actor.FullName}");
                }
            }

            var newMovie = new Movie
            {
                Title = "SpaceBalls",
                PrimaryDirector = "Mel Brooks",
                Genre = "Comedy",
                YearReleased = 1987,
                RatingId = 2
            };

            context.Movies.Add(newMovie);
            context.SaveChanges();

        }

    }
}