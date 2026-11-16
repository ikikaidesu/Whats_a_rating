<p align="center">
      <img src="https://i.ibb.co/KcCxMNV0/movie-Rating-Guess.png" alt="Project Logo">
</p>

<p align="center">
    <img src="https://img.shields.io/badge/CSharp-7.3-blueviolet" alt="Csharp version">
    <img src="https://img.shields.io/badge/.NET-4.8-blueviolet" alt=".Net version">
    <img src="https://img.shields.io/badge/api-2.2.2-blueviolet" alt=".Net version">

</p>

<p align="center">
      <img src="https://i.ibb.co/JFqgnGp3/what-a-guess.gif" alt="Project Gif" width="700">
</p>

## About

What's a Rating! is a simple WPF-based game where the goal is to guess a movie's rating. The game offers two modes: a popular movies mode, where you guess the ratings of well-known films, and a friend mode, where you try to guess how your friend rated a particular movie.

This project relies on the <a href="https://kinopoiskapiunofficial.tech/">API</a>. It is completely free to use, and I am very grateful to its creator, as this project would not have been possible without their work.

To use the application, you need to obtain an API key from the <a href="API_WEBSITE_URL">API website</a> and paste it into the <code>resources/data/apiKey.txt</code> file.
Due to the API's daily limit of 500 requests, I left a code snippet in <code>utilities\managers\apiManager\apiManagerAbstract.cs</code> that retrieves movies from only the first page (20 movies out of the total available).
If you want the application to retrieve all available movies instead, remove this code snippet and uncomment the code containing the <code>while</code> loop.

## Instalation

- open git bush
- git clone https://github.com/ikikaidesu/Whats_a_rating.git

## Author
- [ikikaidesu](https://github.com/ikikaidesu)

📌 Comment

This is my first project after a long break from programming. Working on it helped me rediscover what I love about programming and refresh some of my existing knowledge.
The main purpose of this project was to get back into development, revisit familiar concepts, and regain confidence in my skills. Throughout the process, I encountered plenty of mistakes, shortcomings, and challenges, each of which taught me something valuable that I intend to apply in future projects.
Since this is primarily a portfolio project, I allowed myself to cut corners in some areas where I normally wouldn't during professional development. But most importantly, I had a great time working on it!
