using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    class Program
    {
        //create global instance of logger so other classes/methods can access it
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            // path to movie file
            string file = AppDomain.CurrentDomain.BaseDirectory + "movies.csv";

            // if statement to make sure movie file exists
            if (!File.Exists(file))
            {
                //log file not found error
                logger.Error(file, " doesn't exist.");
            }
            else
            {
                string resp;
                do
                {
                    // display choices to user
                    Console.WriteLine("Type 1 to add movie.");
                    Console.WriteLine("Type 2 to display movies");
                    Console.WriteLine("Type anything else to quit");

                    // input response
                    resp = Console.ReadLine();
                    logger.Info("User resp ", resp);

                    //create list to display the movie name, title, and genre
                    List<string> MovieName = new List<string>();
                    List<int> MovieID = new List<int>();
                    List<string> MovieGenre = new List<string>();
                    try
                    {
                        //create instance of StreamReader
                        StreamReader sr = new StreamReader(file);
                        // Headers for Moviename, MovieID, MovieGenre
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string fileLine = sr.ReadLine();
                            //split comma seperated file
                            int fileSplitter = fileLine.IndexOf(",");
                            if (fileSplitter == -1)
                            {
                                //file elements treated as char
                                string[] movieArray = fileLine.Split(',');
                                // element [0] = movieID
                                MovieID.Add(int.Parse(movieArray[0]));
                                // element [1] = movieName
                                MovieName.Add(movieArray[1]);
                                // element [2] = genre
                                MovieGenre.Add(movieArray[2]);
                            }
                            else
                            {
                                // get movieID
                                MovieID.Add(int.Parse(fileLine.Substring(0, fileSplitter - 1)));
                                fileLine = fileLine.Substring(fileSplitter + 1);
                                fileSplitter = fileLine.IndexOf('"');
                                // get movieName
                                MovieName.Add(fileLine.Substring(0, fileSplitter));
                                fileLine = fileLine.Substring(fileSplitter + 2);
                                //get genre
                                MovieGenre.Add(fileLine);
                            }
                        }
                        // Note from class: close files before exiting loop
                        sr.Close();
                    }
                    catch (Exception exeption)
                    {
                        logger.Error(exeption.Message);
                    }

                    if (resp == "1")
                    {
                        // Response 1 = add movie
                        Console.WriteLine("Enter movie name: ");
                        // user inputs movie name
                        string movieName = Console.ReadLine().ToLower();
                   
                        List<string> Movies = MovieName;
                        // check for duplicate movies
                        if (Movies.Contains(movieName))
                        {
                            logger.Info("Duplicate movie name caught", movieName);
                            Console.WriteLine("Movie already exist in file");
                        }
                        else
                        {
                            // assign movieID
                            Guid movieId = Guid.NewGuid();
                            // input genres
                            List<string> genres = new List<string>();
                    
                            Console.WriteLine("Please enter genre: ");
                            //add user input in field 
                            string genre = Console.ReadLine();
                            // Add user input to genres list
                            genres.Add(genre); 
                            Console.WriteLine("Movie added: " + movieName);
                        }
                    }
                    else if (resp == "2")
                    {
                        //Loop through file and display all categories
                        for (int i = 0; i < MovieID.Count; i++)
                        {
                            // display movie details
                            Console.WriteLine($"MovieID: {MovieID[i]}");
                            Console.WriteLine($"MovieName: {MovieName[i]}");
                            Console.WriteLine($"MovieGenre: {MovieGenre[i]}");
                        }
                    }
                } while (resp == "1" || resp == "2");
            }

            logger.Info("User void");
        }
    }
}
